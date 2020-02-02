using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{

    public int health = 50;
    public GameObject playerObj;

    private bool playing = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            evaluateShell();
        }
    }

    void evaluateShell()
    {
        if (health <= 0)
        {
            print("game over, you win!");
            playing = false;
        }
    }

}
