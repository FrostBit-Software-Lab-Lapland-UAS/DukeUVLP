/******************************************************************************
 * File        : Safetyvalve.cs
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




// -------------------PLACE THIS CODE INTO SAFETYVALVE OBJECT----------------------


using UnityEngine;
using Pressure;

public class Safetyvalve : MonoBehaviour
{


    [Header("Pressurevalve")]
    [SerializeField]    
    GameObject _gameobject;

    [Header("Indicator")]
    [SerializeField]    
    GameObject _indicator;

    
    // [Header("Pressure release time")]
    // [SerializeField]
    float indicatorSpeed;

    public PressureUp _pressure;

    [Header("Particle system")]
    [SerializeField]    
    ParticleSystem _particles;
    
    [Header("Audio source")]
    [SerializeField]    
    AudioSource _audioSource;

    [Header("Pressurevalve rotation axis")]
    [SerializeField]
    bool x = false;
    [SerializeField]
    bool y = false;
    [SerializeField]
    bool z = false;

    [SerializeField]
    float pressurevalveRotationValue;

    [SerializeField]
    float targetRotation = 0;

    float _timerTime = 2;
    [SerializeField]
    float _time = 2;

    bool _trigger;
    bool _tooHighPressure;
    bool rotHasIncreased;

    Quaternion startRotation;
    

    void Start()
    {
        indicatorSpeed = _pressure.indictorReturnSpeed;
        _tooHighPressure = _pressure.tooHighPressure;  
        targetRotation = 0;
        _timerTime = _time;     
        startRotation = transform.rotation; 
    }


    void Update()
    {
//========================================= Bool values for used axes ===============================================
        if (x)
        {
            pressurevalveRotationValue = _gameobject.transform.localRotation.eulerAngles.x;
            y = false;
            z = false;      
        }

        if (y)
        {
            pressurevalveRotationValue = _gameobject.transform.localRotation.eulerAngles.y;
            x = false;
            z = false;
        }

        if(z)
        {
            pressurevalveRotationValue = _gameobject.transform.localRotation.eulerAngles.z;
            x = false;
            y = false;
        }     
//==================================================================================================================


//==================== If targetRotation value is certain amount --> trigger particles and audio ===================

        if(targetRotation == 90 || targetRotation == 180 || targetRotation == 270 || targetRotation == 360){

                _indicator.transform.Rotate(0.5f, 0, 0 * Time.deltaTime);

                _particles.Play();
                _audioSource.Play();

        }

//==================================================================================================================


// Each time value is higher than targetRotation value it adds 45 to target rotation
//==================================================================================================================
        if (targetRotation <= pressurevalveRotationValue) {
            
            _timerTime -= Time.deltaTime;
            targetRotation += 45;
        }
//==================================================================================================================



//==================================================================================================================
        if (targetRotation == 360) {

            targetRotation = 0;
        }
        if (pressurevalveRotationValue == 360){

            pressurevalveRotationValue = 0;
            transform.rotation = startRotation;

        }
//==================================================================================================================
  }

    public void SafetyOn(){

            _indicator.transform.Rotate(2f, 0, 0 * Time.deltaTime);

            _particles.Play();
            _audioSource.Play();
    
    }

}

