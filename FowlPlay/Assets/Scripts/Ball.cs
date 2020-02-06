using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            Vector3 reflect = Vector3.Reflect(rb.velocity, other.transform.forward);
            reflect /= 4;
            rb.velocity = reflect;
        }
    }
}
