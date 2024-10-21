using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScene : MonoBehaviour
{
    private PlayerController player;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        Debug.Log(player.score);
        // Player Score displayed on screen
        scoreText.text = "You have accumilated a score of : " + player.score.ToString();
        
    }
}
