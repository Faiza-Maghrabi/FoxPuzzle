using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    
    public PlayerController player;
    public float maxHealth = 100f;
    public float health;
    public float lerpSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.minValue = 0;
        easeHealthSlider.maxValue = maxHealth;
        easeHealthSlider.minValue = 0;
        health = player.getPlayerHealth();
        
    }

    // Update is called once per frame
    void Update(){
        health = player.getPlayerHealth();

        if(healthSlider.value != health){
            healthSlider.value = health;
        }  

        if(healthSlider.value != easeHealthSlider.value){
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }


    }
}
