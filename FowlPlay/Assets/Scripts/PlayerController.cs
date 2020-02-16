using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //y pos of ground
    private float zero_y = 0;
    //rigidbody
    private Rigidbody rb;
    //movespeed
    public float move_speed = 1;
    //transform of hoop center
    public Transform hoop_center;
    //transform of backboard center
    public Transform backboard_center;
    //where the ball is thrown from
    public Transform ball_origin;
    //ball prefab
    public GameObject ball_pref;
    //starting velocity of ball
    public float shoot_vel1= 8;
    public float shoot_vel2 = 10;
    //max dist to shoot
    public float max_dist;
    public float max_dist2;

    public bool is_defense;
    //transform of the blocking object
    public Transform block;
    //origin of the entire basketball net
    public Transform hoop_base;
    //other player
    public Transform other_player;

    public LayerMask block_mask;
    public LayerMask player_mask;

    public bool is_jumping;

    //does char use input1?
    public bool input1;

    public float gravity;
    public float jump_force;
    
    public bool can_move = true;

    public Manager man;


    public bool is_dashing = false;
    public bool can_dash = true;
    public float dash_speed = 5;
    public float dash_duration = 0.3f;
    [HideInInspector]
    public Vector3 dash_direction;

    public GameObject ball_shadow;

    public Image chance_image;
    public List<Sprite> chance_images;

    public bool vulnerable = false;
    public bool can_swipe = true;
    public Image vulnerable_image;

    private float jump_t = 0;
    private float jump_grace=0.4f;

    public int score = 0;
    public Text score_text;

    public Transform char_;

    public Image can_shoot_image;
    public Sprite can_shoot_full;
    public Sprite can_shoot_faded;

    public Image swipe_image;
    public Sprite swipe_full;
    public Sprite swipe_faded;

    public Transform arrow;

    public Animator char_anim;

    //particles
    public GameObject dash_particle;
    public GameObject jump_particle;

    // Start is called before the first frame update
    void Start()
    {
        zero_y = transform.position.y;

        rb = GetComponent<Rigidbody>();

        float diff_y = ball_origin.position.y - hoop_center.position.y;
        max_dist = (shoot_vel1 / -Physics.gravity.y) * Mathf.Sqrt(Mathf.Pow(shoot_vel1, 2) + 2 * -Physics.gravity.y * diff_y);
        float diff_y2 = ball_origin.position.y - backboard_center.position.y;
        max_dist2 = (shoot_vel2 / -Physics.gravity.y) * Mathf.Sqrt(Mathf.Pow(shoot_vel2, 2) + 2 * -Physics.gravity.y * diff_y2);
        Block_on_off();
        Update_UI();
        vulnerable_image.gameObject.SetActive(false);
    }

    public void Update_UI()
    {
        score_text.text = score.ToString();

        if (can_swipe)
        {
            swipe_image.sprite = swipe_full;
        }
        else
        {
            swipe_image.sprite = swipe_faded;
        }
    }

    //turn blocking on or off
    public void Block_on_off()
    {
        char_anim.SetBool("has_ball", !is_defense);
        char_anim.SetTrigger("switch");
        if (is_defense)
        {
            char_.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            char_.localScale = new Vector3(1, 1, 1);
        }

        block.gameObject.SetActive(is_defense);
        chance_image.gameObject.SetActive(!is_defense);
        can_shoot_image.gameObject.SetActive(!is_defense);
        swipe_image.gameObject.SetActive(is_defense);
    }

    public void Block_Off_Move()
    {
        char_.localScale = new Vector3(-1, 1, 1);

        block.gameObject.SetActive(false);
        chance_image.gameObject.SetActive(false);
        can_shoot_image.gameObject.SetActive(false);
        swipe_image.gameObject.SetActive(false);
        char_anim.SetBool("has_ball", false);
        char_anim.SetTrigger("walk");
    }

    //rotate block
    public void Update_Block()
    {
        Vector3 target = hoop_base.position;
        target.y = block.transform.position.y;
        block.transform.LookAt(target);
    }

    //ballistics stuff for shooting ball
    public float Get_Ballistic(float x, float y, float s, float g, bool plus)
    {
        float a = Mathf.Pow(s, 2);
        float b = Mathf.Sqrt(Mathf.Pow(s, 4) - g * ((g * Mathf.Pow(x, 2)) + (2 * Mathf.Pow(s, 2) * y)));
        float c = g * x;

        if (plus)
        {
            return Mathf.Atan((a + b) / c);
        }
        else
        {
            return Mathf.Atan((a - b) / c);
        }
    }

    void Update()
    {
        Vector3 dist_v = (hoop_center.position - ball_origin.position);
        dist_v = new Vector3(dist_v.x, 0, dist_v.z);

        float dist = dist_v.magnitude;

        float chance = (1-Mathf.Clamp(dist / max_dist2, 0, 1))*0.7f + 0.3f;
        if (dist < 1.5f) chance = 1f;
        if (!is_defense && can_move)
        {
            Vector3 p_diff_ = transform.position - other_player.position;
            p_diff_.y = 0;
            float p_dist_ = p_diff_.magnitude;

            float dot = Vector3.Dot(p_diff_, dist_v);

            //Debug.Log("player_dot: " + dot);

            if ((p_dist_ > 2f || dot >= 1 || is_jumping)&& dist < max_dist2)
            {
                can_shoot_image.sprite = can_shoot_full;
            }
            else
            {
                can_shoot_image.sprite = can_shoot_faded;
            }

            if (dist < max_dist2)
            {
                if (chance > 0.75f)
                {
                    chance_image.sprite = chance_images[3];
                }
                else if (chance > 0.5f)
                {
                    chance_image.sprite = chance_images[2];
                }
                else
                {
                    chance_image.sprite = chance_images[1];
                }
            }
            else
            {
                chance_image.sprite = chance_images[0];
            }
        }

        //Shoot the basketball
        if (!is_dashing && can_move && (Input.GetButtonDown("Shoot2") && !input1) && dist< Mathf.Max(max_dist,max_dist2) && !is_defense)
        {
            GameObject ball_obj = Instantiate(ball_pref, ball_origin.position, ball_pref.transform.rotation);
            Rigidbody ball_rb = ball_obj.GetComponent<Rigidbody>();

            GameObject ball_shadow_obj = Instantiate(ball_shadow, new Vector3(transform.position.x, 0, transform.position.z), ball_shadow.transform.rotation);
            ball_shadow_obj.GetComponent<Shadow>().player = ball_obj.transform;

            ball_obj.GetComponent<Ball>().hoop = hoop_base;
            ball_obj.GetComponent<Ball>().player_off = transform;
            ball_obj.GetComponent<Ball>().player_def = other_player;
            ball_obj.GetComponent<Ball>().shadow = ball_shadow_obj;
            ball_obj.GetComponent<Ball>().player1_ball = false;

            Vector3 diff = (hoop_center.position - ball_origin.position);

            Vector3 diff_0y = new Vector3(diff.x, 0, diff.z);

            float height = -diff.y;
            float width = diff_0y.magnitude;

            //Debug.Log(height + " : " + width);

            float shoot_vel;
            if(dist > max_dist)
            {
                shoot_vel =(((dist - max_dist) / (max_dist2 - max_dist)) * (shoot_vel2 - shoot_vel1)) + shoot_vel1;
            }
            else
            {
                shoot_vel = shoot_vel1;
            }

            if (is_jumping) shoot_vel -= 1;

            float theta = Get_Ballistic(width, height, shoot_vel, Physics.gravity.y, true) * Mathf.Rad2Deg;
            
            Vector3 u = Vector3.Cross(diff_0y, Vector3.up);

            Vector3 ball_dir = (Quaternion.AngleAxis(theta, -u) * diff_0y).normalized;

            if (Random.value > chance)
            {
                float fuck_up = 5f+(1-Mathf.Clamp(((dist - max_dist) / (max_dist2 - max_dist)),0,1))*15f;
                Debug.Log("FAILED SHOT: " + fuck_up);
                ball_dir = (Quaternion.AngleAxis(Mathf.Sign(Random.value-0.5f)* fuck_up, Vector3.up) * ball_dir).normalized;
            }

            ball_rb.velocity = ball_dir * shoot_vel;

            ball_rb.AddTorque(u * shoot_vel / 5);

            can_move = false;
            other_player.GetComponent<PlayerController>().can_move = false;

            //man.GetComponent<PlayableDirector>().Play();
            man.player1_off = true;
            man.Reset(true);
            char_anim.SetBool("has_ball", false);
            char_anim.SetTrigger("switch");
        }
        if (!is_dashing && can_move && (Input.GetButtonDown("Shoot1") && input1) && dist < Mathf.Max(max_dist, max_dist2) && !is_defense)
        {
            GameObject ball_obj = Instantiate(ball_pref, ball_origin.position, ball_pref.transform.rotation);
            Rigidbody ball_rb = ball_obj.GetComponent<Rigidbody>();

            GameObject ball_shadow_obj = Instantiate(ball_shadow, new Vector3(transform.position.x, 0, transform.position.z), ball_shadow.transform.rotation);
            ball_shadow_obj.GetComponent<Shadow>().player = ball_obj.transform;

            ball_obj.GetComponent<Ball>().hoop = hoop_base;
            ball_obj.GetComponent<Ball>().player_off = transform;
            ball_obj.GetComponent<Ball>().player_def = other_player;
            ball_obj.GetComponent<Ball>().shadow = ball_shadow_obj;
            ball_obj.GetComponent<Ball>().player1_ball = true;

            Vector3 diff = (hoop_center.position - ball_origin.position);

            Vector3 diff_0y = new Vector3(diff.x, 0, diff.z);

            float height = -diff.y;
            float width = diff_0y.magnitude;

            //Debug.Log(height + " : " + width);

            float shoot_vel;
            if (dist > max_dist)
            {
                shoot_vel = (((dist - max_dist) / (max_dist2 - max_dist)) * (shoot_vel2 - shoot_vel1)) + shoot_vel1;
            }
            else
            {
                shoot_vel = shoot_vel1;
            }

            if (is_jumping) shoot_vel -= 1;

            float theta = Get_Ballistic(width, height, shoot_vel, Physics.gravity.y, true) * Mathf.Rad2Deg;

            Vector3 u = Vector3.Cross(diff_0y, Vector3.up);

            Vector3 ball_dir = (Quaternion.AngleAxis(theta, -u) * diff_0y).normalized;

            if (Random.value > chance)
            {
                float fuck_up = 5f + (1 - Mathf.Clamp(((dist - max_dist) / (max_dist2 - max_dist)), 0, 1)) * 15f;
                //Debug.Log("FAILED SHOT: " + fuck_up);
                ball_dir = (Quaternion.AngleAxis(Mathf.Sign(Random.value - 0.5f) * fuck_up, Vector3.up) * ball_dir).normalized;
            }

            ball_rb.velocity = ball_dir * shoot_vel;

            ball_rb.AddTorque(u * shoot_vel / 5);

            can_move = false;
            other_player.GetComponent<PlayerController>().can_move = false;

            //man.GetComponent<PlayableDirector>().Play();
            man.player1_off = false;
            man.Reset(true);
            char_anim.SetBool("has_ball", false);
            char_anim.SetTrigger("switch");
        }

        //swipe
        if (!is_dashing && can_move && !is_jumping && can_swipe && (Input.GetButtonDown("Shoot2") && !input1) && is_defense)
        {
            float player_dist = Vector3.Distance(transform.position, other_player.position);
            if (player_dist < 2f && other_player.GetComponent<PlayerController>().vulnerable)
            {
                can_move = false;
                other_player.GetComponent<PlayerController>().can_move = false;
                other_player.GetComponent<PlayerController>().char_anim.SetBool("has_ball", false);
                other_player.GetComponent<PlayerController>().char_anim.SetTrigger("walk");

                StartCoroutine(Swipe(true, false));
            }
            else
            {
                StartCoroutine(Swipe(false, false));
            }
            char_anim.SetTrigger("swipe");
        }
        if (!is_dashing && can_move && !is_jumping && can_swipe && (Input.GetButtonDown("Shoot1") && input1) && is_defense)
        {
            float player_dist = Vector3.Distance(transform.position, other_player.position);
            if (player_dist < 2f && other_player.GetComponent<PlayerController>().vulnerable)
            {
                can_move = false;
                other_player.GetComponent<PlayerController>().can_move = false;
                other_player.GetComponent<PlayerController>().char_anim.SetBool("has_ball", false);
                other_player.GetComponent<PlayerController>().char_anim.SetTrigger("walk");

                StartCoroutine(Swipe(true, true));
            }
            else
            {
                StartCoroutine(Swipe(false,true));
            }
            char_anim.SetTrigger("swipe");
            
        }

        //Dash
        if (!is_jumping && !is_dashing && can_move && Input.GetButtonDown("Dash2") && !input1 && can_dash)
        {
            dash_direction = new Vector3(Input.GetAxis("Horizontal2"), 0, Input.GetAxis("Vertical2"));
            if (dash_direction != Vector3.zero)
            {
                StartCoroutine(Flash_Vulnerability());
                StartCoroutine(Dash());
                char_anim.SetTrigger("dash");
            }
        }
        if (!is_jumping && !is_dashing && can_move && Input.GetButtonDown("Dash1") && input1 && can_dash)
        {
            dash_direction = new Vector3(Input.GetAxis("Horizontal1"), 0, Input.GetAxis("Vertical1"));
            if (dash_direction != Vector3.zero)
            {
                StartCoroutine(Flash_Vulnerability());
                StartCoroutine(Dash());
                char_anim.SetTrigger("dash");
            }
        }

        //fakeout
        if (!is_jumping && !is_dashing && can_move && Input.GetButtonDown("Fake2") && !input1 && can_dash)
        {
            dash_direction = Vector3.zero;
            StartCoroutine(Flash_Vulnerability());
            StartCoroutine(FakeDash());
            char_anim.SetTrigger("fakeout");
        }
        if (!is_jumping && !is_dashing && can_move && Input.GetButtonDown("Fake1") && input1 && can_dash)
        {
            dash_direction = Vector3.zero;
            StartCoroutine(Flash_Vulnerability());
            StartCoroutine(FakeDash());
            char_anim.SetTrigger("fakeout");
        }

        if (!is_jumping && can_move && can_dash)
        {
            arrow.gameObject.SetActive(true);
        }
        else
        {
            arrow.gameObject.SetActive(false);
        }

        if (input1)
        {
            Vector2 input_ = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"));
            if(input_==Vector2.zero && arrow.gameObject.activeSelf) arrow.gameObject.SetActive(false);
            arrow.transform.localScale = new Vector3(1.5f * Mathf.Clamp(input_.magnitude,0,1), 1.5f, 1f);
            arrow.transform.rotation = Quaternion.Euler(new Vector3(90, 0, Mathf.Atan2(input_.y, input_.x) * Mathf.Rad2Deg));
        }
        else
        {
            Vector2 input_ = new Vector2(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
            if (input_ == Vector2.zero && arrow.gameObject.activeSelf) arrow.gameObject.SetActive(false);
            arrow.transform.localScale = new Vector3(1.5f * Mathf.Clamp(input_.magnitude, 0, 1), 1.5f, 1f);
            arrow.transform.rotation = Quaternion.Euler(new Vector3(90, 0, Mathf.Atan2(input_.y, input_.x) * Mathf.Rad2Deg));
        }

        //update block
        if (is_defense) Update_Block();

        //Jumping

        if (can_move && input1 && Input.GetButtonDown("Jump1") && !is_jumping && !is_dashing)
        {
            jump_t = jump_grace;
        }
        if (can_move && !input1 && Input.GetButtonDown("Jump2") && !is_jumping && !is_dashing)
        {
            jump_t = jump_grace;
        }

        if (jump_t > 0)
        {
            jump_t -= Time.deltaTime;
        }

        char_anim.SetBool("jumping", is_jumping);

        Update_UI();
    }

    public IEnumerator Dash()
    {
        GameObject dash1 = Instantiate(dash_particle, transform.position - new Vector3(0.5f, 0, 0), Quaternion.Euler(new Vector3(-90, 0, Mathf.Atan2(rb.velocity.z, rb.velocity.x) * -Mathf.Rad2Deg -90)));
        dash1.transform.SetParent(transform);
        Debug.Log(arrow.transform.eulerAngles.z);
        is_dashing = true;
        can_dash = false;
        yield return new WaitForSeconds(dash_duration);
        is_dashing = false;
        yield return new WaitForSeconds(1);
        can_dash = true;
    }

    public IEnumerator FakeDash()
    {
        GameObject dash1 = Instantiate(dash_particle, transform.position - new Vector3(0.5f, 0, 0), Quaternion.Euler(new Vector3(-90, 0, Mathf.Atan2(rb.velocity.z, rb.velocity.x) * -Mathf.Rad2Deg-90)));
        dash1.transform.SetParent(transform);
        Debug.Log(Mathf.Atan2(rb.velocity.z,rb.velocity.x)*Mathf.Rad2Deg);
        is_dashing = true;
        can_dash = false;
        can_move = false;
        yield return new WaitForSeconds(dash_duration);
        is_dashing = false;
        yield return new WaitForSeconds(0.2f);
        can_move = true;
        can_dash = true;
    }

    public IEnumerator Flash_Vulnerability()
    {
        vulnerable = true;
        vulnerable_image.gameObject.SetActive(!is_defense);
        yield return new WaitForSeconds(0.4f);
        vulnerable = false;
        vulnerable_image.gameObject.SetActive(false);
    }

    public IEnumerator Swipe(bool success, bool def)
    {
        can_swipe = false;
        yield return new WaitForSeconds(1f);
        can_swipe = true;

        if (success)
        {
            man.player1_off = def;
            man.Green_Score();
            man.Reset(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement input
        Vector2 input = Vector2.zero;
        if (can_move)
        {
            if (input1)
            {
                input = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"));
            }
            else
            {
                input = new Vector2(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
            }
        }

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

        //Move
        Vector3 move = (transform.right * input.x + transform.forward * input.y)*move_speed;

        if (is_dashing) move = dash_direction * dash_speed;

        if (is_defense)
        {
            //if on defense - cant go through player

            Vector3 towards_hoop = hoop_base.position - transform.position;
            towards_hoop.y = 0;

            //Debug.Log(Vector3.Dot(move, towards_hoop));

            BoxCollider bcol = block.GetComponentInChildren<BoxCollider>();

            if (Vector3.Dot(move, towards_hoop) < 0 && !is_dashing)
            {
                Collider[] cols1 = Physics.OverlapSphere(transform.position + move * Time.fixedDeltaTime + move.normalized * 0.4f, 0.2f, player_mask);
                foreach (Collider col in cols1)
                {
                    if (col.transform.root.gameObject != gameObject)
                    {
                        Vector3 diff = other_player.position - transform.position;
                        diff.y = 0;
                        Vector3 norm = Vector3.Cross(diff, Vector3.up).normalized;
                        //Debug.Log(Vector3.Dot(move, norm));
                        move = Vector3.zero;
                        //col.transform.root.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
                }
            }
        }
        else
        {
            //if on offense - cant go through block
            Vector3 towards_hoop = hoop_base.position - transform.position;
            towards_hoop.y = 0;

            //Debug.Log(Vector3.Dot(move, towards_hoop));

            if (Vector3.Dot(move, towards_hoop) > 0)
            {
                Collider[] cols = Physics.OverlapSphere(transform.position + move * Time.fixedDeltaTime + move.normalized * 0.4f, 0.2f, block_mask);
                foreach (Collider col in cols)
                {
                    if (col.transform.root.gameObject != gameObject && col.tag == "Block")
                    {
                        Vector3 norm = Vector3.Cross(col.transform.forward, Vector3.up).normalized;
                        //Debug.Log(Vector3.Dot(move, norm));
                        if (is_dashing)
                        {
                            dash_direction = norm * Mathf.Sign(Vector3.Dot(dash_direction, norm));
                            move = norm * dash_speed * Mathf.Sign(Vector3.Dot(dash_direction, norm));
                        }
                        else
                        {
                            move = norm * Vector3.Dot(move, norm);
                        }
                        //col.transform.root.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
                }
            }
        }

        if (rb.velocity.y < 0 && transform.position.y <= zero_y)
        {
            is_jumping = false;
        }

        if (can_move && jump_t>0 && !is_jumping && !is_dashing)
        {
            jump_t = 0;
            StartCoroutine(Flash_Vulnerability());
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jump_force);
            is_jumping = true;
            Instantiate(jump_particle, transform.position - new Vector3(0,1,0), jump_particle.transform.rotation);
        }
        if (can_move && jump_t > 0 && !is_jumping && !is_dashing)
        {
            jump_t = 0;
            StartCoroutine(Flash_Vulnerability());
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jump_force);
            is_jumping = true;
            Instantiate(jump_particle, transform.position - new Vector3(0, 1, 0), jump_particle.transform.rotation);
        }

        move.y = rb.velocity.y - gravity * Time.fixedDeltaTime;
        
        rb.velocity = move;
    }
}
