using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections;
using UnityEngine.Events;
using TMPro;

public class Events : MonoBehaviour
{
    private int luku = 0;
    private int lukuplus = 1;

    bool nosta;

    private TextMeshProUGUI textMesh;


    public Color color;

    void start()
    {

        //GameObject cube = GameObject.GreatePrimitive(PrimitiveType.cube);
       // var cubeRenderer = cube.GetComponent<Renderer>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }
    public void OnPress(Hand hand)
    {
       
       
        luku += lukuplus++;
        textMesh.text = luku.ToString();

            //luku += lukuplus;
            //textMesh.text = "Nro: " + luku;

        Debug.Log("SteamVR Button pressed!");    
                

    
    }

    public void OnCustomButtonPress()
    {
        
        transform.GetComponent<Renderer>().material.color = color;
        
        Debug.Log("We pushed our custom button!");
    }
}