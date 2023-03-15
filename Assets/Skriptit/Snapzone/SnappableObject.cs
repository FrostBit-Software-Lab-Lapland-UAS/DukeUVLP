/******************************************************************************
 * File        : SnappableObject.cs
 * Version     : 1.0
 * Author      : Severi Kangas (severi.kangas@lapinamk.com), Miika Puljujärvi (miika.puljujarvi@lapinamk.fi)
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
using Valve.VR.InteractionSystem;



public enum SnapObjState { Snapped, Held, Free };


/// <summary>
/// Handles the gameObject's snapping to predetermined SnapAreas.
/// </summary>
public class SnappableObject : MonoBehaviour
{

    // --- --- --- Variables --- --- ---


    public SnapObjState state = SnapObjState.Snapped;                       // Current state for this SnappableObject.
    SnapObjState prevState;
     public bool allowSnapping = true; 
     public bool isSnapped = true;                         
     public bool isInsideSnapArea = true;                  
     public bool lerpInProgress = false;                   
    public float lerpDuration = 1f;           
    public List<Transform> snapAreaList = new List<Transform>();            // Position and rotation for the object inside snapCollider.
    public Transform currentSnapArea;
    Hand[] hands;
    [SerializeField] List<Collider> overlappingSnapAreas = new List<Collider>();


    // --- Bounds and error control ---

    Vector3 startPosition;
    Quaternion startRotation;
    public bool visualizeBounds = true;
    public Transform[] boundObjs = new Transform[2]; 
    Vector3[] bounds = new Vector3[] {                                      // Bounds are the minimum and maximum coordinates of the play area.
        new Vector3(-2.25f, 0f, -2.25f),
        new Vector3(0.9f, 2.8f, 1.9f)
    };



    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---

    private void Start() {
        hands = FindObjectsOfType<Hand>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }
    private void FixedUpdate()
    {
        if(!lerpInProgress) UpdateState();
        CheckBounds();
    }



    private void OnTriggerEnter (Collider other)
    {
        if (!BelongsToSnapAreasList(other)) return;

        if (!overlappingSnapAreas.Contains(other))
            overlappingSnapAreas.Add(other);

        isInsideSnapArea = true;        
    }

    private void OnTriggerExit (Collider other)
    {
        if (!BelongsToSnapAreasList(other)) return;

        if(overlappingSnapAreas.Contains(other)) {
            overlappingSnapAreas.Remove(other);
            if (overlappingSnapAreas.Count == 0) isInsideSnapArea = false;
        }
    }


    /// <summary>
    /// Draw Gizmos if visualizeBounds is true.
    /// </summary>
    private void OnDrawGizmos ()
    {
        if(visualizeBounds) {
            if(null != boundObjs[0] && null != bounds[1]) {
                Gizmos.color = Color.red;
                Vector3 center = (boundObjs[0].position + boundObjs[1].position) / 2f;
                Vector3 scale = new Vector3(
                    boundObjs[0].position.x - boundObjs[1].position.x,
                    boundObjs[0].position.y - boundObjs[1].position.y,
                    boundObjs[0].position.z - boundObjs[1].position.z);
                Gizmos.DrawCube(center, scale);
            }
        }
    }


    // --- SnappableObject Control ---

    /// <summary>
    /// Udate the state of the snappableObject between Free, Held and Snapped.
    /// When the state changes, an event is sent to InteractiveDevicesHandler.
    /// </summary>
    private void UpdateState ()
    {
        prevState = state;

        if(AttachedToHand()) {
            state = SnapObjState.Held;
            isSnapped = false;
            StopCoroutine("LerpToPlace");
        } else if (isInsideSnapArea && allowSnapping) {
            state = SnapObjState.Snapped;
            isSnapped = true;
            if (state != prevState) {
                currentSnapArea = overlappingSnapAreas[overlappingSnapAreas.Count - 1].transform;
                StartCoroutine(LerpToPlace(currentSnapArea, lerpDuration));
            }
        } else {
            state = SnapObjState.Free;
            isSnapped = false;
        }

        if (prevState != state) InteractiveDevicesHandler.current.SnappableObjectStateChanged(this, state, currentSnapArea);
    }

    /// <summary>
    /// If the object is misplaced for some reason or ends up outside of the room, it is teleported back to it's original position.
    /// </summary>
    private void CheckBounds ()
    {
        if(transform.position.x > bounds[0].x && transform.position.x < bounds[1].x) {
            if (transform.position.y > bounds[0].y && transform.position.y < bounds[1].y) {
                if (transform.position.z > bounds[0].z && transform.position.z < bounds[1].z) {
                    return;
                }
            }    
        }

        transform.position = startPosition;
        transform.rotation = startRotation;
    }

    /// <summary>
    /// Boolean check on if a collider provided in here is equal to any of the SnapAreas.
    /// </summary>
    /// <param name="_col"></param>
    /// <returns></returns>
    private bool BelongsToSnapAreasList (Collider _col)
    {
        return snapAreaList.Contains(_col.transform);
    }

    /// <summary>
    /// Boolean check on if the object is currently being held (attached to) a Hand (SteamVR).
    /// </summary>
    /// <returns></returns>
    private bool AttachedToHand ()
    {
        for(int i = 0; i < hands.Length; i++) {
            if(hands[i].AttachedObjects.Count > 0) {
                foreach (Hand.AttachedObject _ao in hands[i].AttachedObjects) {
                    if (_ao.attachedObject == this.gameObject) return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Coroutine responsible for moving and rotating the object to Snapped-position. 
    /// </summary>
    /// <param name="_t"></param>
    /// <param name="_lerpDuration"></param>
    /// <returns></returns>
    private IEnumerator LerpToPlace (Transform _t, float _lerpDuration) 
    {
        float lerpVal = 0f;
        while (lerpVal < 1f) {
            lerpVal += Time.fixedDeltaTime / _lerpDuration;
            transform.position = Vector3.Lerp(transform.position, _t.position, lerpVal);
            transform.rotation = Quaternion.Lerp(transform.rotation, _t.rotation, lerpVal);
            if (state == SnapObjState.Held) lerpVal = 1f;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        lerpInProgress = false;
    }

}
