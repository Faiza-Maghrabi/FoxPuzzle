

using UnityEngine;
using UnityEngine.Scripting;

public class ProjectileScript : MonoBehaviour
{
    AudioManager audioManager;

    void Awake(){
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    //assigned to child enemy projectile - is destroyed on collision with anything
    void OnCollisionEnter(Collision other) {
        //if collided with anything then disappear
        Destroy(gameObject);
        audioManager.PlaySFX(audioManager.snowballHit, audioManager.boySFXSource);
    }
}