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
    [SerializeField]
    private Button normalButton;
    [SerializeField]
    private Button hardButton;
    private bool menuActivated = false;
    //code shared by main meny's settings panel and the pause menu
    //sets the colour of difficulty buttons at start
    public void Start(){
        int diffVal = PlayerPrefs.GetInt("difficulty");
        Image normImage  = normalButton.GetComponent<Image>();
        Image hardImage  = hardButton.GetComponent<Image>();
        Color selectColour;
        ColorUtility.TryParseHtmlString("#FDD2AD", out selectColour);
        if (diffVal == 0){
            normImage.color = selectColour;
            hardImage.color = Color.white;
        }
        else {
            normImage.color = Color.white;
            hardImage.color = selectColour;
        }
    }

    //called by player, opens pause menu
    public void OnPauseGame(InputValue value){
        if(!menuActivated){
            menuActivated =  true; 
            OpenSettings();
            
        }
        else if(menuActivated){
            CloseSettings();
            menuActivated = false;
        }
    }

    //used on main menu to open settings page
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

    public void OnToggleChanged(bool value)
    {
        // Update the boolean based on the toggle's state
        PlayerController.isDamageFlashOn = value;
    }

    private IEnumerator SelectAfterFrame(GameObject button) {
        yield return null;  // Wait for the next frame
        EventSystem.current.SetSelectedGameObject(button);
    }

    //closes the settings/pause menu with if statement on the type of scene it is
    public void CloseSettings(){
        SettingsPanel.SetActive(false);
        Time.timeScale = 1;
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "MainMenu"){
            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
        }else{
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false;
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

    // set user prefrence for difficulty and edits button colour
    public void setDifficulty(int diffVal){
        PlayerPrefs.SetInt("difficulty", diffVal);
        Image normImage  = normalButton.GetComponent<Image>();
        Image hardImage  = hardButton.GetComponent<Image>();
        Color selectColour;
        ColorUtility.TryParseHtmlString("#FDD2AD", out selectColour);
        if (diffVal == 0){
            normImage.color = selectColour;
            hardImage.color = Color.white;
        }
        else {
            normImage.color = Color.white;
            hardImage.color = selectColour;
        }
    }
}
