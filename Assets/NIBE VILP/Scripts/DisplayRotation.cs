using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayRotation : MonoBehaviour
{

//Funktiot ja liitettävät komponentit editorissa------------------------------------
    public GameObject myGameObject;
    public TextMeshProUGUI m_textMesh;
    public TextMeshProUGUI m_textMeshTwo;
    public TextMeshProUGUI m_textMeshThree;


//Akselien rotaatiot-desimaaleina (rotation on aina float)---------------------------------------------------------------
    float rotationX = 0f;
    float rotationY = 0f;
    float rotationZ = 0f;

    

    // Start is called before the first frame update
    void Start()
    {
        rotationX = myGameObject.transform.rotation.eulerAngles.x;
        rotationY = myGameObject.transform.rotation.eulerAngles.y;
        rotationZ = myGameObject.transform.rotation.eulerAngles.z;

        m_textMesh = GetComponent<TextMeshProUGUI>();
        m_textMeshTwo = GetComponent<TextMeshProUGUI>();
        m_textMeshThree = GetComponent<TextMeshProUGUI>();

    }


    void Update(){

// Rotaation päivittäminen canvakselle---------------------------------------------

        m_textMesh.SetText ("X-akseli: " + rotationX.ToString());
        m_textMeshTwo.SetText ("Y-akseli: " + rotationY.ToString());
        m_textMeshThree.SetText ( "Z-akseli: " + rotationZ.ToString());

        Debug.Log ("rotaatio X: " + rotationX);

// Rotaation päivittäminen canvakselle--------------------------------------------
        
        
    }
}
