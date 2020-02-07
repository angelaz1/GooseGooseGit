using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //Loads the scene input
    public void loadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    //Exits the game
    public void exitGame()
    {
        Application.Quit();
    }
}
