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
        
        if (PlayerController.score >= 3500)
        {
            messageText.text = "Absolute top tier player. Your reward is a round of applause from the developers of the game.";
        }
        else if (PlayerController.score >= 2500)
        {
            messageText.text = "Amazing high score! Well Done :)";
        }
        else if (PlayerController.score >= 1500)
        {
            messageText.text = "Solid score. Good job and keep it up!";
        }
        else if (PlayerController.score >= 500)
        {
            messageText.text = "You gave it your all, and that’s what counts!";
        }
        else
        {
            messageText.text = "Was that a warm-up? Let’s see your real skills next time!";
        }

    }
}
