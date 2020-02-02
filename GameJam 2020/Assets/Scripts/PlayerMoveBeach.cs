using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveBeach : MonoBehaviour
{

    private float speed = 20.0f;
    public bool stunned = false;
    private bool stunTimer = false;
    private WaveMove wm;


    public GameObject stars;
    public GameObject pow;
    public GameObject shellHitBox;

    public GameObject waves;
    public GameObject shell1;
    public GameObject shell2;
    public GameObject shell3;
    public GameObject shell4;
    public GameObject shell5;

    // Start is called before the first frame update
    private void Start()
    {
        wm = waves.GetComponent<WaveMove>();
    }












    // Update is called once per frame
    void Update()
    {
        evaluatePlayer();
        evaluateHit();
    }

    void evaluatePlayer()
    {
        Vector3 currentPosition = transform.position;

        if (!stunned)
        {
            if (Input.GetKey("w"))
            {
                currentPosition.z += speed * Time.deltaTime;
            }

            if (Input.GetKey("s"))
            {
                currentPosition.z -= speed * Time.deltaTime;
            }

            //currentPosition.z = Mathf.Clamp(currentPosition.z, -offset, offset);

            if (Input.GetKey("d"))
            {
                currentPosition.x += speed * Time.deltaTime;
            }

            if (Input.GetKey("a"))
            {
                currentPosition.x -= speed * Time.deltaTime;
            }
        }
        

        if (stunned)
        {
            if (!stunTimer)
            {
                currentPosition.z -= .2f;
            }
            
            if (currentPosition.z <= -22f && !stunTimer)
            {
                stunTimer = true;
                StartCoroutine(PauseStunned());
                
            }
        }
        currentPosition.x = Mathf.Clamp(currentPosition.x, -55, 55);
        currentPosition.z = Mathf.Clamp(currentPosition.z, -22, 100);
        transform.position = currentPosition;
    }

    public IEnumerator PauseStunned()
    {
        yield return new WaitForSeconds(2.0f);
        stunned = false;
        stunTimer = false;
        speed = 20.0f;
        stars.SetActive(false);
    }

    public IEnumerator PausePow()
    {
        yield return new WaitForSeconds(.2f);
        pow.SetActive(false);
    }

    void evaluateHit()
    {
        if (Vector3.Distance(transform.position, shellHitBox.transform.position) < 10 && Input.GetMouseButtonDown(0) && !stunned)
        {
            ShellScript ss = shellHitBox.GetComponent<ShellScript>();
            ss.health -= 1;

            if (ss.health == 60)
            {
                shell1.SetActive(false);
                shell2.SetActive(true);
                wm.initSlowWave = .004f;
                //wm.topStop += 1f;
                //wm.botStop -= 1f;
                wm.maxSpeed = .2f;
                wm.pauseTime -= .3f;
            }
            if (ss.health == 40)
            {
                shell2.SetActive(false);
                shell3.SetActive(true);
                wm.initSlowWave = .04f;
                //wm.topStop += 1f;
                //wm.botStop -= 1f;
                wm.maxSpeed = .3f;
                wm.pauseTime -= .3f;
            }
            if (ss.health == 20)
            {
                shell3.SetActive(false);
                shell4.SetActive(true);
                wm.initSlowWave = .1f;
                wm.topStop = 60f;
                wm.botStop = 33f;
                wm.maxSpeed = .4f;
                wm.pauseTime -= .3f;
            }
            if (ss.health == 0)
            {
                wm.maxSpeed = 0;
                shell4.SetActive(false);
                shell5.SetActive(true);
            }

            pow.SetActive(true);
            StartCoroutine(PausePow());

        }

    }




    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Waves")
        {
            print("stunned!");
            stunned = true;
            speed = 0;
            stars.SetActive(true);
        }
    }
}
