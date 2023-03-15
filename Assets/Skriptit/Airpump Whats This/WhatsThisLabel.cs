/******************************************************************************
 * File        : WhatsThisLabel.cs
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

using UnityEngine;

public enum SceneInUse { Airpump, Nibe };


/// <summary>
/// Display the name of an object (set in-engine, not automatically).
/// The script is to be used with WhatsThis-prefab. Attach the prefab to an object if you want the label to move with it.
/// By default, te label can be shown by holding the HTC Vive controller's TPad "down". 
/// </summary>
public class WhatsThisLabel : MonoBehaviour
{

    // --- --- --- --- --- Variables --- --- --- --- ---

    public SceneInUse sceneInUse;
    public string infoTextFin;                  // Object name that will be displayed on the panel.
    public string infoTextEng;                  
    public string infoTextSwe;
    public float fontSize = 30f;
    public float textPadding = 10f;
    public bool lookAtPlayer = true;            // Does the label face player?
    public TMPro.TextMeshProUGUI labelText;     // Text object which displays the name of the object.
    private bool infoPressed;                   // Is the info button pressed?
    private bool objVisible;                    // Is the canvas visible?




    // --- --- --- --- --- Methods --- --- --- --- ---

    // --- MonoBehaviour ---
    void Start()
    {
        SetText(SceneInfoManager.Current.language);
        SetCanvasVisibility();
    }
    void Update ()
    {
        if (lookAtPlayer)   transform.LookAt(Camera.main.transform);

    }


    private void OnEnable()
    {
        SceneInfoManager.OnLangChange += SetText;
    }
    private void OnDisable()
    {
        SceneInfoManager.OnLangChange -= SetText;
    }




    // --- Label ---

    /// <summary>
    ///  Toggle the panel (or rather the parent canvas) visibility. 
    /// </summary>
    /// <param name="_visible"></param>
    public void SetCanvasVisibility ()
    {
        bool visible = SceneInfoManager.Current.InfoLabelsVisible;
        transform.GetChild(0).gameObject.SetActive(visible);
        objVisible = visible;
    }

    /// <summary>
    /// Set the text with proper language and scale the background object to fit the text.
    /// </summary>
    /// <param name="_lang"></param>
    private void SetText (Language _lang)
    {
        labelText.text = GetTextByLanguage(_lang);
        RectTransform parentRect = (RectTransform)labelText.transform.parent;
        Vector2 prefValues = labelText.GetPreferredValues();
        parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, prefValues.x + textPadding);
        parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, prefValues.y + textPadding);
    }

    /// <summary>
    /// Get the text in the correct language.
    /// </summary>
    /// <param name="_lang"></param>
    /// <returns></returns>
    private string GetTextByLanguage (Language _lang)
    {
        switch (_lang) {

            default:
            case Language.Finnish:
                return infoTextFin;

            case Language.English:
                return infoTextEng;

            case Language.Swedish:
                return infoTextSwe;
        }
    }




}
