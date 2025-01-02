

using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        //if collided with anything then disappear
        Destroy(gameObject);
    }
}