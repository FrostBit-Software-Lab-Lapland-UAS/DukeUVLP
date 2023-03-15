using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayRotation2 : MonoBehaviour
{

    public GameObject gameobject;
    public TMP_Text rotationX;
    private Vector3 lastFwd;
    private float curAngleX = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rotationX = GetComponent<TMP_Text>();
        lastFwd = gameobject.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        var curFwd = gameobject.transform.forward;

        var ang = Vector3.Angle(curFwd, lastFwd);
        
        if(ang > 0.01f)
        {
            rotationX.SetText("X-akseli: " + curAngleX.ToString());
        }

        if(Vector3.Cross(curFwd, lastFwd).x < 0) ang = -ang;
        {
            curAngleX += ang;
            lastFwd = curFwd;

            

        }

    }
}
