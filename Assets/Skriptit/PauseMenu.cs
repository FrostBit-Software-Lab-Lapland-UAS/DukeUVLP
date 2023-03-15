/******************************************************************************
 * File        : PauseMenu.cs
 * Version     : 1.0
 * Author      : Miika Puljuj‰rvi (miika.puljujarvi@lapinamk.fi)
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
 *****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the toggling of Pause.
/// </summary>
public class PauseMenu : MonoBehaviour
{

    // --- --- --- Variables --- --- ---

    // --- Main level ---
    [Header("Settings")]
    [Range(0.25f, 2f)] public float scale = 1f;
    [Range(0.25f, 1.5f)] public float distanceFromPlayer = 0.75f;
    [Range(-1f, 0.5f)] public float positionalHeightOffset = -0.5f;
    [Range(-0.5f, 0.5f)] public float lookAtHeightOffset = -0.25f;
    public bool lookAtPlayer = false;
    public Canvas pauseMenuCanvas;
    public Button resumeBtn;
    public Button settingsBtn;
    public Button toMainMenuBtn;
    public Button returnBtn;
    public bool canPause = true;


    [Space(20f)]
    // --- Settings-specific ---
    public RectTransform settingsPanel;
    public TextMeshProUGUI volumeTextObj;
    public RectTransform volumeLevelIndicatorObj;


    // --- Texts ---
    // Button and label texts are stored in string arrays. The ordering corresponds to Language enum's order of Finnish, English, Swedish.
    private string[] resumeButtonText = new string[]        { "Jatka",              "Continue",     "[swe] jatka" };
    private string[] settingsButtonText = new string[]      { "Asetukset",          "Settings",     "[swe] asetukset" };
    private string[] toMainMenuButtonText = new string[]    { "P‰‰valikko",         "Main Menu",    "[swe] p‰‰valikko" };
    private string[] quitButtonText = new string[]          { "Lopeta",             "Quit",         "[swe] lopeta" };

    private string[] returnButtonText = new string[]        { "Palaa",              "Return",       "[swe] palaa" };
    private string[] volumeText = new string[]              { "ƒ‰nenvoimakkuus",    "Volume",       "[swe] ‰‰nenvoimakkuus" };




    // --- --- --- Methods --- --- ---

    // --- MonoBehaviour ---
    private void OnEnable ()
    {
        SceneInfoManager.OnLangChange += LanguageChanged;
    }
    private void OnDisable ()
    {
        SceneInfoManager.OnLangChange -= LanguageChanged;
    }
    private void Update ()
    {
        if (lookAtPlayer) LookAtPlayer();

        transform.localScale = Vector3.one * scale;
    }


    /// <summary>
    /// Called when SceneInfoManager changes language.
    /// </summary>
    /// <param name="_lang"></param>
    private void LanguageChanged (Language _lang)
    {
        UpdateTexts(_lang);
    }

    /// <summary>
    /// Show text with correct language.
    /// </summary>
    /// <param name="_lang"></param>
    private void UpdateTexts (Language _lang)
    {
        resumeBtn.GetComponentInChildren<TextMeshProUGUI>().text = resumeButtonText[(int)_lang];
        settingsBtn.GetComponentInChildren<TextMeshProUGUI>().text = settingsButtonText[(int)_lang];
        returnBtn.GetComponentInChildren<TextMeshProUGUI>().text = returnButtonText[(int)_lang];

        if(SceneInfoManager.Current.currentScene == 0) 
            toMainMenuBtn.GetComponentInChildren<TextMeshProUGUI>().text = quitButtonText[(int)_lang];
        else 
            toMainMenuBtn.GetComponentInChildren<TextMeshProUGUI>().text = toMainMenuButtonText[(int)_lang];


        volumeTextObj.text = volumeText[(int)_lang];
    }

    /// <summary>
    /// Toggle the Settings panel ON / OFF.
    /// This is called directly from button objects defined in the inspector.
    /// </summary>
    /// <param name="_on"></param>
    public void ToggleSettingsPanel (bool _on)
    {
        settingsPanel.gameObject.SetActive(_on);
        if(_on) UpdateVolumeIndicator();
    }

    /// <summary>
    /// Toggle the Pause Menu canvas ON / OFF. Update the Paused state at the same time.
    /// </summary>
    /// <param name="_on"></param>
    public void TogglePauseCanvas (bool _on)
    {
        if (_on) MoveCanvasToPlayerFront();
        pauseMenuCanvas.gameObject.SetActive(_on);
        ToggleSettingsPanel(false);
        UpdateTexts(SceneInfoManager.Current.language);
    }


    /// <summary>
    /// Place the menu in front of player.
    /// </summary>
    private void MoveCanvasToPlayerFront ()
    {
        Transform playerCam = SceneInfoManager.Current.Player.GetComponentInChildren<Camera>().transform;
        Vector3 pos = playerCam.position + Vector3.up * positionalHeightOffset;
        Vector3 dir = new Vector3(playerCam.forward.x, 0, playerCam.forward.z).normalized;
        transform.position = pos + dir * distanceFromPlayer;
        LookAtPlayer();

    }
    private void LookAtPlayer ()
    {
        Vector3 camPoint = SceneInfoManager.Current.Player.GetComponentInChildren<Camera>().transform.position;
        Vector3 lookDir = camPoint + Vector3.up * lookAtHeightOffset;
        transform.LookAt(lookDir);
    }


    /// <summary>
    /// Called from Settings panel's arrow buttons. Increases volume by 0.1f per click and clamps it between 0...1.
    /// </summary>
    /// <param name="_increase"></param>
    public void AdjustVolume (bool _increase)
    {
        SceneInfoManager.Current.AdjustVolume(_increase ? 0.1f : -0.1f);
        UpdateVolumeIndicator();
    }
    private void UpdateVolumeIndicator ()
    {
        volumeLevelIndicatorObj.localScale = new Vector3(SceneInfoManager.Current.volume, 1f, 1f);
    }


    /// <summary>
    /// Called when player presses the pause button. Toggles the pause state.
    /// </summary>
    public void ResumePressed ()
    {
        if (!SceneInfoManager.Current.CanPause) return;

        SceneInfoManager.Current.TogglePause();
        TogglePauseCanvas(SceneInfoManager.Current.paused);
    }

    /// <summary>
    /// Called when user clicks the MainMenu-button. 
    /// Note the ForceUnsubscribeToAll. If this is not done, event listeners will not be unsubscribed and will cause issues when re-entering a scene.
    /// </summary>
    public void ReturnToMainMenu ()
    {
        if(SceneInfoManager.Current.CanPause) {
            if (SceneInfoManager.Current.currentScene != 0) {
                if(null != TaskController.current)              TaskController.current.currentTask.currentSubtask.ForceUnsubscribeToAll();
                if (null != AirpumpStoryController.current)     AirpumpStoryController.current.ForceUnsubscribeToAll();
                SceneInfoManager.Current.GoToScene(0);
            } else {
                SceneInfoManager.Current.QuitGame();
            }
        } else {
            Debug.Log("Tried to go to Main Menu, but CanPause is false.");
        }

    }

}
