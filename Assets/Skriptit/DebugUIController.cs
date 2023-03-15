/******************************************************************************
 * File        : DebugUIController.cs
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
 *****************************************************************************/

using UnityEngine;


/// <summary>
/// Debugging and testing tool used for showing information and enabling button clicks.
/// </summary>
public class DebugUIController : MonoBehaviour
{


    public bool lookAtPlayer = false;
    public bool followHead = false;
    [Range(0.1f, 1f)] public float followHeadDistance = 0.5f;
    [Range(0.2f, 3f)] public float debugUIScale = 1f;

    private TMPro.TextMeshProUGUI debugTextObj;
    private Camera vrCam;
    private Transform scaleObjTransform;


    public delegate void OnButtonAClick();
    public static event OnButtonAClick OnAClicked;
    public delegate void OnButtonBClick();
    public static event OnButtonBClick OnBClicked;
    public delegate void OnButtonCClick();
    public static event OnButtonCClick OnCClicked;




    void Start()
    {
        vrCam = GameObject.Find("VRCamera").GetComponent<Camera>();
        scaleObjTransform = GameObject.Find("ScaleObject (DebugUI)").transform;
        debugTextObj = GameObject.Find("DebugText").GetComponent<TMPro.TextMeshProUGUI>();
    }


    void Update()
    {
        if (followHead) MoveUIToPlayerFront();
        if (lookAtPlayer) LookAtPlayer();

        scaleObjTransform.localScale = Vector3.one * 0.001f * debugUIScale;
    }


    void LookAtPlayer ()
    {
        Vector3 fixedTargetPosition = vrCam.transform.position;
        scaleObjTransform.LookAt(fixedTargetPosition);
    }


    void MoveUIToPlayerFront ()
    {
        Vector3 loc = vrCam.transform.position + vrCam.transform.forward * followHeadDistance;
        scaleObjTransform.parent.position = loc;
    }



    public void SetDebugText (string _s)
    {
        debugTextObj.text = _s;
    }


    public void ButtonAClicked()
    {
        Debug.Log("Button A was clicked.");
        OnAClicked?.Invoke();
    }
    public void ButtonBClicked()
    {
        Debug.Log("Button B was clicked.");
        OnBClicked?.Invoke();
    }
    public void ButtonCClicked()
    {
        Debug.Log("Button C was clicked.");
        OnCClicked?.Invoke();
    }


}
