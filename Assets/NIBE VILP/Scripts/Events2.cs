using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections;
using UnityEngine.Events;
using TMPro;

public class Events2 : MonoBehaviour
{

    private int luku = 0;
    private int lukuplus = 1;

    bool nosta;

    private TextMesh textMesh;


    void start()
    {
        textMesh = GetComponent<TextMesh>();
    }
    public void OnPress(Hand hand)
    {
       
       
        luku += lukuplus;
        textMesh.text = luku.ToString();

            //luku += lukuplus;
            //textMesh.text = "Nro: " + luku;

        Debug.Log("SteamVR Button pressed!");    
                

    
    }

    public void OnCustomButtonPress()
    {
        
        Debug.Log("We pushed our custom button!");
    }
}