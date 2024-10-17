using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
    public int health;
    public int PlayerHealth{
        get { return health; }
        set { health = value; }
    }

    public Inventory inventory;
    // public InventoryDisplay inventoryDisplay;

    // public TextMeshProUGUI scoreText;
    private Rigidbody rb;
    public JumpSettings jump;


    // hold score here as player has easy access to values on collision
    public int score;
    public int PlayerScore{
        get { return score; }
        set { score = value; }
    }


    void Start (){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        speedSettings.normalSpeed = speed;  // Store the normal speed
        speedSettings.slowSpeed = speed / 2;  // Define the reduced speed
        rb = GetComponent<Rigidbody>(); // Allows access to the rigid body for readability purposes
        rb.freezeRotation= true;
        //rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.drag = 1f;
        jump.isJumping = false; // Player is not jumping when the game launches
        jump.buttonTime = 0.5f;
        jump.duration = 0;
        jump.height = 3; 
        score = 0;
        health = 100;        
    }

    void OnMove(InputValue value){
        moveValue = value.Get<Vector2>();
        
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
        //use the camera to find out direction of movement for player
        float horizontalAxis = moveValue.x;
        float verticalAxis = moveValue.y;
        var camera = Camera.main;

        var forward = camera.transform.forward;
        var right = camera.transform.right;
        //remove the differences in y axis and normalise
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

        // adjust speed when shift key is held as player is crouching
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            speed = speedSettings.slowSpeed;  // Reduce speed when shift is held
        } else {
            speed = speedSettings.normalSpeed;  // Restore normal speed when shift is not held
        }

        rb.AddForce(desiredMoveDirection * speed * Time.deltaTime);
        //Player Score displayed on screen
        //scoreText.text = "Score: " + score.ToString();

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

    void OnTriggerEnter(Collider other) {
        // if collided with a FoodItem, hide the item and add score to counter
        if (other.gameObject.CompareTag("FoodItem")) {
            other.gameObject.SetActive(false);
            FoodScript food = other.GetComponent<FoodScript>();
            inventory.AddItemToInventory(food.food);
            score += food.scoreVal;
        }
        // Debug.LogFormat(items[0].foodName);
        // Debug.Log(items[0].scoreVal);
        // Debug.Log(items[0].healthRegen);
    }

    void OnCollisionEnter(Collision other) {
        //if collided with Enemy then take damage
        if (other.gameObject.tag == "Enemy") {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            health -= enemy.getAttackVal();
            Debug.Log(health);
        }
    }
}
