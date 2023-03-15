using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGL_Player : MonoBehaviour
{
    public GameObject webGLplayerObject;
    public GameObject vRplayer;
    // Start is called before the first frame update
    void Awake()
    {

        #if UNITY_WEBGL
            webGLplayerObject.SetActive(true);
            vRplayer.SetActive(false);
        #endif

        // if(Application.platform == RuntimePlatform.WebGLPlayer) {

        //     webGLplayerObject.SetActive(true);
        //     vRplayer.SetActive(false);

        // }
    //}

    }
}
