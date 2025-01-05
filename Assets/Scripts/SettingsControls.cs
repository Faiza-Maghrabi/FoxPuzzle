using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SettingsControls : MonoBehaviour
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
    private bool menuActivated = false;

    public void OnPauseGame(InputValue value){
        if(!menuActivated){
            menuActivated =  true; 
            OpenSettings();
            
        }
        else if(menuActivated){
            CloseSettings();
        }
    }

    public void OpenSettings() {
        SettingsPanel.SetActive(true);
        StartCoroutine(SelectAfterFrame(settingsMenuFirst));
        damageFlashToggle.onValueChanged.RemoveListener(OnToggleChanged);
        damageFlashToggle.isOn = PlayerController.isDamageFlashOn; // Update toggle state
        damageFlashToggle.onValueChanged.AddListener(OnToggleChanged);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true;
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
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu"){
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
        }
    }

    public void OpenKeyboardControlsSettings(){
        KeyboardControlsPanel.SetActive(true);
        StartCoroutine(SelectAfterFrame(keyboardControlsMenuFirst));
    }

    public void CloseKeyboardControlsSettings(){
        KeyboardControlsPanel.SetActive(false);
        OpenSettings();
    }

    public void OpenGamepadControlsSettings(){
        GamepadControlsPanel.SetActive(true);
        StartCoroutine(SelectAfterFrame(gamepadControlsMenuFirst));
    }

    public void CloseGamepadControlsSettings(){
        GamepadControlsPanel.SetActive(false);
        OpenSettings();
    }
}
