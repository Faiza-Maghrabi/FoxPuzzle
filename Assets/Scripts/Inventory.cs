using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    //Player Inventory
    public List<FoodListItem> playerInventory = new();
    public List<FoodListItem> PlayerInventory{
        get { return playerInventory; }
        set { playerInventory = value; }
    }

    // Start is called before the first frame update

    public PlayerController player;

    public void AddItemToInventory(FoodListItem food){
        playerInventory.Add(food);
    }

    public void EatFood(FoodListItem food){
        player.health += food.healthRegen;
        playerInventory.Remove(food);
        player.score -= food.scoreVal;
    }
}
