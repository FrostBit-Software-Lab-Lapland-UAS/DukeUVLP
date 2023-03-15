using UnityEngine;

public class CameraFade : MonoBehaviour
{
    public Animator _FadeIn;
    public Animator _FadeOut;

    public GameObject _blackScreen;
    public GameObject _blackScreenTwo;

    public void FadeOutBlack ()
    {
        _blackScreen.SetActive(true);
        _blackScreenTwo.SetActive(false);
        _FadeOut.enabled = true;

    }

    public void FadeInBlack()
    {
        _blackScreen.SetActive(false);
        _blackScreenTwo.SetActive(true);
        _FadeIn.enabled = true;
    }
}