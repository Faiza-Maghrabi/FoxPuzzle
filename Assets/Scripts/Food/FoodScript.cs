using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
//Class to hold data each JSON object obtains
public class FoodListItem
{
    public string foodName;
    public int healthRegen;
    public int scoreVal;
}

[System.Serializable]
//Class to create list of FoodListItems
public class FoodList {
    public List<FoodListItem> foods;
}

public class FoodScript : MonoBehaviour
{
    //food attributes
    public int id;  // value set in editor
    private string jsonFilePath;
    private string foodName;
    private int healthRegen;
    private int scoreVal;

    // import JSON file with FoodList, read contents, parse JSON and index with id
    void Start() {
        jsonFilePath = Application.dataPath + "/Scripts/Food/FoodList.json";
        if (File.Exists(jsonFilePath)){
            string jsonContent = File.ReadAllText(jsonFilePath);
            FoodList foodList = JsonUtility.FromJson<FoodList>(jsonContent);

            foodName = foodList.foods[id].foodName;
            healthRegen = foodList.foods[id].healthRegen;
            scoreVal = foodList.foods[id].scoreVal;
        }
        else {
            Debug.LogError("No file at " + jsonFilePath);
        }
    }

    //getters for attributes
    public virtual string getName() {
        return foodName;
    }

    public virtual int getHealth() {
        return healthRegen;
    }

    public virtual int getScore() {
        return scoreVal;
    }

}
