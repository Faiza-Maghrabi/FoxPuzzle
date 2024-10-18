using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //========ITEM DATA=======//
    public string foodName;
    public int scoreVal;
    public int healthRegen;
    public bool isFull;
    public string foodDescription;

    //========ITEM SLOT=======//
    [SerializeField]
    private TMP_Text itemName;

    [SerializeField]
    private Image itemImage;

    Color resetColor;
    //=======ITEM DESCRIPTION=====//
    public TMP_Text ItemDescriptionName;
    public Image ItemDescriptionImage;
    public TMP_Text ItemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;
    
    private Inventory inventoryManager;

    void Start(){
        inventoryManager = GameObject.Find("Inventory").GetComponent<Inventory>();
        isFull = false;
        UnityEngine.ColorUtility.TryParseHtmlString("#eeeeee", out resetColor);
    }

    public void AddItem(FoodListItem food){

        this.foodName = food.foodName;
        this.scoreVal = food.scoreVal;
        this.healthRegen = food.healthRegen;
        this.foodDescription = food.foodDescription;
        isFull = true;

        itemName.text = foodName;
        itemName.gameObject.SetActive(true);

        itemImage.color = Color.white;

    }

    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button == PointerEventData.InputButton.Left){
            OnLeftClick();
        }
    }

    // This function is called when an item slot is clicked
    public void OnLeftClick(){
        if(thisItemSelected){
            bool usable = inventoryManager.EatFood(foodName, scoreVal, healthRegen);
            if(usable){
                EmptySlot();
            }
        }
        else{
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            UpdateDescription();
        }
    }

    public void UpdateDescription() {
        ItemDescriptionName.text = foodName;
        ItemDescriptionText.text = foodDescription;
        ItemDescriptionImage.color = Color.white;
    }


    public void EmptySlot(){
        foodName = "";
        scoreVal = 0;
        healthRegen = 0;
        foodDescription = "";

        itemName.text = "";
        itemName.gameObject.SetActive(false);

        // Clear the description UI
        ItemDescriptionName.text = "";
        ItemDescriptionText.text = "";
        ItemDescriptionImage.color = resetColor;  // Make image transparent.
    }
}
