using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bush : MonoBehaviour
{
    public GameObject leaf_part;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(leaf_part, transform.position, leaf_part.transform.rotation);
    }
}
