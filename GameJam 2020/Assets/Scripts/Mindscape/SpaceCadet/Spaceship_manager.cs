using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spaceship_manager : MonoBehaviour
{
    public void Fail()
    {
        StartCoroutine(Fail_Ship());
    }

    public IEnumerator Fail_Ship()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }
}
