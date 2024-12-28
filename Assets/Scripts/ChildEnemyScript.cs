using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ChildEnemyScript : EnemyScript {
    public GameObject projectile;
    public Transform spawnPoint;
    public float projectileSpeed;

    void ShootProjectile() {
        //used as animation event in Throw Object
        if (playerInView && !hitPlayer) {
            GameObject projectileObj = Instantiate(projectile, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody projectileRig = projectileObj.GetComponent<Rigidbody>();
            projectileRig.AddForce(projectileRig.transform.forward * projectileSpeed);
            Destroy(projectileObj, 1.0f);
        }
    }

}