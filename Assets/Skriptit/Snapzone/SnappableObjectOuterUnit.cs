/******************************************************************************
 * File        : SnappableObject.cs
 * Version     : 1.0
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

using UnityEngine;
using Valve.VR;
using Snap;
using OuterUnitSnap;

public class SnappableObjectOuterUnit : MonoBehaviour
{

    public GameObject SnapLoc; //Snapzone collider object
    public GameObject MainObj; // Parent object to which objects are attached to
    public bool isSnapped; // Check if objects are snapped to place 

    private bool objectSnapped;
    // Boolean value for outerunits dropzone
    private bool objectSnappedTwo;

// Is object being held check for both hands
    private bool grabbed; 
    public SteamVR_Input_Sources Any;

    void FixedUpdate()
    {
        // Setting up interactions for grabbing objects in VR
        // grabbed = SteamVR_Input.GetStateDown("Grab Pinch", Any) == false;
        objectSnapped = SnapLoc.GetComponent<SnapZones>().Snapped;
        objectSnappedTwo = SnapLoc.GetComponent<OuterUnitMainHatchZone>().Snapped;

        if (objectSnapped == true || objectSnappedTwo == true){

            transform.SetParent(MainObj.transform);
            isSnapped = true;
        }

    }

}
