using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class FoodListItem
{
    public string foodName;
    public int healthRegen;
    public int scoreVal;
}

[System.Serializable]
public class FoodList {
    public List<FoodListItem> foods;
}

public class FoodScript : MonoBehaviour
{
    public int id;
    private string jsonFilePath;
    private string foodName;
    private int healthRegen;
    private int scoreVal;

    public FoodScript(string name, int health, int score) {
        this.foodName = name;
        this.healthRegen = health;
        this.scoreVal = score;
    }

    void Start() {
        jsonFilePath = Application.data
    }

    public virtual string getName() {
        return this.foodName;
    }

    public virtual int getHealth() {
        return this.healthRegen;
    }

    public virtual int getScore() {
        return this.scoreVal;
    }

}
