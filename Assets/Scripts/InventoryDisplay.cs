using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.EventSystems;

public class InventoryDisplay : MonoBehaviour, IPointerClickHandler
{
    
    public static InventoryDisplay instance;    
    public Inventory inventory;
    public Transform itemContent;
    public GameObject inventoryItem;
    private int itemIndex = 0;
    public bool thisItemSelected;

    private void Awake(){
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start(){
        gameObject.SetActive(false);
        UpdateInventory();
    }

    // Update is called once per frame
    void Update(){
        UpdateInventory();
    }

    public void OpenInventory(){
        Time.timeScale = 0;
        gameObject.SetActive(true);  // Toggle on
    }


    void UpdateInventory(){
        // clear the old items
        foreach (Transform child in itemContent)
        {
            Destroy(child.gameObject);  // Safely destroy old slots
        }
        itemIndex = -1;
        // Populate the inventory with current items
        foreach (FoodListItem item in inventory.playerInventory){
            GameObject slot = Instantiate(inventoryItem, itemContent);
            // Set the itemName to the foodName
            TMP_Text itemName = slot.transform.Find("ItemName").GetComponent<TMP_Text>();
            itemName.text = item.foodName;

            // Set the background color to white
            UnityEngine.UI.Image background = slot.GetComponent<UnityEngine.UI.Image>();
            background.color = Color.white;

            // // Add click listener to the slot
            // int index = itemIndex;  // Capture the current index
            // // slot.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
            // {
            //     OnItemClicked(index);
            // };
            itemIndex += 1;
        }
    }

    public void CloseInventory(){
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left){
            OnLeftClick();
        }
 
    }

    // This function is called when an item slot is clicked
    public void OnLeftClick()
    {
        Debug.Log("Item clicked");
        thisItemSelected = true;
        inventory.EatFood(inventory.playerInventory[itemIndex]);
        UpdateInventory();
    }
}
