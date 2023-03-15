/******************************************************************************
 * File        : Virtakytkin.cs
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
/// Used to turn the safety switch (incorrectly named power switch elsewhere) ON / OFF.
/// </summary>
public class Virtakytkin : Subtask
{

    // --- --- --- Variables --- --- ---

    public SwitchState taskObjective;
    private SwitchState prevState;
    public SwitchState state;

    public Collider switchCollider;




    // --- --- --- Methods --- --- ---

    // --- Monobehaviour ---
    void Start ()
    {
        prevState = state;
        switchCollider.enabled = false;
    }


    // --- Override methods ---
    protected override void SubscribeToEvents()
    {
        InteractiveDevicesHandler.OnVirtaTurned += VirtaTurned;
    }
    protected override void UnsubscribeToEvents()
    {
        InteractiveDevicesHandler.OnVirtaTurned -= VirtaTurned;
    }

    protected override void OnBegin ()
    {
        switchCollider.enabled = true;
    }
    protected override void OnDone ()
    {
        switchCollider.enabled = false;
    }



    // --- Virtakytkin-specific Methods ---
    private void VirtaTurned (bool _on)
    {
        state = _on ? SwitchState.On : SwitchState.Off;

        //Debug.Log("VirtaTurned = " + _on);

        if (state == taskObjective) {
            if(state != prevState) {
                ShowNextStory();
            }
        } else {
            ShowStory(0);
            currentStoryIndex = 0;
        }
        prevState = state;
    }
}
