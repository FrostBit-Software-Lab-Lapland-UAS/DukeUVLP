using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.2f * Time.deltaTime);
    }

    #pragma warning disable 649
    [SerializeField] Vector3 targetPos = new Vector3(-84.88f, -0.7f, -7.24f);
    #pragma warning restore 649
}

