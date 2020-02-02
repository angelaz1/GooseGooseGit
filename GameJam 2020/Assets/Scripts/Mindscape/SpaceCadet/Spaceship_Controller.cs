using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spaceship_Controller : Player_Controller
{
    private float verticalVelocity;
    private float maxVertVelocity = 20f;
    private float velIncrement = 0.01f;
    public RectTransform progressBar;
    public RectTransform fullBar;
    private float startPlanetY;
    private float endPlanetY;
    private float fullDisplacement;

    public override void Start() {
        base.Start();
        startPlanetY = GameObject.FindWithTag("StartingPlanet").transform.position.y;
        endPlanetY = GameObject.FindWithTag("ObjectivePlanet").transform.position.y;

        fullDisplacement = endPlanetY - startPlanetY;
        Debug.Log(endPlanetY);
        Debug.Log(startPlanetY);
        Debug.Log(fullDisplacement);
        verticalVelocity = 1f;
        
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

    public override void FixedUpdate() 
    {
        base.FixedUpdate();
        float playerDisp = endPlanetY - transform.position.y;
        float fullLength = fullBar.rect.width;
        float frac = (fullDisplacement - playerDisp) / fullDisplacement;
        Debug.Log(frac * fullLength);
        progressBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, frac * fullLength);
        // progressBar.rect.width = frac * fullLength;
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
    }
}
