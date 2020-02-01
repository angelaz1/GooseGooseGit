﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spaceship_Controller : Player_Controller
{
    private float verticalVelocity;
    private float maxVertVelocity = 20f;
    private float velIncrement = 0.01f;

    public override void Start() {
        verticalVelocity = 1f;
        base.Start();
    }

    public override void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        velocity = input.normalized * move_speed*0.25f 
                    + new Vector2(0, verticalVelocity);

        if (input.x != 0)
        {
            char_.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        }

        Update_Token_Timer();

        if(verticalVelocity < maxVertVelocity) {
          verticalVelocity += velIncrement;
        }
    }

    public override void Take_Damage(int damage) {
      verticalVelocity = 0f;
      base.Take_Damage(damage);
    }
}
