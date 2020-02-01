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

    public float smooth = 0.05f;
    [HideInInspector]
    public Vector3 m_Velocity = Vector3.zero;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = max_health;
        Update_UI();
        StartCoroutine(Test_Attack());
    }

    public void Take_Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
        Update_UI();
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

    public IEnumerator Test_Attack()
    {
        yield return new WaitForSeconds(2f);
        anim.SetTrigger("attack");
        StartCoroutine(Test_Attack());
    }

    public void Add_Impact(float mag, Vector2 dir)
    {
        impact = dir.normalized * mag;
    }

    // Update is called once per frame
    public virtual void Update()
    {
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
