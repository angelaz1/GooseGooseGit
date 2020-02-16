using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Camera : MonoBehaviour
{

    float offset_z = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void LateUpdate()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        if (ball != null)
        {
            Vector3 ball_p = ball.transform.position;
            ball_p.z -= offset_z;
            transform.position = ball_p;
        }
    }
}
