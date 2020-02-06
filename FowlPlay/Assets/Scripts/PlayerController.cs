using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float max_velocity_change = 10f;

    public float zero_y = 0;

    private Rigidbody rb;

    public float move_speed = 1;

    public Transform hoop_center;
    public Transform backboard_center;

    public Transform ball_origin;

    public GameObject ball_pref;

    public float shoot_vel= 10;

    public float max_dist;
    public float max_dist2;

    public bool is_defense;
    public Transform block;

    public Transform hoop_base;

    public LayerMask block_mask;
    public LayerMask player_mask;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        float diff_y = ball_origin.position.y - hoop_center.position.y;
        max_dist = (shoot_vel / -Physics.gravity.y) * Mathf.Sqrt(Mathf.Pow(shoot_vel, 2) + 2 * -Physics.gravity.y * diff_y);
        float diff_y2 = ball_origin.position.y - backboard_center.position.y;
        max_dist2 = (shoot_vel / -Physics.gravity.y) * Mathf.Sqrt(Mathf.Pow(shoot_vel, 2) + 2 * -Physics.gravity.y * diff_y2);
        Block_on_off();
    }

    public void Block_on_off()
    {
        block.gameObject.SetActive(is_defense);
    }

    public void Update_Block()
    {
        Vector3 target = hoop_base.position;
        target.y = block.transform.position.y;
        block.transform.LookAt(target);
    }

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

        if (Input.GetButtonDown("Fire1") && dist< max_dist && !is_defense)
        {
            GameObject ball_obj = Instantiate(ball_pref, ball_origin.position, ball_pref.transform.rotation);
            Rigidbody ball_rb = ball_obj.GetComponent<Rigidbody>();

            Vector3 diff = (hoop_center.position - ball_origin.position);

            Vector3 diff_0y = new Vector3(diff.x, 0, diff.z);

            float height = -diff.y;
            float width = diff_0y.magnitude;

            Debug.Log(height + " : " + width);

            float theta = Get_Ballistic(width, height, shoot_vel, Physics.gravity.y, true)*Mathf.Rad2Deg;

            Vector3 u = Vector3.Cross(diff_0y, Vector3.up);

            Vector3 ball_dir = (Quaternion.AngleAxis(theta, -u) * diff_0y).normalized;

            ball_rb.velocity = ball_dir * shoot_vel;

            ball_rb.AddTorque(u*shoot_vel/5);
        }

        if (Input.GetButtonDown("Fire2") && dist < max_dist2 && !is_defense)
        {
            GameObject ball_obj = Instantiate(ball_pref, ball_origin.position, ball_pref.transform.rotation);
            Rigidbody ball_rb = ball_obj.GetComponent<Rigidbody>();

            Vector3 diff = (backboard_center.position - ball_origin.position);

            Vector3 diff_0y = new Vector3(diff.x, 0, diff.z);

            float height = -diff.y;
            float width = diff_0y.magnitude;

            Debug.Log(height + " : " + width);

            float starting_vel = 10;

            float theta = Get_Ballistic(width, height, starting_vel, Physics.gravity.y, false) * Mathf.Rad2Deg;

            Vector3 u = Vector3.Cross(diff_0y, Vector3.up);

            Vector3 ball_dir = (Quaternion.AngleAxis(theta, -u) * diff_0y).normalized;

            ball_rb.velocity = ball_dir * starting_vel;
        }

        if (is_defense) Update_Block();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Movement input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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

            Debug.Log(Vector3.Dot(move, towards_hoop));

            BoxCollider bcol = block.GetComponentInChildren<BoxCollider>();

            if (Vector3.Dot(move, towards_hoop) < 0)
            {
                Collider[] cols1 = Physics.OverlapBox(bcol.transform.position + move * Time.fixedDeltaTime, bcol.bounds.extents / 2, block.transform.rotation, player_mask);
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

            Debug.Log(Vector3.Dot(move, towards_hoop));

            if (Vector3.Dot(move, towards_hoop) > 0)
            {
                Collider[] cols = Physics.OverlapSphere(transform.position + move * Time.fixedDeltaTime + move.normalized * 0.4f, 0.2f, block_mask);
                foreach (Collider col in cols)
                {
                    if (col.transform.root.gameObject != gameObject && col.tag == "Block")
                    {
                        Vector3 norm = Vector3.Cross(col.transform.forward, Vector3.up).normalized;
                        Debug.Log(Vector3.Dot(move, norm));
                        move = norm * Vector3.Dot(move, norm);
                        //col.transform.root.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    }
                }
            }
        }
        rb.velocity = move;
    }
}
