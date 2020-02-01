using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    private float speed = 20.0f;
    private float offset = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EvaluatePlayer();
    }

    void EvaluatePlayer()
    {
        Vector3 currentPosition = transform.position;
        if (Input.GetKey("up"))
        {
            currentPosition.z += speed * Time.deltaTime;
        }

        if (Input.GetKey("down"))
        {
            currentPosition.z -= speed * Time.deltaTime;
        }

        //currentPosition.z = Mathf.Clamp(currentPosition.z, -offset, offset);

        if (Input.GetKey("right"))
        {
            currentPosition.x += speed * Time.deltaTime;
        }

        if (Input.GetKey("left"))
        {
            currentPosition.x -= speed * Time.deltaTime;
        }

        //currentPosition.x = Mathf.Clamp(currentPosition.x, -offset, offset);

        transform.position = currentPosition;
    }
}
