using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{

    //Player Inventory
    public GameObject inventoryMenu;
    private bool menuActivated;
    public PlayerController player;
    public ItemSlot[] itemSlot;

    void Update(){
        if(Input.GetButtonDown("Inventory") && !menuActivated){
            Time.timeScale = 0;
            inventoryMenu.SetActive(true);
            menuActivated =  true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if(Input.GetButtonDown("Inventory") && menuActivated){
            Time.timeScale = 1;
            inventoryMenu.SetActive(false);
            menuActivated = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void CloseInventory(){
        Time.timeScale = 1;
        inventoryMenu.SetActive(false);
        menuActivated = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool EatFood(string foodName, int scoreVal, int healthRegen){
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(itemSlot[i].foodName == foodName){
                if(player.health == 100){
                    return false;
                }
                player.health += healthRegen;
                player.score -= scoreVal;
                return true;
            }
        }
        return false;
    }

    public void AddItemToInventory(FoodListItem food){
        for (int i = 0; i < itemSlot.Length; i++){
            if(itemSlot[i].isFull == false){
                itemSlot[i].AddItem(food);
                return;
            }
        }
    }

    public void DeselectAllSlots(){
        for (int i = 0; i < itemSlot.Length; i++){
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
