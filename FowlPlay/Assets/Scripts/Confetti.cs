using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    public GameObject conf;

    public Manager man;

    bool can_score = true;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball" && can_score)
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
            man.whomp.Play();

            man.player1_off = other.GetComponent<Ball>().player1_ball;

            StartCoroutine(Score());
        }
    }

    public IEnumerator Score()
    {
        can_score = false;
        yield return new WaitForSeconds(1);
        can_score = true;
    }
}
