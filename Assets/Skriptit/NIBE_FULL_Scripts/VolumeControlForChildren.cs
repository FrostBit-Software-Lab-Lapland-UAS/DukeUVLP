/******************************************************************************
 * File        : VolumeChangeForChildren.cs
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
/// Adjust volume for each AudioSource component in children.
/// </summary>
public class VolumeControlForChildren : MonoBehaviour
{

    AudioSource[] sources;          // All AudioSources attached to this transform's child objects.
    float[] initialVolumes;         // Array of volume levels that corresponds to sources-array's AudioSources.




    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---

    /// <summary>
    /// Start
    /// </summary>
    private void Start ()
    {
        sources = GetComponentsInChildren<AudioSource>();
        initialVolumes = new float[sources.Length];
        for(int i = 0; i < sources.Length; i++) {
            initialVolumes[i] = sources[i].volume;
            sources[i].volume = initialVolumes[i] * SceneInfoManager.Current.volume;
        }
    }

    /// <summary>
    /// OnEnable
    /// </summary>
    private void OnEnable ()
    {
        SceneInfoManager.OnVolChange += OnVolumeChange;
    }

    /// <summary>
    /// OnDisable
    /// </summary>
    private void OnDisable ()
    {
        SceneInfoManager.OnVolChange -= OnVolumeChange;
    }




    // --- Volume Change ---

    /// <summary>
    /// Receive current volume from SceneInfoManager and multiply it with the initial volume level.
    /// </summary>
    /// <param name="_vol"></param>
    private void OnVolumeChange (float _vol)
    {
        for(int i = 0; i < sources.Length; i++) {
            sources[i].volume = initialVolumes[i] * _vol;
        }
    }



}
