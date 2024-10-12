using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Jump functionality properties
[Serializable]
public struct JumpSettings {
    public float buttonTime;
    // hold jump height for player
    public float height;
    // duration of the jump
    public float duration;
    // Is the player jumping
    public bool isJumping;
    // Is the jump cancelled
    public bool isJumpCancelled;
}

// Speed settings for crouching and walking
[Serializable]
public struct SpeedSettings {
    public float normalSpeed;
    public float slowSpeed;
}

public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    //player speed
    public float speed;
    public SpeedSettings speedSettings;
    // player health
    private int health = 100;
    private Rigidbody rb;
    public JumpSettings jump;

    // hold score here as player has easy access to values on collision
    private int score;


    void Start (){
        speedSettings.normalSpeed = speed;  // Store the normal speed
        speedSettings.slowSpeed = speed / 2;  // Define the reduced speed
        rb = GetComponent<Rigidbody>(); // Allows access to the rigid body for readability purposes
        jump.isJumping = false; // Player is not jumping when the game launches
        jump.buttonTime = 0.5f;
        jump.duration = 0;
        jump.height = 3; 
        score = 0;
    }

    void OnMove(InputValue value){
        moveValue = value.Get<Vector2>();  
        // Debug.Log("speed: " + speed);
    }

    private void Update(){
        // Player jumps when the space key is pressed and not in mid air
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
            Jump();
        }

        if (jump.isJumping){
            jump.duration += Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space)){
                jump.isJumpCancelled = true;
                jump.isJumping = false;
            }

            if (jump.duration > jump.buttonTime){
                jump.isJumping = false;
            }
        }
    }

    private bool IsGrounded() {
        // Simple check for whether the player is on the ground
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    
    void FixedUpdate() {
        // handles movement logic
        Vector3 movement = new(moveValue.x, 0.0f, moveValue.y);

        // adjust speed when shift key is held as player is crouching
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            speed = speedSettings.slowSpeed;  // Reduce speed when shift is held
        } else {
            speed = speedSettings.normalSpeed;  // Restore normal speed when shift is not held
        }

        rb.AddForce(speed * Time.fixedDeltaTime * movement);

        if(jump.isJumpCancelled && jump.isJumping && rb.velocity.y > 0){
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
            jump.isJumping = false;
        }
    }

    // handles jump logic
    private void Jump() {
        float jumpForce = Mathf.Sqrt(2 * jump.height * -Physics.gravity.y);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        jump.isJumping = true;
        jump.isJumpCancelled = false;
        jump.duration = 0;
    }

    // return player health
    public virtual int GetPlayerHealth(){
        return health;
    }
 
    //return player score
    public virtual int GetPlayerScore(){
        return score;
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
