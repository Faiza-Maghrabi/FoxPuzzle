using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

[System.Serializable]
//Class to hold data each JSON object obtains
public class ItemData
{
    public string foodName;
    public string foodIcon;
    public int quantity;
    public int healthRegen;
    public int scoreVal;
    public string foodDescription;
    public string foodAttributes;
    public bool isFull;

    public ItemData(){
        ResetItem();
    }

    public void AddItem(FoodListItem food){
        this.foodName = food.foodName;
        this.foodIcon = food.foodIcon;
        this.scoreVal = food.scoreVal;
        this.quantity += food.quantity;
        this.healthRegen = food.healthRegen;
        this.foodDescription = food.foodDescription;
        this.foodAttributes = food.foodAttributes;
        isFull = true;
    }

    public int UseItem(){
        this.quantity -= 1;
        return this.quantity;
    }

    public void ResetItem(){
        foodName = "";
        foodIcon = null;
        quantity = 0;
        scoreVal = 0;
        healthRegen = 0;
        foodDescription = "";
        foodAttributes = "";
        isFull = false;
    }
}

public class Inventory : MonoBehaviour
{

    //Player Inventory
    public GameObject inventoryObj;
    public GameObject healthNotifObj;
    public GameObject closeHealthNotifButton;
    public GameObject closeOutOfStockNotifButton;
    public GameObject outOfStockNotif;
    private GameObject selectedItem;
    private bool menuActivated;
    public PlayerController player;
    public ItemSlot[] itemSlot;
    public static ItemData[] items;

    void Awake(){
        InitOrResetInventory();
    }

    public static void InitOrResetInventory() {
        if(Inventory.items == null){
            Inventory.items = new ItemData[12];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new ItemData();
            }
        }
        else {
             for (int i = 0; i < items.Length; i++)
            {
                if (items[i].isFull){
                    items[i].ResetItem();
                }
            }
        }
    }

    //Updates so we can open and close inventory when pressing the I key by checking if it has been activated via the menuActivated bool
    public void OnInventory(InputValue value){
        if(!menuActivated){
            Time.timeScale = 0; //Pauses the game
            inventoryObj.SetActive(true);
            menuActivated =  true; 
            Cursor.lockState = CursorLockMode.None; //Unlocks cursor so player can freely select items
            Cursor.visible = true;
            healthNotifObj.gameObject.SetActive(false);
            outOfStockNotif.gameObject.SetActive(false);
        }
        else if(menuActivated){
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

    public void OpenHealthNotif(){
        selectedItem =  EventSystem.current.currentSelectedGameObject;
        healthNotifObj.SetActive(true);
        EventSystem.current.SetSelectedGameObject(closeHealthNotifButton);
    }

    public void CloseHealthNotif(){
        healthNotifObj.SetActive(false);
        Debug.Log(selectedItem);
        EventSystem.current.SetSelectedGameObject(selectedItem);
    }

    public void OpenOutOfStockNotif(){
        selectedItem = EventSystem.current.currentSelectedGameObject;
        outOfStockNotif.SetActive(true);
        EventSystem.current.SetSelectedGameObject(closeOutOfStockNotifButton);
    }

    public void CloseOutOfStockNotif(){
        outOfStockNotif.SetActive(false);
        EventSystem.current.SetSelectedGameObject(selectedItem);
    }

    //Allows the player to eat food
    public bool EatFood(string foodName, int scoreVal, int healthRegen){
        for (int i = 0; i < itemSlot.Length; i++)
        {   
            // Checks if the food equals the foodname given ans checks if the players health is max before allowing player to eat.
            if(items[i].foodName == foodName){
                if(PlayerController.health == 100){
                    OpenHealthNotif();
                    return false;
                }
                PlayerController.health += healthRegen; //health increase
                PlayerController.score -= scoreVal; //score is decreased
                return true;
            }
        }
        return false;
    }

    //Adds an item to the itemslot when player picks it up. 
    public bool AddItemToInventory(FoodListItem food){
        Sprite resolvedSprite = FoodScript.GetSprite(food.foodIcon);

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].isFull == true && items[i].foodName == food.foodName || items[i].quantity == 0 && items[i].isFull == false)
            {
                itemSlot[i].AddItem(food, resolvedSprite);  // Pass sprite to UI
                return true;
            }
        }
        return false;
    }


    //Deselects all slots by setting the selected item panel to false so only one slot is selected at a time
    public void DeselectAllSlots(){
        for (int i = 0; i < itemSlot.Length; i++){
            itemSlot[i].thisItemSelectedOnce = false;
            itemSlot[i].thisItemSelectedTwice = false;
        }
    }
}
