using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    private Vector2 moveValue;
    //player speed
    public float speed;
    private SpeedSettings speedSettings;
    // player health
    public static int health;
    //player inventory
    public Inventory inventory;
    public GameObject gameOverObj;
    private Rigidbody rb;
    private JumpSettings jump;
    private float triggerTime;
    // hold score here as player has easy access to values on collision
    public static int score;
    public static bool init = false;
    //animator variables
    int jumpHash = Animator.StringToHash("Jump");
    int speedHash = Animator.StringToHash("Speed");
    int groundHash = Animator.StringToHash("isGrounded");
    public Animator anim;

    //cinemachine collider to add damping when jumping
    public CinemachineCollider cinemachineCollider;

    void Start (){
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (currentScene == "EndScene"){
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        speedSettings.normalSpeed = speed;  // Store the normal speed
        speedSettings.slowSpeed = speed / 2;  // Define the reduced speed
        rb = GetComponent<Rigidbody>(); // Allows access to the rigid body for readability purposes
        rb.freezeRotation= true;
        //rb.constraints = RigidbodyConstraints.FreezePositionZ;
        rb.drag = 1f;
        jump.isJumping = false; // Player is not jumping when the game launches
        jump.buttonTime = 0.5f;
        jump.duration = 0;
        jump.height = 10; 
        if (!init){
            score = 0;
            health = 100;
            init = true;
        }
    }

    void OnMove(InputValue value){
        moveValue = value.Get<Vector2>();
        
    }

    //Opens inventory when the e key is pressed
    void OnInventory(InputValue value){
        inventory.OnInventory(value);
    }

    void OnJump(InputValue input){
        // Player jumps when the space key is pressed and not in mid air
        if (IsGrounded()){
            Jump();
        }
    }

    private void Update(){
        anim.SetFloat(speedHash, moveValue.x* moveValue.x + moveValue.y* moveValue.y);
        anim.SetBool(jumpHash, jump.isJumping);
        anim.SetBool(groundHash, IsGrounded());

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

        if(!jump.isJumping && cinemachineCollider.m_Damping != 0f && IsGrounded() && cinemachineCollider.m_DampingWhenOccluded != 0f) {
            cinemachineCollider.m_Damping = 0f;
            cinemachineCollider.m_DampingWhenOccluded = 0f;
        }   

        if(health == 0){
            gameOverObj.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
    }

    private bool IsGrounded() {
        // Simple check for whether the player is on the ground
        //does not hit the floor if using transform.position - hence the slight upwards movement
        var pos = transform.position;
        pos.y = pos.y + 1f;
        return Physics.Raycast(pos, Vector3.down, 1.1f);
    }

    
    void FixedUpdate() {
        //Debug.Log(score == 0 ? "" : score);
        // handles movement logic
        //use the camera to find out direction of movement for player
        float horizontalAxis = moveValue.x;
        float verticalAxis = moveValue.y;
        var camera = Camera.main;

        var forward = camera.transform.forward;
        var right = camera.transform.right;
        // //remove the differences in y axis and normalise
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

        if (desiredMoveDirection.magnitude >= 0.1f){
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-desiredMoveDirection), 0.15F);

            // adjust speed when shift key is held as player is crouching
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                speed = speedSettings.slowSpeed;  // Reduce speed when shift is held
            } else {
                speed = speedSettings.normalSpeed;  // Restore normal speed when shift is not held
            }

            rb.AddForce(desiredMoveDirection * speed * Time.deltaTime, ForceMode.Impulse);

            if(jump.isJumpCancelled && jump.isJumping && rb.velocity.y > 0){
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
                jump.isJumping = false;
            }
        }

    }

    // handles jump logic
    private void Jump() {
        cinemachineCollider.m_Damping = 2f;
        cinemachineCollider.m_DampingWhenOccluded = 2f;
        float jumpForce = Mathf.Sqrt(2 * jump.height * -Physics.gravity.y);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        jump.isJumping = true;
        jump.isJumpCancelled = false;
        jump.duration = 0;
    }

    void OnTriggerEnter(Collider other) {
        // if collided with a FoodItem, hide the item and calls the function to add the item to the players inventory and add score to counter
        if (other.gameObject.CompareTag("FoodItem")) {
            other.gameObject.SetActive(false);
            FoodScript food = other.GetComponent<FoodScript>();
            inventory.AddItemToInventory(food.food);
            PlayerController.score += food.scoreVal;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            triggerTime = Time.time;
        }
    }

    void OnCollisionEnter(Collision other) {
        //if collided with Enemy then take damage
        if (other.gameObject.tag == "Enemy") {
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            health -= enemy.getAttackVal();
        }
    }

    void OnCollisionStay(Collision other) {
        //if still collided with Enemy then continue to take damage;
        if (Time.time - triggerTime > 1)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
                health -= enemy.getAttackVal();
            }
            triggerTime += 1;
        }
    }
}
