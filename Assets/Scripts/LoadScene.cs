using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
//Class to hold data each JSON object obtains
public class SceneListItem
{
    public string objName;
    public string sceneToLoad;
    public float[] position;
}

[System.Serializable]
//Class to create list of FoodListItems
public class SceneList {
    public List<SceneListItem> scenes;

}

//position refrenced by the player
public static class PlayerScenePos {
    public static float[] position;
}


public class LoadScene : MonoBehaviour
{
    public int id;  // value set in editor
    private string jsonFilePath;
    private string objName;
    private string sceneToLoad;
    
    private SceneList sceneList = new();
    private SceneListItem scene = new();

    // import JSON file with SceneList, read contents, parse JSON and index with id
    void Start() {
        jsonFilePath = Application.streamingAssetsPath + "/SceneList.json";
        if (File.Exists(jsonFilePath)){
            string jsonContent = File.ReadAllText(jsonFilePath);
            sceneList = JsonUtility.FromJson<SceneList>(jsonContent);
            scene = sceneList.scenes[id];

            objName = scene.objName;
            sceneToLoad = scene.sceneToLoad;
            PlayerScenePos.position = scene.position;
        }
        else {
            Debug.LogError("No file at " + jsonFilePath);
        }
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player") && gameObject.name == objName){
            PlayerScenePos.position = scene.position;
            //Debug.Log($"Loading scene: {sceneToLoad} from object: {objName}");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
