using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    float y_pos;

    public float z_pos = 0.1f;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        y_pos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.position;
        pos.y = y_pos;
        pos.z += z_pos;
        transform.position = pos;
    }
}
