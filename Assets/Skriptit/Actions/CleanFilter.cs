/******************************************************************************
 * File        : CleanFilter.cs
 * Version     : 1.0
 * Author      : Severi Kangas (severi.kangas@lapinamk.fi), Miika Puljujärvi (miika.puljujarvi@lapinamk.fi)
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
using TMPro;


/// <summary>
/// Changes the material of the Filter if it has been under the faucet for long enough. 
/// </summary>
public class CleanFilter : MonoBehaviour
{

    // --- --- --- Variables --- --- ---

    public ParticleSystem vesiPS;
    public GameObject mutasihti;
    public Renderer mutasihtiRenderer;


    public bool isCleaning = false;
    public bool isClean = false;
    public float timeToClean = 2f;

    public Material cleanMat;
    public Material dirtyMat;
    public Material highlightMat;

    AudioSource waterAudioSource;
    float initialVolume;




    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---
    void Start () {  
        mutasihtiRenderer.enabled = true;
        mutasihtiRenderer.material = dirtyMat;
        waterAudioSource = GetComponent<AudioSource>();
        initialVolume = waterAudioSource.volume;
    }

    /// <summary>
    /// Subtract from timeToClean if the Filter is under the faucet and change the material if the timer runs out (drops to 0). 
    /// </summary>
    private void Update ()
    {
        if(isCleaning) {
            waterAudioSource.volume = SceneInfoManager.Current.volume * initialVolume;
            if (timeToClean > 0f) {
                timeToClean -= Time.deltaTime;
            } else if (!isClean) {
                isClean = true;
                mutasihtiRenderer.sharedMaterial = cleanMat;
            }
        } 
    }

    void OnTriggerEnter(Collider other) {    
        if(other.gameObject.CompareTag("Mutasihti")){
            vesiPS.Play();
            isCleaning = true;
            waterAudioSource.volume = SceneInfoManager.Current.volume * initialVolume;
            waterAudioSource.Play();

        }
    }
    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Mutasihti")) {
            vesiPS.Stop();
            isCleaning = false;
            waterAudioSource.Stop();
        }
    }



    // --- Material Setup ---

    /// <summary>
    /// Return the correct material depending on the cleanliness of the Filter.
    /// </summary>
    /// <returns></returns>
    public Material GetFilterMaterial ()
    {
        if (isClean) return cleanMat;
        return dirtyMat;
    }

    /// <summary>
    /// Set the material of Mutasihti object. 
    /// </summary>
    /// <param name="_highlighted"></param>
    public void SetFilterMaterial (bool _highlighted)
    {
        if(_highlighted)    mutasihti.GetComponentInChildren<MeshRenderer>().material = highlightMat;
        else                mutasihti.GetComponentInChildren<MeshRenderer>().material = GetFilterMaterial();
    }
}

