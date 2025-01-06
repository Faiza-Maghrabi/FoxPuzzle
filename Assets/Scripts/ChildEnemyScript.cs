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
        //throws a projectile aimed at the player
        if (playerInView && !hitPlayer) {
            GameObject projectileObj = Instantiate(projectile, spawnPoint.transform.position, spawnPoint.transform.rotation);
            Rigidbody projectileRig = projectileObj.GetComponent<Rigidbody>();
            UnityEngine.Vector3 direction = (player.position - projectileObj.transform.position).normalized;
            direction.y = direction.y + 0.2f;
            projectileRig.transform.forward = direction;
            projectileRig.AddForce(projectileRig.transform.forward * projectileSpeed);
            Destroy(projectileObj, 2.0f);
        }
    }

}