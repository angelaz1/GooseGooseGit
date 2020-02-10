using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Up_n_down : MonoBehaviour
{
    public float amplitude;
    public float period;

    private Vector3 init_pos;
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        init_pos = transform.position;
        offset = Random.value * 2*period;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = init_pos;
        pos.y += amplitude * Mathf.Cos((Time.time*period) + offset);
        transform.position = pos;
    }
}
