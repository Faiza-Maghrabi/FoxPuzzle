using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 moveValue;
    public float speed;
    private int health = 100;
    private float normalSpeed;
    private float slowSpeed;

    void Start (){
        normalSpeed = speed;  // Store the normal speed
        slowSpeed = speed / 2;  // Define the reduced speed
    }

    void OnMove(InputValue value){
        moveValue = value.Get<Vector2>();  
        Debug.Log("speed: " + speed);
    }

    
    void FixedUpdate() {
        Vector3 movement = new(moveValue.x, 0.0f, moveValue.y);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
            speed = slowSpeed;  // Reduce speed when shift is held
        } else {
            speed = normalSpeed;  // Restore normal speed when shift is not held
        }

        GetComponent<Rigidbody>().AddForce(speed * Time.fixedDeltaTime * movement);
    }

    public virtual int getPlayerHealth(){
        return health;
    }
}
