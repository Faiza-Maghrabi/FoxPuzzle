using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    private PlayerController player;
    private readonly float maxHealth = 100f;
    private float health;
    private readonly float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        healthSlider.maxValue = maxHealth; 
        healthSlider.minValue = 0; 
        easeHealthSlider.maxValue = maxHealth; 
        easeHealthSlider.minValue = 0;
        health = player.health; // player health
        
    }

    // Update is called once per frame
    void Update(){
        health = player.health; // update player health

        // Check if health slider value is equal to player health and update value appropriately
        if(healthSlider.value != health){
            healthSlider.value = health;
        }  

        // Check if health slider value equals ease health slider value, and update it appropriate using lerp to show the damage taken
        if(healthSlider.value != easeHealthSlider.value){
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }


    }
}
