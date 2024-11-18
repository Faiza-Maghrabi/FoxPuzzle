using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //angle of FOV and how far enemy can see
    public float fovAngle;
    public float detectionRadius;
    //refrence to player object
    private Transform player;
    //speed of enemy
    public float speed = 1.0f;
    //boolean to control behaviour
    private bool playerInView;
    //damage enemy does to player
    public int attackVal;
    private Rigidbody rb;

    private Vector3 directionToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        attackVal = attackVal == 0 ? 10 : attackVal;
        playerInView = false;
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Look for player
        playerInView = FoundPlayer();
        // if seen, look towards player and travel towards them
        if (playerInView) {
            //move enemy to face player on x and z
            Vector3 targetPosition = player.position;
            targetPosition.y = rb.position.y;
            transform.LookAt(targetPosition);
            //calculate distance per fram
            var step = speed * Time.deltaTime;
            //move enemy towards player via rb
            Vector3 newPosition = Vector3.MoveTowards(rb.position, player.position, step);
            rb.MovePosition(newPosition);
        }

    }

    public int getAttackVal() {
        return attackVal;
    }

    private bool FoundPlayer() {
        //use a sphere layers on the 'PlayerLayer' to see if player is nearby enemy
        int layerMask = LayerMask.GetMask("PlayerLayer");
        Collider[] FOVTargets = Physics.OverlapSphere(transform.position, detectionRadius, layerMask);
        if (FOVTargets.Count() > 0) {   //if nearby then count > 0
            //[0] should be FoxEnemyCollider - was not able to hit the mesh collider in Fox_Model
            directionToPlayer = FOVTargets[0].transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);  //Find angle between enemy and player
            RaycastHit hit;
            if (angle < fovAngle / 2) { //if angle less than FOV/2 use a raycast to see if enemy can see Player
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius)) {
                    if (hit.transform == player) {  //return true if player is hit
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //gizmo for testing
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, directionToPlayer * detectionRadius);
    }
}
