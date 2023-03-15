/******************************************************************************
 * File        : HighlighPingPong.cs
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
 *****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Make the user aware that this object can be interacted with.
/// </summary>
public class HighlightPingPong : MonoBehaviour {

    bool Playing { get { return null != playCoroutine; } }
    Coroutine playCoroutine = null;
    [SerializeField] List<MeshRenderer> renderers = new List<MeshRenderer>();       // List of all MeshRenderers with a material we want to affect.

    [Header("PingPong Control")]
    public int pingPongCount = -1;
    public float minValue = 0f;
    public float maxValue = 1f;
    public float timeFromMinToMax = 0.5f;




    /// <summary>
    /// Start the PingPong-coroutine if all the conditions are met.
    /// </summary>
    public void Play ()
    {
        if (Playing) {
            Debug.Log("HighlightPingPong - PlayCorountine is not null.");
            return;     // Playing is already active. 
        }

        foreach(MeshRenderer mr in renderers) {
            if (!mr.material.HasProperty("_Value")) {
                Debug.Log("HighlightPingPong - Material does not have the right property.");
                return;     // Current material does not have the property we need to highlight the object. 
            }
        }

        playCoroutine = StartCoroutine(PingPong(pingPongCount, timeFromMinToMax, minValue, maxValue));
    }

    /// <summary>
    /// Stop the currently playing PingPong-coroutine. 
    /// </summary>
    public void Stop ()
    {
        if (!Playing) return;
        StopCoroutine(playCoroutine);
        playCoroutine = null;
        foreach(MeshRenderer mr in renderers) {
            mr.material.SetFloat("_Value", 0f);
        }
    }


    /// <summary>
    /// Move the Value property of the material between two values.
    /// </summary>
    /// <param name="_pingPongCount"></param>
    /// <param name="_timeFromMinToMax"></param>
    /// <param name="_min"></param>
    /// <param name="_max"></param>
    /// <returns></returns>
    private IEnumerator PingPong (int _pingPongCount, float _timeFromMinToMax, float _min = 0f, float _max = 1f)
    {
        //Debug.Log("Starting PingPong(): ppCount=" + _pingPongCount+" timeFromMinToMax="+_timeFromMinToMax);
        bool increacing = true;
        float currentVal = _min;
        _timeFromMinToMax = Mathf.Max(0.05f, _timeFromMinToMax);

        // If _pingPongCount is set to -1 (as it is by default), loop indefinitely.
        // Otherwise reduce the number of counts each time we reach _min value.
        // When _pingPongCount reaches 0 the loop is terminated.
        while ((_pingPongCount > 0 || _pingPongCount == -1)) {

            currentVal += (Time.deltaTime / _timeFromMinToMax * (increacing ? 1 : -1));
            currentVal = Mathf.Clamp(currentVal, _min, _max);

            if (currentVal == _max) {
                increacing = false;
            } else if (currentVal == _min) {
                increacing = true;
                _pingPongCount -= _pingPongCount == -1 ? 0 : 1;
            }

            foreach (MeshRenderer mr in renderers) {
                mr.material.SetFloat("_Value", currentVal);
            }
            //Debug.Log("Yielding when currentVal=" + currentVal + ", increacing=" + increacing);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Debug.Log("PingPong() finished.");
        playCoroutine = null;
    }

}