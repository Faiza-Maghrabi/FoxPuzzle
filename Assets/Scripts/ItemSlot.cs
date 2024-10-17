using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ItemSlot : MonoBehaviour
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

    void Start(){
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
        // itemName.enabled = true;
        // Debug.Log(itemName.enabled);

        itemImage.color = Color.white;

    }
}
