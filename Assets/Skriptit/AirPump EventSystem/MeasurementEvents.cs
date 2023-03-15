/******************************************************************************
 * File        : MeasurementEvents.cs
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
using UnityEngine.UI;
using OuterUnitSnap;
using System.Collections.Generic;

public class MeasurementEvents : MonoBehaviour
{
    public GameObject _player;
    public GameObject[] _gameObject;
    public GameObject[] _forwardButton;
    public Animator[] _animation;

    public float[] _timer;
    public bool[] _booleanCheck;

    private float speed = 2f;

    // Attach Outerunits hatch object here in the inspector!
    [SerializeField]
    private OuterUnitMainHatchZone _airpumpHatch;


    // *** Miikan lisäämät muuttujat: ***

    //Digitaalisen näytön muuttujat
    private float pressureValue = 0f;                   // Digitaalisen näytön painetta esittävä lukuarvo
    private float targetPressureValue = -0.7f;          // Tavoitearvo paineelle
    public TMPro.TextMeshProUGUI digitalDisplayText;    // Digitaalisen näytön tekstiobjekti
    private bool digitalDisplayOn = false;              // Digitaalisen näyttö päällä/pois päältä
    private bool updateDigitalDisplay = false;          // Digitaalisen näytön lukeman päivittäminen
    private bool calibrateDigitalDisplay = false;       // Digitaalisen näytön kalibrointivaihe (paineen tasaus)

    [Space(20)]
    public List<GameObject> buttonsList = new List<GameObject>();   // Painikkeet sisältävä lista, jota käytetään värien muuttamiseen
    public Color panelColor;                            // Paneelien taustaväri
    public Color buttonColor;                           // Painikkeiden taustaväri
    public Color arrowButtonColor;                      // Nuolipainikkeiden taustaväri (tarvitaanko?)
    public Color textColor;                             // Paneelien tekstin väri


    // --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---




    private void Start()
    {
        for(var i = 0; i < _booleanCheck.Length; i++)
        {
            _booleanCheck[i] = false;
        }

        SetUIColors();
    }




    private void Update()
    {
        // If hatch is in the outerunit mainhatch zone then activate second method
        if (_airpumpHatch.GetComponent<OuterUnitMainHatchZone>().Snapped == true)
        {
            Second();
        }

        if(_booleanCheck[0] == true)
        {
            _timer[0] -= Time.deltaTime;

            if (_timer[0] <= 0)
            {
                _booleanCheck[0] = false;
                
            }
        }

        // Fifth "event" timer check
        if (_booleanCheck[1] == true)
        {
            _timer[1] -= Time.deltaTime;

            if (_timer[1] <= 0)
            {
                _gameObject[12].SetActive(false);
                _animation[3].enabled = false;
                _animation[4].enabled = false;

                _booleanCheck[1] = false;
                
            }
        }

        if (_booleanCheck[4] == true)
        {
            _timer[2] -= Time.deltaTime;

            if (_timer[2] <= 0)
            {
                _animation[5].SetBool("BoltUpperPlay", true);
            }
        }



        //Päivitetään digitaalisen näytön tekstikenttä. 
        if (digitalDisplayOn) {
            if(updateDigitalDisplay) {
                if (!calibrateDigitalDisplay) {
                    ChangePressureOnDigitalDisplay(false);
                } else {
                    ChangePressureOnDigitalDisplay(true);
                }
            } else {
                digitalDisplayText.text = (Mathf.RoundToInt(pressureValue * 100f) / 100f).ToString() + " bar";
            }
        } else {
            digitalDisplayText.text = " ";
        }


    }




    // Digitaalisen näytön painearvot muuttuvat kohdearvoon ennalta määritetyllä nopeudella. _increasePressure säätelee paineen muutoksen suuntaa.
    private void ChangePressureOnDigitalDisplay(bool _increasePressure)
    {
        float valueChange = Time.deltaTime / 10f; // Jaetaan aika kymmenellä, jolloin paineen muutos kestää 7 sekuntia 0.7 sekunnin sijaan. 
        pressureValue = _increasePressure ? pressureValue + valueChange : pressureValue - valueChange;
        if ((_increasePressure && pressureValue > targetPressureValue) || (!_increasePressure && pressureValue < targetPressureValue))
            pressureValue = targetPressureValue;
        digitalDisplayText.text = (Mathf.RoundToInt(pressureValue * 100f) / 100f).ToString() + " bar"; // Muutetaan float kahden desimaalin tarkkuudelle.
    }




    // Asetetaan UI-elementtien väritys muuttujien mukaisiksi.
    // HUOM! Tällä hetkellä (08.02.2022) spritet sisältävät värin, eli todellinen väri poikkeaa asetetusta väristä.
    // Ratkaisu on käyttää mustavalkoisia spritejä.
    private void SetUIColors()
    {
        // Haetaan nimetty empty, joka sisältää kaikki paneeliobjektit. Haetaan sen lapsista kaikki Image-komponentit, joiden väri vaihdetaan.
        Transform panelsContainer = GameObject.Find("Event UI Container").transform;
        foreach (Transform child in panelsContainer) {
            child.GetComponentInChildren<Image>().color = panelColor;
        }

        // Haetaan nimetty empty, joka sisältää kaikki nuoliobjektit. Haetaan sen lapsista kaikki Image-komponentit, joiden väri vaihdetaan.
        Transform fwdButtonsContainer = GameObject.Find("Forward Button Container").transform;
        foreach (Transform child in fwdButtonsContainer) {
            child.GetComponentInChildren<Image>().color = arrowButtonColor;
        }

        // Käydään läpi käsin asetettu buttonsList-lista, josta jokaisen elementin Image-komponentille vaihdetaan väri. 
        foreach(GameObject obj in buttonsList) {
            obj.GetComponent<Image>().color = buttonColor;
        }

        // Käydään paneelien tekstit läpi ja vaihdetaan väri
        foreach (Transform child in panelsContainer) {
            TMPro.TextMeshProUGUI[] texts = child.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            foreach (TMPro.TextMeshProUGUI t in texts) {
                t.color = textColor;
            }
        } 
    }




    public void First()
    {
        var moveSpeed = speed * Time.deltaTime; 
        _player.transform.position = Vector3.MoveTowards(_player.transform.position, _gameObject[0].transform.position, moveSpeed);
        _booleanCheck[2] = true;
    }

    public void Second()
    {
        if (_booleanCheck[2] == true)
        {
            // First info panel
            _gameObject[1].SetActive(true);
            // First "forward" button
            _forwardButton[0].SetActive(true);
        }
    }

    public void Third()
    {
        _gameObject[1].SetActive(false);
        _booleanCheck[2] = false;
        _animation[0].SetBool("PlayTubeSocket", true);
        // Activate second forward button 
        //_forwardButton[0].SetActive(false);
        //_forwardButton[1].SetActive(true);
    }




    public void CalibrateOne()
    {
        _gameObject[2].SetActive(true);
    }
    public void CalibrateTwo()
    {
        _gameObject[2].SetActive(false);
        _gameObject[3].SetActive(true);
    }
    public void CalibrateThree()
    {
        _gameObject[12].SetActive(true);
        _animation[3].enabled = true;
        _animation[4].enabled = true;
        _gameObject[3].SetActive(false);
        _gameObject[4].SetActive(true);
    }
    public void CalibrateFour()
    {
        _gameObject[4].SetActive(false);
        _gameObject[5].SetActive(true);
        digitalDisplayOn = true;
    }




    public void Fourth()
    {
        _booleanCheck[0] = true;       
        _animation[1].SetBool("PlayHose", true);
        //_forwardButton[1].SetActive(false);
        //_forwardButton[2].SetActive(true);
    }

    public void Fifth()
    {
        _booleanCheck[1] = true;
        //_forwardButton[2].SetActive(false);
        //_forwardButton[3].SetActive(true);
        _gameObject[6].SetActive(true);
        _gameObject[5].SetActive(false);
        _animation[2].SetBool("PlayTester", true);
    }

    public void Sixth()
    {
        _booleanCheck[3] = false;
        _gameObject[6].SetActive(false);
        _gameObject[7].SetActive(true);      
        _animation[0].SetBool("PlayTubeSocketSwitch", true);
        _animation[1].SetBool("PlayHoseSwitch", true);
        updateDigitalDisplay = true;
        calibrateDigitalDisplay = false;
        targetPressureValue = -0.7f;
    }

    public void Seventh()
    {
        _gameObject[7].SetActive(false);
        _gameObject[8].SetActive(true);
        _animation[5].SetBool("BoltPlay", true);
        _animation[7].SetBool("Bolt2Play", true);
    }

    public void Eight()
    {
        _gameObject[8].SetActive(false);
        _gameObject[9].SetActive(true);
        _animation[6].SetBool("HexkeyPlay", true);
    }

    public void Ninth()
    {
        _gameObject[9].SetActive(false);
        _gameObject[10].SetActive(true);
        _booleanCheck[4] = true;
        _animation[7].SetBool("BoltSecondPlay", true);
        _animation[6].SetBool("HexkeySecondPlay", true);
    }

    public void DetachEverything()
    {
        _gameObject[9].SetActive(false);
        _gameObject[10].SetActive(true);
        _animation[0].SetBool("ExitTubeSocket", true);
        _animation[1].SetBool("ExitHose", true);
        _animation[2].SetBool("ExitTester", true);
        calibrateDigitalDisplay = true;
        targetPressureValue = 0.0f;
    }

}
