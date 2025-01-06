using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEditor.Timeline.Actions;

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

// Speed settings for sneaking and walking
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
    public SettingsControls settings;
    public GameObject selectedItemIcon;
    public GameObject gameOverObj;
    public GameObject restartButton;
    private Rigidbody rb;
    private JumpSettings jump;
    private float triggerTime;
    private bool hitEnemy = false;
    private int enemyDamage = 0;
    // hold score here as player has easy access to values on collision
    public static int score;
    public static bool init = false;
    //stealth value
    public static bool isStealth = false;
    private PlayerInput playerInput;
    private InputAction sneakAction;
    //animator variables
    int jumpHash = Animator.StringToHash("Jump");
    int speedHash = Animator.StringToHash("Speed");
    int groundHash = Animator.StringToHash("isGrounded");
    public Animator anim;
    //number of enemies attacking the player at a time
    public static int huntedVal;

    //cinemachine collider to add damping when jumping
    public CinemachineCollider cinemachineCollider;

    SkinnedMeshRenderer meshRenderer;
    Material[] origMaterials;

    public static bool isDamageFlashOn = true;
    public Material[] damageFlash;
    float flashTime = .025f;
    private AudioManager audioManager;
    private bool playedHurtSound = false;

    private AudioClip walk;
    private AudioClip run;


    void Awake(){
        if (!init){
            score = 0;
            health = 100;
            init = true;
            FoodTracker.Init();
            Inventory.InitOrResetInventory();
            //prevent errors during dev work - remove on prod?
            if (PlayerScenePos.position == null) {
                PlayerScenePos.position = new float[3];
            }
            PlayerScenePos.position[0] = gameObject.transform.position.x;
            PlayerScenePos.position[1] = gameObject.transform.position.y;
            PlayerScenePos.position[2] = gameObject.transform.position.z;
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        playerInput = GetComponent<PlayerInput>();
        sneakAction = playerInput.actions["Sneak"]; //gets sneak action
    }   

    void Start (){
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (currentScene == "EndScene" || currentScene == "MainMenu"){
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if(currentScene == "IndoorHouse"){
            walk = audioManager.foxWalk2;
            run = audioManager.foxRun2;
        } 
        else{
            walk = audioManager.foxWalk1;
            run = audioManager.foxRun1;
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
        jump.height = 15; 
        //no enemy is hunting the player on start
        huntedVal = 0;

        //gets the skinned mesh renderer and the materials used
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        origMaterials = meshRenderer.sharedMaterials;

        PlayerScenePos.loadingScene = false;

        //comment out if testing specific locations
        rb.position = new Vector3(PlayerScenePos.position[0], PlayerScenePos.position[1], PlayerScenePos.position[2]);
        //use this to find coords to input in SceneList.json
        Debug.Log(rb.position);
    }

    void OnMove(InputValue value){
        moveValue = value.Get<Vector2>();
        if(IsGrounded())
        {
            audioManager.PlaySFX(run, audioManager.foxSFXSource);
        }
    }

    //Opens inventory when the e key is pressed
    void OnInventory(InputValue value){
        inventory.OnInventory(value);
        StartCoroutine(SelectAfterFrame(selectedItemIcon));
    }

    void OnPauseGame(InputValue value){
        settings.OnPauseGame(value);
    }

    void OnJump(InputValue input){
        // Player jumps when the space key is pressed and not in mid air
        if (IsGrounded()){
            Jump();
            audioManager.Stop(audioManager.foxSFXSource);

        }
    }

    //Enables sneak
    void OnEnable() {
        sneakAction.performed += OnSneakPerformed;
        sneakAction.canceled += OnSneakCanceled;
    }

    //disables sneak
    void OnDisable() {
        sneakAction.performed -= OnSneakPerformed;
        sneakAction.canceled -= OnSneakCanceled;
    }

    private void OnSneakPerformed(InputAction.CallbackContext context)
    {
        speed = speedSettings.slowSpeed;  // Reduce speed when sneaking
        isStealth = true;
        audioManager.Stop(audioManager.foxSFXSource);
        audioManager.PlaySFX(walk, audioManager.foxSFXSource);
    }

    private void OnSneakCanceled(InputAction.CallbackContext context)
    {
        speed = speedSettings.normalSpeed;  // Restore normal speed
        isStealth = false;
        audioManager.Stop(audioManager.foxSFXSource);
        audioManager.PlaySFX(run, audioManager.foxSFXSource);
    }

    private IEnumerator SelectAfterFrame(GameObject button) {
        yield return null;  // Wait for the next frame
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void ToggleFlash(){
        isDamageFlashOn = !isDamageFlashOn;
    }

    private void Update(){
        var notMoving = moveValue.x* moveValue.x + moveValue.y* moveValue.y == 0;
        anim.SetFloat(speedHash, notMoving ? 0 : speed);
        anim.SetBool(jumpHash, jump.isJumping);
        anim.SetBool(groundHash, IsGrounded());

        if(notMoving){
            audioManager.Stop(audioManager.foxSFXSource);
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

        

        if(!jump.isJumping && cinemachineCollider.m_Damping != 0f && IsGrounded() && cinemachineCollider.m_DampingWhenOccluded != 0f) {
            cinemachineCollider.m_Damping = 0f;
            cinemachineCollider.m_DampingWhenOccluded = 0f;
            audioManager.PlaySFX(audioManager.foxLand, audioManager.foxSFXSource);

        }   

        if(health <= 0){
            gameOverObj.SetActive(true);
            PlayFoxHurtSound();
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
        
        if(huntedVal > 0)
        {
            audioManager.PlayMusic(audioManager.background2, audioManager.musicSource);
        }
        else
        {
            audioManager.PlayMusic(audioManager.background1, audioManager.musicSource);
        }
    }


    // changes the materials when player is attacked to reflect health decrease
    IEnumerator EFlash(){
        meshRenderer.sharedMaterials = damageFlash;
        yield return new WaitForSeconds(flashTime);
        meshRenderer.sharedMaterials = origMaterials;
    }

    private bool IsGrounded() {
        // Simple check for whether the player is on the ground
        //does not hit the floor if using transform.position - hence the slight upwards movement
        int layerMask = LayerMask.GetMask("Default");
        var pos = transform.position;
        var forward = transform.forward;
        pos.y = pos.y + 1f;
        pos.z = pos.z + forward.z;
        pos.x = pos.x + forward.x;
        var ray1 = Physics.Raycast(pos, Vector3.down * 1.5f, layerMask);
        pos.z = transform.position.z - forward.z;
        pos.x = transform.position.x - forward.x;
        var ray2 = Physics.Raycast(pos, Vector3.down * 1.5f, layerMask);
        return ray1 | ray2;
    }

    
    void FixedUpdate() {
        // handles movement logic
        //use the camera to find out direction of movement for player
        float horizontalAxis = moveValue.x;
        float verticalAxis = moveValue.y;
        var camera = Camera.main;

        var forward = camera.transform.forward;
        var right = camera.transform.right;
        // remove the differences in y axis and normalise
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

        if (desiredMoveDirection.magnitude >= 0.1f){
            //axis on fox are wrong so negate directions
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-desiredMoveDirection), 0.15F);

            rb.AddForce(desiredMoveDirection * speed * Time.deltaTime, ForceMode.Impulse);

            if(jump.isJumpCancelled && jump.isJumping && rb.velocity.y > 0){
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, rb.velocity.z);
                jump.isJumping = false;
            }
        }

        if (hitEnemy && (Time.time - triggerTime > 1))
        {
            health -= enemyDamage;
            PlayFoxHurtSound();
            if (isDamageFlashOn){
                flashTime = .01f;
                StartCoroutine(EFlash());
            }
            triggerTime += 1;
        }

        if(gameOverObj == isActiveAndEnabled)
        {
            StartCoroutine(SelectAfterFrame(restartButton));
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

    // plays fox hurt sound if it was not already played
    private void PlayFoxHurtSound()
    {
        if (!playedHurtSound)
        {
            audioManager.PlaySFX(audioManager.foxHurt, audioManager.foxSFXSource);
            playedHurtSound = true;
            StartCoroutine(ResetHurtSoundCooldown());
        }
        else 
        {
            playedHurtSound = false;
        }
    }

    // Reset the hurt sound cooldown
    private IEnumerator ResetHurtSoundCooldown()
    {
        yield return new WaitForSeconds(0.5f); // Cooldown duration
        playedHurtSound = false; // Allow the sound to play again
    }

    void OnTriggerEnter(Collider other) {
        // if collided with a FoodItem, hide the item and calls the function to add the item to the players inventory and add score to counter
        if (other.gameObject.CompareTag("FoodItem")) {
            FoodScript food = other.GetComponent<FoodScript>();
            bool added = inventory.AddItemToInventory(food.food);
            if (added){
                other.gameObject.SetActive(false);
                FoodTracker.markCollected(gameObject.scene.name, other.gameObject.name);
                SceneCompletion.increaseFoodCount();
                PlayerController.score += food.scoreVal;
                audioManager.PlaySFX(audioManager.pickUpFood, audioManager.inventorySFXSource);
            }
        }
    }

    void OnCollisionEnter(Collision other) {
        //if collided with Enemy then take damage
        if (other.gameObject.tag == "Enemy") {
            if(isDamageFlashOn){
                flashTime = .005f;
                StartCoroutine(EFlash());
            }
            triggerTime = Time.time;
            hitEnemy = true;
            EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
            enemyDamage = enemy.getAttackVal();
            
            health -= enemyDamage/5;
            if(!playedHurtSound) {
                PlayFoxHurtSound();
            }
        }
        else if (other.gameObject.tag == "Projectile") {
            //set damage dealt as 15
            if(isDamageFlashOn){
                flashTime = .05f;
                StartCoroutine(EFlash());
            }
            health -= 15;
            if(!playedHurtSound) {
                PlayFoxHurtSound();
            }
        }
        else if (other.gameObject.tag == "Fire") {
            if(isDamageFlashOn){
                flashTime = .025f;
                StartCoroutine(EFlash());
            }
            triggerTime = Time.time;
            hitEnemy = true;
            enemyDamage = 5;
            health -= 5;
            if(!playedHurtSound) {
                PlayFoxHurtSound();
            }
        }
    }

    void OnCollisionExit(Collision other) {
        //if collision with enemy ends then set hitEnemy false
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Fire") {
            hitEnemy = false;
        }
    }

    //gizmo for testing
    void OnDrawGizmosSelected()
    {
        var forward = transform.forward;
        Gizmos.color = Color.red;
        var pos = transform.position;
        pos.y = pos.y + 1f;
        Gizmos.DrawRay(pos, Vector3.down * 1.05f);
        pos.z = pos.z + forward.z;
        pos.x = pos.x + forward.x;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(pos, Vector3.down * 1.05f);
        pos.z = transform.position.z - forward.z;
        pos.x = transform.position.x - forward.x;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(pos, Vector3.down * 1.05f);
    }
}

