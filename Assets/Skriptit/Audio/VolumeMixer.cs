using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeMixer : MonoBehaviour
{
    [SerializeField] string mixerGroupParameter = "MasterVolume";
    [SerializeField] AudioMixer _mixer;
    [SerializeField] Slider _slider;
    [SerializeField] float _multiplier = 30f;

    private void Awake()
    {
        _slider.onValueChanged.AddListener(ValueHandler);
    }

    private void ValueHandler(float value)
    {
        _mixer.SetFloat(mixerGroupParameter, value:Mathf.Log10(value) * _multiplier);
    }

    private void OnDisable() 
    {
        PlayerPrefs.SetFloat(mixerGroupParameter, _slider.value);
    }

    void Start()
    {
        _slider.value = PlayerPrefs.GetFloat(mixerGroupParameter, _slider.value);
    }


    void Update()
    {
        
    }
}
