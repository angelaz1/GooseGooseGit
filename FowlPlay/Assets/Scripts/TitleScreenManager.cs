﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public Button playButton;  
    public Image playButtonBox; 
    public Button exitButton;
    public Image exitButtonBox;
    public Button tutorialButton;
    public Image tutorialButtonBox;
    public Image owlImage;
    public Sprite owlSquint;
    public Sprite owlOpen;

    private int selectingPlay; //0 for tutorial, 1 for play game, 2 for exit
    private float bufferTimer;
    private float bufferTimeAmt;

    // Start is called before the first frame update
    void Start()
    {
        selectingPlay = 0;
        bufferTimer = 0;
        bufferTimeAmt = 5;
    }

    void Update()
    {
        if(Input.GetButtonDown("Jump") || Input.GetButtonDown("Jump2")) {
          if(selectingPlay == 0) loadScene(1); //LOAD TUTORIAL
          else if(selectingPlay == 1) loadScene(2); //LOAD GAME
          else exitGame();
        }

        if((Input.GetAxis("Vertical") < 0 || Input.GetAxis("Vertical2") < 0) &&
          bufferTimer == 0) { 
          selectingPlay = (selectingPlay + 1) % 3;
          bufferTimer = bufferTimeAmt;
        }
        else if((Input.GetAxis("Vertical1") > 0 || Input.GetAxis("Vertical2") > 0) &&
          bufferTimer == 0) { 
          selectingPlay = (selectingPlay + 2) % 3;
          bufferTimer = bufferTimeAmt;
        }
        if(bufferTimer > 0) {
          bufferTimer -= 1;
        }

        //Moving the button selection back/forth
        switch(selectingPlay) {
          case 0: 
            tutorialButtonBox.enabled = true;
            playButtonBox.enabled = false;
            exitButtonBox.enabled = false;
            owlImage.GetComponent<Image>().sprite = owlOpen;
            break;
          case 1: 
            tutorialButtonBox.enabled = false;
            playButtonBox.enabled = true;
            exitButtonBox.enabled = false;
            owlImage.GetComponent<Image>().sprite = owlSquint;
            break;
          case 2:
            tutorialButtonBox.enabled = false; 
            playButtonBox.enabled = false;
            exitButtonBox.enabled = true;
            owlImage.GetComponent<Image>().sprite = owlOpen;
            break;
        }
    }

    //Loads the given scene
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
