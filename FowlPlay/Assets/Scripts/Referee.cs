using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referee : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    float target_x;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target_x = (player1.position.x + player2.position.x) / 2;

        transform.position = Vector3.Lerp(transform.position, new Vector3(target_x, 1, transform.position.z),0.5f);
    }
}
