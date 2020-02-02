using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : Enemy_Controller
{
    public Transform playerTransform;

    private Vector2 startLocation;
    private Vector2 moveDir;

    private float horizDist = 8f;
    private float vertDist = 10f;
    private float vertRange = 5f;
    private float speedRange = 3f;
    private float minSpeed = 2f;

    public int damage;

    public BoxCollider2D asteroid_hitbox;

    // private Text thoughtText;
    // private GameObject thisText;

    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player").transform;
        // thoughtText = gameObject.GetComponentInChildren(typeof(Text), true) as Text;
        // Debug.Log(GameObject.FindGameObjectWithTag("ThoughtText"));
        // thoughtText.enabled = false;
        asteroid_hitbox.enabled = true;
        
        float size = Random.value + 0.7f;
        transform.localScale = new Vector3(size, size, 1);
        setStartLocation();
    }

    private void setStartLocation() {
        //Setting the starting location for the asteroid to spawn at
        float xPos = (Random.value * horizDist * 2) - horizDist;
        float playerYPos = playerTransform.position.y;
        float yPos = (vertRange * Random.value) + playerYPos + vertDist;
        transform.position = new Vector2(xPos, yPos);

        moveDir = new Vector2(Random.value - 0.5f, -0.5f).normalized;
        move_speed = (Random.value * speedRange) + minSpeed;

        // bool goLeft = (Random.value <= 0.5);
        
        // if(goLeft) startLocation = new Vector2(-horizDist, vertPos);
        // else startLocation = new Vector2(horizDist, vertPos);
        // transform.position = startLocation;

        //Determining move direction to move towards the player
        // moveDir = (playerTransform.position - transform.position).normalized;
        
    }

    public override void Update()
    {
        if(transform.position.y < playerTransform.position.y - 20) {
          Destroy(gameObject);
        }
        velocity = moveDir * move_speed;
    }

    public override void Death() 
    {
      //Get text to appear
      // Vector3 mPos = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
      // mPos.z = 0;
      // mPos.x += body.GetComponent<SpriteRenderer> ().bounds.size.x / 2;
      // mPos.y += body.GetComponent<SpriteRenderer> ().bounds.size.y / 2;
      // thoughtText.transform.position = mPos;

      //thoughtText.transform.position = transform.position;
      // thoughtText.enabled = true;
      base.Death();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Spaceship_Controller>() != null)
        {
            Spaceship_Controller player = collision.GetComponent<Spaceship_Controller>();
            player.Take_Damage(damage);
            Enemy_Controller enem = GetComponentInParent<Enemy_Controller>();

            //Add knockback if not too far down
            if(player.char_.transform.position.y > 5f) {
              Vector2 displVec = new Vector2((player.char_.transform.position - enem.char_.transform.position).x, -100f);
              player.Add_Impact(move_speed * 15f, displVec);
            } else {
              Vector2 displVec = new Vector2((player.char_.transform.position - enem.char_.transform.position).x, -100f);
              player.Add_Impact(move_speed * 1f, displVec);
            }
            Death();
        }
    }
}
