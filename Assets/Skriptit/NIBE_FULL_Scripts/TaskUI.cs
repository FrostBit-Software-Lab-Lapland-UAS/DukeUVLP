/******************************************************************************
 * File        : TaskUI.cs
 * Version     : 1.0
 * Author      : Miika Puljujärvi (miika.puljujarvi@lapinamk.fi)
 * Copyright   : Lapland University of Applied Sciences
 * Licence     : MIT-Licence
 * 
 * Copyright (c) 2022 Lapland University of Applied Sciences
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 *****************************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



/// <summary>
/// Shows information to user in the form of a floating panel. SubtaskStory-objects provide content to be displayed.
/// </summary>
public class TaskUI : MonoBehaviour
{

    // --- --- --- Variables --- --- ---

    public static TaskUI current;
    public TaskController taskController;




    // --- Color setup ---

    public Color backgroundColor;
    public Color textColor;
    public Color headlineColor;



    [Space(20f)]
    [Range(0.5f, 2f)] public float UIScale;
    public bool lookAtPlayer;
    public bool LookAtEnabled { get; set; } = true;

    private TextMeshProUGUI headlineTextObj;
    private TextMeshProUGUI mainTextObj;
    private TextMeshProUGUI rectBtnTextObj;
    private Image backgroundPanelObj;
    private Camera vrCam;
    private Transform scaleObjTransform;
    private Transform rectangularBtn;
    private Transform arrowBtn;

    private Image fadePanel;
    public bool fadeInProgress = false;
    public List<Button> languageButtons = new List<Button>();
    public List<ParticleSystem> particleEfects = new List<ParticleSystem>();



    // --- delegates ---

    public delegate void OnArrowClicked();
    public static event OnArrowClicked OnArrowClick;
    public delegate void OnRectangularClicked();
    public static event OnRectangularClicked OnRectangularClick;






    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---

    private void Start()
    {
        SetUIScale();
        SetUIColors();
    }
    private void Awake()
    {
        current = this;
        GetHierarchyObjectReferences();
    }
    private void Update()
    {
        LookAtPlayer();
    }



    // --- UI Position, Rotation, Scale and Color ---

    /// <summary>
    /// Get references to required components.
    /// </summary>
    private void GetHierarchyObjectReferences ()
    {
        vrCam = Camera.main;
        headlineTextObj = GameObject.Find("HeadlineText (TaskUI)").GetComponent<TextMeshProUGUI>();
        mainTextObj = GameObject.Find("MainText (TaskUI)").GetComponent<TextMeshProUGUI>();
        backgroundPanelObj = GameObject.Find("Background (TaskUI)").GetComponent<Image>();
        scaleObjTransform = GameObject.Find("ScaleObject (TaskUI)").transform;
        rectangularBtn = GameObject.Find("RectangularButton (TaskUI)").transform;
        rectBtnTextObj = rectangularBtn.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        arrowBtn = GameObject.Find("ArrowButton (TaskUI)").transform;
        fadePanel = GameObject.Find("FadePanel (vrCam)").GetComponent<Image>();
    }

    /// <summary>
    /// Turn the UI to face the player if the requirements are met.
    /// </summary>
    void LookAtPlayer()
    {
        Vector3 targetPos = (lookAtPlayer && LookAtEnabled) ? vrCam.transform.position : transform.position + transform.forward;
        scaleObjTransform.LookAt(targetPos);
    }

    /// <summary>
    /// Set the scale of the UI.
    /// (This is transform.localScale, so the whole object is scaled. Change the FontSize parameter of text objects instead if you want to fit more text in a panel.)
    /// </summary>
    void SetUIScale ()
    {
        scaleObjTransform.localScale = Vector3.one * 0.001f * UIScale;
    }

    /// <summary>
    /// Receive positional and rotational information from Subtask and set the UI's position and rotation based on those. 
    /// Called only at Start().
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    public void SetUIPositionAndRotation (Vector3 _pos, Quaternion _rot)
    {
        transform.position = _pos;
        //if (!lookAtPlayer || !LookAtEnabled)
            transform.rotation = _rot;
    }

    /// <summary>
    /// Set the color of different UI elements.
    /// Called only at Start().
    /// </summary>
    void SetUIColors()
    {
        headlineTextObj.color = headlineColor;
        mainTextObj.color = textColor;
        backgroundPanelObj.color = backgroundColor;
    }




    // --- Visibility Setup ---

    /// <summary>
    /// Set button visibility based on the current SubtaskStory's settings.
    /// </summary>
    /// <param name="_btnSetup"></param>
    public void SetButtonVisibility(SubtaskStoryButton _btnSetup)
    {
        switch (_btnSetup)
        {
            default:
            case SubtaskStoryButton.None:
                arrowBtn.transform.parent.gameObject.SetActive(false);
                rectangularBtn.transform.parent.gameObject.SetActive(false);
                break;

            case SubtaskStoryButton.Arrow:
                arrowBtn.transform.parent.gameObject.SetActive(true);
                rectangularBtn.transform.parent.gameObject.SetActive(false);
                break;

            case SubtaskStoryButton.Rectangular:
                arrowBtn.transform.parent.gameObject.SetActive(false);
                rectangularBtn.transform.parent.gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Toggle panel visibility based on the current SubtaskStory's settings.
    /// </summary>
    /// <param name="_showUI"></param>
    public void SetPanelVisibility (bool _showUI)
    {
        backgroundPanelObj.gameObject.SetActive(_showUI);
    }




    // --- Text Setup ---

    public void SetHeadlineText (string _s)
    {
        headlineTextObj.text = _s;
    }
    public void SetMainText(string _s)
    {
        mainTextObj.text = _s;
    }
    public void SetRectangularButtonText (string _s)
    {
        rectBtnTextObj.text = _s;
    }


    // --- Button Handling ---

    public void ArrowClicked ()
    {
        //Debug.Log("Arrow clicked");
        OnArrowClick?.Invoke();
    }
    public void RectangularClicked ()
    {
        OnRectangularClick?.Invoke();
    }


    // --- Particle Effect ---
    public void PlayParticleEffects ()
    {
        foreach(ParticleSystem ps in particleEfects) {
            ps.Play();
        }
    }


}
