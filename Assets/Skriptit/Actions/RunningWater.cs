using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningWater : MonoBehaviour
{

public ParticleSystem Vesihana;

void Start() {
    
    Vesihana.Stop();

}

void OnTriggerEnter(Collider other) {
    
    if(other.tag == "Mutasihti"){

        Vesihana.Play();
    }

}

void OnTriggerExit(Collider other) {

    Vesihana.Stop(); 

}

}
