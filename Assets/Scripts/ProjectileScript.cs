

using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        //if collided with Player then disappear
        if (other.gameObject.tag == "Player") {
            Destroy(gameObject);
        }
    }
}