/******************************************************************************
 * File        : MainMenu.cs
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
using UnityEngine.SceneManagement;



/// <summary>
/// 
/// </summary>
public class MainMenu : MonoBehaviour
{

    public void StartFull()
    {
        SceneInfoManager.Current.nibeFullSelectedTask = SelectedTaskList.Full;
        SceneInfoManager.Current.GoToScene(1);
    }
    public void StartPressure ()
    {
        SceneInfoManager.Current.nibeFullSelectedTask = SelectedTaskList.Pressure;
        SceneInfoManager.Current.GoToScene(1);
    }
    public void StartFilter(){
        SceneInfoManager.Current.nibeFullSelectedTask = SelectedTaskList.Filter;
        SceneInfoManager.Current.GoToScene(1);
    }
    public void StartMessy(){
        SceneInfoManager.Current.nibeFullSelectedTask = SelectedTaskList.Messy;
        SceneInfoManager.Current.GoToScene(1);
    }
    public void StartAirpump()
    {
        SceneInfoManager.Current.GoToScene(2);
    }


    /// <summary>
    /// Why is this here? Probably to allow other classes to call for changes to scenes from one place.
    /// This is currently unused.
    /// </summary>
    public void BackToMainMenu(){
        SceneInfoManager.Current.GoToScene(0);
    }


    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
