using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMove : MonoBehaviour
{
    private float speed = 20.0f;
    private float offset = 20.0f;

    public GameObject player;
    public float maxZ;
    public float gateLeft;
    public float gateRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateSheep();
    }

    void EvaluateSheep()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 5)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x += (currentPosition.x - player.transform.position.x)/10;
            currentPosition.z += (currentPosition.z - player.transform.position.z) / 10;
            transform.position = currentPosition;
        }

        if (transform.position.z > maxZ && transform.position.x > gateLeft && transform.position.x < gateRight)
        {
            Vector3 currentPosition = transform.position;
            currentPosition.z += .2f;
            transform.position = currentPosition;
        }
    }

}
