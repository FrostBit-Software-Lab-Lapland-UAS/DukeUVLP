/******************************************************************************
 * File        : LanguageButtonController.cs
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
using UnityEngine.UI;

/// <summary>
/// Simple handling of language button clicks. Message is sent to SceneInfoManager.
/// </summary>
public class LanguageButtonController : MonoBehaviour
{

    // --- --- --- Variables --- --- ---

    public Button[] languageButtons = new Button[3];




    // --- --- --- Methods --- --- ---

    // --- UnityEvents (Button) ---
    public void FinClicked ()
    {
        SceneInfoManager.Current.SetLanguage(0);
        UpdateLanguageButtons(0);
    }
    public void EngClicked ()
    {
        SceneInfoManager.Current.SetLanguage(1);
        UpdateLanguageButtons(1);
    }
    public void SweClicked ()
    {
        SceneInfoManager.Current.SetLanguage(2);
        UpdateLanguageButtons(2);
    }



    private void Start ()
    {
        UpdateLanguageButtons((int)SceneInfoManager.Current.language);
    }
    private void Update ()
    {
        UpdateLanguageButtons((int)SceneInfoManager.Current.language);

    }

    /// <summary>
    /// Set the scale and color of buttons to indicate which language is currently selected.
    /// </summary>
    /// <param name="_clickedIndex"></param>
    private void UpdateLanguageButtons (int _clickedIndex)
    {
        for (int i = 0; i < languageButtons.Length; i++) {
            if (SceneInfoManager.Current.GetEnabledLanguages()[i]) {
                if (_clickedIndex == i) {
                    languageButtons[i].transform.localScale = Vector3.one * 1.2f;
                    languageButtons[i].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                } else {
                    languageButtons[i].transform.localScale = Vector3.one * 0.8f;
                    languageButtons[i].GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
                }
            } else {
                languageButtons[i].transform.localScale = Vector3.one * 0f;
            }
        }
    }
}
