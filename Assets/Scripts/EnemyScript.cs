using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class DangerData {
    public Material material;
    public float range;
    public float angle;
    public float speed;
    

    public DangerData(Material material, float range, float angle, float speed)
    {
        this.material = material;
        this.range = range;
        this.angle = angle;
        this.speed = speed;
    }
}

public class EnemyScript : MonoBehaviour
{
    //angle of FOV and how far enemy can see
    private float fovAngle;
    public float maxFovAngle;
    private float detectionRadius;
    public float maxDetectionRadius;
    //refrence to player object
    public Transform player;
    //speed of enemy
    private float speed;
    public float maxSpeed = 1.0f;
    //boolean to control behaviour
    public bool playerInView;
    //damage enemy does to player
    public int attackVal;
    private Rigidbody rb;
    public bool hitPlayer = false;
    //animator variables
    int hitHash = Animator.StringToHash("HitPlayer");
    int moveHash = Animator.StringToHash("Moving");
    public Animator anim;
    private UnityEngine.Vector3 directionToPlayer;
    //vision cone variables
    public GameObject visionCone;
    public Material lowDangerMaterial;
    public Material mediumDangerMaterial;
    public Material highDangerMaterial;
    public Material maxDangerMaterial;
    private MeshRenderer coneRenderer;
    private int dangerVal;
    private Dictionary<int, DangerData> dangerToData;
    public int visionConeResolution = 120;
    private Mesh visionConeMesh;
    private MeshFilter meshFilter;
    private float coneAngle;

    // Start is called before the first frame update
    void Start()
    {
        attackVal = attackVal == 0 ? 10 : attackVal;
        playerInView = false;
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();

        dangerToData = new Dictionary<int, DangerData> {
            {0, new DangerData(lowDangerMaterial, 0.7f, 0.3f, 0.6f)},
            {1, new DangerData(mediumDangerMaterial, 0.8f, 0.6f, 0.7f)},
            {2, new DangerData(highDangerMaterial, 0.9f, 0.8f, 0.8f)},
            {3, new DangerData(maxDangerMaterial, 1.0f, 1.0f, 1.0f)},
        };

        dangerVal = SceneCompletion.getDangerLevel();
        visionCone.transform.AddComponent<MeshRenderer>().material = dangerToData[dangerVal].material;
        coneRenderer = visionCone.transform.GetComponent<MeshRenderer>();
        adjustValsForDanger();
        meshFilter = visionCone.transform.AddComponent<MeshFilter>();
        visionConeMesh = new Mesh();
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        DrawVisionCone();
        int tempOldVal = dangerVal;
        dangerVal = SceneCompletion.getDangerLevel();
        if (tempOldVal != dangerVal){
            adjustValsForDanger();
        }
        //Look for player
        playerInView = FoundPlayer();
        anim.SetBool(moveHash, playerInView && !hitPlayer);
        anim.SetBool(hitHash, hitPlayer);
        // if seen, look towards player and travel towards them
        if (playerInView && !hitPlayer) {
            //move enemy to face player on x and z
            UnityEngine.Vector3 targetPosition = player.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
            //calculate distance per fram
            var step = speed * Time.deltaTime;
            //move enemy towards player via rb
            UnityEngine.Vector3 newPosition = UnityEngine.Vector3.MoveTowards(rb.position, player.position, step);
            // Debug.Log(step);
            rb.MovePosition(newPosition);
        }

    }

    //adjust enemy speed, detection range, fovAngle, coneAngle, and coneMaterial according to difficulty
    public void adjustValsForDanger() {
        coneRenderer.material = dangerToData[dangerVal].material;
        speed = dangerToData[dangerVal].speed * maxSpeed;
        fovAngle = dangerToData[dangerVal].angle * maxFovAngle;
        detectionRadius = dangerToData[dangerVal].range * maxDetectionRadius;
        coneAngle = fovAngle * Mathf.Deg2Rad;
    }

    public int getAttackVal() {
        return attackVal;
    }

    private bool FoundPlayer() {
        //use a sphere layers on the 'PlayerLayer' to see if player is nearby enemy
        //is player is stealthing, detection radisus and FOV is reduced slightly
        int layerMask = LayerMask.GetMask("PlayerLayer");
        Collider[] FOVTargets = Physics.OverlapSphere(transform.position, detectionRadius - (PlayerController.isStealth ? 2 : 0 ), layerMask);
        if (FOVTargets.Count() > 0) {   //if nearby then count > 0
            //[0] should be FoxEnemyCollider - was not able to hit the mesh collider in Fox_Model

            directionToPlayer = (FOVTargets[0].transform.position - (transform.position)).normalized;
            float angle = UnityEngine.Vector3.Angle(transform.forward, directionToPlayer);  //Find angle between enemy and player
            if (angle < fovAngle / (PlayerController.isStealth ? 2.5: 2)) { //if angle less than FOV/2 use a raycast to see if enemy can see Player
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius, layerMask)) {
                    //Debug.Log(hit.transform);
                    if (hit.transform == player) {  //return true if player is hit
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void OnCollisionEnter(Collision other) {
        // if collided with a Player, set hitHash to true
        if (other.gameObject.tag == "Player")
        {  
            hitPlayer = true;
        }
    }

    void OnCollisionExit(Collision other) {
        // if left collision with a Player, set hitHash to false
        if (other.gameObject.tag == "Player")
        {
            hitPlayer = false;
        }
    }

    void DrawVisionCone() {
        int[] triangles = new int[(visionConeResolution - 1) * 3];
    	UnityEngine.Vector3[] Vertices = new UnityEngine.Vector3[visionConeResolution + 1];
        Vertices[0] = UnityEngine.Vector3.zero;
        float Currentangle = -coneAngle / (PlayerController.isStealth ? 2.5f: 2.0f);
        float angleIcrement = coneAngle * (PlayerController.isStealth ? 0.9f: 1f) / (visionConeResolution - 1);
        float Sine;
        float Cosine;

        for (int i = 0; i < visionConeResolution; i++)
        {
            Sine = Mathf.Sin(Currentangle);
            Cosine = Mathf.Cos(Currentangle);
            UnityEngine.Vector3 RaycastDirection = (transform.forward * Cosine) + (transform.right * Sine);
            UnityEngine.Vector3 VertForward = (UnityEngine.Vector3.forward * Cosine) + (UnityEngine.Vector3.right * Sine);
            if (Physics.Raycast(transform.position, RaycastDirection, out RaycastHit hit, detectionRadius - (PlayerController.isStealth ? 2 : 0 ), LayerMask.GetMask("Default")))
            {
                Vertices[i + 1] = VertForward * hit.distance;
            }
            else
            {
                Vertices[i + 1] = VertForward * ((detectionRadius - (PlayerController.isStealth ? 2 : 0 )) * 46);
            }


            Currentangle += angleIcrement;
        }
        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }
        visionConeMesh.Clear();
        visionConeMesh.vertices = Vertices;
        visionConeMesh.triangles = triangles;
        meshFilter.mesh = visionConeMesh;
    }

    //gizmo for testing
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, directionToPlayer * detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius - (PlayerController.isStealth ? 5 : 0 ));
    }
    
}
