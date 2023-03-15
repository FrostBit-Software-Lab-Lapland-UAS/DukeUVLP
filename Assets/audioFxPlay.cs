/******************************************************************************
 * File        : audioFxPlay.cs
 * Version     : 1.0
 * Author      : Petteri Maljamäki (petteri.maljamaki@lapinamk.fi)
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
/// Collection of methods to play audio at predetermined moments during animation.
/// </summary>
public class audioFxPlay : MonoBehaviour
{

    // --- --- --- Variables --- --- ---

    public AudioSource audioplayer;
    public AudioClip slideattach;
    public AudioClip handleturn;
    public AudioClip metalcover;
    public AudioClip audioclip;
    public AudioClip audioclick;



    // --- --- --- Methods --- --- ---

    // --- Animation-specific Methods ---

    public void PlayFXAudioSlide()
    {
        Debug.Log("Audio playing?");
        audioplayer.PlayOneShot(slideattach, 0.8f*SceneInfoManager.Current.volume);
    }

    public void PlayFXAudioHandle()
    {
        Debug.Log("Audio playing?");
        audioplayer.PlayOneShot(handleturn, 0.3f * SceneInfoManager.Current.volume);
    }

    public void PlayFXAudioCover()
    {
        Debug.Log("Audio playing?");
        audioplayer.PlayOneShot(metalcover, 1f * SceneInfoManager.Current.volume);
    }
    public void PlayFXAudioClick()
    {
        Debug.Log("Audio playing?");
        audioplayer.PlayOneShot(audioclick, 1f * SceneInfoManager.Current.volume);
    }
    public void PlayFXAudioOther()
    {
        Debug.Log("Audio playing?");
        audioplayer.PlayOneShot(audioclip, 1f * SceneInfoManager.Current.volume);
    }
}

  
