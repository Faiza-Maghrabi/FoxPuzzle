using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public float speed;
    private int health = 100;
    private float normalSpeed;
    private float slowSpeed;
    public TextMeshProUGUI scoreText;

    // hold score here as player has easy access to values on collision
    public int score;

    void Start (){
        normalSpeed = speed;  // Store the normal speed
        slowSpeed = speed / 2;  // Define the reduced speed

        score = 0;
    }

    void OnMove(InputValue value){
        moveValue = value.Get<Vector2>();  
        // Debug.Log("speed: " + speed);
    }

    
    void FixedUpdate() {
        Vector3 movement = new(moveValue.x, 0.0f, moveValue.y);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            speed = slowSpeed;  // Reduce speed when shift is held
        } else {
            speed = normalSpeed;  // Restore normal speed when shift is not held
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            health -= 5;
        }

        GetComponent<Rigidbody>().AddForce(speed * Time.fixedDeltaTime * movement);
        scoreText.text = "Score: " + score.ToString();
    }

    public virtual int getPlayerHealth(){
        return health;
    }

    void OnTriggerEnter(Collider other) {
        // if collided with a FoodItem, hide the item and add score to counter
        if (other.gameObject.tag == "FoodItem") {
            other.gameObject.SetActive(false);
            FoodScript food = other.GetComponent<FoodScript>();
            score += food.getScore();
        }
    }
}
