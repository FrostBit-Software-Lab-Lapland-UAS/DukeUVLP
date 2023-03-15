using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeakingWaterInfo : MonoBehaviour
{

    public GameObject Infopanel;



    void OnTriggerEnter(Collider col){

        if (col.CompareTag("Box")){

            Infopanel.SetActive(false);

        }


    }

    void OnTriggerExit(Collider col) {
        
        if (col.CompareTag("Box")){

            Infopanel.SetActive(true);

        }

    }



}
