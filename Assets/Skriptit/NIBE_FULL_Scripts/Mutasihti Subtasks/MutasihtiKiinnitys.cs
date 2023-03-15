/******************************************************************************
 * File        : MutasihtiKiinnitys.cs
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
/// A Subtask that handles the third part of Filter Task.
/// </summary>
public class MutasihtiKiinnitys : Subtask
{

    // --- --- --- Variables --- --- ---

    public Collider sprayCollider;
    private Transform targetLocation;
    private ParticleSystem sprayPs;
    private ParticleSystem[] effectParticleSystems;
    public Transform filterTransform;
    private SnappableObject filterSnapObj;
    private IsInsideTrigger filterInsideInspectArea;
    public SwitchState leverState;
    public SnapObjState filterState = SnapObjState.Snapped;
    public SubtaskStory errorStory;
    public AudioSource waterAudioSource;
    private bool lerpToTargetPos = false;





    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---
    private void Start ()
    {
        filterSnapObj = filterTransform.GetComponent<SnappableObject>();
        filterInsideInspectArea = filterTransform.GetComponent<IsInsideTrigger>();
        targetLocation = sprayCollider.transform.GetChild(0).transform;
        effectParticleSystems = sprayCollider.GetComponentsInChildren<ParticleSystem>();
        filterSnapObj.allowSnapping = true;
        leverState = SwitchState.Off;
        PlayParticleSystems(false);
        waterAudioSource.volume = SceneInfoManager.Current.volume;
    }
    private void Update ()
    {
        if (!isCurrentlyActiveSubtask) return;

        if (lerpToTargetPos && !filterSnapObj.lerpInProgress) {
            filterTransform.position = Vector3.Lerp(filterTransform.position, targetLocation.position, 0.1f);
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
    /// Turn the colliders of Filter and Lever off to prevent the user from manipulating them outside of Filter Task.
    /// </summary>
    protected override void OnBegin ()
    {
        SetCollidersEnabled(true, true);


    }
    protected override void OnDone ()
    {
        SetCollidersEnabled(false, false);
    }
    protected override void OnAfterMoving ()
    {
        if (filterSnapObj.state != SnapObjState.Held) {
            filterSnapObj.transform.GetComponent<TeleportToPlace>().TeleportToPosition(0);
        }
    }


    // --- State Control ---

    private void MutasihtiStateChanged (SnapObjState _state, Transform _snapArea)
    {
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

        if (leverState == SwitchState.Off) {
            PlayParticleSystems(false);
            waterAudioSource.Stop();
            filterSnapObj.allowSnapping = true;
            lerpToTargetPos = false;
            if (filterState == SnapObjState.Snapped) {
                ShowStory(1);
                currentStoryIndex = 1;
            } else {
                ShowStory(0);
                currentStoryIndex = 0;
            }
        } else {
            if (filterState == SnapObjState.Snapped) {
                ShowStory(2);
                currentStoryIndex = 2;
                lerpToTargetPos = false;
                PlayParticleSystems(false);
                waterAudioSource.Stop();
            } else {
                if (!filterSnapObj.lerpInProgress) {
                    ShowStory(errorStory);
                    currentStoryIndex = 0;
                    filterSnapObj.allowSnapping = false;
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
    /// Toggle the particle effects on/off.
    /// </summary>
    /// <param name="_play"></param>
    private void PlayParticleSystems (bool _play)
    {
        foreach (ParticleSystem ps in effectParticleSystems) {
            if (_play) ps.Play();
            else ps.Stop();
        }
    }


    /// <summary>
    /// Set the enabled-state of colliders.
    /// Used in OnDone method to prevent user from manipulating the objects later.
    /// </summary>
    /// <param name="_enabled"></param>
    private void SetCollidersEnabled (bool _filterEnabled, bool _leverEnabled)
    {

        filterSnapObj.GetComponent<Collider>().enabled = _filterEnabled;
        GameObject.Find("Mutasihti-Handle").GetComponent<BoxCollider>().enabled = _leverEnabled;

    }
}
