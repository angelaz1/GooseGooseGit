using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    public Transform hoop;
    public Transform player_off;
    public Transform player_def;

    bool has_reflected = false;

    bool clean_shot = false;

    public float reflect_dist;

    public GameObject shadow;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        float dist = (new Vector3(player_off.position.x, 0, player_off.position.z) - new Vector3(player_def.position.x, 0, player_def.position.z)).magnitude;

        Debug.Log("Player dist: " + dist);

        Vector3 player_diff = player_def.position - player_off.position;
        Vector3 hoop_vector = hoop.position - player_off.position;

        float dot = Vector3.Dot(player_diff, hoop_vector);

        Debug.Log("dot: " + dot);

        if (dist > reflect_dist || dot <=-1) clean_shot = true;
        
        if(player_off.GetComponent<PlayerController>().is_jumping && !player_def.GetComponent<PlayerController>().is_jumping)
        {
            Debug.Log("jumpshot");
            clean_shot = true;
        }
    }

    public void Death()
    {
        Destroy(shadow, 5f);
        Destroy(gameObject, 5f);
    }

    public void Update()
    {
        if (!has_reflected && !clean_shot)
        {
            float dist_p = (new Vector3(hoop.position.x, 0, hoop.position.z) - new Vector3(player_def.position.x, 0, player_def.position.z)).magnitude;

            float dist_b = (new Vector3(hoop.position.x, 0, hoop.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).magnitude;

            if (dist_b <= dist_p)
            {
                Vector3 reflect = Vector3.Reflect(rb.velocity, player_def.GetComponent<PlayerController>().block.forward);
                reflect /= 4;
                rb.velocity = reflect;
                Death();
                has_reflected = true;
            }
        }
        
    }
}
