/******************************************************************************
 * File        : SettingsMenu.cs
 * Version     : 0.6 aplha
 * Author      : Severi Kangas (severi.kangas@lapinamk.com)
 * Copyright   : Lapland University of Applied Sciences
 * Licence     : MIT-Licence
 * 
 * Copyright (c) 2021 Lapland University of Applied Sciences
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;


public class SettingsMenu : MonoBehaviour
{

    // public AudioMixer AudioMixer;
    // public AudioMixer AudioMixerSFX;
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public RenderPipelineAsset[] qualityLevels;
    Resolution[] resolutions;


    void Start()
    {
        resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions(); // clear premade list of resolutions 
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        List<string> options = new List<string>(); // new list (string) for resolution options 

        int currentResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; //Resolution width x height 
            options.Add(option); //Adds available resolution options to the list

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolution = i;
                }
        }

        
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolution;
        ResolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int reso) // This applies default resolution of screen for game
    {
        Resolution resolution = resolutions[reso];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    public void SetGraphic(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
        QualitySettings.renderPipeline = qualityLevels[quality];
    }


    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        Debug.Log(fullscreen);
    }


}
