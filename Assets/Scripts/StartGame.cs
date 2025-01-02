using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        //player spawn pos hardcoded in
        PlayerScenePos.position[0] = 4.06f;
        PlayerScenePos.position[1] = 0.3f;
        PlayerScenePos.position[2] = -14.96f;
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

}
