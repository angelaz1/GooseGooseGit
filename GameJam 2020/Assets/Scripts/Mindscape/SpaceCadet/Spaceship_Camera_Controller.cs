using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship_Camera_Controller: Camera_Controller
{
  public Transform PlayerTransform;

  public void Update()
  {
    Vector3 newPos = new Vector3(0, PlayerTransform.position.y, -10f);
  	transform.position = Vector3.Slerp(transform.position, newPos, 1.0f);
  }
}
