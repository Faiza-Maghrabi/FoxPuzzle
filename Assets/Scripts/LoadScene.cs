using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UIElements;

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

    // //fade in and out code
    public CanvasGroup sceneFade;

    // import JSON file with SceneList, read contents, parse JSON and index with id
    void Start() {
        jsonFilePath = Application.streamingAssetsPath + "/SceneList.json";
        if (File.Exists(jsonFilePath)){
            string jsonContent = File.ReadAllText(jsonFilePath);
            sceneList = JsonUtility.FromJson<SceneList>(jsonContent);
            scene = sceneList.scenes[id];

            objName = scene.objName;
            sceneToLoad = scene.sceneToLoad;
        }
        else {
            Debug.LogError("No file at " + jsonFilePath);
        }
    }

    private IEnumerator FadeLoad()
    {
        var op = SceneManager.LoadSceneAsync(sceneToLoad);
        op.allowSceneActivation = false;
        float duration = 1.5f;
        float startAlpha = sceneFade.alpha;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            sceneFade.alpha = Mathf.MoveTowards(startAlpha, 1.0f, elapsedTime / duration);
            yield return null; // Wait until the next frame
        }
        sceneFade.alpha = 1.0f;
        op.allowSceneActivation = true;
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player") && gameObject.name == objName){
            PlayerScenePos.position = scene.position;
            //Debug.Log($"Loading scene: {sceneToLoad} from object: {objName}");
            if (sceneFade == null){
                SceneManager.LoadScene(sceneToLoad);
            }
            else {
                StartCoroutine(FadeLoad());
            }
        }
    }

    public void CutsceneLeave() {
        PlayerScenePos.position = scene.position;
        StartCoroutine(FadeLoad());
    }
}
