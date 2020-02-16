using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public PlayerController player1;
    public PlayerController player2;

    public Transform start_off;
    public Transform start_def;

    public Camera ball_cam;
    public Camera hoop_cam;
    public Camera player_cam;

    public Vector3 cam_bot_pos;
    public Vector3 cam_top_pos;

    public bool player1_off = true;

    bool lerping = false;
    float lerp_t = 0;
    public float time_to_move = 3;

    Vector3 player1_start_pos;
    Vector3 player2_start_pos;

    public Animator blue_banner;
    public Animator red_banner;
    public Animator green_banner;

    float t;
    public float max_t;

    public Text timer;
    public bool can_timer = true;

    public AudioSource whomp;

    // Start is called before the first frame update
    void Start()
    {
        Set_Pos(player1_off);
        t = max_t;
    }

    public void Blue_Score()
    {
        StartCoroutine(Blue_Banner_co());
    }

    public IEnumerator Blue_Banner_co()
    {
        yield return new WaitForSeconds(2f);
        blue_banner.SetTrigger("fadein");
        yield return new WaitForSeconds(2.5f);
        blue_banner.SetTrigger("fadeout");
    }

    public void Red_Score()
    {
        StartCoroutine(Red_Banner_co());
    }

    public IEnumerator Red_Banner_co()
    {
        yield return new WaitForSeconds(2f);
        red_banner.SetTrigger("fadein");
        yield return new WaitForSeconds(2.5f);
        red_banner.SetTrigger("fadeout");
    }

    public void Green_Score()
    {
        StartCoroutine(Green_Banner_co());
    }

    public IEnumerator Green_Banner_co()
    {
        //yield return new WaitForSeconds(2f);
        green_banner.SetTrigger("fadein");
        yield return new WaitForSeconds(2.5f);
        green_banner.SetTrigger("fadeout");
    }

    void LateUpdate()
    {
        if (t > 0 && can_timer) t -= Time.deltaTime;

        if(Mathf.FloorToInt(t % 60) > 9)
        {
            timer.text = Mathf.FloorToInt(t / 60).ToString() + " : " + Mathf.FloorToInt(t % 60).ToString();
        }
        else
        {
            timer.text = Mathf.FloorToInt(t / 60).ToString() + " : 0" + Mathf.FloorToInt(t % 60).ToString();
        }

        float p_z = ((player1.transform.position.z + player2.transform.position.z) / 2)-1;

        p_z = Mathf.Clamp(p_z/5, -0.5f, 0.5f);

        player_cam.transform.position = Vector3.Lerp(cam_bot_pos, cam_top_pos, 0.5f+ p_z);

        if (lerping)
        {
            lerp_t += Time.deltaTime / time_to_move;

            //Debug.Log("lerp: " + lerp_t);

            if (player1_off)
            {
                player1.transform.position = Vector3.Lerp(player1_start_pos, start_off.position, lerp_t);
                player2.transform.position = Vector3.Lerp(player2_start_pos, start_def.position, lerp_t);
            }
            else
            {
                player1.transform.position = Vector3.Lerp(player1_start_pos, start_def.position, lerp_t);
                player2.transform.position = Vector3.Lerp(player2_start_pos, start_off.position, lerp_t);
            }
        }
    }

    public void Reset(bool is_shot)
    {
        StartCoroutine(Reset_co(is_shot));
    }

    public IEnumerator Reset_co(bool is_shot)
    {
        if (is_shot) yield return new WaitForSeconds(3);

        player1_start_pos = player1.transform.position;
        player1_start_pos.y = 1;
        player2_start_pos = player2.transform.position;
        player2_start_pos.y = 1;

        lerping = true;
        lerp_t = 0;

        player1.is_jumping = false;
        player2.is_jumping = false;

        player1.is_dashing = false;
        player2.is_dashing = false;

        player1.can_dash = true;
        player2.can_dash = true;

        player1.Block_Off_Move();
        player2.Block_Off_Move();

        can_timer = false;

        yield return new WaitForSeconds(time_to_move);

        lerping = false;
        Set_Pos(player1_off);
        can_timer = true;
    }

    public void Set_Pos(bool player1_off)
    {
        if (player1_off)
        {
            player1.transform.position = start_off.position;
            player2.transform.position = start_def.position;

            player1.is_defense = false;
            player2.is_defense = true;
        }
        else
        {
            player1.transform.position = start_def.position;
            player2.transform.position = start_off.position;

            player1.is_defense = true;
            player2.is_defense = false;
        }

        player1.can_move = true;
        player2.can_move = true;

        player1.man = this;
        player2.man = this;

        player1.Block_on_off();
        player2.Block_on_off();
        
        GetComponent<PlayableDirector>().time = 0;
        GetComponent<PlayableDirector>().
        GetComponent<PlayableDirector>().Stop();

        ball_cam.gameObject.SetActive(false);
        hoop_cam.gameObject.SetActive(false);
        player_cam.gameObject.SetActive(true);

        GameObject[] ballz = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject ball in ballz)
        {
            Destroy(ball);
        }

        GameObject[] ballz_ = GameObject.FindGameObjectsWithTag("Ball Shadow");
        foreach (GameObject ball in ballz_)
        {
            Destroy(ball);
        }
    }
}
