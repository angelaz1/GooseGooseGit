using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spaceship_Controller : Player_Controller
{
    private float verticalVelocity = 1f;

    public override void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), verticalVelocity);
        velocity = input.normalized * move_speed*0.25f;

        if (input.x != 0)
        {
            char_.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        }

        Update_Token_Timer();
    }
}
