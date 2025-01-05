using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, ISelectHandler
{
    //========ITEM DATA=======//
    // public string foodName;
    // public int scoreVal;
    // public int healthRegen;
    // public bool isFull;
    // public string foodDescription;

    public int id;

    //========ITEM SLOT=======//
    [SerializeField]
    private TMP_Text itemName;

    [SerializeField]
    public TMP_Text itemQuantity;

    [SerializeField]
    private Image itemImage;
    //=======ITEM DESCRIPTION=====//
    public TMP_Text ItemDescriptionName;
    public Image ItemDescriptionImage;
    public TMP_Text ItemDescriptionText;
    public TMP_Text ItemAttributesText;
    public TMP_Text EatInstructionText;
    public Image usedImage;

    public bool thisItemSelectedOnce;
    public bool thisItemSelectedTwice;
    
    private Inventory inventoryManager;

    void Start(){
        inventoryManager = GameObject.Find("Inventory").GetComponent<Inventory>(); //Inventory
        if (Inventory.items[id].isFull){
            itemName.text = Inventory.items[id].foodName;
            itemImage.sprite = FoodScript.GetSprite(Inventory.items[id].foodIcon);
            itemName.gameObject.SetActive(true);
            itemQuantity.text = Inventory.items[id].quantity.ToString();
            itemQuantity.gameObject.SetActive(true);
            EatInstructionText.gameObject.SetActive(false);
        }
        EatInstructionText.gameObject.SetActive(false);
    }

    //Updates the empty slot to change it to the given item data
    public void AddItem(FoodListItem food, Sprite foodSprite){
        //Sets the text itemName to the food name and sets it to active to appear on the inventory menu
        Inventory.items[id].AddItem(food);
        itemName.text = food.foodName;
        itemName.gameObject.SetActive(true);
        itemQuantity.text = Inventory.items[id].quantity.ToString();
        itemQuantity.gameObject.SetActive(true);

        itemImage.sprite = foodSprite;
        usedImage.gameObject.SetActive(false);

    }

    // This function is called when an item slot is clicked
    public void OnLeftClick(){
        //If the item is selected and used we empty the item slot
        if (thisItemSelectedTwice){
            Debug.Log(Inventory.items[id].foodName);
            Debug.Log(Inventory.items[id].quantity);
            bool usable = false;
            if(Inventory.items[id].foodName != "" && Inventory.items[id].quantity > 0){
                usable = inventoryManager.EatFood(Inventory.items[id].foodName, Inventory.items[id].scoreVal, Inventory.items[id].healthRegen);
            }
            else if(Inventory.items[id].foodName != "" && Inventory.items[id].quantity == 0){
                inventoryManager.OpenOutOfStockNotif();
            }
            Debug.Log(usable);
            if(usable){
                int quantity = Inventory.items[id].UseItem();
                itemQuantity.text = Inventory.items[id].quantity.ToString();
                if(quantity <= 0){
                    usedImage.gameObject.SetActive(true);
                    itemQuantity.text = "0";
                }
            }
        }
        else{
            thisItemSelectedTwice = true;
        }
    }

    //Updates the description to reflect the data of the selected item
    public void UpdateDescription() {
        ItemDescriptionName.text = Inventory.items[id].foodName;
        Debug.Log(Inventory.items[id].foodName);
        ItemDescriptionText.text = Inventory.items[id].foodDescription;
        ItemAttributesText.text = Inventory.items[id].foodAttributes;
        ItemDescriptionImage.sprite = FoodScript.GetSprite(Inventory.items[id].foodIcon);
        if(Inventory.items[id].foodName != ""){
            EatInstructionText.gameObject.SetActive(true);
        }
    }

    public void OnSelect(BaseEventData eventData) // Called by the EventSystem when this slot is selected
    {
        inventoryManager.DeselectAllSlots();
        EatInstructionText.gameObject.SetActive(false);
        thisItemSelectedOnce = true;
        UpdateDescription();
    }
}
