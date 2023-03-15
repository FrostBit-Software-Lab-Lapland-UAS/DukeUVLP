/******************************************************************************
 * File        : ClickToTurn.cs
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
 * This script utilises Valve's SteamVR-plugin, which can be downloaded from Unity Asset Store.
 * 
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;


[RequireComponent(typeof(Interactable))]
public class ClickToTurn : MonoBehaviour
{
    Interactable interactable;
    Hand hand;

    public SnapAxis axis;
    public float angleA;
    public float angleB;
    //float currentAngle;
    bool aSelected = true;
    Vector3 initialEulers;

    public Vector3 AxisVector { get { return GetAxis(); } }


    public UnityEvent onTurnA;
    public UnityEvent onTurnB;


    private void Start ()
    {
        interactable = GetComponent<Interactable>();
        initialEulers = transform.eulerAngles;
    }
    private void Update ()
    {
        SetHand();

        if ((SteamVR_Input.GetStateDown("GrabPinch", SteamVR_Input_Sources.Any)/* || SteamVR_Input.GetStateDown("GrabPinch", SteamVR_Input_Sources.LeftHand)*/) && hand != null) 
        {
            Turn();
        }
    }



    private void SetHand ()
    {
        List<Hand> hoveringHands = interactable.hoveringHands;
        if (hoveringHands.Count > 0) {
            hand = hoveringHands[0];
        } else {
            hand = null;
        }
    }


    public void Turn ()
    {
        aSelected = !aSelected;
        transform.eulerAngles = initialEulers + AxisVector * (aSelected ? angleA : angleB);
        if (aSelected)      onTurnA.Invoke();
        else                onTurnB.Invoke();
    }

    Vector3 GetAxis ()
    {
        switch (axis) {
            default:
            case SnapAxis.X:
                return transform.right;
            case SnapAxis.Y:
                return transform.up;
            case SnapAxis.Z:
                return transform.forward;
        }
    }



}
