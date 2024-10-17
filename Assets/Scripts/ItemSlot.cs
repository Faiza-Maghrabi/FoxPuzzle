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


    //========ITEM SLOT=======//
    [SerializeField]
    private TMP_Text itemName;

    [SerializeField]
    private Image itemImage;

    public GameObject selectedShader;
    public bool thisItemSelected;
    
    private Inventory inventoryManager;

    void Start(){
        inventoryManager = GameObject.Find("Inventory").GetComponent<Inventory>();
        isFull = false;
    }

    public void AddItem(FoodListItem food){

        Debug.Log("Hello world");
        this.foodName = food.foodName;
        this.scoreVal = food.scoreVal;
        this.healthRegen = food.healthRegen;
        isFull = true;

        itemName.text = foodName;
        Debug.Log(itemName.text);
        itemName.gameObject.SetActive(true);
        Debug.Log(itemName.enabled);

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
                this.foodName = "";
                this.scoreVal = 0;
                this.healthRegen = 0;
                EmptySlot();
            }
        }
        else{
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
        }
    }

    public void EmptySlot(){
        itemName.text = "";
        itemName.gameObject.SetActive(false);
    }
}
