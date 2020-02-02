using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spaceship_Controller : Player_Controller
{
<<<<<<< HEAD
    private float verticalVelocity = 1f;
=======
    private float verticalVelocity;
    private float maxVertVelocity = 20f;
    private float velIncrement = 0.01f;

    public override void Start() {
        verticalVelocity = 1f;
        base.Start();
    }
>>>>>>> parent of 4e0d591... Progress Bar Added to Space Cadet

    public override void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), verticalVelocity);
        velocity = input.normalized * move_speed*0.25f;

        if (input.x != 0)
        {
            char_.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        }

        Update_Token_Timer();
<<<<<<< HEAD
=======

        if(verticalVelocity < maxVertVelocity) {
          verticalVelocity += velIncrement;
        }
    }

    public override void Take_Damage(int damage) {
      verticalVelocity = 0f;
      //Run a spin-out animation
      StartCoroutine(startSpinning());
      base.Take_Damage(damage);
    }

    private IEnumerator startSpinning() {
      anim.SetBool("Spinning_out", true);
      yield return new WaitForSeconds(0.5f);
      anim.SetBool("Spinning_out", false);
>>>>>>> parent of 4e0d591... Progress Bar Added to Space Cadet
    }
}
