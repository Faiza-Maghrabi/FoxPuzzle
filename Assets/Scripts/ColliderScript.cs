using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   //need to hide at start of game - only acting as collider for enemy to spot
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
