using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    public GameObject conf;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            Instantiate(conf, transform.position, conf.transform.rotation);
            other.GetComponent<Ball>().Death();
        }
    }
}
