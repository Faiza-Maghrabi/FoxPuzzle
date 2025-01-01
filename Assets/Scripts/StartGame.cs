using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class StartGame : MonoBehaviour
{

    public GameObject SettingsPanel;
    public GameObject KeyboardControlsPanel;
    public GameObject GamepadControlsPanel;
    public Toggle damageFlashToggle; 
    public PlayerController player;

    [SerializeField]
    private GameObject mainMenuFirst;
    [SerializeField]
    private GameObject settingsMenuFirst;
    [SerializeField]
    private GameObject keyboardControlsMenuFirst;
    [SerializeField]
    private GameObject gamepadControlsMenuFirst;

    public void Awake(){
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }
    

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

        //reset anything that may have changed in tutorial
        PlayerController.health = 100;
        PlayerController.score = 0;
        FoodTracker.Init();
        for (int i = 0; i < Inventory.items.Length; i++)
        {
            if (Inventory.items[i].isFull){
                Inventory.items[i].ResetItem();
            }
        }
        PlayerController.init = false;
    }

    public void LoadTutorial() {
        PlayerScenePos.position[0] = 49.7f;
        PlayerScenePos.position[1] = 0.0f;
        PlayerScenePos.position[2] = 6.66f;
        SceneManager.LoadScene("Tutorial");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    public void OpenSettings() {
        SettingsPanel.SetActive(true);
        StartCoroutine(SelectAfterFrame(settingsMenuFirst));
        damageFlashToggle.onValueChanged.RemoveListener(OnToggleChanged);
        damageFlashToggle.isOn = PlayerController.isDamageFlashOn; // Update toggle state
        damageFlashToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool value)
    {
        // Update the boolean based on the toggle's state
        PlayerController.isDamageFlashOn = value;
    }

    private IEnumerator SelectAfterFrame(GameObject button) {
        yield return null;  // Wait for the next frame
        EventSystem.current.SetSelectedGameObject(button);
    }

    public void CloseSettings(){
        SettingsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    public void OpenKeyboardControlsSettings(){
        KeyboardControlsPanel.SetActive(true);
        CloseSettings();
        StartCoroutine(SelectAfterFrame(keyboardControlsMenuFirst));
    }

    public void CloseKeyboardControlsSettings(){
        KeyboardControlsPanel.SetActive(false);
        OpenSettings();
    }

    public void OpenGamepadControlsSettings(){
        GamepadControlsPanel.SetActive(true);
        CloseSettings();
        StartCoroutine(SelectAfterFrame(gamepadControlsMenuFirst));
    }

    public void CloseGamepadControlsSettings(){
        GamepadControlsPanel.SetActive(false);
        OpenSettings();
    }

}
