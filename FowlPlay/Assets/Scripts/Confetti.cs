using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    public GameObject conf;

    public Manager man;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            Instantiate(conf, transform.position, conf.transform.rotation);
            other.GetComponent<Ball>().Score();
            other.GetComponent<Ball>().Death();

            if (other.GetComponent<Ball>().player1_ball)
            {
                man.Blue_Score();
            }
            else
            {
                man.Red_Score();
            }
            man.player1_off = other.GetComponent<Ball>().player1_ball;
        }
    }
}
