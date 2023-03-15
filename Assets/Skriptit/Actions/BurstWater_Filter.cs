using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstWater_Filter : MonoBehaviour
{
    public ParticleSystem _particles;
    public GameObject _infoText;

    public void OnTriggerEnter(Collider col){

        if(col.gameObject.tag == "Mutasihti"){
            _particles.Stop();
            _infoText.SetActive(false);
        }

    }

    public void OnTriggerExit(Collider other) {

        if(other.gameObject.tag == "Mutasihti") {
            _particles.Play();
            _infoText.SetActive(true);
        }


    }

}
