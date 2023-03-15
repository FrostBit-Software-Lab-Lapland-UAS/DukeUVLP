/******************************************************************************
 * File        : MutasihtiIrrotus.cs
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A Subtask that handles the first part of Filter Task.
/// </summary>
public class MutasihtiIrrotus : Subtask
{

    // --- --- --- Variables --- --- ---

    public Collider sprayCollider;
    private Transform targetLocation;
    private ParticleSystem[] effectParticleSystems;
    public Transform filterTransform;
    public Collider inspectAreaTrigger;
    private SnappableObject filterSnapObj;
    private IsInsideTrigger filterInsideInspectArea;
    public SwitchState leverState = SwitchState.On;
    public SnapObjState filterState = SnapObjState.Snapped;
    public SubtaskStory errorStory;
    public AudioSource waterAudioSource;
    private bool lerpToTargetPos = false;
    private bool readyToInspect = false;




    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---

    private void Start ()
    {
        filterSnapObj = filterTransform.GetComponent<SnappableObject>();
        filterInsideInspectArea = filterTransform.GetComponent<IsInsideTrigger>();
        targetLocation = sprayCollider.transform.GetChild(0).transform;
        effectParticleSystems = sprayCollider.GetComponentsInChildren<ParticleSystem>();
        filterSnapObj.allowSnapping = true;
        PlayParticleSystems(false);
        waterAudioSource.volume = SceneInfoManager.Current.volume;

    }
    private void Update ()
    {
        if (!isCurrentlyActiveSubtask) return;

        if(lerpToTargetPos) {
            if (filterState == SnapObjState.Held) {
                filterTransform.position = Vector3.Lerp(filterTransform.position, targetLocation.position, 0f);
            } else {
                filterTransform.position = Vector3.Lerp(filterTransform.position, targetLocation.position, 0.1f);
            }
        }
        
        if(readyToInspect) {
            CheckInspectArea();
        }
    }




    // --- Events Sub and Unsub ---

    protected override void SubscribeToEvents ()
    {
        InteractiveDevicesHandler.MutasihtiStateChanged += MutasihtiStateChanged;
        InteractiveDevicesHandler.OnMutaTurned += VipuTurned;
        SceneInfoManager.OnVolChange += VolumeChanged;
    }
    protected override void UnsubscribeToEvents ()
    {
        InteractiveDevicesHandler.MutasihtiStateChanged -= MutasihtiStateChanged;
        InteractiveDevicesHandler.OnMutaTurned -= VipuTurned;
        SceneInfoManager.OnVolChange -= VolumeChanged;
    }

    /// <summary>
    /// Turn the colliders of Filter and Lever on to allow the user to manipulate them.
    /// </summary>
    protected override void OnBegin ()
    {
        SetCollidersEnabled(true, true);
    }
    protected override void OnDone ()
    {
        SetCollidersEnabled(true, false);
    }




    // --- State Control ---

    private void MutasihtiStateChanged (SnapObjState _state, Transform _snapArea)
    {
        //Debug.Log("Updated Mutasihti state to " + _state);
        filterState = _state;
        StatesChanged();
    }
    private void VipuTurned (bool _open)
    {
        leverState = _open ? SwitchState.On : SwitchState.Off;
        StatesChanged();
    }

    /// <summary>
    /// Main logic of the Subtask is set here.
    /// Set the correct SubtaskStory based on the states of leverState (Lever) and snapObjState (Filter).
    /// </summary>
    private void StatesChanged ()
    {
        //filterState = filterSnapObj.state;

        if (leverState == SwitchState.Off) {
            PlayParticleSystems(false);
            waterAudioSource.Stop();
            filterSnapObj.allowSnapping = true;
            lerpToTargetPos = false;

            if (filterState == SnapObjState.Snapped) {
                ShowStory(1);
                currentStoryIndex = 1;
                readyToInspect = false;
            } else {
                ShowStory(2);
                currentStoryIndex = 3;
                readyToInspect = true;
            }
        } else {
            readyToInspect = false;
            if (filterState == SnapObjState.Snapped) {
                ShowStory(0);
                currentStoryIndex = 0;
                lerpToTargetPos = false;
                PlayParticleSystems(false);
                waterAudioSource.Stop();
            } else {
                if (!filterSnapObj.lerpInProgress) {
                    ShowStory(errorStory);
                    currentStoryIndex = 1;
                    filterSnapObj.allowSnapping = false;
                    //filterSnapObj.state = SnapObjState.Free;
                    lerpToTargetPos = true;
                    PlayParticleSystems(true);
                    waterAudioSource.Play();
                }
            }
        }
    }

    /// <summary>
    /// Receive volume change events to update the waterAudioSource component.
    /// </summary>
    /// <param name="_vol"></param>
    private void VolumeChanged (float _vol)
    {
        waterAudioSource.volume = _vol;
    }

    /// <summary>
    /// Check if Filter has been brought inside a trigger area defined in IsInsideTrigger-class.
    /// If player has detached Filter correctly and has brought it close to their face, this Subtask is complete.
    /// </summary>
    private void CheckInspectArea ()
    {
        if (filterInsideInspectArea.isInsideTrigger) ShowNextStory();
    }

    /// <summary>
    /// Toggle the particle effects on/off.
    /// </summary>
    private void PlayParticleSystems (bool _play)
    {
        foreach (ParticleSystem ps in effectParticleSystems) {
            if (_play)  ps.Play();
            else        ps.Stop();
        }
    }

    /// <summary>
    /// Set the enabled-state of colliders.
    /// Used in OnBegin method to to allow user to manipulate the objects.
    /// </summary>
    private void SetCollidersEnabled (bool _filterEnabled, bool _leverEnabled)
    {

        filterSnapObj.GetComponent<Collider>().enabled = _filterEnabled;
        GameObject.Find("Mutasihti-Handle").GetComponent<BoxCollider>().enabled = _leverEnabled;

    }
}
