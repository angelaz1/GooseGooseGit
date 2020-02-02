using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective_Controller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Spaceship_Controller>() != null)
        {
            Debug.Log("Objective Reached! Transition back to patient....");
            Time.timeScale = 0;
        }
    }
}
