using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{

    //Player Inventory
    public GameObject inventoryObj;
    private bool menuActivated;
    public PlayerController player;
    public ItemSlot[] itemSlot;

    //Updates so we can open and close inventory when pressing the I key by checking if it has been activated via the menuActivated bool
    void Update(){
        if(Input.GetButtonDown("Inventory") && !menuActivated){
            Time.timeScale = 0; //Pauses the game
            inventoryObj.SetActive(true);
            menuActivated =  true; 
            Cursor.lockState = CursorLockMode.None; //Unlocks cursor so player can freely select items
            Cursor.visible = true;
        }
        else if(Input.GetButtonDown("Inventory") && menuActivated){
            Time.timeScale = 1; //Unpauses the game
            inventoryObj.SetActive(false);
            menuActivated = false;
            Cursor.lockState = CursorLockMode.Locked; //Locks cursor so player can play normally again
            Cursor.visible = false;
        }
    }

    //An onclick function to close the inventory for the close button
    public void CloseInventory(){
        Time.timeScale = 1;
        inventoryObj.SetActive(false);
        menuActivated = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //Allows the player to eat food
    public bool EatFood(string foodName, int scoreVal, int healthRegen){
        for (int i = 0; i < itemSlot.Length; i++)
        {   
            // Checks if the food equals the foodname given ans checks if the players health is max before allowing player to eat.
            if(itemSlot[i].foodName == foodName){
                if(player.health == 100){
                    return false;
                }
                player.health += healthRegen; //health increase
                player.score -= scoreVal; //score is decreased
                return true;
            }
        }
        return false;
    }

    //Adds an item to the itemslot when player picks it up. 
    public void AddItemToInventory(FoodListItem food){
        for (int i = 0; i < itemSlot.Length; i++){
            // checks if the item slot is full and if it isn't it updates the empty item slot
            if(itemSlot[i].isFull == false){
                itemSlot[i].AddItem(food);
                return;
            }
        }
    }

    //Deselects all slots by setting the selected item panel to false so only one slot is selected at a time
    public void DeselectAllSlots(){
        for (int i = 0; i < itemSlot.Length; i++){
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
