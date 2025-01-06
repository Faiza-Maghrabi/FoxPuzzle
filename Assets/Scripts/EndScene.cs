using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScene : MonoBehaviour
{
    //private PlayerController player;
    public TMP_Text scoreText;
    public TMP_Text messageText;

    void Awake(){
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(PlayerController.score);
        // Player Score displayed on screen
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
        scoreText.text = "You have accumilated a score of : " + PlayerController.score.ToString();
        
        if (PlayerController.score >= 60000)
        {
            messageText.text = "Absolute top tier player. Your reward is a round of applause from the developers of the game.";
        }
        else if (PlayerController.score >= 45000)
        {
            messageText.text = "You’ve outsmarted the pack. What a clever fox!";
        }
        else if (PlayerController.score >= 30000)
        {
            messageText.text = "A solid score! But even foxes dream of higher peaks.";
        }
        else if (PlayerController.score >= 10000)
        {
            messageText.text = "A fox in training! Time to pounce on that score.";
        }
        else
        {
            messageText.text = "Don’t worry, every fox stumbles before it learns to hunt!";
        }

    }
}
