using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
//Class to create a disctionary of scenes with their foodlists
public class SceneFoodList {
    public Dictionary<string, List<SceneFood>> scene;
}


public class FoodTracker : MonoBehaviour
{
    void Start()
    {   //load json and save in static variable
        gameObject.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
