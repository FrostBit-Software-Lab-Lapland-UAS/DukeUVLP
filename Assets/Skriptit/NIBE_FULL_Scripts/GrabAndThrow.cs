/******************************************************************************
 * File        : GrabAndThrow.cs
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
 * This script utilises Valve's SteamVR-plugin, which can be downloaded from Unity Asset Store.
 * 
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;


/// <summary>
/// Allows the user to grab and throw the object without having to parent it to the Hand.
/// Throwable-script handles this usually, we ran into problems when checking if a held object was inside or outside of a triggerArea's bounds. 
/// /// </summary>
[RequireComponent(typeof(Interactable))]
public class GrabAndThrow : MonoBehaviour {

    // --- --- --- Variables --- --- ---

    public bool interactionEnabled = true;
    private Hand rHand;
    private Hand lHand;
    private Interactable interactable;
    public Hand hand;

    public bool grabbing = false;
    Rigidbody rb;
    private Vector3[] prevPositions;
    private Quaternion[] prevRotations;
    private Vector3 pos, forward, up;
    private Quaternion rotOffset;
    public int prevPositionsCount = 10;
    public int prevRotationsCount = 10;
    public Vector3 angularVelocity;

    [Space(40f)]
    public UnityEvent onHoverBegin;
    public UnityEvent onHoverEnd;




    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---
    private void Start ()
    {
        rb = GetComponent<Rigidbody>();
        prevPositions = new Vector3[prevPositionsCount];
        prevRotations = new Quaternion[prevRotationsCount];
        interactable = GetComponent<Interactable>();
        rHand = GameObject.Find("RightHand").GetComponent<Hand>();
        lHand = GameObject.Find("LeftHand").GetComponent<Hand>();
    }

    /// <summary>
    /// Input handling is done here. 
    /// Start grabbing the object if the user is holding a trigger button down and a Hand is inside the triggerArea of the object.
    /// Otherwise check if a Hand enters the grabbable area.
    /// </summary>
    private void FixedUpdate ()
    {
        if (!interactionEnabled) return;

        UpdatePrevPosAndRot();

        if (hand != null) {
            if (SteamVR_Input.GetState("GrabPinch", SteamVR_Input_Sources.RightHand) && hand == rHand) {
                if (!grabbing) StartGrab();
            } else if (SteamVR_Input.GetState("GrabPinch", SteamVR_Input_Sources.LeftHand) && hand == lHand) {
                if (!grabbing) StartGrab();
            } else {
                if (grabbing) {
                    Throw();
                }
            }
        }
        if (grabbing) {
            UpdatePositionAndRotation();
        } else {
            SetHand();
        }
    }




    // --- Grab and Throw ---

    /// <summary>
    /// Calculate velocities for the object when the user stops holding the trigger button.
    /// </summary>
    private void Throw ()
    {
        grabbing = false;
        rb.isKinematic = false;
        Vector3 throwDirection = (prevPositions[0] - prevPositions[prevPositionsCount - 1]).normalized;
        float throwStrength = Vector3.Distance(prevPositions[0], prevPositions[prevPositionsCount - 1]) / (Time.fixedDeltaTime * prevPositionsCount);
        rb.velocity = throwDirection * throwStrength;
        UpdateAngularVelocity();
        rb.angularVelocity = angularVelocity;
    }

    /// <summary>
    /// Record the positional and rotational offsets when the grabbing starts.
    /// </summary>
    private void StartGrab ()
    {
        grabbing = true;
        rb.isKinematic = true;
        rotOffset = Quaternion.Inverse(hand.transform.rotation) * transform.rotation;
        pos = hand.transform.InverseTransformPoint(transform.position);
        forward = hand.transform.TransformDirection(transform.forward);
        up = hand.transform.TransformDirection(transform.up);
    }

    /// <summary>
    /// Update the object's position and rotation each frame to match the movement of the Hand.
    /// </summary>
    private void UpdatePositionAndRotation ()
    {
        Vector3 newForward = hand.transform.TransformDirection(forward);
        Vector3 newUp = hand.transform.TransformDirection(up);
        transform.position = hand.transform.TransformPoint(pos);
        transform.rotation = hand.transform.rotation * rotOffset;
    }

    /// <summary>
    /// Used for debugging purposes.
    /// </summary>
    private void UpdateAngularVelocity ()
    {
        angularVelocity = GetAngularVelocity(prevRotations[prevRotationsCount - 1], prevRotations[0]);
    }

    /// <summary>
    /// Source: https://forum.unity.com/threads/manually-calculate-angular-velocity-of-gameobject.289462/ - DavidSWu, Mar 10, 2019
    /// Calculate angular velocity with triangle magic.
    /// </summary>
    /// <param name="foreLastFrameRotation"></param>
    /// <param name="lastFrameRotation"></param>
    /// <returns></returns>
    private Vector3 GetAngularVelocity (Quaternion foreLastFrameRotation, Quaternion lastFrameRotation)
    {
        var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);
        // no rotation?
        // You may want to increase this closer to 1 if you want to handle very small rotations.
        // Beware, if it is too close to one your answer will be Nan
        if (Mathf.Abs(q.w) > 1023.5f / 1024.0f)
            return new Vector3(0, 0, 0);
        float gain;
        // handle negatives, we could just flip it but this is faster
        if (q.w < 0.0f) {
            var angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        } else {
            var angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        return new Vector3(q.x * gain, q.y * gain, q.z * gain);
    }

    /// <summary>
    /// Store previous positions and rotations to lists. 
    /// This information is used when calculating velocities on Throw().
    /// </summary>
    private void UpdatePrevPosAndRot ()
    {
        for (int i = prevPositions.Length - 1; i > 0; i--) {
            prevPositions[i] = prevPositions[i - 1];
        }
        prevPositions[0] = transform.position;

        for (int i = prevRotations.Length - 1; i > 0; i--) {
            prevRotations[i] = prevRotations[i - 1];
        }
        prevRotations[0] = transform.localRotation;
    }

    /// <summary>
    /// Check if a hand is hovering over the object and set is as the Hand that can grab the object.
    /// </summary>
    private void SetHand ()
    {
        List<Hand> hoveringHands = interactable.hoveringHands;
        if (hoveringHands.Count > 0) {
            hand = hoveringHands[0];
        } else {
            hand = null;
        }
    }

    private void OnHandHoverBegin ()
    {
        if (interactionEnabled) onHoverBegin.Invoke();
    }

    private void OnHandHoverEnd ()
    {
        if (interactionEnabled) onHoverEnd.Invoke();
    }

}
