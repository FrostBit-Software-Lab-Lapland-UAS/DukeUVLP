/******************************************************************************
 * File        : AirpumpStoryController.cs
 * Version     : 1.0
 * Author      : Miika Puljujärvi (miika.puljujarvi@lapinamk.fi)
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
 * This script utilises Valve's SteamVR-plugin, which can be downloaded from Unity Asset Store.
 * 
 *****************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Valve.VR;
using UnityEngine.SceneManagement;


public enum StoryTextType { Headline, Ordinary, NextInfoText, PrevInfoText, RectText }


/// <summary>
///  This script allows the user to progress through the process by cycling through pre-defined by StoryEvents.
///  StoryEvents are scriptableObjects that can be created and placed in a list (storyEvents).
///  Each SE can contain one or more animations and effects, which are parsed from strings (animLists & effectList) and played when the SE is activated.
/// </summary>
public class AirpumpStoryController : MonoBehaviour
{

    // --- --- --- --- --- Variables --- --- --- --- ---

    [Header("Setup")]
    //public Language language = Language.Finnish;                                // Which language is currently being used / displayed?
    public static AirpumpStoryController current;                               
    public Canvas mainCanvas;                                                   // Canvas containing main text, arrow buttons, arrow info and rectangular button.
    public RectTransform mainPanel;                                             // Main text background object.
    public TMPro.TextMeshProUGUI panelTextObj;                                  // Text object responsible for displaying the main text.
    public Button nextArrowBtn;                                                 // Arrow button responsible for moving the story forward.
    public Button prevArrowBtn;                                                 // Arrow button responsible for moving the story backward.
    public TMPro.TextMeshProUGUI arrowInfoTextObj;                              // Text object responsible for displaying info when user hovers over an arrow button.
    public RectTransform arrowInfoPanel;                                        // Panel containing arrow info text. Visible when either arrow button is hovered over.
    public Button rectangularButton;                                            // Button performing tasks defined in the current SE's RectangularButtonAction.
    public TMPro.TextMeshProUGUI rectangularBtnText;                            // Text object responsible for displaying info on the rectangular button.
    public bool hoverOnNextArrow = false;
    public bool hoverOnPrevArrow = false;
    public GameObject digitalDisplay;                                           // PressureTester's digital display reference.
    public GameObject calibrationIconContainer;                                 // Empty containing two calibration icons. Toggled on/off through code.
    public TMPro.TextMeshProUGUI[] digitalDisplayText = new TMPro.TextMeshProUGUI[2];
    public Button[] languageButtons = new Button[3];
    
    
    [Space(20f)] public bool faceCanvasToPlayer = false;                        // Does the canvas turn to face the player when they move?
    [Range(-1f, 1f)] public float canvasFacingHeightAdjustment = 0f;            // How high does the canvas face in relation to the HMD?
    [Range(0.1f, 2f)] public float canvasScale = 1f;                            // Adjusts the scale of the UI elements.
    private Transform containerTransform;                                       // Empty containing main canvas. Used with LookAt() to face the user.
    private Camera vrCam;                                                       // Current camera (user's HMD).


    [Header("Stories")]
    public List<StoryEvent> storyEvents = new List<StoryEvent>();               // List containing every StoryEvent in the scene. 
    public StoryEvent currentStoryEvent;                                        // StoryEvent that is currently being displayed.
    private int currentStoryIndex = 0;                                          // Index on the StoryEvents list of the current StoryEvent.

    [Header("Color Settings")]
    public Color backgroundColor;                                               // Color of panel backgrounds.
    public Color textColor;                                                     // Color of texts displayed on top of panels.
    public Color btnColor;                                                      // Color of button backgrounds.
    public Color btnTextColor;                                                  // Color of texts displayd on top of buttons. 


    private float targetPressure = 0f;                                          // Target value to which currentPressure attempts to settle.
    private float currentPressure = 0f;                                         // Current pressure value. 
    private float pressureChangeSpeed = 1f;                                     // How fast does the pressure change?
    private bool digitaldisplayOn = false;                                      // Is the digital display on or off?

    // http://digitalnativestudios.com/textmeshpro/docs/rich-text/#size contains the syntax for rich text used here.
    private string headlineFormatOn = "<b><size=130%><uppercase><cspace=0em><color=#FF5032>";
    private string headlineFormatOff = "</b><size=100%></uppercase></cspace><color=#";
    private string textColorHex;




    // --- --- --- --- --- Structs --- --- --- --- ---

    /// <summary>
    /// Carries information about animator and its parameters from StoryEvent's animation parser.
    /// </summary>
    public struct AnimPackage {
        public AnimPackage(Animator _animator, AnimationTrigger[] _triggers)
        {
            Animator = _animator;
            Triggers = _triggers;
        }

        public Animator Animator;
        public AnimationTrigger[] Triggers;
    }


    /// <summary>
    /// Animator parameter name paired with the boolean value. Used for controlling animations.
    /// </summary>
    public struct AnimationTrigger
    {
        public AnimationTrigger(string _name, bool _bool)
        {
            Name = _name;
            Bool = _bool;
        }

        public string Name;
        public bool Bool;
    }




    // --- --- --- --- --- General Functions --- --- --- --- ---

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        PointerController.OnEnter += PointingOnElementStarted;
        PointerController.OnExit += PointingOnElementEnded;
        SceneInfoManager.OnLangChange += LanguageChanged;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        PointerController.OnEnter -= PointingOnElementStarted;
        PointerController.OnExit -= PointingOnElementEnded;
        SceneInfoManager.OnLangChange -= LanguageChanged;
    }
    public void ForceUnsubscribeToAll ()
    {
        PointerController.OnEnter -= PointingOnElementStarted;
        PointerController.OnExit -= PointingOnElementEnded;
        SceneInfoManager.OnLangChange -= LanguageChanged;
    }


    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        containerTransform = GameObject.Find("MainPanelContainer").GetComponent<Transform>();
        vrCam = GameObject.Find("VRCamera").GetComponent<Camera>();

        if (null == SceneInfoManager.Current) {
            Instantiate(Resources.Load("SceneInfoManager", typeof(GameObject)) as GameObject);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        current = this;
        if (null == SceneInfoManager.Current) {
            Instantiate(Resources.Load("SceneInfoManager", typeof(GameObject)) as GameObject);
        }


        TransitionFader.Current.FadeOutBlack(1f);
        SetUIColors();

        // Set the first SE of the storyEvents list as current SE. If the list is empty, quit application.
        if (currentStoryEvent == null) {
            if (storyEvents.Count == 0) {
                Debug.Log("Zero StoryEvents in the list. The application is shutting down.");
                QuitGame();
            } else {
                SetStoryEvent(storyEvents[0],true);
            }
        }

        //language = SceneInfoManager.Current.language;
    }


    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if(digitaldisplayOn) {
            UpdatePressureValue();
        }

        if(faceCanvasToPlayer) {
            Vector3 fixedTargetPosition = vrCam.transform.position + Vector3.up * canvasFacingHeightAdjustment;
            containerTransform.LookAt(fixedTargetPosition);
        }
    }


    /// <summary>
    /// Quit the application.
    /// </summary>
    void QuitGame ()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }



    // --- --- --- Color Functions --- --- ---

    /// <summary>
    /// Set UI colors. Colors can be defined in the inspector. 
    /// </summary>
    private void SetUIColors ()
    {
        mainPanel.GetComponent<Image>().color = backgroundColor;
        panelTextObj.color = textColor;
        rectangularButton.GetComponent<Image>().color = btnColor;
        rectangularBtnText.color = btnTextColor;
        textColorHex = ColorUtility.ToHtmlStringRGB(textColor);
    }




    // --- --- --- StoryEvent Functions --- --- ---

    /// <summary>
    /// Move to the next StoryEvent if there are any.
    /// The ability to move to the next StoryEvent is controlled by the presence of nextArrowButton, which can be toggled for each StoryEvent.
    /// </summary>
    public void NextStoryEvent () 
    {
        // Do we have additional StoryEvents in the list to progress to?
        if((currentStoryIndex + 1) < storyEvents.Count) {
            currentStoryIndex++;
            SetStoryEvent(storyEvents[currentStoryIndex], true);
        } else {
            Debug.Log("No more StoryEvents that way!");
        }
    }


    /// <summary>
    /// Return to the previous StoryEvent if there are any.
    /// The ability to move to the previous StoryEvent is controlled by the presence of prevArrowButton, which can be toggled for each StoryEvent.
    /// </summary>
    public void PrevStoryEvent()
    {
        // Do we have additional StoryEvents in the list to progress to?
        if ((currentStoryIndex) > 0) {
            currentStoryIndex--;
            SetStoryEvent(storyEvents[currentStoryIndex], false);
        } else {
            Debug.Log("No more StoryEvents that way!");
        }
    }


    /// <summary>
    /// Parse the information of current StoryEvent's RectangularButtonAction to determine the outcome of pressing the button. 
    /// </summary>
    public void RectangularButtonPressed ()
    {
        switch (currentStoryEvent.rectangularButtonAction) {

            case "ALOITA":
            case "LOPETA":
                NextStoryEvent();
                break;

            case "MAINMENU":
                SceneInfoManager.Current.GoToScene(0);
                break;

            default:
                break;
        }
    }


    /// <summary>
    /// Change the current StoryEvent to one determined by NextStoryEvent or PrevStoryEvent. 
    /// Update text and button states, parse and play animations and effects of the new StoryEvent.
    /// </summary>
    private void SetStoryEvent (StoryEvent _se, bool _forward)
    {     
        currentStoryEvent = _se;
        UpdateTexts(_se);

        // Control the visibility of different UI elements by toggling them on/off.
        mainPanel.gameObject.SetActive(_se.showMainPanel);
        nextArrowBtn.transform.parent.gameObject.SetActive(_se.nextArrowActive);
        prevArrowBtn.transform.parent.gameObject.SetActive(_se.prevArrowActive);
        rectangularButton.transform.parent.gameObject.SetActive(_se.rectangularButtonActive);

        // Process through animation and effect lists.
        if (_forward) {
            foreach (string s in _se.animList) {
                PlayAnimation(s,true);
            }
        } else {
            StoryEvent prevEvent = storyEvents[currentStoryIndex + 1];
            foreach(string s in prevEvent.animList) {
                PlayAnimation(s,false);
            }
        }

        if (_se.effectList.Count != 0) {
            ParseEffectsList(_se, _forward);       
        }
    }


    /// <summary>
    /// Update UI texts to show correct information in the right language.
    /// </summary>
    /// <param name="_se"></param>
    public void UpdateTexts (StoryEvent _se)
    {
        Language lang = SceneInfoManager.Current.language;
        panelTextObj.text =
            headlineFormatOn +
            _se.GetTextByLanguage(StoryTextType.Headline, lang) +
            headlineFormatOff +
            textColorHex + ">\n" +
            _se.GetTextByLanguage(StoryTextType.Ordinary, lang);

        arrowInfoTextObj.text =
            hoverOnNextArrow ?
            _se.GetTextByLanguage(StoryTextType.NextInfoText, lang) :
            _se.GetTextByLanguage(StoryTextType.PrevInfoText, lang);

        rectangularBtnText.text = _se.GetTextByLanguage(StoryTextType.RectText, lang);
        panelTextObj.fontSize = _se.panelTextFontSize;
        arrowInfoTextObj.fontSize = _se.arrowInfoTextFontSize;
        rectangularBtnText.fontSize = _se.rectangularButtonFontSize;
    }



    private void LanguageChanged (Language _lang)
    {
        //language = _lang;
        if(null != currentStoryEvent) UpdateTexts(currentStoryEvent);
    }





    // --- --- --- Arrow OnPointer Handling --- --- ---

    /// <summary>
    /// Listening to PointerController.OnEnter.
    /// When the event is triggered, check which button the target transform is, and display the arrow info panel with correct text to the user.
    /// </summary>
    public void PointingOnElementStarted (Transform _t)
    {
        Language lang = SceneInfoManager.Current.language;

        if (_t.gameObject == nextArrowBtn.gameObject) {
            arrowInfoPanel.gameObject.SetActive(true);
            arrowInfoTextObj.text = currentStoryEvent.GetTextByLanguage(StoryTextType.NextInfoText, lang);
            hoverOnNextArrow = true;
            hoverOnPrevArrow = false;
        } else if (_t.gameObject == prevArrowBtn.gameObject) {
            arrowInfoPanel.gameObject.SetActive(true);
            arrowInfoTextObj.text = currentStoryEvent.GetTextByLanguage(StoryTextType.PrevInfoText, lang);
            hoverOnPrevArrow = true;
            hoverOnNextArrow = false;
        }
    }


    /// <summary>
    /// Listening to PointerController.OnExit.
    /// When the event is triggered, hide the arrow info panel.
    /// </summary>
    public void PointingOnElementEnded (Transform _t)
    {
        arrowInfoPanel.gameObject.SetActive(false);
        hoverOnNextArrow = false;
        hoverOnPrevArrow = false;
    }




    // --- --- --- Animation and Effect Parsing --- --- ---  


    /// <summary>
    /// Play animations based on a string from StoryEvent.
    /// _forward controls the direction of animation; false means the animations must be flipped in order and value.
    /// </summary>
    public void PlayAnimation (string _str, bool _forward)
    {
        if (null == _str)
            return;
        AnimPackage aPack = _forward ? ParsePackage(_str) : FlipTriggers(ParsePackage(_str));

        foreach(AnimationTrigger t in aPack.Triggers) {
            aPack.Animator.SetBool(t.Name,t.Bool);
        }
    }

    /// <summary>
    /// The string from StoryEvent is parsed in the following way:
    /// '.' separates the object containing the animator and each following boolean parameters used to control the animation.
    /// ' ' separates the boolean name and value. 
    /// </summary>
    private AnimPackage ParsePackage (string _str)
    {
        string[] stringArray = _str.Split('.');
        Animator animator = GameObject.Find(stringArray[0]).GetComponent<Animator>();
        List<AnimationTrigger> triggers = new List<AnimationTrigger>();

        for(int i = 1; i < stringArray.Length; i++) {
            string[] trigger = stringArray[i].Split(' ');
            triggers.Add(new AnimationTrigger(trigger[0], trigger[1] == "true"));
        }

        return new AnimPackage(animator, triggers.ToArray());
    }


    /// <summary>
    /// Flip trigger order and booleans to animate the transition in reverse. 
    /// </summary>
    private AnimPackage FlipTriggers (AnimPackage _originalPackage)
    {
        List<AnimationTrigger> flippedTriggers = new List<AnimationTrigger>();

        for (int i = _originalPackage.Triggers.Length-1; i >= 0; i--) {
            AnimationTrigger ogTrigger = _originalPackage.Triggers[i];
            flippedTriggers.Add(new AnimationTrigger(ogTrigger.Name, !ogTrigger.Bool));
        }

        return new AnimPackage(_originalPackage.Animator, flippedTriggers.ToArray());
    }


    /// <summary>
    /// Read effects list of the StoryEvent. 
    /// Parse the information and move execution to relevant functions.
    /// Several of the functions called from this function are IEnumerators to facilitate delaying the execution.
    /// </summary>
    private void ParseEffectsList (StoryEvent _se, bool _forward)
    {
        // Check if the effect contains REV. keyword. If it does, execute it only if _forward is false. Otherwise ignore the effect. 
        // In the same manner, if _forward is true, ignore every effect with REV. on it.
        foreach (string s in _se.effectList) {
            string str = s;
            if((str.Contains("REV.") && !_forward) || (!str.Contains("REV.") && _forward)) {
                if(str.Contains("REV.")) { 
                    str = str.Remove(0, 4);                 
                }

                string[] effect = str.Split(' ');
                float delay = effect.Length > 2 ? float.Parse(effect[3]) : 0f;
                switch (effect[0]) {

                    case "DigitalDisplay.Power":
                        StartCoroutine(ToggleDigitalDisplay(bool.Parse(effect[1]), delay));
                        break;

                    case "DigitalDisplay.CalibrationIcons":
                        StartCoroutine(ToggleCalibrationIcons(bool.Parse(effect[1]), delay));
                        break;

                    case "DigitalDisplay.Pressure":
                        StartCoroutine(SetPressureValues(float.Parse(effect[1]), delay));
                        break;

                    case "DigitalDisplay.ForcePressure":
                        StartCoroutine(ForcePressureValues(float.Parse(effect[1]), delay));
                        break;

                    case "DigitalDisplay.Speed":
                        pressureChangeSpeed = float.Parse(effect[1]);
                        break;

                    default:
                        break;
                }
            }
        }
    }


    /// <summary>
    /// Set digital display on/off. 
    /// </summary>
    private IEnumerator ToggleDigitalDisplay(bool _on, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);
        digitaldisplayOn = _on;
        digitalDisplay.SetActive(_on);
        if (!_on)
            currentPressure = 0f;
    }


    /// <summary>
    /// Show/hide calibration icons on PressureTester.
    /// </summary>
    private IEnumerator ToggleCalibrationIcons(bool _on, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);
        calibrationIconContainer.SetActive(_on);
    }


    /// <summary>
    /// Set target pressure value. UpdatePressureValue() is responsible for reaching the target pressure.
    /// </summary>
    private IEnumerator SetPressureValues(float _targetValue, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);
        targetPressure = _targetValue;
    }


    /// <summary>
    /// Change pressure to target value instantaneously.
    /// </summary>
    private IEnumerator ForcePressureValues (float _targetValue, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);
        targetPressure = _targetValue;
        currentPressure = targetPressure;
        SetDigitalDisplayText();
    }


    /// <summary>
    /// Calculate the pressure change every update cycle.
    /// </summary>
    private void UpdatePressureValue ()
    {
        bool increasePressure = (currentPressure < targetPressure) ? true : false;
        float valueChange = Time.deltaTime * pressureChangeSpeed; 
        currentPressure = increasePressure ? currentPressure + valueChange : currentPressure - valueChange;
        if ((increasePressure && currentPressure > targetPressure) || (!increasePressure && currentPressure < targetPressure))
            currentPressure = targetPressure;
        SetDigitalDisplayText();
    }


    /// <summary>
    /// Update the text of two text components on PressureTester's digital display.
    /// The text is divided into two to improve readability. 
    /// The first text component displays integers and is justified to right. 
    /// The second text component displays decimals and is justified to left.
    /// </summary>
    private void SetDigitalDisplayText ()
    {
        string str = string.Format("{0:F2}", currentPressure);
        string[] s = str.Split('.');
        digitalDisplayText[0].text = s[0];
        digitalDisplayText[1].text = "." + s[1];
    }

}
