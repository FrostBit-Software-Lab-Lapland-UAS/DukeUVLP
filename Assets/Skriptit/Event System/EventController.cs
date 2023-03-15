/******************************************************************************
 * File        : EventController.cs
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
using Valve.VR.InteractionSystem;
using Snap;
using FilterSnap;
using DoorSnap;


public class EventController : MonoBehaviour
{
    public CameraFade _fadeCamera;
    public CleanFilter cleanFilterScript;
    public TriggerEvents triggerEvent;
    public OnIndicatorTrigger indicatorTriggered;

    public GameObject _player;
    public TMPro.TextMeshProUGUI HeadsetDebugText;

    [Header("Tutorial infos")]
    // Manage these on OnTeleportEventActivate()
    
    [SerializeField] public GameObject[] InfoSet;


    [Header("Reference point in the scene where player can be moved to")]
    [Tooltip("Reference point: in front of safety switch")]
    public GameObject _firstPoint;
    [Tooltip("Reference point: in front of UVLP")]
    public GameObject _secondPoint;
    [Tooltip("Reference point: in front of Mutasihti and handle")]
    public GameObject _thirdPoint;
    [Tooltip("Reference point: in front of the sink")]
    public GameObject _fourthPoint;


    private float _timer = 1.25f;
    private float _secondTimer = 1f;
    // PM: vaihdettu 1 -> 4 (ei vaikutusta?)
    private float _thirdTimer = 4f;
    private float _fourthTimer = 4f;

    // vaihdettu 2 -> 4
    // mutasihti timer
    private float _fifthTimer = 5f;
    // vaihdettu sixth 3 -> 3
    private float _sixthTimer = 3f;

    // vaihdettu 2 -> 4
    private float _seventhTimer = 4f;

    public float _speed = 0.85f;

    private bool setTimer;
    private bool setTimerTwo;
    private bool setTimerThree;
    private bool setTimerFour;
    private bool setTimerFive;
    private bool setTimerSix;
    private bool setTimerSeven;
    // PetteriM: lis‰tty eigth
    private bool setTimerEight;

    // PetteriM: eventtien tsekkaamiseen
    private int eventCounter = 0;

    private bool filterCleaned = false;
    private bool doorInPlace = false;

    // PM: emergencyOn k‰ytet‰‰n jos tulee virhetilanne venttiilin ollessa v‰‰r‰ss‰ asennossa ja mutasihti on irti
    private bool emergencyOn = false;

    Quaternion start, end;

    private void Start()
    {
        
        //Event timer boolean checks
        setTimer = false;
        setTimerTwo = false;
        setTimerThree = false;
        setTimerFour = false;
        setTimerFive = false;
        // PetteriM: n‰m‰ kaksi lis‰tty
        setTimerSix = false;
        setTimerSeven = false;


        start = Quaternion.LookRotation(transform.forward);
        end = Quaternion.LookRotation(transform.right);

        triggerEvent._animatedObjectTwo.enabled = false;
        triggerEvent._animatedObjectThree.enabled = false;
        triggerEvent._animatedObjectSix.enabled = false;
        triggerEvent._animatedObjectSeven.enabled = false;

       
        InfoSet[0].SetActive(true);
        

        GameEvents.onFirstEvent += OnFirstEventActivate;
    }

    private void Update()
    {
        //HeadsetDebugText.text = "Event: " + eventCounter.ToString();

        // kokeillaan switch case t‰h‰n, k‰yt‰ eventCounter laskuria tarkistukseen
        // muuten esim firstpoint jne sekoittaa kun testataan erikseen

        HeadsetDebugText.text =
            " valveClosed: " + (triggerEvent._valveClosed ? "ON" : "Off") 
            + "\n ValvePosition: " + (triggerEvent._valvePosition ? "ON" : "Off") 
            + "\n Emergency: " + (emergencyOn ? "ON" : "OFF");
        
            //HeadsetDebugText.text = "Emergency: " + (emergencyOn ? "ON" : "OFF"); 

        CheckValveFail();

        switch (eventCounter)
        {
            case 1:
                First();
                break;
            case 2:
                Second();
                break;
            case 3:
                Third();
                break;
            case 4:
                Fourth();
                break;
            case 5:
                Fifth();
                break;
            case 6:
                Sixth();
                break;
            case 7:
                Seventh();
                break;
            case 8:
                Eight();
                break;
            default:
                Debug.Log("DEFAULT case HERE");
                break;
        }
      
        /*
        First();
        Second();
        Third();
        Fourth();
        Fifth();
        Sixth();
        Seventh();
        Eight();
        */

        //When fronthatch is attached to wall run this -->
        if (triggerEvent._FrontHatchDropZone.GetComponent<SnapZones>().Snapped)
        {
            triggerEvent._animatedObjectThree.enabled = false;
            triggerEvent._CosmosControllerTwo.SetActive(false);
            InfoSet[2].SetActive(false);

            if (indicatorTriggered._rightPressure == false) 
            {
                InfoSet[3].SetActive(true);
            }
        }

        if(triggerEvent._animatedObjectThree.enabled == false && triggerEvent._FrontHatchDropZone.GetComponent<SnapZones>().Snapped)
        {
            InfoSet[2].SetActive(false);
        }
    }


    /// <summary>
    /// List of Events starts here
    /// </summary>
    /// 
    private void OnFirstEventActivate()
    {
        setTimer = true;

        GameEvents.onFirstEvent -= OnFirstEventActivate;
        GameEvents.onSecondEvent += OnSecondEventActivate;

        eventCounter++;

    }

    private void OnSecondEventActivate()
    {
        setTimerTwo = true;

        GameEvents.onSecondEvent -= OnSecondEventActivate;
        GameEvents.onThirdEvent += OnThirdEventActivate;

        eventCounter++;
    }

    private void OnThirdEventActivate()
    {
        setTimerThree = true;

        GameEvents.onThirdEvent -= OnThirdEventActivate;
        GameEvents.onFourthEvent += OnFourthEventActivate;

        eventCounter++;
    }

    private void OnFourthEventActivate()
    {
        setTimerFour = true;
        GameEvents.onFourthEvent -= OnFourthEventActivate;
        GameEvents.onFifthEvent += OnFifthEventActivate;

        eventCounter++;

    }

    public void OnFifthEventActivate()
    {
        setTimerFive = true;
        GameEvents.onFifthEvent -= OnFifthEventActivate;
        GameEvents.onSixthEvent += OnSixthEventActivate;

        eventCounter++;
    }

    private void OnSixthEventActivate()
    {
        setTimerSix = true;
        doorInPlace = true;
        GameEvents.onSixthEvent -= OnSixthEventActivate;
        GameEvents.onSeventhEvent += OnSeventhEventActivate;

        eventCounter++;

    }

    private void OnSeventhEventActivate()
    {
        setTimerSeven = true;
        GameEvents.onSeventhEvent -= OnSeventhEventActivate;
        GameEvents.onEighthEvent += OnEightEventActivate;

        eventCounter++;
    }

    private void OnEightEventActivate()
    {
        // PetteriM: lis‰tty rivi:
        setTimerEight = true;
        GameEvents.onEighthEvent -= OnEightEventActivate;

        eventCounter++;
    }

    private void First()
    {

        if (setTimer == true)
        {

            
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
        }

        //After first event

        // PetteriM: "K‰‰nn‰ turvakytkin" info panel visible

        if (_timer <= 0f)
        {
            triggerEvent._arrow.SetActive(true);
            triggerEvent.StartAnimation();

            InfoSet[1].SetActive(true);
            
            setTimer = false;
        }

        // Move player to different location
        var moveSpeed = _speed * Time.deltaTime;

        if (triggerEvent._firstStep == true)
        {
            _fadeCamera.FadeOutBlack();
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, _firstPoint.transform.position, moveSpeed);

        }

        //While moving player run _fadeCamera script to ease motionsickness..
        if (_player.transform.position == _firstPoint.transform.position)
        {
            _fadeCamera.FadeInBlack();
            triggerEvent._firstStep = false;
            
        }
    }


    // PetteriM: Etupaneelin irroitus / Front hatch removal event
    private void Second()
    {
        //Timer for second part
        if (setTimerTwo == true)
        { 
            if (_secondTimer > 0)
            {
                _secondTimer -= Time.deltaTime;
            }
        }

        if (_secondTimer <= 0)
        {
            triggerEvent._animatedObjectThree.enabled = true;
            InfoSet[1].SetActive(false);
            InfoSet[2].SetActive(true);            
            setTimerTwo = false;
        }

        // Move player second time
        var moveSpeed = _speed * Time.deltaTime;

        if (triggerEvent._secondStep == true)
        {
            //While moving player run _fadeCamera script to ease motionsickness..
            _fadeCamera.FadeOutBlack();
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, _secondPoint.transform.position, moveSpeed);
            _player.transform.localRotation = Quaternion.Slerp(start, end, Time.time * 1f);
        }

        if (_player.transform.position == _secondPoint.transform.position)
        {
            _fadeCamera.FadeInBlack();
            triggerEvent._secondStep = false;
        }
    }

    private void Third()
    {

        if (setTimerThree == true)
        {
            if(_thirdTimer > 0)
            {
                _thirdTimer -= Time.deltaTime;
            }
        }

        if(_thirdTimer <= 0)
        {
            var moveSpeed = _speed * Time.deltaTime;

            if (triggerEvent._thirdStep == true && indicatorTriggered._rightPressure == true)
            {
                //While moving player run _fadeCamera script to ease motionsickness..
                _fadeCamera.FadeOutBlack();

                triggerEvent._animatedObjectSix.enabled = true;
                InfoSet[4].SetActive(false);
                InfoSet[2].SetActive(false);
                _player.transform.position = Vector3.MoveTowards(_player.transform.position, _thirdPoint.transform.position, moveSpeed);
            }

            if (_player.transform.position == _thirdPoint.transform.position)
            {
                triggerEvent._animatedObjectSix.enabled = false;
                triggerEvent._animatedObjectSeven.enabled = true;
                InfoSet[9].SetActive(true);
                InfoSet[5].SetActive(true);
                triggerEvent._thirdStep = false;
                _fadeCamera.FadeInBlack();
            }

            setTimerThree = false;
        }

    }
    
    private void Fourth()
    {

        if (setTimerFour == true)
        {
            triggerEvent._animatedObjectSeven.enabled = false;
            InfoSet[5].SetActive(false);
            if (_fourthTimer > 0)
            {
                _fourthTimer -= Time.deltaTime;
            }
        }

        if(triggerEvent._valvePosition == true)
        {
            InfoSet[9].SetActive(false);
            InfoSet[11].SetActive(true);
        }

        if(triggerEvent._valveClosed == true)
        {
            InfoSet[11].SetActive(false);
        }

      


            if (triggerEvent._filterDropZone.GetComponent<FilterSnapZones>().Snapped == false && triggerEvent._valvePosition == true)
        {    

            if (_fourthTimer <= 0 && !emergencyOn) 
            //if (_fourthTimer <= 0 ) 
            {
                // PM: testing, voi poistaa
                //HeadsetDebugText.text = "Event: tilanne ok";

                // PM: mutasihdin ohje piiloon
                InfoSet[5].SetActive(false);
                var moveSpeed = _speed * Time.deltaTime;

                if (triggerEvent._fourthStep == true)
                {
                    // PM: "Poista mutasihti paikaltaan jne..."
                    InfoSet[11].SetActive(false);

                    //While moving player run _fadeCamera script to ease motionsickness..
                    _fadeCamera.FadeOutBlack();
                    
                    _player.transform.position = Vector3.MoveTowards(_player.transform.position, _fourthPoint.transform.position, moveSpeed);
                }

                if (_player.transform.position == _fourthPoint.transform.position)
                {
                    
                    InfoSet[6].SetActive(true);
                    triggerEvent._fourthStep = false;
                    _fadeCamera.FadeInBlack();
                }

            }
        }
    }

    private void Fifth()
    {

        if (setTimerFive == true)
        {
            InfoSet[6].SetActive(false);

            if (_fifthTimer > 0)
            {
                _fifthTimer -= Time.deltaTime;
            }
        }

        var moveSpeed = _speed * Time.deltaTime;

        // PM: lis‰tty emergency testi
        // poista jos menee rikki
        if (triggerEvent._fifthStep == true  && _fifthTimer <= 0 )
        //if (triggerEvent._fifthStep == true  && _fifthTimer <= 0 && !emergencyOn)
        {
            _fadeCamera.FadeOutBlack();
            InfoSet[7].SetActive(true);
            filterCleaned = true;
            _player.transform.position = Vector3.MoveTowards(_player.transform.position, _thirdPoint.transform.position, moveSpeed);
        }

        if (_player.transform.position == _thirdPoint.transform.position)
        {
           
            _fadeCamera.FadeInBlack();
            triggerEvent._fifthStep = false;
        }
    }

    private void Sixth()
    {
        var moveSpeed = _speed * Time.deltaTime;

        if (setTimerSix == true)
        {
            if (_sixthTimer > 0)
            {  
                _sixthTimer -= Time.deltaTime;
            }
        }

     
            // PM: lis‰tty emergency testi
        if (filterCleaned == true && triggerEvent._filterDropZone.GetComponent<FilterSnapZones>().Snapped == true && triggerEvent._valveClosed == true && !emergencyOn)
        {
            // PM: lis‰t‰‰n timer true ehto jotta ei suorita t‰t‰ ja seuraavaa if:i‰ per‰kk‰in
            if (_sixthTimer <= 0 && setTimerSix)
            {
                _fadeCamera.FadeOutBlack();
                // PM: piilota "aseta mutasihti paikalleen jne" ja "irroita etupaneelin jne" paneelit
                InfoSet[7].SetActive(false);
                InfoSet[2].SetActive(false);
                // PM: liikuta pelaaja kaapin eteen
                InfoSet[13].SetActive(true);
                _player.transform.position = Vector3.MoveTowards(_player.transform.position, _secondPoint.transform.position, moveSpeed);
                _player.transform.localRotation = Quaternion.Slerp(start, end, Time.time * 1f);
            }
        }

        if (_player.transform.position == _secondPoint.transform.position)
        {

            _fadeCamera.FadeInBlack();
            triggerEvent._sixthStep = false;
            setTimerSix = false;

            // PM: kokeillaan, siirt‰‰ v‰kisin SeventhStep:iin
            triggerEvent.SeventhStep();
        }

    }

    private void Seventh()
    {
        var moveSpeed = _speed * Time.deltaTime;
        //Debug.Log("Event SEISKA k‰ynniss‰!");

        // PM: kokeillaan
        if (filterCleaned == true && triggerEvent._doorSnapZone.GetComponent<DoorSnapZones>().Snapped == true)
        //if (filterCleaned == true && triggerEvent._doorSnapZone.GetComponent<DoorSnapZones>().Snapped == true)
        {
            _seventhTimer -= Time.deltaTime;
            Debug.Log("Timer 7: " + _seventhTimer);


            // PM: lis‰tty triggerEvent t‰h‰n:
            if (_seventhTimer <= 0 && triggerEvent._seventhStep == true)
            {
                //While moving player run _fadeCamera script to ease motionsickness..
                _fadeCamera.FadeOutBlack();

                // PM: "k‰‰nn‰ turvakytkint‰ myˆt‰p‰iv‰‰n jne" ELI t‰m‰ pit‰isi tapahtua etupaneelin kiinnityksen JƒLKEEN
                // ja vanha turvakytkin info piiloon
                //InfoSet[8].SetActive(true);
                //InfoSet[2].SetActive(false);
                _player.transform.position = Vector3.MoveTowards(_player.transform.position, _firstPoint.transform.position, 0.1f);
                //_player.transform.position = Vector3.MoveTowards(_player.transform.position, _firstPoint.transform.position, moveSpeed);
                _player.transform.localRotation = Quaternion.Slerp(end, start, Time.time * 1f);
                
            }
        }
        
        // PM: ollaan turvakytkimen luona ja sit‰ pit‰isi k‰‰nt‰‰?
        if (Vector3.Distance(_player.transform.position, _firstPoint.transform.position) <= 0.1f)
        //if (_player.transform.position == _firstPoint.transform.position)
        {
            Debug.Log("PLAYER IN POINT SEVEN");


            // PetteriM: testataan - pakotetaan sijainti
            // EI TOIMINU - KEKSI JOTAIN MUUTA
            //_player.transform.position = _firstPoint.transform.position;

            InfoSet[8].SetActive(true);
            InfoSet[2].SetActive(false);
            triggerEvent._seventhStep = false;
            _fadeCamera.FadeInBlack();
        }


    }

    private void Eight()
    {
        // PM: suoritetaan kun Emergency-switch rotaatio = 0
        if(triggerEvent._eightStep == true && eventCounter > 5)
        {
            InfoSet[8].SetActive(false);
            InfoSet[10].SetActive(true);
        }
    }


    // PetteriM: tehd‰‰np‰ t‰lle oma metodi niin ei tarvitse mietti‰ miss‰ Stepiss‰/Eventiss‰ tapahtuu
    // TODO: mieti miksi ei toimi takaisin p‰in tullessa. Muuten skulaa ok.
    private void CheckValveFail()
    {
        // PM: jos valve auki ja mutasihti pois, error tilanne
        if (triggerEvent._filterDropZone.GetComponent<FilterSnapZones>().Snapped == false && triggerEvent._valveClosed == false && !triggerEvent._valvePosition)
        {
            //HeadsetDebugText.text = triggerEvent._valveClosed.ToString();
            emergencyOn = true;
            HeadsetDebugText.text = "Event: VALVE 1 ERROR";
            Debug.Log("ERROR emergencyyyyyyyyy");
        }

        // ja h‰lytyksen purku kun mutasihti takaisin
        if (triggerEvent._valvePosition && !triggerEvent._valveClosed && emergencyOn)
        // VANHA: if (triggerEvent._filterDropZone.GetComponent<FilterSnapZones>().Snapped == true && triggerEvent._valveClosed == false && emergencyOn)
        {
            HeadsetDebugText.text = "Event: VALVE 1 FIXED";
            Debug.Log("Emergency over. Phew!");
            emergencyOn = false;
        }

    }

}
