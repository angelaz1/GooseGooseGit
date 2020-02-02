using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    //[HideInInspector]
    public Vector3 center;

    [HideInInspector]
    public Vector3 shake_vec;

    public Transform target;

    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        center = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Define a target position above and behind the target transform

        center = Vector3.SmoothDamp(center, target.position, ref velocity, smoothTime);

        // Smoothly move the camera towards that target position
        transform.position = center + shake_vec + new Vector3(0,0,-10);
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
