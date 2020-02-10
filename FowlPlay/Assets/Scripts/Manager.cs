using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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

    // Start is called before the first frame update
    void Start()
    {
        Set_Pos(false);
    }

    void LateUpdate()
    {
        float p_z = ((player1.transform.position.z + player2.transform.position.z) / 2)-1;

        p_z = Mathf.Clamp(p_z/5, -0.5f, 0.5f);

        player_cam.transform.position = Vector3.Lerp(cam_bot_pos, cam_top_pos, 0.5f+ p_z);
    }

    public void Reset()
    {
        StartCoroutine(Reset_co());
    }

    public IEnumerator Reset_co()
    {
        yield return new WaitForSeconds(3);
        Set_Pos(false);
    }

    public void Set_Pos(bool inverse)
    {
        player1.transform.position = start_off.position;
        player2.transform.position = start_def.position;

        player1.is_defense = false;
        player2.is_defense = true;

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
