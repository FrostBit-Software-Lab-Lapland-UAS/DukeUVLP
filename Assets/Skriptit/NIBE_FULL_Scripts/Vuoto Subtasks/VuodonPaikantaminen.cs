/******************************************************************************
 * File        : VuodonPaikantaminen.cs
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

using UnityEngine;


/// <summary>
/// Responsible for handling the clearing of boxes on top of the leak.
/// Most of the actual logic needed for this Subtask is handled in ClearAreaBounds and GrabAndThrow.
/// </summary>
public class VuodonPaikantaminen : Subtask
{

    // --- --- --- Variables --- --- ---

    public ClearAreaBounds clearAreaBounds;
    public GameObject boxesContainer;




    // --- --- --- Methods --- --- ---

    // --- Events Sub and Unsub ---
    protected override void SubscribeToEvents ()
    {
        ClearAreaBounds.AreaIsCleared += ToggleCleared;
    }
    protected override void UnsubscribeToEvents ()
    {
        ClearAreaBounds.AreaIsCleared -= ToggleCleared;
    }


    // --- OnBegin and OnDone ---
    protected override void OnBegin ()
    {
        EnableGrabAndThrow(true);
    }
    protected override void OnDone ()
    {
        EnableGrabAndThrow(false);
    }


    // --- Vuodon Paikantaminen ---
    private void ToggleCleared (bool _isClear)
    {
        if(_isClear) {
            ShowStory(1);
            currentStoryIndex = 1;
        } else {
            ShowStory(0);
            currentStoryIndex = 0;
        }
    }

    /// <summary>
    /// Find every object with GrabAndThrow in the scene and toggle the script  ON / OFF.
    /// </summary>
    /// <param name="_enabled"></param>
    void EnableGrabAndThrow (bool _enabled)
    {
        foreach (GrabAndThrow grab in boxesContainer.GetComponentsInChildren<GrabAndThrow>()) {
            grab.interactionEnabled = _enabled;
        }
    }

}
