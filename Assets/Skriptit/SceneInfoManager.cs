/******************************************************************************
 * File        : SceneInfoManager.cs
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

using UnityEngine;
using Valve.VR;


public enum Language { 
    Finnish, 
    English, 
    Swedish 
}

/// <summary>
/// Moves between scenes (starting from Main Menu) and carries relevant information between scenes.
/// Also handles the transitions between scenes by utilising SteamVR_LoadLevel.
/// </summary>
public class SceneInfoManager : MonoBehaviour
{

    // --- --- --- Variables --- --- --- 

    public static SceneInfoManager Current { get; private set; }
    public Transform Player { get; set; }


    [Header("Language")]
    public Language language;
    public bool finnishEnabled = true;
    public bool englishEnabled = true;
    public bool swedishEnabled = false;


    [Header("Pause Menu Settings")]
    public float volume = 0.5f;
    public bool paused = false;
    public bool CanPause { get; protected set; } = true;


    [Header("Scene Selection")]
    public SteamVR_LoadLevel loader;
    public bool Loading { get; protected set; } = false;
    public int currentScene;
    public string[] levelNames = new string[] {
        "Main_Menu_LOBBY",
        "NIBE_FULL_2022",
        "AIRPUMP_SCENE"
    };
    public SelectedTaskList nibeFullSelectedTask;
    public bool InfoLabelsVisible { get; set; } = false;
    Vector3 recordedPlayerPos;
    Quaternion recordedPlayerRot;




    // --- --- --- Delegates and Events --- --- --- 

    public delegate void OnLanguageChange (Language _language);
    public static event OnLanguageChange OnLangChange;

    public delegate void OnVolumeChange (float _volume);
    public static event OnVolumeChange OnVolChange;

    public delegate void OnPauseChange (bool _paused);
    public static event OnPauseChange OnPause;




    // --- --- --- Methods --- --- --- 

    // --- MonoBehaviour ---

    private void Awake ()
    {
        if (Current == null) {
            Current = this;
        } else {
            if (Current != this) Destroy(gameObject);
        }    
        if(null == loader) {
            loader = GetComponent<SteamVR_LoadLevel>();       
        }
        SteamVR_Events.Loading.AddListener(SceneLoadInProgress);
    }
    private void Start ()
    {
        SetLanguage((int)language);
    }




    // --- Scene Loading ---

    /// <summary>
    /// Called when SteamVR_LoadLevel starts or finishes loading.
    /// </summary>
    /// <param name="_loading"></param>
    private void SceneLoadInProgress (bool _loading)
    {
        Loading = _loading;
        SetPauseable(!_loading,"SceneLoadInProgress");
        if (!_loading) {
            if (paused) {
                recordedPlayerPos = Player.position;
                recordedPlayerRot = Player.rotation;
                TogglePause();
            }
        }
    }

    /// <summary>
    /// Go to a specific scene (as set in Build Settings).
    /// Use SteamVR_LoadLevel to make the transition smooth. 
    /// SteamVR_LoadLevel uses strings by default, so levelNames-array contains the names of scenes in the same order as they appear in Build Settings. 
    /// </summary>
    /// <param name="_sceneIndex"></param>
    public void GoToScene (int _sceneIndex)
    {
        currentScene = _sceneIndex;
        SetPauseable(false, "GoToScene" );
        //ResetRecordedPosAndRot();

        //Debug.Log("Attempting to go to " + levelNames[_sceneIndex]);
        loader.Trigger(levelNames[_sceneIndex]);
    }

    /// <summary>
    /// Close the application.
    /// If in Editor, quit play mode instead.
    /// </summary>
    public void QuitGame ()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    // --- Language and Volume Control ---

    /// <summary>
    /// Called when a language button is clicked. The change is broadcasted as an event.
    /// </summary>
    /// <param name="_langIndex"></param>
    public void SetLanguage (int _langIndex)
    {
        language = (Language)_langIndex;
        OnLangChange?.Invoke(language);
    }

    /// <summary>
    /// Return a boolean value for each language. True is returned if the language is enabled and therefore selectable.
    /// </summary>
    /// <returns></returns>
    public bool[] GetEnabledLanguages ()
    {
        return new bool[3] { finnishEnabled, englishEnabled, swedishEnabled };
    }

    /// <summary>
    /// Called from UI. Adjusts the current volume. 
    /// </summary>
    /// <param name="_volChange"></param>
    public void AdjustVolume (float _volChange)
    {
        volume = Mathf.Clamp01(volume + _volChange);
        OnVolChange?.Invoke(volume);
    }


    // --- Pause Control ---

    /// <summary>
    /// Pause the game if it is currently allowed.
    /// Teleport the player to Pause Area.
    /// </summary>
    public void TogglePause ()
    {

        if (!CanPause) return;
        if (Loading) return;
             
        paused = !paused;
        OnPause?.Invoke(paused);


        if (paused) {

            recordedPlayerPos = Player.position;
            recordedPlayerRot = Player.rotation;
            Player.position = Vector3.up * -10f;
            if (currentScene != 0) 
                Player.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

        } else {

            Player.position = recordedPlayerPos;
            if(currentScene != 0)
                Player.rotation = recordedPlayerRot;

        }

        //Debug.Log("AT END: Paused=" + paused + ", Scene=" + currentScene + ", pos=" + Player.position + ", recordedPos=" + recordedPlayerPos);
    }

    /// <summary>
    /// Set pauseable-boolean state.
    /// </summary>
    /// <param name="_pauseable"></param>
    /// <param name="_debug"></param>
    public void SetPauseable (bool _pauseable, string _debug)
    {
        CanPause = _pauseable;
    }
}
