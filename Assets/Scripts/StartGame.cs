using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuFirst;

    public void Awake(){
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    //sets difficulty as normal on start
    public void Start(){
        if(!PlayerPrefs.HasKey("difficulty"))
        {
            setDifficulty(0);
        }
    }
    

    public void LoadScene(string sceneName)
    {
        //player spawn pos hardcoded in
        PlayerScenePos.position[0] = 431.33f;
        PlayerScenePos.position[1] = 7.85f;
        PlayerScenePos.position[2] = 238.60f;
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;

        //reset anything that may have changed in tutorial
        PlayerController.health = 100;
        PlayerController.score = 0;
        FoodTracker.Init();
        Inventory.InitOrResetInventory();
        PlayerController.init = false;
    }

    //loads tutorial scene
    public void LoadTutorial() {
        PlayerScenePos.position[0] = 49.7f;
        PlayerScenePos.position[1] = 0.0f;
        PlayerScenePos.position[2] = 6.66f;
        SceneManager.LoadScene("Tutorial");
        Inventory.InitOrResetInventory();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    // set user prefrence for difficulty
    public void setDifficulty(int diffVal){
        PlayerPrefs.SetInt("difficulty", diffVal);
    }

}
