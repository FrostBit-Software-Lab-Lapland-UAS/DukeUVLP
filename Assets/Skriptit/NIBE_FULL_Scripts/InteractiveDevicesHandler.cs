/******************************************************************************
 * File        : InteractiveDevicesHandler.cs
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
 * This script utilises Valve's SteamVR-plugin, which can be downloaded from Unity Asset Store.
 * 
 *****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;


/// <summary>
/// Collect state and input data from various interactable objects in the scene and send information to Subtasks that have subscribed to corresponding events.
/// </summary>
public class InteractiveDevicesHandler : MonoBehaviour
{
    // --- --- --- Variables --- --- ---

    public static InteractiveDevicesHandler current;
    public CircularDrive virtakytkinCD;
    public Button paineenlisaysButton;
    public Button varoventtiiliButton;
    public CircularDrive mutasihtiCD;
    public SnappableObject mutasihtiSO;
    public SnappableObject etupaneeliSO;
    private Transform pointedObject;




    // --- --- --- Delegates and Events --- --- ---

    public delegate void OnVirtakytkinTurned(bool _on);
    public static event OnVirtakytkinTurned OnVirtaTurned; 
    
    public delegate void OnMutavipuTurned(bool _on);
    public static event OnMutavipuTurned OnMutaTurned;

    public delegate void OnPaineenlisaysHeld(bool _held);
    public static event OnPaineenlisaysHeld OnPaineHeld;

    public delegate void OnVaroventtiiliHeld(bool _held);
    public static event OnVaroventtiiliHeld OnVaroHeld;

    public delegate void OnPaneeliStateChange(SnapObjState _state, Transform _snapArea);
    public static event OnPaneeliStateChange PaneeliStateChanged;

    public delegate void OnMutasihtiStateChange(SnapObjState _state, Transform _snapArea);
    public static event OnMutasihtiStateChange MutasihtiStateChanged;





    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour methods ---

    private void Start()
    {
        current = this;
    }
    private void OnEnable()
    {
        PointerController.OnEnter += PointingOnElementStarted;
        PointerController.OnExit += PointingOnElementEnded;
    } 
    private void OnDisable()
    {
        PointerController.OnEnter -= PointingOnElementStarted;
        PointerController.OnExit -= PointingOnElementEnded;
    }
    private void Update()
    {
        if(null != pointedObject)
        {                
            if(SteamVR_Input.GetState("GrabPinch", SteamVR_Input_Sources.Any)) {
                ValidateHeldTarget(true);
            } else {
                ValidateHeldTarget(false);
            }
        } else {
            PaineenlisaysHeldEnded();
            VaroventtiiliHeldEnded();
        }
    }




    // --- Event-related methods ---

    /// <summary>
    /// Circular Drives can be either the Safety Switch or Filter Lever. Check which one is sending the information and pass it on.
    /// </summary>
    /// <param name="_cd"></param>
    public void CircularDriveTurnedOn (CircularDrive _cd)
    {
        if (_cd == virtakytkinCD)
        {
            //Debug.Log("Virtakytkin ON (CircularDriveOn)");
            OnVirtaTurned?.Invoke(true);
        }
       else if (_cd == mutasihtiCD)
        {
            OnMutaTurned?.Invoke(true);
        }

    }

    /// <summary>
    /// Circular Drives can be either the Safety Switch or Filter Lever. Check which one is sending the information and pass it on.
    /// </summary>
    public void CircularDriveTurnedOff (CircularDrive _cd)
    {
        if (_cd == virtakytkinCD)
        {
            //Debug.Log("Virtakytkin OFF (CircularDriveOn)");
            OnVirtaTurned?.Invoke(false);
        }
        else if (_cd == mutasihtiCD)
        {
            OnMutaTurned?.Invoke(false);
        }

    }

    /// <summary>
    /// Alternative way to turn Safety Switch or other interactable rotating objects is to use ClickToTurn.
    /// </summary>
    /// <param name="_ctt"></param>
    public void ClickToTurn (bool _on)
    {
        OnVirtaTurned?.Invoke(_on);
    }



    /// <summary>
    /// This script subscribes to PointerController's OnEnter event at OnEnable. Record the object currently being pointed.
    /// </summary>
    /// <param name="_t"></param>
    public void PointingOnElementStarted (Transform _t)
    {
        pointedObject = _t;
    }

    /// <summary>
    /// Clear pointedObject if the pointing ends. 
    /// </summary>
    public void PointingOnElementEnded (Transform _t)
    {
        pointedObject = null;
    }


    /// <summary>
    /// Check which button (paineenlisays or varoventtiili) the user is currently pointing at.
    /// </summary>
    /// <param name="_held"></param>
    private void ValidateHeldTarget (bool _held)
    {
        if(_held) {
            if (pointedObject == paineenlisaysButton.transform) {
                PaineenlisaysHeldStarted();
                VaroventtiiliHeldEnded();
            } else if (pointedObject == varoventtiiliButton.transform) {
                VaroventtiiliHeldStarted();
                PaineenlisaysHeldEnded();
            }
        } else {
            if (pointedObject == paineenlisaysButton.transform) {
                PaineenlisaysHeldEnded();
            } else if (pointedObject == varoventtiiliButton.transform) {
                VaroventtiiliHeldEnded();
            }
        }
    }


    public void PaineenlisaysHeldStarted ()
    {
        OnPaineHeld?.Invoke(true);
    }
    public void PaineenlisaysHeldEnded()
    {
        OnPaineHeld?.Invoke(false);
    }
    public void VaroventtiiliHeldStarted()
    {
        OnVaroHeld?.Invoke(true);
    }
    public void VaroventtiiliHeldEnded()
    {
        OnVaroHeld?.Invoke(false);
    }


    /// <summary>
    /// Check the state of SnappableObjects and pass information onward.
    /// Currently two SnappableObjects (FIlter and FrontHatch) are being handled.
    /// </summary>
    /// <param name="_so"></param>
    /// <param name="_state"></param>
    /// <param name="_snapArea"></param>
    public void SnappableObjectStateChanged (SnappableObject _so, SnapObjState _state, Transform _snapArea)
    {
        if(_so == etupaneeliSO) {
            PaneeliStateChanged?.Invoke(_state, _snapArea);
        } else if (_so == mutasihtiSO) {
            MutasihtiStateChanged?.Invoke(_state, _snapArea);
        }
    }



}
