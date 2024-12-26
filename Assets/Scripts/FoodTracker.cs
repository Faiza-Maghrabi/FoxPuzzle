using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.VisualScripting;

//will load up a json of food in each secene and assign to a static list
//this list is referred to by players on contact and keeps track of food that has been collected
//on food load, the object checks here if it should exist - if it should not then it is destroyed.

[System.Serializable]
//Class to hold data each JSON object obtains
public class SceneFood
{
    public string name;
    public bool active;
}

[System.Serializable]
//Class to hold data each JSON object obtains
public class SceneItem
{
    public string name;
    public List<SceneFood> food;
}

[System.Serializable]
//Class to create a disctionary of scenes with their foodlists
public class SceneFoodList {
    public List<SceneItem> scene;
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
            sceneFoodList = JsonUtility.FromJson<SceneFoodList>(jsonContent);
            //List<string> idk;
            Debug.Log(sceneFoodList);

            // listJson.TryGetValue("OpenHouseScene", out idk);
            Debug.Log(sceneFoodList.scene[0].name);
            Debug.Log(sceneFoodList.scene[0].food[0].name);
    
            // foreach (KeyValuePair<string, List<string>> kvp in listJson.scene)
            // {
            //     //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            //     Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
            // }
            //Debug.Log(listJson.scene.["OpenHouseScene"][0]);
        }
        else {
            Debug.LogError("No file at " + jsonFilePath);
        }
    }

    // public bool isCollected(string scene, string name) {
    //     if (sceneFoodList.[scene].name)
    // }
}
