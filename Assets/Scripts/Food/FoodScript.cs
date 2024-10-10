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

    void Start() {
        Debug.Log(id);
        jsonFilePath = Application.dataPath + "/Scripts/Food/FoodList.json";
        if (File.Exists(jsonFilePath)){
            string jsonContent = File.ReadAllText(jsonFilePath);
            FoodList foodList = JsonUtility.FromJson<FoodList>(jsonContent);

            foodName = foodList.foods[id].foodName;
            healthRegen = foodList.foods[id].healthRegen;
            scoreVal = foodList.foods[id].scoreVal;
            Debug.Log(""+foodName+""+healthRegen+""+scoreVal);
        }
        else {
            Debug.LogError("No file at " + jsonFilePath);
        }
    }

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
