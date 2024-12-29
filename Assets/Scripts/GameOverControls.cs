using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverControls : MonoBehaviour
{
    private PlayerController player;
    public Inventory inventory;
    public GameObject gameOverObj;

    public void Start(){
        player = GameObject.Find("Player").GetComponent<PlayerController>(); //Player
    }
    public void Restart(string sceneName){
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        //player spawn pos hardcoded in
        PlayerScenePos.position[0] = 4.06f;
        PlayerScenePos.position[1] = 0.3f;
        PlayerScenePos.position[2] = -14.96f;
        RestartProperties();
    } 

    public void OpenMainMenu(string sceneName){
        SceneManager.LoadScene(sceneName);
        RestartProperties();
    } 

    public void RestartProperties(){
        PlayerController.health = 100;
        PlayerController.score = 0;
        
        for (int i = 0; i < Inventory.items.Length; i++)
        {
            if (Inventory.items[i].isFull){
                Inventory.items[i].ResetItem();
            }
        }
    }
}
