﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : Enemy_Controller
{
    public Transform playerTransform;

    private Vector2 startLocation;
    private Vector2 moveDir;

    private float horizDist = 8f;
    private float vertDist = 20f;
    private float vertRange = 10f;
    private float speedRange = 2f;
    private float minSpeed = 2f;

    public int damage;

    public BoxCollider2D asteroid_hitbox;

    public override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        asteroid_hitbox.enabled = true;
        setStartLocation();
    }

    private void setStartLocation() {
        bool goLeft = (Random.value <= 0.5);
        float playerYPos = playerTransform.position.y;
        float vertPos = (vertRange * Random.value) + playerYPos + vertDist;
        if(goLeft) startLocation = new Vector2(-horizDist, vertPos);
        else startLocation = new Vector2(horizDist, vertPos);
        transform.position = startLocation;

        moveDir = (playerTransform.position - transform.position).normalized; 
        move_speed = (Random.value * speedRange) + minSpeed;
    }

    public override void Update()
    {
        velocity = moveDir * move_speed;
        if (rb.velocity.x!=0)
        {
            char_.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Spaceship_Controller>() != null)
        {
            Spaceship_Controller player = collision.GetComponent<Spaceship_Controller>();
            player.Take_Damage(damage);
            Enemy_Controller enem = GetComponentInParent<Enemy_Controller>();
            Vector2 displVec = new Vector2((player.char_.transform.position - enem.char_.transform.position).x, 0);
            player.Add_Impact(8f, displVec);
            Death();
        }
    }
}