using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Example : Enemy_Controller
{
    public override void Update()
    {
        velocity = Vector2.right * move_speed;

        base.Update();
    }
}
