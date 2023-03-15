/******************************************************************************
 * File        : MutasihtiPuhdistus.cs
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
/// A Subtask that handles the second part of Filter Task.
/// </summary>
public class MutasihtiPuhdistus : Subtask
{

    // --- --- --- Variables --- --- ---

    public SnappableObject filterSnapObj;
    public SnapObjState filterState = SnapObjState.Held;
    public Collider inspectAreaTrigger;
    public CleanFilter cfScript;
    public IsInsideTrigger filterInsideInspectArea;




    // --- --- --- Methods --- --- --- 

    // --- MonoBehaviour ---

    /// <summary>
    /// Main logic has been defined in here.
    /// Simply check if CleanFilter-instance attached to the sink has cleaned the FIlter and begin to check for the triggerArea near the HMD.
    /// </summary>
    private void Update ()
    {
        if (!isCurrentlyActiveSubtask) return;

        if(cfScript.isClean) {
            if(currentStoryIndex == 0) {
                ShowNextStory();
            } else {
                CheckInspectArea();
            }
        }
    }

    /// <summary>
    /// Check if Filter has been brought inside a trigger area defined in IsInsideTrigger-class.
    /// If player has detached Filter correctly and has brought it close to their face, this Subtask is complete.
    /// </summary>
    private void CheckInspectArea ()
    {
        if (filterInsideInspectArea.isInsideTrigger) Done();
    }




    // --- Events Sub and Unsub ---

    protected override void SubscribeToEvents ()
    {
        InteractiveDevicesHandler.MutasihtiStateChanged += MutasihtiStateChanged;
    }
    protected override void UnsubscribeToEvents ()
    {
        InteractiveDevicesHandler.MutasihtiStateChanged -= MutasihtiStateChanged;
    }



    // --- Override Methods ---

    protected override void OnAfterMoving ()
    {
        base.OnBegin();
        if(filterSnapObj.state != SnapObjState.Held) {
            filterSnapObj.transform.GetComponent<TeleportToPlace>().TeleportToPosition(1);
        }
    }





    // --- State Control ---

    private void MutasihtiStateChanged (SnapObjState _state, Transform _snapArea)
    {
        filterState = _state;
    }



}
