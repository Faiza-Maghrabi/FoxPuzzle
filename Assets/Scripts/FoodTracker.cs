using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.VisualScripting;
using Mono.Collections.Generic;

//will load up a json of food in each secene and assign to a static list
//this list is referred to by players on contact and keeps track of food that has been collected
//on food load, the object checks here if it should exist - if it should not then it is destroyed.

// [System.Serializable]
// //Class to hold data each JSON object obtains
// public class SceneFood
// {
//     public string name;
//     public bool active;
// }

// [System.Serializable]
// //Class to hold data each JSON object obtains
// public class SceneItem
// {
//     public string name;
//     public List<SceneFood> food;
// }

// [System.Serializable]
// //Class to create a disctionary of scenes with their foodlists
// public class SceneFoodList {
//     public List<SceneItem> scene;
// }

[System.Serializable]
public class testDict {
    public Dictionary<string, Dictionary<string, bool>> scene;
}

public class FoodTracker : MonoBehaviour
{
    //private static SceneFoodList sceneFoodList;
    private static testDict test;
    private static string jsonFilePath;

    public static void Init() {
        //load json and save in static variable
        jsonFilePath = Application.streamingAssetsPath + "/SceneFood.json";
        if (File.Exists(jsonFilePath)){
            string jsonContent = File.ReadAllText(jsonFilePath);
            Debug.Log(jsonContent);
            test = JsonUtility.FromJson<testDict>(jsonContent);
            Debug.Log(test);
            Debug.Log(test.scene);
            Debug.Log(test.scene["OpenHouseScene"]);
            // sceneFoodList = JsonUtility.FromJson<SceneFoodList>(jsonContent);
            // Debug.Log(sceneFoodList);
            // Debug.Log(sceneFoodList.scene[0].name);
            // Debug.Log(sceneFoodList.scene[0].food[0].name);
        }
        else {
            Debug.LogError("No file at " + jsonFilePath);
        }
    }

    // public bool isCollected(string sceneIndex, string foodName) {
    //     if (sceneFoodList.[scene].name)
    // }
}
