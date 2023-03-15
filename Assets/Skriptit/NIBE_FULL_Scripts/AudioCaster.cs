/******************************************************************************
 * File        : AudioCaster.cs
 * Version     : 1.0
 * Author      : Petteri Maljam‰ki (petteri.maljamaki@lapinamk.fi)
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

// An audio playback helper script
// PetteriM testing


/// <summary>
/// Use RayCasting to check if Front Hatch is attached to the Air-to-water heating pump, and change the audio accordingly. 
/// </summary>
public class AudioCaster : MonoBehaviour
{
    [Tooltip("Dummy objecto to control pos and direction")]
    public GameObject rayCastOrigin;        // dummy to control the ray origin and direction - blue arrow=forward is used
    
    [Tooltip("Object to be raycast-tested. Must have collider!")]
    public GameObject damperObject;         // this object is raycast-tested (must have collider!)

    public SnappableObject frontHatchSO;
    private Ray audioRay;
    private RaycastHit damperHit;
    private AudioSource audioPlayer;
    private AudioLowPassFilter filtteri;

    [Range(0, 22000)]
    public int cutoffOnFreq = 3500;             // muffled audio frequency 
  
    private int cutoffOffFreq = 22000;          // default frequency - nothing gets filtered, no need to change this value

    [Range(0f, 1f)]
    public float volumeLow;              // volume when occluded (=muffled, behind the door/other object)

    [Range(0f, 1f)]
    public float volumeHi;                 // volume when not occluded

    private float pressureVolumeMultiplier;      // this one sets the amount of change caused by pressure 
    public float blockedVolumeMultiplier;       //  this one controls the amount of dampening, use values under 1 to get effect, e.g. 0.5f
    private bool isBlocked;                     // set during Raycast
    bool panelAttached = true;
    



    void Start()
    {
        // set this to 1 if pressure is not used as controller
        pressureVolumeMultiplier = 1f;
        // using rayCastOrigin to set the position and direction
        audioRay = new Ray(rayCastOrigin.transform.position, rayCastOrigin.transform.forward);
       
        audioPlayer = GetComponent<AudioSource>();
        if (null == audioPlayer) Debug.LogWarning("(AudioCaster): AudioPlayer is null.");

        filtteri = audioPlayer.GetComponent<AudioLowPassFilter>();

        // laitetaan voluumi ja cutoff occluded -tilaan
        audioPlayer.Play();
        audioPlayer.volume = volumeLow;
        filtteri.cutoffFrequency = cutoffOnFreq;

    }
    void FixedUpdate ()
    {
        // jos ray osuu damperObjektiin niin tapahtuu muutos ‰‰ness‰
        // cutoff taajuus muutetaan viel‰ t‰‰ll‰, voisi mietti‰ onko parempikin paikka?
        //Debug.Log("Panel Attached = "+panelAttached);

        if (Physics.Raycast(audioRay, out damperHit)) {
            if (damperHit.transform.name == damperObject.name) {
                isBlocked = true;
                filtteri.cutoffFrequency = cutoffOnFreq;
            }
        } else {
            if(!panelAttached) {
                isBlocked = false;
                filtteri.cutoffFrequency = cutoffOffFreq;
            }

        }
        



        // create final audio level by multiplying with blocked and pressure multipliers
        
        float multipliedVolume =
            volumeHi *
            (isBlocked ? blockedVolumeMultiplier : 1f) *
            pressureVolumeMultiplier *
            SceneInfoManager.Current.volume;

        audioPlayer.volume = multipliedVolume;
        
    }


    private void OnEnable ()
    {
        InteractiveDevicesHandler.PaneeliStateChanged += OnPaneeliStateChanged;
    }
    private void OnDisable ()
    {
        InteractiveDevicesHandler.PaneeliStateChanged -= OnPaneeliStateChanged;
    }


    /// <summary>
    /// Called when Front Hatch changes it's state.
    /// </summary>
    /// <param name="_state"></param>
    /// <param name="_snapArea"></param>
    private void OnPaneeliStateChanged (SnapObjState _state, Transform _snapArea)
    {
        if (null != _snapArea)
            panelAttached = (_state == SnapObjState.Snapped && _snapArea == frontHatchSO.snapAreaList[0]);
    }


    // use this method is you want to change the relation between pressure and multiplier
    // now 1:1 relation which works okay
    public void SetPressureVolumeMultiplier(float _pressure)
    {
        pressureVolumeMultiplier = _pressure;
    }


}
