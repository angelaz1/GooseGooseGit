using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMove : MonoBehaviour
{

    public float initSlowWave = .0008f;
    public float topStop = 55f;
    public float botStop = 33f;
    public float maxSpeed = .1f;
    public float pauseTime = 1.0f;


    private float slowwave;
    private bool moveDown = true;
    private bool checking = true;

    private float waveSpeed = .1f;

    public GameObject shell;


    // Start is called before the first frame update
    void Start()
    {
        slowwave = initSlowWave;
    }

    public IEnumerator PauseWave()
    {
        checking = false;
        yield return new WaitForSeconds(pauseTime);
        checking = true;

    }

    // Update is called once per frame
    void Update()
    {
        /*if (shell.GetComponent<"Shell Script">.Health == "0")
        {
            checking = false;
        }*/
        if (checking){
            CheckWaves();
        }
        
    }

    void CheckWaves()
    {
        Vector3 currentPosition = transform.position;

        if (waveSpeed < maxSpeed)
        {
            waveSpeed += slowwave;
        }

        if (moveDown)
        {
            currentPosition.z -= waveSpeed;
        }
        else
        {
            currentPosition.z += waveSpeed;
        }

        transform.position = currentPosition;

        
        if (transform.position.z > topStop && !moveDown){
            slowwave = 0f;
            waveSpeed -= initSlowWave;
            if (waveSpeed <= 0)
            {
                StartCoroutine(PauseWave());
                moveDown = !moveDown;
                waveSpeed = 0f;
                slowwave = initSlowWave;
            }
        }
        if (transform.position.z < botStop && moveDown)
        {
            slowwave = 0f;
            waveSpeed -= initSlowWave;
            if (waveSpeed <= 0)
            {
                StartCoroutine(PauseWave());
                moveDown = !moveDown;
                waveSpeed = 0f;
                slowwave = initSlowWave;
            }
            
        }
    }
}
