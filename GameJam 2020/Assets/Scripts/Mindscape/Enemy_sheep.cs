using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_sheep : Enemy_Controller
{
    public bool is_in_pen = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "pen" && !is_in_pen)
        {
            GameObject.FindGameObjectWithTag("Manager").GetComponent<sheep_manager>().Add_Sheep();

            wander_center = collision.transform.position;
            wander_target = wander_center;

            is_in_pen = true;
        }
    }

    public override void Take_Damage(int damage)
    {
        if (health <= 0)
        {
            Death();
        }
        Update_UI();

        Instantiate(hit_part, transform.position + hit_offset, hit_part.transform.rotation);

        if (hit_stun_co != null) StopCoroutine(hit_stun_co);
        hit_stun_co = StartCoroutine(Hit_Stun());
    }

    public override void Update()
    {
        if (!is_in_pen)
        {
            float dist = Vector2.Distance(player_origin.position, char_.transform.position);
            if (dist <= alert_distance)
            {
                velocity = Vector2.zero;
                alerted = true;
            }
            else if (dist > alert_distance)
            {
                if (alerted)
                {
                    alerted = false;
                    wander_target -= wander_center;
                    wander_center = transform.position;
                    wander_target += wander_center;
                }
                if (can_wander)
                {
                    velocity = (wander_target - (Vector2)char_.transform.position).normalized * move_speed;
                    wander_t += Time.deltaTime;
                    if (Vector2.Distance(wander_target, char_.transform.position) < 0.2f || wander_t > wander_max_t)
                    {
                        StartCoroutine(New_Wander_Target());
                    }
                }
                else
                {
                    velocity = Vector2.zero;
                }
            }
            else
            {
                velocity = Vector2.zero;
            }

            if (alerted)
            {
                float d = Mathf.Clamp(-1 * (dist - alert_distance) / alert_distance, 0, 1);
                anim.SetFloat("move", 1 + d);
                velocity -= (Vector2)(player_origin.position - char_.transform.position).normalized * chase_speed * d;
            }
            else
            {
                anim.SetFloat("move", 1);
            }
        }
        else
        {
            anim.SetFloat("move", 1);
            if (can_wander)
            {
                velocity = (wander_target - (Vector2)char_.transform.position).normalized * move_speed;
                wander_t += Time.deltaTime;
                if (Vector2.Distance(wander_target, char_.transform.position) < 0.2f || wander_t > wander_max_t)
                {
                    StartCoroutine(New_Wander_Target());
                }
            }
            else
            {
                velocity = Vector2.zero;
            }
        }
        

        if (velocity.x != 0)
        {
            char_.transform.localScale = new Vector3(Mathf.Sign(velocity.x), 1, 1);
        }

        anim.SetFloat("speed", rb.velocity.magnitude);
    }
}
