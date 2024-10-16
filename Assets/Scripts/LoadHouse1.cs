using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHouse1 : MonoBehaviour
{
    public string sceneToLoad;

    /*
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Trigger"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
