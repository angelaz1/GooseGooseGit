using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepMove : MonoBehaviour
{
    private float speed = 20.0f;
    private float offset = 20.0f;
    private bool safe = false;
    private float randX;
    private float randZ;
    private Vector3 velocity;

    public GameObject player;
    public GameObject fence;
    [HideInInspector]
    public Vector3 wander_center;
    [HideInInspector]
    public Vector3 wander_target;
    public float wander_range = 5;

    // Start is called before the first frame update
    void Start()
    {
        randX = Random.Range(-.2f, .2f);
        randZ = Random.Range(-.2f, .2f);

        wander_center = fence.transform.position;
        wander_target = (Random.insideUnitSphere * wander_range) + wander_center;
        wander_target.y = 0;
    }
   



    // Update is called once per frame
    void Update()
    {
        EvaluateSheep();
    }

    public IEnumerator New_Wander_Target()
    {
        yield return new WaitForSeconds(2f);
        wander_target = (Random.insideUnitSphere * wander_range) + wander_center;
        wander_target.y = 0;
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

        if (safe)
        {
            Vector3 currentPosition = transform.position;

            velocity = (wander_target - (Vector3)transform.position).normalized / 10;
            velocity.y = 0;
            currentPosition += velocity;
            
            if (Vector2.Distance(wander_target, transform.position) < 0.2f)
            {
                StartCoroutine(New_Wander_Target());
            }
            /*
            currentPosition.z += randZ;
            currentPosition.x += randX;
            if (currentPosition.z > fence.transform.position.z + (fence.transform.localScale.z/2))
            {
                randZ = -randZ;
                currentPosition.z = fence.transform.position.z + (fence.transform.localScale.z / 2);
            }
            if (currentPosition.z < fence.transform.position.z - (fence.transform.localScale.z / 2))
            {
                randZ = -randZ;
                currentPosition.z = fence.transform.position.z - (fence.transform.localScale.z / 2);
            }
            if (currentPosition.x > fence.transform.position.x + (fence.transform.localScale.x / 2))
            {
                randX = -randX;
                currentPosition.x = fence.transform.position.x + (fence.transform.localScale.x / 2);
            }
            if (currentPosition.x < fence.transform.position.x - (fence.transform.localScale.x / 2))
            {
                randX = -randX;
                currentPosition.x = fence.transform.position.x - (fence.transform.localScale.x / 2);
            }*/


            transform.position = currentPosition;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fence"))
        {
            print("it works");
            safe = true;
        }
    }

}
