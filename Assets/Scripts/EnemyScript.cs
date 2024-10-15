using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float fovAngle;
    public float detectionRadius;
    private Transform player;
    // Start is called before the first frame update

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool playerInView = false;

        playerInView = FoundPlayer();
        

    }

    private bool FoundPlayer() {
        int layerMask = LayerMask.GetMask("PlayerLayer");
        Collider[] FOVTargets = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);
        if (FOVTargets.Count() > 0) {
            Vector3 directionToPlayer = (FOVTargets[0].transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            RaycastHit hit;
            if (angle < fovAngle / 2) {
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius)) {
                    if (hit.transform == player) {
                        Debug.Log("player spotted");
                        return true;
                    }
                }
            }
        }
        Debug.Log("not found");
        return false;
    }
}
