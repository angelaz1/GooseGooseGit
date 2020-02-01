using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    //[HideInInspector]
    public Vector3 center;

    [HideInInspector]
    public Vector3 shake_vec;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = center + shake_vec;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shake_vec = new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        shake_vec = Vector3.zero;
    }
}
