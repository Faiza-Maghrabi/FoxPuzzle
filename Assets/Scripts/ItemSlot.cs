using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
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

    //reset colour so we can use it when player eats the food
    private Color resetColour1;
    private Color resetColour2;
    //=======ITEM DESCRIPTION=====//
    public TMP_Text ItemDescriptionName;
    public Image ItemDescriptionImage;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;
    
    private Inventory inventoryManager;

    void Start(){
        inventoryManager = GameObject.Find("Inventory").GetComponent<Inventory>(); //Inventory
        //Debug.Log(assignedItem.isFull);
        UnityEngine.ColorUtility.TryParseHtmlString("#eeeeee", out resetColour1); //asigns a hexcode to a colour field
        UnityEngine.ColorUtility.TryParseHtmlString("#ffffff", out resetColour2); //asigns a hexcode to a colour field
        if (Inventory.items[id].isFull){
            itemName.text = Inventory.items[id].foodName;
            itemName.gameObject.SetActive(true);
            if (Inventory.items[id].quantity >= 99){
                itemQuantity.text = Inventory.items[id].quantity.ToString();
                itemQuantity.gameObject.SetActive(true);

                int extraItems = Inventory.items[id].quantity - 99;

            }
            itemImage.color = Color.white;
        }
    }

    //Updates the empty slot to change it to the given item data
    public void AddItem(FoodListItem food){
        //Sets the text itemName to the food name and sets it to active to appear on the inventory menu
        Inventory.items[id].AddItem(food);
        itemName.text = food.foodName;
        itemName.gameObject.SetActive(true);
        itemQuantity.text = food.quantity.ToString();
        itemQuantity.gameObject.SetActive(true);

        itemImage.color = Color.white;

    }

    //Looks out for mouse clicks and if theres a left click it calls the OnLeftClick() function
    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Left){
            OnLeftClick();
        }
    }

    // This function is called when an item slot is clicked
    public void OnLeftClick(){
        //If the item is selected and used we empty the item slot
        if(thisItemSelected){
            bool usable = inventoryManager.EatFood(Inventory.items[id].foodName, Inventory.items[id].scoreVal, Inventory.items[id].healthRegen);
            if(usable){
                Inventory.items[id].ResetItem();
                EmptySlot();
            }
        }
        //otherwise we select another slot and set the selected panel around the item image to active and call the UpdateDescription function
        else{
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            UpdateDescription();
        }
    }

    //Updates the description to reflect the data of the selected item
    public void UpdateDescription() {
        ItemDescriptionName.text = Inventory.items[id].foodName;
        ItemDescriptionText.text = Inventory.items[id].foodDescription;
        ItemDescriptionImage.color = Color.white;
    }

    //deletes all item slot data and item data
    public void EmptySlot(){
        itemName.text = "";
        itemName.gameObject.SetActive(false);
        itemQuantity.text = "";
        itemQuantity.gameObject.SetActive(false);
        itemImage.color = resetColour1;

        // Clear the description UI
        ItemDescriptionName.text = "";
        ItemDescriptionText.text = "";
        ItemDescriptionImage.color = resetColour2;  // Reset item image.
    }
}
