using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScene : MonoBehaviour
{
    //private PlayerController player;
    public TMP_Text scoreText;

    void Awake(){
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PlayerController.score);
        // Player Score displayed on screen
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
        scoreText.text = "You have accumilated a score of : " + PlayerController.score.ToString();
        
    }
}
