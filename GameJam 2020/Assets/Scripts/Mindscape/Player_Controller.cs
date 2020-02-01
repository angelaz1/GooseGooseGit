using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{
    [HideInInspector]
    public Vector2 input = Vector2.zero;

    [HideInInspector]
    public Vector2 velocity;

    [HideInInspector]
    public Vector2 impact;

    public float move_speed=1;

    public float smooth = 0.1f;
    
    [HideInInspector]
    public Vector3 m_Velocity = Vector3.zero;

    [HideInInspector]
    public Rigidbody2D rb;

    public GameObject char_;

    public Animator anim;

    public bool is_attacking = false;

    public int max_health = 4;
    public int health;

    public Sprite spr_health;
    public Sprite spr_health_broken;

    public List<Image> health_tokens;

    public float token_time_limit = 30;
    public float token_t;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = max_health;
        Update_UI();
        token_t = token_time_limit;
    }

    public void Update_Token_Timer()
    {
        if (token_t > 0)
        {
            token_t -= Time.deltaTime;
            if(health>0) health_tokens[health-1].fillAmount = token_t / token_time_limit;
        }
        else
        {
            Take_Damage(1);
        }
    }

    public void Take_Damage(int damage)
    {
        Camera_Controller cam_c = Camera.main.GetComponent<Camera_Controller>();
        cam_c.StartCoroutine(cam_c.Shake(0.2f, 0.1f));

        health -= damage;
        if (health <= 0)
        {
            Death();
        }
        token_t = token_time_limit;
        Update_UI();
    }

    public void Death()
    {

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

    // Update is called once per frame
    public virtual void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //make diagnols the same speed as forward
        if (input != Vector2.zero)
        {
            float input_th = Mathf.Atan2(input.y, input.x);
            //Debug.Log(th * Mathf.Rad2Deg);
            float input_scalar = Mathf.Sqrt(Mathf.Pow(input.x, 2) + Mathf.Pow(input.y, 2));
            float unit_input_scalar;
            if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
            {
                float x_scale = Mathf.Abs(input.x);
                unit_input_scalar = Mathf.Sqrt(Mathf.Pow(input.x / x_scale, 2) + Mathf.Pow(input.y / x_scale, 2));
            }
            else
            {
                float y_scale = Mathf.Abs(input.y);
                unit_input_scalar = Mathf.Sqrt(Mathf.Pow(input.x / y_scale, 2) + Mathf.Pow(input.y / y_scale, 2));
            }
            //Debug.Log(input_scalar + " : " + unit_input_scalar);

            float r = input_scalar / unit_input_scalar;
            input.x = r * Mathf.Cos(input_th);
            input.y = r * Mathf.Sin(input_th);
        }

        if (Input.GetButtonDown("Fire1") && !is_attacking)
        {
            anim.SetTrigger("Attack");
        }
        else if (Input.GetButtonDown("Fire1") && is_attacking)
        {
            anim.SetBool("Combo_Attempt",true);
        }

        if (!is_attacking)
        {
            velocity = input.normalized * move_speed;
        }
        else
        {
            velocity = input.normalized * move_speed*0.25f;
        }

        if (input.x != 0 && !is_attacking)
        {
            char_.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        }

        Update_Token_Timer();

        anim.SetFloat("speed", velocity.magnitude);
    }

    public void Add_Impact(float mag, Vector2 dir)
    {
        impact = dir.normalized * mag;
    }

    public virtual void FixedUpdate()
    {
        rb.velocity = Vector3.SmoothDamp(rb.velocity, velocity+impact, ref m_Velocity, smooth);

        impact = Vector2.Lerp(impact, Vector2.zero, 5 * Time.deltaTime);
        if (impact.magnitude < 3 && impact.magnitude > 0)
        {
            impact = Vector2.zero;
        }
    }
}
