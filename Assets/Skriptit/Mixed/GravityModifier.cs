using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityModifier : MonoBehaviour
{


    public float x;
    public float y;
    public float z;
    private Rigidbody rb;
    public bool updateVelocity;


    private void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!updateVelocity) return;
        if(null != rb) {
            rb.AddForce(new Vector3(
                (x + (Physics.gravity.x * -1f)),
                (y + (Physics.gravity.y * -1f)),
                (z + (Physics.gravity.z * -1f))) * 
                Time.fixedDeltaTime, ForceMode.Acceleration);
        }
            
    }
}


