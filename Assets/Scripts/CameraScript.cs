using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CameraScript : MonoBehaviour
{
    private Transform player;
    public Transform orientationObj;
    public Rigidbody rb;
    public float RotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientationObj.forward = viewDir.normalized;

        //FeatureType horizontalInput = Input.GetAxis("Horizontal")
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
