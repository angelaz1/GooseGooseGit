using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Controller : MonoBehaviour
{
    public int max_health = 3;
    public int health;

    public Sprite spr_health;
    public Sprite spr_health_broken;

    public List<Image> health_tokens;

    public GameObject char_;

    public Animator anim;

    [HideInInspector]
    public Rigidbody2D rb;

    [HideInInspector]
    public Vector2 velocity;

    [HideInInspector]
    public Vector2 impact;

    public float move_speed = 1;
    public float chase_speed = 1;

    public float smooth = 0.05f;
    [HideInInspector]
    public Vector3 m_Velocity = Vector3.zero;

    public bool can_attack = true;

    public float time_between_attacks = 1f;

    public float alert_distance;
    public float attack_distance;

    [HideInInspector]
    public bool alerted = false;

    [HideInInspector]
    public Vector2 wander_center;
    [HideInInspector]
    public Vector2 wander_target;
    public float wander_range;

    [HideInInspector]
    public float wander_t;
    [HideInInspector]
    public float wander_max_t=4f;

    public bool can_wander = true;

    GameObject player;

    [HideInInspector]
    public Transform player_origin;

    private Coroutine hit_stun_co;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        can_attack = true;
        health = max_health;
        Update_UI();

        player = GameObject.FindGameObjectWithTag("Player");
        player_origin = player.GetComponent<Player_Controller>().char_.transform;

        wander_center = transform.position;
        wander_target = (Random.insideUnitCircle * wander_range) + wander_center;
    }

    public void Take_Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
        Update_UI();

        if (hit_stun_co!=null) StopCoroutine(hit_stun_co);
        hit_stun_co = StartCoroutine(Hit_Stun());
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void Update_UI()
    {
        for (int i = 0; i < health_tokens.Count; i++)
        {
            health_tokens[i].fillAmount = 1;
            if (i < health)
            {
                health_tokens[i].sprite = spr_health;
            }
            else
            {
                health_tokens[i].sprite = spr_health_broken;
            }
        }
    }

    public void End_Attack()
    {
        StartCoroutine(Attack_Rest());
    }

    public IEnumerator Attack_Rest()
    {
        yield return new WaitForSeconds(time_between_attacks);
        can_attack = true;
    }

    public IEnumerator Hit_Stun()
    {
        can_attack = false;
        yield return new WaitForSeconds(1f);
        can_attack = true;
    }

    public void Add_Impact(float mag, Vector2 dir)
    {
        impact = dir.normalized * mag;
    }

    public IEnumerator New_Wander_Target()
    {
        can_wander = false;
        yield return new WaitForSeconds(2f);
        can_wander = true;
        wander_t = 0;
        wander_target = (Random.insideUnitCircle * wander_range) + wander_center;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        float dist = Vector2.Distance(player_origin.position, char_.transform.position);
        if (dist <= alert_distance && dist>attack_distance)
        {
            velocity = (player_origin.position - char_.transform.position).normalized * chase_speed;
            alerted = true;
        }
        else if(dist<attack_distance && can_attack)
        {
            anim.SetTrigger("attack");
            velocity = Vector2.zero;
            can_attack = false;
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
                if (Vector2.Distance(wander_target, char_.transform.position) < 0.2f || wander_t>wander_max_t)
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

        if (rb.velocity.x!=0)
        {
            char_.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        }

        anim.SetFloat("speed", rb.velocity.magnitude);
    }

    public virtual void FixedUpdate()
    {
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocity + impact, ref m_Velocity, smooth);

        impact = Vector2.Lerp(impact, Vector2.zero, 5 * Time.deltaTime);
        if (impact.magnitude < 3 && impact.magnitude > 0)
        {
            impact = Vector2.zero;
        }
    }
}
