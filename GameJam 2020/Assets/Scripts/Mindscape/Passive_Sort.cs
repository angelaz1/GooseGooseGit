using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive_Sort : MonoBehaviour
{
    public Transform center;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-center.position.y * 100);
    }
}
