using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //does char use arrows to move?
    public bool input_arrow;

    public float gravity;
    public float jump_force;

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
    }

    //turn blocking on or off
    public void Block_on_off()
    {
        block.gameObject.SetActive(is_defense);
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

        //Shoot the basketball
        if (Input.GetButtonDown("Fire1") && dist< Mathf.Max(max_dist,max_dist2) && !is_defense)
        {
            GameObject ball_obj = Instantiate(ball_pref, ball_origin.position, ball_pref.transform.rotation);
            Rigidbody ball_rb = ball_obj.GetComponent<Rigidbody>();

            ball_obj.GetComponent<Ball>().hoop = hoop_base;
            ball_obj.GetComponent<Ball>().player_off = transform;
            ball_obj.GetComponent<Ball>().player_def = other_player;

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

            float theta = Get_Ballistic(width, height, shoot_vel, Physics.gravity.y, true) * Mathf.Rad2Deg;

            Vector3 u = Vector3.Cross(diff_0y, Vector3.up);

            Vector3 ball_dir = (Quaternion.AngleAxis(theta, -u) * diff_0y).normalized;

            ball_rb.velocity = ball_dir * shoot_vel;

            ball_rb.AddTorque(u * shoot_vel / 5);
        }

        //update block
        if (is_defense) Update_Block();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement input
        Vector2 input;
        if (!input_arrow)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal_key"), Input.GetAxisRaw("Vertical_key"));
        }
        else
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal_arrow"), Input.GetAxisRaw("Vertical_arrow"));
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

        if (is_defense)
        {
            //if on defense - cant go through player

            Vector3 towards_hoop = hoop_base.position - transform.position;
            towards_hoop.y = 0;

            //Debug.Log(Vector3.Dot(move, towards_hoop));

            BoxCollider bcol = block.GetComponentInChildren<BoxCollider>();

            if (Vector3.Dot(move, towards_hoop) < 0)
            {
                Collider[] cols1 = Physics.OverlapSphere(transform.position + move * Time.fixedDeltaTime + move.normalized * 0.4f, 0.2f, player_mask);
                foreach (Collider col in cols1)
                {
                    if (col.transform.root.gameObject != gameObject)
                    {
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
                        move = norm * Vector3.Dot(move, norm);
                        //col.transform.root.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
                }
            }
        }

        if (rb.velocity.y < 0 && transform.position.y <= zero_y)
        {
            is_jumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !is_jumping && !is_defense)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jump_force);
            is_jumping = true;
        }

        move.y = rb.velocity.y - gravity * Time.fixedDeltaTime;
        
        rb.velocity = move;
    }
}
