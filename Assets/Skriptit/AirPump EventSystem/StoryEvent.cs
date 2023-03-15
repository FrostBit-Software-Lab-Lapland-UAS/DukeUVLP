using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Used with AirpumpStoryController to display information on UI and execute animations and effects.
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName ="StoryEvent", menuName ="StoryEvents/New StoryEvent", order =1)]
public class StoryEvent : ScriptableObject
{


    // --- --- --- --- --- Variables --- --- --- --- ---

    [Header("Setup Settings")]
    public int id;                                                      // Unique storyID used for identification.

    [Space(20f)]
    [Header("Main Panel Settings")]                                     // Main panel contains text-based information about the process.
    public bool showMainPanel = false;
    public string eventNameFin;
    public string eventNameEng;
    public string eventNameSwe;
    [Space(10f)]
    [TextArea(1, 30)] public string panelTextFin;
    [TextArea(1, 30)] public string panelTextEng;
    [TextArea(1, 30)] public string panelTextSwe;
    public int panelTextFontSize;

    [Space(20f)]
    [Header("Arrow Button Settings")]                                   // Arrow buttons allow the user to progress through the events.
    public bool nextArrowActive = false;
    [TextArea(1, 10)] public string nextArrowInfoTextFin;
    [TextArea(1, 10)] public string nextArrowInfoTextEng;
    [TextArea(1, 10)] public string nextArrowInfoTextSwe;

    [Space(20f)]
    public bool prevArrowActive = false;
    [TextArea(1, 10)] public string prevArrowInfoTextFin;
    [TextArea(1, 10)] public string prevArrowInfoTextEng;
    [TextArea(1, 10)] public string prevArrowInfoTextSwe;
    public int arrowInfoTextFontSize;

    [Space(20f)]
    [Header("Rectangular Button Settings")]                             // Rectangular button is used to perform simple tasks such as starting and concluding the tutorial.
    public bool rectangularButtonActive = false;
    [Tooltip("Allowed commands:" +
        "\nALOITA" +
        "\nLOPETA")]
    public string rectangularButtonAction;
    [TextArea(3, 10)] public string rectangularButtonTextFin;
    [TextArea(3, 10)] public string rectangularButtonTextEng;
    [TextArea(3, 10)] public string rectangularButtonTextSwe;
    public int rectangularButtonFontSize;

    [Space(20f)]
    [Header("Animations and Effects (tooptip with more information)")]
    [Tooltip("Syntax: \n [Animator transform].[Parameter name] [value]" +
        "\n\n Example: \n PressureTester.PlayTester true")]
    public List<string> animList = new List<string>();                  // Animations performed during the SE are stored here.

    [Tooltip("Allowed commands: " +
        "\n DigitalDisplay.Power [true/false] " +
        "\n DigitalDisplay.CalibrationIcons [true/false]" +
        "\n DigitalDisplay.Pressure [value] " +
        "\n DigitalDisplay.ForcePressure [value] " +
        "\n DigitalDisplay.Speed [value]" +
        "\n\n REV. denotes effect as playable when going backwards. " +
        "\n\n delay followed by a value delays the effect's activation. " +
        "\n Example: REV.DigitalDisplay.Pressure 10.0 delay 2.5")]
    public List<string> effectList = new List<string>();                // Effects such as turning the digital display on or changing the pressure value are stored here.




    /// <summary>
    /// Return a text string of a selected type and language.
    /// </summary>
    /// <param name="_stType"></param>
    /// <param name="_lang"></param>
    /// <returns></returns>
    public string GetTextByLanguage (StoryTextType _stType, Language _lang)
    {
        string[] nameInfos = new string[3] { eventNameFin, eventNameEng, eventNameSwe };
        string[] panelInfos = new string[3] { panelTextFin, panelTextEng, panelTextSwe };
        string[] nextInfos = new string[3] { nextArrowInfoTextFin, nextArrowInfoTextEng, nextArrowInfoTextSwe };
        string[] prevInfos = new string[3] { prevArrowInfoTextFin, prevArrowInfoTextEng, prevArrowInfoTextSwe };
        string[] rectInfos = new string[3] { rectangularButtonTextFin, rectangularButtonTextEng, rectangularButtonTextSwe };

        switch (_stType)
        {

            case (StoryTextType.Headline):
                return nameInfos[(int)_lang];

            case (StoryTextType.Ordinary):
                return panelInfos[(int)_lang];

            case (StoryTextType.NextInfoText):
                return nextInfos[(int)_lang];

            case (StoryTextType.PrevInfoText):
                return prevInfos[(int)_lang];

            case (StoryTextType.RectText):
                return rectInfos[(int)_lang];

            default:
                return "";
        }
    }


}
