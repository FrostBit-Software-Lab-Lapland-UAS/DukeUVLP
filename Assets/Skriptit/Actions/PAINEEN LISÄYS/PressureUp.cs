/******************************************************************************
 * File        : PressureUp.cs
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




// ------------------- PLACE THIS CODE INTO THE COLLIDER OBJECT ON BP-5 ---------------------------

// Documentation is in Readme file.

using UnityEngine;
using UnityEngine.UI;


namespace Pressure{
public class PressureUp : MonoBehaviour
{
//=========================================== VARIABLES ============================================================
    [Header("Pressure valves")]
    [Tooltip("Pressure adjusting valve is placed here")]
    [SerializeField]
    GameObject valveOne;

    // [SerializeField]
    // GameObject valveTwo;

    [Header("Pressure indicator")]
    [SerializeField]
    GameObject pressureIndicator;


    float valveOneRotationValue;
    
    //[SerializeField]
    float valveTwoRotationValue;

    [Header("Indicator rotation and return speeds")]
    [SerializeField]
    float indicatorRotationSpeed = -0.2f;

    [SerializeField]
    public float indictorReturnSpeed = 0.5f;

    [Header("Choose axis for valves")]
    [SerializeField]
    bool x = true;

    [SerializeField]
    bool y = false;

    [SerializeField]
    bool z = false;

    [Header("Particles and audio source")]    
    [SerializeField]
    ParticleSystem _particleSystem;
    [SerializeField]
    AudioSource _audioSource;

    float timer = 0; // Indicator timer when releasing pressure

    [Header("Timer seconds")]    
    [SerializeField]
    float _time = 2f;
    bool timerRunning;
    
    [HideInInspector]
    public bool tooHighPressure;

    bool disablePressure = false;

    bool desktopPressureUp = false;
    public int _rotationValue = -90;

    public ParticleController partController;




//===================================================================================================================
    public void Update() {

//========================================= Bool values for used axes ===============================================
        if (x == true)
        {
            valveOneRotationValue = valveOne.transform.localRotation.eulerAngles.x;
            y = false;
            z = false;      
        }

        if (y == true)
        {
            valveOneRotationValue = valveOne.transform.localRotation.eulerAngles.y;
            x = false;
            z = false;
        }

        if(z == true)
        {
            valveOneRotationValue = valveOne.transform.localRotation.eulerAngles.z; 
            x = false;
            y = false;
        }

//==================================================================================================================


// Pressure rises until indicator hits maximum pressure.
// Max pressure is defined with collider object that is tagged "Indicator" in the scene.

        if(disablePressure == false) {

            if (valveOneRotationValue == _rotationValue ||  desktopPressureUp == true) {

                pressureIndicator.transform.Rotate(indicatorRotationSpeed, 0, 0 * Time.deltaTime);

            }
        }

// Timer value is set in the inspector. Timers time indicates how long pressure is releasing.

            if(timerRunning == true && timer < _time){
                
                    timer += Time.deltaTime;
                    pressureIndicator.transform.Rotate(indictorReturnSpeed, 0, 0 * Time.deltaTime);
                    disablePressure = true;
           
                if (timer >= _time) {
                    timer = 0f;
                    timerRunning = false;
                    tooHighPressure = false;
                    disablePressure = false;
                }           
            }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Indicator")
        {

            tooHighPressure = true;
            timerRunning = true;
            disablePressure = true;
            desktopPressureUp = false;
                //_audioSource.Play();
                //_particleSystem.Play();
                // PM: kokeillaan
               // partController.ToggleParticles(true);

        }

    }

    private void OnTriggerExit(Collider other)
    {
           // partController.ToggleParticles(false);
            if (timerRunning == false) {
            
            disablePressure = false;
            

        }

    }

    public void RisePressure()
    {
        desktopPressureUp = true;       
    }

    public void StopPressureRising()
    {
         desktopPressureUp = false;
    }

    }
}


