/******************************************************************************
 * File        : SnapZones.cs
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


// This script is attached to snapzone (collider object). 
// GameObject that enters the collider area is moved into the center of SnapZones collider.

using UnityEngine;
using Valve.VR;

namespace FilterSnap {
public class FilterSnapZones : MonoBehaviour
{

// Boolean variables used for determing if the object is held on left or right hand by the player
    private bool grabbed;

// Returns true when the object is within the SnapZone radius
    private bool inZone;

// Returns true when the object updates its location
    public bool Snapped;

// Part that is being snapped into parent location 
    public GameObject _part;

// Set rotation in relation to other object
    public GameObject SnapRotationRef;

// Get Inputs from VR controllers
    public SteamVR_Input_Sources Any;

    void Start() {  
        //grabbed = SteamVR_Input.GetStateDown("Grab Pinch", Any) == false;
    }
    
    // Update is called once per frame
    void Update()
    {
        SnapObject();
        //_part.GetComponent<Rigidbody>().useGravity = true;
        //_part.GetComponent<Rigidbody>().isKinematic = false;

    }

// GameObject in SnapZone
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.name == _part.name){
            // Debug.Log("SnapZones on OK Enter");
            //_part.GetComponent<Rigidbody>().useGravity = false;
            //_part.GetComponent<Rigidbody>().isKinematic = true;
            //_part.GetComponent<Collider>().isTrigger = true;      
            inZone = true;
        }
    }

// GameObject has exited SnapZone
    private void OnTriggerExit(Collider other){
        if (other.gameObject.name == _part.name){ 
            //_part.GetComponent<Rigidbody>().useGravity = true;
            //_part.GetComponent<Rigidbody>().isKinematic = false;
            //_part.GetComponent<Collider>().isTrigger = false;                      
            inZone = false;
        }
    }

// Checks if object has been released and is in SnapZone area
// When true ==> transforms rotation and location to specific coordinates
// Sets boolean 'snapped' to true.
    void SnapObject(){

        if ( inZone == true){
            _part.gameObject.transform.position = transform.position;
            _part.gameObject.transform.rotation = SnapRotationRef.transform.rotation;
        Snapped = true;
        }
        else
        Snapped = false;
    }


    }

}
