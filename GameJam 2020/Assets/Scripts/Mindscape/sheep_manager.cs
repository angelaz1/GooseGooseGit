using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sheep_manager : MonoBehaviour
{
    public Text sheep_amount;

    public int sheep_max=4;
    public int sheep_num = 0;

    // Start is called before the first frame update
    void Start()
    {
        sheep_num = 0;
        sheep_amount.text = sheep_num + "/" + sheep_max;
    }

    public void Add_Sheep()
    {
        sheep_num++;
        sheep_amount.text = sheep_num + "/" + sheep_max;

        if (sheep_num == sheep_max) StartCoroutine(Finished_Collecting_Sheep());
    }

    public void Fail_sheep()
    {
        StartCoroutine(Fail_Collecting_Sheep());
    }

    public IEnumerator Finished_Collecting_Sheep()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }

    public IEnumerator Fail_Collecting_Sheep()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }
}
