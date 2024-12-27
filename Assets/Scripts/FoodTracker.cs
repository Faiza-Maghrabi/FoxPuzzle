using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

//will load up a json of food in each secene and assign to a static list
//this list is referred to by players on contact and keeps track of food that has been collected
//on food load, the object checks here if it should exist - if it should not then it is destroyed.

[System.Serializable]
public class SceneFoodList {
    public Dictionary<string, Dictionary<string, bool>> scene;
}

public class FoodTracker : MonoBehaviour
{
    private static SceneFoodList sceneFoodList;
    private static string jsonFilePath;

    public static void Init() {
        //load json and save in static variable
        jsonFilePath = Application.streamingAssetsPath + "/SceneFood.json";
        if (File.Exists(jsonFilePath)){
            string jsonContent = File.ReadAllText(jsonFilePath);
            Debug.Log(jsonContent);
            sceneFoodList = JsonConvert.DeserializeObject<SceneFoodList>(jsonContent);
        }
        else {
            Debug.LogError("No file at " + jsonFilePath);
        }
    }

    public bool isCollected(string sceneName, string foodName) {
        return sceneFoodList.scene[sceneName][foodName];
    }

    public void markCollected(string sceneName, string foodName) {
        sceneFoodList.scene[sceneName][foodName] = false;
    }
}
