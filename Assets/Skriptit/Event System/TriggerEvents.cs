/******************************************************************************
 * File        : TriggerEvents.cs
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

public class TriggerEvents : MonoBehaviour
{
    // Boolean checks to indicate event step has been done
    [HideInInspector]
    public bool _firstStep = false;

    [HideInInspector]
    public bool _secondStep = false;

    [HideInInspector]
    public bool _thirdStep = false;

    [HideInInspector]
    public bool _fourthStep = false;

    [HideInInspector]
    public bool _fifthStep = false;

    [HideInInspector]
    public bool _sixthStep = false;

    [HideInInspector]
    public bool _seventhStep = false;

    [HideInInspector]
    public bool _eightStep = false;

    [HideInInspector]
    public bool _valvePosition = false;
    [HideInInspector]

    // PM: kokeillaan aloittaa true:lla
    //public bool _valveClosed = true;
    public bool _valveClosed = false;

    public Animator _animatedObject;
    public Animator _animatedObjectTwo;
    public Animator _animatedObjectThree;
    public Animator _animatedObjectFour;
    public Animator _animatedObjectFive;
    public Animator _animatedObjectSix;
    public Animator _animatedObjectSeven;

    public GameObject _arrow;
    public GameObject _CosmosControllerTwo;
    public GameObject _FrontHatchDropZone;
    public GameObject _doorSnapZone;

    public GameObject _filterDropZone;

    public void StopAnimation()
    {
        _animatedObject.enabled = false;
    }

    public void StartAnimation()
    {
        _animatedObjectTwo.enabled = true;
    }

    public void StopFirstAnimation()
    {
        _animatedObjectFour.enabled = false;
    }

    public void FilterValveCheck()
    {
        _valvePosition = true;
    }

    public void FilterValveCheckTwo()
    {
        _valveClosed = true;
    }

    
    public void FirstStep()
    {
        _firstStep = true;
        GameEvents.FirstEvent();
    }

    public void SecondStep()
    {
        _secondStep = true;
        GameEvents.SecondEvent();
    }

    public void ThirdStep()
    {
        _thirdStep = true;
        GameEvents.ThirdEvent();
    }
    // Mutasihti-Handle -> 270 astetta (=pystyss‰) k‰ynnist‰‰ t‰m‰n
    public void FourthStep()
    {
        _fourthStep = true;
        GameEvents.FourthEvent();
    }

    public void FifthStep()
    {
        _fifthStep = true;
        GameEvents.FifthEvent();
    }

    // Mutasihti-Handle -> 180 astetta (=vaakatasossa) k‰ynnist‰‰ t‰m‰n
    public void SixthStep()
    {
        _sixthStep = true;
        GameEvents.SixthEvent();
    }

    public void SeventhStep()
    {
        _seventhStep = true;
        GameEvents.SeventhEvent();
    }

    public void EightStep()
    {
        _eightStep = true;
        GameEvents.EighthEvent();
    }

}
