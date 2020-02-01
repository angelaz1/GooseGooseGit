using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
	public Transform PlayerTransform;

  [HideInInspector]
	public Vector3 _cameraOffset;

	[Range(0.1f, 1.0f)]
	public float SmoothFactor = 1.0f;

  // private float maxDistAway = 5f;

  // Start is called before the first frame update
  public virtual void Start()
  {
  	_cameraOffset = transform.position - PlayerTransform.position;
  }

  // Update is called once per frame
  public virtual void Update()
  {	
    // Vector3 camPos = new Vector3(transform.position.x, transform.position.y, 0);
  	// float dist = Vector3.Distance(camPos, PlayerTransform.position);
  	// if(dist > maxDistAway) {
    //   Vector3 newPos = transform.position + (dist-maxDistAway) * Vector3.Normalize(PlayerTransform.position - camPos);
  	// 	// Vector3 newPos = Vector3.MoveTowards(transform.position, PlayerTransform.position, dist-5f);
  	// 	// Vector3 newPos = PlayerTransform.position + _cameraOffset;
  	// 	transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor); //newPos;
  	// }

    Vector3 newPos = PlayerTransform.position + _cameraOffset;
  	transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
  }
}
