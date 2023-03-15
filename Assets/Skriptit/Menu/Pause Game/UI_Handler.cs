using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Audio;
using TMPro;


public class UI_Handler : MonoBehaviour
{
    [SerializeField] public AudioMixer AudioMixer;

    [SerializeField] public TMP_Dropdown ResolutionDropdown;

    [SerializeField] public GameObject UserInterface;

    [SerializeField] private bool isShowing;

    [SerializeField] Resolution[] resolutions;


void Start()
    {

        // Check what are all available resolutions for used monitor and makes an array of it
        resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions(); // clear premade list of resolutions 

        List<string> options = new List<string>(); // new list (string) for resolution options 

        int currentResolution = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option); //Adds available resolution options to the list

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolution = i;
                }
        }

        
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolution;
        ResolutionDropdown.RefreshShownValue();

    }

    public void SetResolution(int reso) // This applies default resolution of screen for game
    {
        Resolution resolution = resolutions[reso];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); // Checks what is screens default resolution
    }


    public void SetVolume(float volume)
    {
        AudioMixer.SetFloat("Volume", volume); // Adjust float value for volume between -80db - 0db
    }


    public void SetGraphic(int quality)
    {
        QualitySettings.SetQualityLevel(quality); //Set quality by its integer
    }


    public void SetFullScreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen; // Toggle for seeing if fullscreen is on or not 
    }


    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0); // Launch scene by its index number  
    }
}