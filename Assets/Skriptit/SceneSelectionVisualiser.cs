/******************************************************************************
 * File        : SceneSelectionVisualiser.cs
 * Version     : 1.0
 * Author      : Miika Puljujärvi (miika.puljujarvi@lapinamk.fi), Petteri Maljamäki (petteri.maljamaki@lapinamk.fi)
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


/// <summary>
/// Let player select the scene they want to play by pointing at an object.
/// Highlight the selection by playing particle effect when player points at the object, and confirm the selection (and start transition) when they press the trigger button.
/// </summary>
public class SceneSelectionVisualiser : MonoBehaviour
{
    private PointerReader pReader;
    private ParticleSystem selectPS;
    private Light selectLight;
    public GameObject textObjectParent;
    private bool highlighted = false;

    public TMPro.TextMeshProUGUI nameTextObj;
    public string[] text = new string[3];

    [Header("Scene selection settings")]
    public int sceneIndex;
    public SelectedTaskList selectedTaskList;

    private AudioSource audioPlayer;
    public AudioClip selectionAudioClip;




    private void Awake ()
    {
        pReader = GetComponent<PointerReader>();
        selectPS = GetComponentInChildren<ParticleSystem>();
        selectLight = GetComponentInChildren<Light>();
    }
    private void OnEnable ()
    {
        PointerController.OnEnter += PointingStarted;
        PointerController.OnExit += PointingEnded;
        PointerController.OnClicked += Clicked;
        SceneInfoManager.OnLangChange += UpdateText;
    }
    private void OnDisable ()
    {
        PointerController.OnEnter -= PointingStarted;
        PointerController.OnExit -= PointingEnded;
        PointerController.OnClicked -= Clicked;
        SceneInfoManager.OnLangChange -= UpdateText;
    }

    /// <summary>
    /// Called when a SteamVR_LaserPointer (or PointerController) starts pointing on this object.
    /// </summary>
    /// <param name="_t"></param>
    public void PointingStarted (Transform _t)
    {
        if (transform == _t)    ToggleSelection(true);
        else                    ToggleSelection(false);
    }

    /// <summary>
    /// Called when a SteamVR_LaserPointer (or PointerController) stops pointing on this object.
    /// </summary>
    public void PointingEnded (Transform _t)
    {
        ToggleSelection(false);
    }

    /// <summary>
    /// Called when a SteamVR_LaserPointer (or PointerController) currently pointing at this object registers a trigger button activation.
    /// </summary>
    public void Clicked (Transform _t)
    {
        if(transform == _t) {
            SceneInfoManager.Current.nibeFullSelectedTask = selectedTaskList;
            SceneInfoManager.Current.GoToScene(sceneIndex);
            AudioPlaySelection();
        }
    }


    /// <summary>
    /// Mark this instance as selected. Play the particle system and set light as active.
    /// </summary>
    /// <param name="_selected"></param>
    private void ToggleSelection (bool _selected)
    {
        highlighted = _selected;
        selectLight.enabled =_selected;
        textObjectParent.SetActive(_selected);
        UpdateText(SceneInfoManager.Current.language);
       

        if (_selected)      selectPS.Play();
        else                selectPS.Stop();     
    }

    /// <summary>
    /// Update the name with the correct language. 
    /// </summary>
    /// <param name="_lang"></param>
    private void UpdateText (Language _lang)
    {
        nameTextObj.text = text[(int)_lang];
    }

    private void AudioPlaySelection()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.PlayOneShot(selectionAudioClip, SceneInfoManager.Current.volume);
    }


}
