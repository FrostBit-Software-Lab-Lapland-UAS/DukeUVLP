using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyHighlight : MonoBehaviour
{

    public Material _materialOne;
    public Material _materialTwo;
    public GameObject _safetyvalve;

    private float timer = 3;
    private bool _buttonClick;

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0 && _buttonClick == true)
        {
            timer -= Time.deltaTime;
            _safetyvalve.GetComponent<MeshRenderer>().material = _materialOne;
        }

        if (timer <= 0 && _buttonClick == true)
        {
            _safetyvalve.GetComponent<MeshRenderer>().material = _materialTwo;
            timer = 3;
            _buttonClick = false;

        }
    }

    public void ButtonClicked()
    {
        _buttonClick = true;


    }
}
