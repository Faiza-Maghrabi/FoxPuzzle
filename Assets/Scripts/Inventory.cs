using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{

    //Player Inventory
    public GameObject inventoryMenu;
    private bool menuActivated;
    public List<FoodListItem> playerInventory = new();
    public List<FoodListItem> PlayerInventory{
        get { return playerInventory; }
        set { playerInventory = value; }
    }
    public PlayerController player;
    public ItemSlot[] itemSlot;
    // Start is called before the first frame update
    void Start(){
        inventoryMenu.SetActive(false);
    }

    void Update(){
        if(Input.GetButtonDown("Inventory") && !menuActivated){
            // Time.timeScale = 0;
            Debug.Log("I don't");
            inventoryMenu.SetActive(true);
            menuActivated =  true;
        }
        else if(Input.GetButtonDown("Inventory") && menuActivated){
            // Time.timeScale = 1;
            inventoryMenu.SetActive(false);
            menuActivated = false;
        }
    }

    public void CloseInventory(){
        Time.timeScale = 1;
        gameObject.SetActive(false);
        menuActivated = false;
    }

    public void AddItemToInventory(FoodListItem food){
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(!itemSlot[i].isFull){
                itemSlot[i].AddItem(food);
                // playerInventory.Add(food);
                return;
            }
        }
    }

    public void EatFood(FoodListItem food){
        player.health += food.healthRegen;
        playerInventory.Remove(food);
        player.score -= food.scoreVal;
    }
}