using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship_Camera_Controller : Camera_Controller
{
  public override void Update()
  {
    Vector3 newPos = new Vector3(0, (PlayerTransform.position + _cameraOffset).y, _cameraOffset.z);
  	transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
  }
}
