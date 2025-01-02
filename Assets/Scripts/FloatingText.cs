using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    Transform mainCam;
    Transform unit;
    Transform worldSpaceCanvas;
    public Vector3 offset;
    void Start()
    {
        mainCam = Camera.main.transform;
        unit = transform.parent;
        worldSpaceCanvas = GameObject.FindGameObjectWithTag("Tutorial").transform;

        transform.SetParent(worldSpaceCanvas);
    }

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position);
        transform.position = unit.position + offset;
    }
}
