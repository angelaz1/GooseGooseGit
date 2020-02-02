using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_button : MonoBehaviour
{
    public void Load_Scene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
