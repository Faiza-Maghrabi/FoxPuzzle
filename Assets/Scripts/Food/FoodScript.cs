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
    public string foodDescription;
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
    public FoodList foodList = new();
    public FoodListItem food = new();

    public FoodListItem Food { 
        get { return foodList.foods[id];}
        set { food = value; }
    }
    public string foodName;

    public string FoodName {
        get {return foodName;}
        set { foodName = value; }
    }
    public int healthRegen;
    public int HealthRegen {
        get {return healthRegen;}
        set { healthRegen = value; }
    }
    public int scoreVal;
    public int ScoreVal {
        get {return scoreVal;}
        set {scoreVal = value; }
    }
    public string foodDescription;
    public string Description {
        get {return foodDescription;}
        set {foodDescription = value; }
    }

    // import JSON file with FoodList, read contents, parse JSON and index with id
    void Start() {
        jsonFilePath = Application.dataPath + "/Scripts/Food/FoodList.json";
        if (File.Exists(jsonFilePath)){
            string jsonContent = File.ReadAllText(jsonFilePath);
            foodList = JsonUtility.FromJson<FoodList>(jsonContent);
            food = foodList.foods[id];

            foodName = food.foodName;
            healthRegen = food.healthRegen;
            scoreVal = food.scoreVal;
            foodDescription = food.foodDescription;
        }
        else {
            Debug.LogError("No file at " + jsonFilePath);
        }
    }
}
