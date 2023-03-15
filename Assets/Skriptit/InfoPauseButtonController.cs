/******************************************************************************
 * File        : InfoPauseButtonController.cs
 * Version     : 1.0
 * Author      : Miika Puljuj�rvi (miika.puljujarvi@lapinamk.fi)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;


/// <summary>
/// Handling of info and menu buttons. 
/// </summary>
public class InfoPauseButtonController : MonoBehaviour
{
    
    public Button infoButton;
    public Button pauseButton;
    public float highlightScale;

    Hand rightHand;
    Hand leftHand;



    private void Start ()
    {   
        foreach (WhatsThisLabel wt in FindObjectsOfType<WhatsThisLabel>()) {
            wt.SetCanvasVisibility();
        }
        UpdateButtonSize(infoButton, SceneInfoManager.Current.InfoLabelsVisible);
        UpdateButtonSize(pauseButton, SceneInfoManager.Current.paused);

        Hand[] hands = FindObjectsOfType<Hand>(true);
        foreach (Hand hand in hands)
        {
            if (hand.handType == Valve.VR.SteamVR_Input_Sources.RightHand)
            {
                rightHand = hand;
            }
            else if (hand.handType == Valve.VR.SteamVR_Input_Sources.RightHand)
            {
                leftHand = hand;
            }
        }
    }


    private void OnEnable ()
    {
        SceneInfoManager.OnPause += PauseToggled;

    }
    private void OnDisable ()
    {
        SceneInfoManager.OnPause -= PauseToggled;
    }


    public void InfoClicked ()
    {
        SceneInfoManager.Current.InfoLabelsVisible = !SceneInfoManager.Current.InfoLabelsVisible;   // Toggle the boolean here instead of in the loop.
        foreach (WhatsThisLabel wt in FindObjectsOfType<WhatsThisLabel>()) {
            wt.SetCanvasVisibility();
        }
        UpdateButtonSize(infoButton, SceneInfoManager.Current.InfoLabelsVisible);
    }
    public void PauseClicked ()
    {
        FindObjectOfType<PauseMenu>().ResumePressed();
        UpdateButtonSize(pauseButton, SceneInfoManager.Current.paused);
    }

    void PauseToggled (bool _paused)
    {
        UpdateButtonSize(pauseButton, _paused);
    }

    void UpdateButtonSize (Button _btn, bool _highlight)
    {
        _btn.transform.localScale = Vector3.one * ((_highlight) ? highlightScale : 1f);
    }
}
