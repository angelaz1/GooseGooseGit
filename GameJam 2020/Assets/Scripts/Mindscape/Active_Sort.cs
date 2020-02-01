using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active_Sort : MonoBehaviour
{
    public Transform center;

    public int offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-center.position.y * 100 + offset);
    }

    public void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(-center.position.y * 100 + offset);
    }
}
