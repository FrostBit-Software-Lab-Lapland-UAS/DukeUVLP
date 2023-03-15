/******************************************************************************
 * File        : AirpumpEventController.cs
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
using OuterUnitSnap;

public class AirpumpEventController : MonoBehaviour
{
    // Referenssi pumpun eventsystemin AirpumpEventTrigger -skriptiin.
    [Tooltip("AirpumpEventTrigger scripts reference")]
    [SerializeField]
    private AirpumpEventTrigger _airpumpTrigger;

    [Tooltip("CameraFade script reference")]
    [SerializeField]
    private CameraFade _fadeCamera;

    // Animaatio, joka aktivoidaan tarvittavassa kohtaa esim. n‰ytt‰m‰‰n mit‰ kohtaa pit‰‰ painaa/k‰‰nt‰‰/liikuttaa.
    [Tooltip("Animation which is activated when needed")]
    public Animator[] _animation;

    // Aktivoitava peliobjekti, joka aktivoituu aina edellisen eventin tapahduttua.
    [Tooltip("Activatable gameobject which activates when a event has occured")]
    [SerializeField]
    private GameObject[] _eventObject;

    // Tyhj‰ gameobject, jonka tarkoituksena on n‰ytt‰‰ sijainti mihin pelaaja liikkuu automaattisesti eventin tapahduttua. 
    [Tooltip("Empty gameobject that refers where player is going to move when event occures")]
    [SerializeField]
    private GameObject _playerPlacementPoint;

    // SteamVR Player prefabin sis‰lt‰ 'SteamVRObjects' liitet‰‰n t‰h‰n 
    [Tooltip("From SteamVR Player prefab add 'SteamVRObjects' here.")]
    [SerializeField]
    private GameObject _playerObject;

    // Referenssi ilmal‰mpˆpumpun OuterUnitMainHatchZone scriptiin (inspectorissa t‰h‰n vedet‰‰n objekti, joka sis‰lt‰‰ kyseisen skriptin)
    [SerializeField]
    private OuterUnitMainHatchZone _airpumpHatch;

    [Tooltip("Array for timer float values")]
    [SerializeField]
    private float[] _timer;

    private float _speed = 0.7f;

    public bool playAnim = false;

    Quaternion start, end;


    void Update()
    {
        RunFirstEvent();
        RunSecondEvent();
        RunThirdEvent();
        RunFourthEvent();
        RunFifthEvent();
        RunSixthEvent();
        RunSeventhEvent();
        RunEightEvent();

        _eventObject[0].SetActive(false);

        if (_airpumpHatch.GetComponent<OuterUnitMainHatchZone>().Snapped == true)
        {
            _eventObject[10].SetActive(false);
            //_eventObject[0].SetActive(false);
            _airpumpTrigger._step[1] = true;
            //_animation[3].SetBool("PlayArrowOne", false);
        }


        if(_timer[0] == 0)
        {
            _eventObject[3].SetActive(false);
            _eventObject[1].SetActive(false);
        }

    }


    //Detach all measurement parts with this method.
    public void DetachAll()
    {
        _eventObject[9].SetActive(false);
        _airpumpTrigger._step[7] = false;
        _eventObject[11].SetActive(false);
        _eventObject[12].SetActive(true);
        _animation[0].SetBool("ExitTubeSocket", true);
        _animation[1].SetBool("ExitHose", true);
        _animation[2].SetBool("ExitTester", true);
        
    }

    // These methods are for making code look more clean 
    private void RunFirstEvent()
    {
        // ------- First Event Start -------
        var moveSpeed = _speed * Time.deltaTime;

        if (_airpumpTrigger._step[0] == true)
        {
            _fadeCamera.FadeOutBlack();
            _playerObject.transform.position = Vector3.MoveTowards(_playerObject.transform.position, _playerPlacementPoint.transform.position, moveSpeed);
            _playerObject.transform.localRotation = Quaternion.Slerp(start, end, Time.time * 1f);
        }

        if (_playerObject.transform.position == _playerPlacementPoint.transform.position)
        {
            _fadeCamera.FadeInBlack();
            _eventObject[0].SetActive(true);

            
            //_animation[3].SetBool("PlayArrowOne", true);
            _eventObject[10].SetActive(true);

            _airpumpTrigger._step[0] = false;
        }

        // ------- First Event Stop -------

    }

    private void RunSecondEvent()
    {
        // ------- Second Event Start -------

        if (_airpumpTrigger._step[1] == true)
        {
            _eventObject[1].SetActive(true);
        }

        if (_eventObject[1])
        {

            _airpumpTrigger._step[1] = false;
        }

        // ------- Second Event Stop -------
    }

    private void RunThirdEvent()
    {
        // ------- Third Event Start -------

        if (_airpumpTrigger._step[2] == true)
        {
            _eventObject[0].SetActive(false);
            _animation[0].SetBool("PlayTubeSocket", true);
            _eventObject[1].SetActive(false);
            _eventObject[2].SetActive(true);
        }

        // ------- Third Event Stop -------

    }

    private void RunFourthEvent()
    {
        // ------- Fourth Event Start -------

        if (_airpumpTrigger._step[3] == true)
        {
            _airpumpTrigger._step[2] = false;

            _eventObject[1].SetActive(false);
            _eventObject[2].SetActive(false);
            _eventObject[3].SetActive(true);
            _animation[1].SetBool("PlayHose", true);
        }
        // ------- Fourth Event Stop -------
    }

    private void RunFifthEvent()
    {
        // ------- Fifth Event Start -------
        if (_airpumpTrigger._step[4] == true)
        {
            _airpumpTrigger._step[3] = false;
            _eventObject[13].SetActive(true);

            //Timer before moving to next part
            if (_timer[0] > 0)
            {
                _timer[0] -= Time.deltaTime;

                _eventObject[1].SetActive(false);
                _eventObject[2].SetActive(false);
                _eventObject[3].SetActive(true);
                _eventObject[4].SetActive(true);

                // Tester arrows
                _eventObject[5].SetActive(true);
                _eventObject[6].SetActive(true);
                _animation[3].enabled = true;
                _animation[4].enabled = true;
            }

            if (_timer[0] <= 0)
            {
                _eventObject[3].SetActive(false);
                _eventObject[4].SetActive(false);
                _animation[2].SetBool("PlayTester", true);

                // Tester Arrows
                _eventObject[5].SetActive(false);
                _eventObject[6].SetActive(false);
                _animation[3].enabled = false;
                _animation[4].enabled = false;

                // Activate next event here...
                _airpumpTrigger._step[4] = false;
                //_airpumpTrigger._step[5] = true;
            }

        }

        // ------- Fifth Event Stop -------
    }

    private void RunSixthEvent()
    {
        // ------- Sixth Event Start -------
        if (_airpumpTrigger._step[5] == true)
        {
            _eventObject[13].SetActive(false);
            _eventObject[14].SetActive(true);

            _eventObject[1].SetActive(false);
            _eventObject[7].SetActive(true);


            if (_timer[1] > 0)
            {
                _timer[1] -= Time.deltaTime;
            }

            if (_timer[1] <= 0)
            {
                _animation[0].SetBool("PlayTubeSocketSwitch", true);
                _animation[1].SetBool("PlayHoseSwitch", true);
                

                //_airpumpTrigger._step[6] = true;
            }
        }
        // -------Sixth Event Stop -------
    }

    private void RunSeventhEvent()
    {
        // -------Seventh Event Start -------
        if (_airpumpTrigger._step[6] == true)
        {
            _eventObject[14].SetActive(false);
            _eventObject[15].SetActive(true);

            _animation[5].SetBool("BoltPlay", true);
            _animation[7].SetBool("Bolt2Play", true);

            if (_animation[5])
            {
                _timer[2] -= Time.deltaTime;
                _timer[3] -= Time.deltaTime;
            }

            if (_timer[2] <= 0)
            {
                _animation[6].SetBool("HexkeyPlay", true);
            }

            _eventObject[7].SetActive(false);
            _eventObject[8].SetActive(true);
        }

        if (_timer[3] <= 0)
        {
            _eventObject[15].SetActive(false);
            _airpumpTrigger._step[6] = false;
            _airpumpTrigger._step[7] = true;
        }
        // -------Seventh Event Stop -------
    }

    private void RunEightEvent()
    {
        // -------Eighth Event Start -------

        if (_airpumpTrigger._step[7] != false)
        {
            _eventObject[8].SetActive(false);
            _eventObject[9].SetActive(true);

            _animation[7].SetBool("BoltSecondPlay", true);
            _animation[6].SetBool("HexkeySecondPlay", true);

            _timer[4] -= Time.deltaTime;
        }

        if (_timer[4] <= 0)
        {
            _eventObject[10].SetActive(true);
            _airpumpTrigger._step[7] = false;
            _eventObject[9].SetActive(false);
            _animation[5].SetBool("BoltUpperPlay", true);
            _eventObject[11].SetActive(true);

        }
        // -------Eighth Event Stop -------
    }



    //=============Subscribe Events=============
    private void OnEventOneActivate()
    {

        AirpumpEvents.onEventOne -= OnEventOneActivate;
        AirpumpEvents.onEventTwo += OnEventTwoActivate;
    }
    private void OnEventTwoActivate()
    {

        AirpumpEvents.onEventTwo -= OnEventTwoActivate;
        AirpumpEvents.onEventThree += OnEventThreeActivate;
    }
    private void OnEventThreeActivate()
    {
        AirpumpEvents.onEventThree -= OnEventThreeActivate;
        AirpumpEvents.onEventFour += OnEventFourActivate;
    }
    private void OnEventFourActivate()
    {

        AirpumpEvents.onEventFour -= OnEventFourActivate;
        AirpumpEvents.onEventFive += OnEventFiveActivate;
    }
    private void OnEventFiveActivate()
    {

        AirpumpEvents.onEventFive -= OnEventFiveActivate;
        AirpumpEvents.onEventSix += OnEventSixActivate;
    }
    private void OnEventSixActivate()
    {

        AirpumpEvents.onEventSix -= OnEventSixActivate;
        AirpumpEvents.onEventSeven += OnEventSevenActivate;
    }
    private void OnEventSevenActivate()
    {

        AirpumpEvents.onEventSeven -= OnEventSevenActivate;
        AirpumpEvents.onEventEight += OnEventEightActivate;
    }
    private void OnEventEightActivate()
    {

        AirpumpEvents.onEventEight -= OnEventEightActivate;
    }
    //=============Subscribe Events=============
}
