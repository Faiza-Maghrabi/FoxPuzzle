using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Transform player;

    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var rayDirection = player.position - transform.position;

        if (Physics.Raycast(transform.position, rayDirection, 10)) {

            if (hit.transform == player) {
                // enemy can see the player!
            } else {
                // there is something obstructing the view
            }

        }

    }
}
