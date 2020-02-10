using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        if(Camera.current!=null) transform.rotation = Quaternion.LookRotation(Camera.current.transform.forward, Vector3.up);
    }
}
