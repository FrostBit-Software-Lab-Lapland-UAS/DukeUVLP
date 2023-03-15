/******************************************************************************
 * File        : Paineenlisays.cs
 * Version     : 1.0
 * Author      : Petteri Maljam‰ki (petteri.maljamaki@lapinamk.fi), Miika Puljuj‰rvi (miika.puljujarvi@lapinamk.fi)
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
using UnityEngine;

public class Paineenlisays : Subtask
{
    bool paineHeld;
    bool varoHeld;
    public AudioCaster acaster;
    public AudioClip pressureReleaseAudioClip;       // testing one shot
    public AudioClip pressureReleaseClickAudioClip;       // testing one shot
    public AudioSource pressureSound;               // to control the audio pitch
    public AudioSource pressureReleaseSound;        // one shot audio when pressure released
    public GameObject pressureControlUI;
    public Transform pressurePointerObject;         // object to be turned

    public float pressureIncreasePerSecond;         // speed of change
    public float varoventtiiliDecreasePerSecond;    // speed of varoventtiili automatic decrease -> SHOULD BE FASTER (bigger) THAN pressureIncrease!
    public float varoventtiiliActivationPeriod;     // how fast varoventtiili does the pressure drop, e.g. 0.5f
    

    private float minPressure = 0f;                 // bottom limit 
    private float limitPressure = 1.5f;             // when varoventtiili should activate
    private float currentPressure = 0.8f;           // starting pressure
    public TMPro.TextMeshProUGUI HeadsetDebugText;
    private bool varoventtiiliActivated = false;    // bool for coroutine


    // turning 62 degrees is 1 bar in pressure meter
    private float degreesToBar = 62f;
    // default X value for pointer object "BP-5-Pointer" angle when points to 0 bar
    private float pointerZeroRotAngle = 30f;

    // Progress flags
    [Header("Progress Flags and SnappableObject")]
    public SnappableObject frontHatchSO;
    public bool hatchRemoved = false;
    public bool pressureTested = false;


    [Header("Pressure ParticleSystem Settings")]
    public ParticleSystem pressurePS;               // water particles when pressure is released = Varoventtiili activated
    public float pressurePSMultiplier = 0.5f;


    








    private void Start ()
    {
        //Debug.Log("AS: " + pressureReleaseSound.name + " AC: " + pressureReleaseSound.clip.name);
    }
    private void Update ()
    {
        if (!isCurrentlyActiveSubtask) return;


        IncreasePressure();
        VaroventtiiliActivationCheck();
        DecreasePressure();
        //HeadsetDebugText.text = "Paine = " + paineHeld + " Varo = " + varoHeld;




        // set the pointer angle
        pressurePointerObject.localEulerAngles = new Vector3(pointerZeroRotAngle - currentPressure * degreesToBar, 0, 0);
        //Debug.LogWarning("Pointer angle = " + (pointerZeroRotAngle - currentPressure * degreesToBar));

        // set the sound pitch
        pressureSound.pitch = 0.9f + (currentPressure / 8f);
        //pressureSound.volume = 0.2f + (currentPressure / 1f);

        // send the pressure to audiocaster script for processing
        acaster.SetPressureVolumeMultiplier(currentPressure);
    }

    private void OnEnable ()
    {
        AllowInteraction(false);
    }
    private void OnDisable ()
    {

    }



    // subscribe to InteractiveDevicesHandler script

    protected override void SubscribeToEvents()
    {
        InteractiveDevicesHandler.OnPaineHeld += SetPaineHeld;
        InteractiveDevicesHandler.OnVaroHeld += SetVaroHeld;
        InteractiveDevicesHandler.PaneeliStateChanged += OnPaneeliStateChanged;
    }
    protected override void UnsubscribeToEvents()
    {
        InteractiveDevicesHandler.OnPaineHeld -= SetPaineHeld;
        InteractiveDevicesHandler.OnVaroHeld -= SetVaroHeld;
        InteractiveDevicesHandler.PaneeliStateChanged -= OnPaneeliStateChanged;
    }



    protected override void OnBegin ()
    {
        AllowInteraction(true);
    }
    protected override void OnDone ()
    {
        AllowInteraction(false);
        StopAllCoroutines();
    }




    private void SetPaineHeld(bool _held)
    {
        paineHeld = _held;
    }
    private void SetVaroHeld(bool _held)
    {
        varoHeld = _held;
    }


    // changing the pressure
    private void IncreasePressure()
    {
        if (!paineHeld) return;
        currentPressure += pressureIncreasePerSecond * Time.deltaTime;

        UpdateParticleSystemValues();

    }
    private void DecreasePressure()
    {
        if (!varoHeld) return;

        if (varoHeld && currentPressure > minPressure)
        {
            currentPressure -= pressureIncreasePerSecond * Time.deltaTime;
        }

        UpdateParticleSystemValues();
        pressurePS.Play();
    }

    private void UpdateParticleSystemValues ()
    {
        ParticleSystem.MainModule mainModule = pressurePS.main;
        mainModule.startSpeed = currentPressure * pressurePSMultiplier;
        ParticleSystem.EmissionModule emissionModule = pressurePS.emission;
        emissionModule.rateOverTime = currentPressure * ((mainModule.maxParticles / mainModule.startLifetime.constantMax) / limitPressure);
    }


    // when limitPressure is reached, varoventtiili is activated automatically
    // pressure drops 
    private IEnumerator VaroventtiiliAutomaticActivation()
    {
        int id = Random.Range(0, 100000);
        //Debug.Log("Coroutine started ID: " + id);
        PlayReleaseSound();
        float timer = varoventtiiliActivationPeriod;
        
        while (timer > 0)
        {
            if (currentPressure < limitPressure)
            {
             timer -= Time.deltaTime;
            }
            currentPressure -= varoventtiiliDecreasePerSecond * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        varoventtiiliActivated = false; 
        //Debug.Log("Coroutine ended ID: " + id);
    }
    private void VaroventtiiliActivationCheck ()
    {
        if (currentPressure > limitPressure && !varoventtiiliActivated) {
            varoventtiiliActivated = true;
            pressureTested = true;
            ProgressStory(2);
            StartCoroutine(VaroventtiiliAutomaticActivation());
        }
    }
    private void PlayReleaseSound ()
    {

        if (pressureReleaseSound != null) {
            //Debug.Log("‰‰ni toimii!");
            pressureReleaseSound.PlayOneShot(pressureReleaseClickAudioClip, 1.0f);
            pressureReleaseSound.PlayOneShot(pressureReleaseAudioClip, 0.1f);
            pressurePS.Play();
        } else {
            //Debug.Log("Ei toimi ‰‰ni?");
        }

    }


    /// <summary>
    /// Receive state changes of the Front Hatch SnappableObject from InteractiveDevicesHandler.
    /// </summary>
    /// <param name="_state"></param>
    /// <param name="_snapArea"></param>
    private void OnPaneeliStateChanged (SnapObjState _state, Transform _snapArea)
    {

        if (frontHatchSO.isInsideSnapArea) {

            if (_state == SnapObjState.Snapped && _snapArea == frontHatchSO.snapAreaList[0]) {

                hatchRemoved = false;
                currentStoryIndex = pressureTested ? 3 : 0;
                UpdateStoryText();
                pressureControlUI.gameObject.SetActive(false);

            } else if (_state == SnapObjState.Snapped && _snapArea == frontHatchSO.snapAreaList[1]) {
                
                hatchRemoved = true;
                currentStoryIndex = pressureTested ? 2 : 1;
                UpdateStoryText();
                pressureControlUI.gameObject.SetActive(true);

            } else {
                pressureControlUI.gameObject.SetActive(false);
            }
        } else {

            currentStoryIndex = pressureTested ? 2 : 0;
            UpdateStoryText();
            pressureControlUI.gameObject.SetActive(false);

        }
    }




    /// <summary>
    /// Set the story manually. 
    /// </summary>
    /// <param name="_index"></param>
    private void ProgressStory (int _index)
    {
        currentStoryIndex = _index;
        UpdateStoryText();
    }


    private void AllowInteraction (bool _on)
    {
        frontHatchSO.transform.GetComponent<Collider>().enabled = _on;
    }

}
