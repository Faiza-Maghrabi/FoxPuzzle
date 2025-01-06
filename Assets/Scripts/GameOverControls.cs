using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverControls : MonoBehaviour
{
    private PlayerController player;
    public Inventory inventory;
    public GameObject gameOverObj;

    //options for the user when given a game over
    public void Start(){
        player = GameObject.Find("Player").GetComponent<PlayerController>(); //Player
    }
    //re-inits static values for a new game.
    public void Restart(string sceneName){
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        //player spawn pos hardcoded in
        PlayerScenePos.position[0] = 431.33f;
        PlayerScenePos.position[1] = 6.85f;
        PlayerScenePos.position[2] = 238.60f;
        RestartProperties();
    } 

    public void OpenMainMenu(string sceneName){
        SceneManager.LoadScene(sceneName);
        RestartProperties();
    } 

    public void RestartProperties(){
        PlayerController.score = 0;
        PlayerController.health = 100;
        PlayerController.init = true;
        FoodTracker.Init();

        Inventory.InitOrResetInventory();
        gameOverObj.SetActive(false);
    }
}
