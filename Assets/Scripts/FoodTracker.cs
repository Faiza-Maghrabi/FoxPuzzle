using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Threading.Tasks;

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
    public static bool isInit = false;

    async public static void Init() {
        //load json and save in static variable
        jsonFilePath = Application.streamingAssetsPath + "/SceneFood.json";

        UnityWebRequest request = UnityWebRequest.Get(jsonFilePath);
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }
        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonContent = request.downloadHandler.text;
            sceneFoodList = JsonConvert.DeserializeObject<SceneFoodList>(jsonContent);
            isInit = true;
        }
        else {
            Debug.LogError("Cannot load file at " + jsonFilePath);
        }
    }

    public static bool isCollected(string sceneName, string foodName) {
        return sceneFoodList.scene[sceneName][foodName];
    }

    public static void markCollected(string sceneName, string foodName) {
        // Debug.Log("Collected " + foodName + " from " + sceneName);
        sceneFoodList.scene[sceneName][foodName] = true;
    }

    public static int numFoodInScene(string sceneName) {
        return sceneFoodList.scene[sceneName].Count;
    }

    public static int numFoodCollectedInScene(string sceneName) {
        return sceneFoodList.scene[sceneName].Values.Count(value => value);
    }
}
