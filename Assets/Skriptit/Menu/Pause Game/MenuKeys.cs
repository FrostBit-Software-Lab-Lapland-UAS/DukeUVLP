using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuKeys : MonoBehaviour
{

public GameObject UserInterface;

void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                UserInterface.gameObject.SetActive(!UserInterface.gameObject.activeSelf);

            }
    }
}
