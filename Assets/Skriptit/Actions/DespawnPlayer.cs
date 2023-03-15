using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnPlayer : MonoBehaviour
{
    public void DestroyAllGameObjects()
    {
        GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);

        for (int i = 0; i < GameObjects.Length; i++)
        {
            Destroy(GameObjects[i]);
        }


    }

}