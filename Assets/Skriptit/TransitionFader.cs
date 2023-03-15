/******************************************************************************
 * File        : TransitionFader.cs
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
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Manages the fades to and from black during the teleportation of the user. 
/// </summary>
public class TransitionFader : MonoBehaviour
{

    public static TransitionFader Current { get; private set; }
    public bool fadeInProgress = false;
    public bool PanelIsActive { get { return fadePanel.gameObject.activeSelf; } }
    public Image fadePanel;




    private void Awake ()
    {
        Current = this;
    }

    public void FadeInBlack (float _duration = 1f, float _delay = 0f)
    {
        StartCoroutine(Fade(true, _duration, _delay));
    }
    public void FadeOutBlack (float _duration = 1f, float _delay = 0f)
    {
        StartCoroutine(Fade(false, _duration, _delay));
    }



    /// <summary>
    /// Fade either to or from black based on parameters provided.
    /// First wait until any possible fades are complete.
    /// Then wait further if the timer has delay. (The duration of delay is not reduced by waiting for other fades to complete. This should probably be fixed.)
    /// Process the transition of the fading panel's alpha between 0 and 1.
    /// </summary>
    /// <param name="_toBlack"></param>
    /// <param name="_duration"></param>
    /// <param name="_delay"></param>
    /// <returns></returns>
    public IEnumerator Fade (bool _toBlack, float _duration, float _delay)
    {
        float timer = _delay;
        while (fadeInProgress) {
            yield return new WaitUntil(() => fadeInProgress == false);
        }
        fadeInProgress = true;

        while (timer >= 0f) {
            timer -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }


        float targetVal = _toBlack ? 1f : 0f;

        if (_toBlack) {
            fadePanel.color = new Color(0f, 0f, 0f, 0f);
            fadePanel.gameObject.SetActive(true);

            while (fadePanel.color.a < targetVal) {
                fadePanel.color = new Color(0f, 0f, 0f, fadePanel.color.a + (Time.deltaTime / _duration));
                yield return new WaitForSeconds(Time.deltaTime);
            }

        } else {
            fadePanel.color = new Color(0f, 0f, 0f, 1f);

            while (fadePanel.color.a > targetVal) {
                fadePanel.color = new Color(0f, 0f, 0f, fadePanel.color.a - (Time.deltaTime / _duration));
                yield return new WaitForSeconds(Time.deltaTime);
            }

            fadePanel.gameObject.SetActive(false);
        }

        fadeInProgress = false;
    }


}
