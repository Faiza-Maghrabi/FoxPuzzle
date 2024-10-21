using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static GameObject persistentObjects;
    public int objectIndex;
    // Start is called before the first frame update
    private void Awake()
    {
        persistentObjects = GameObject.Find("PlayerCanvas");

        DontDestroyOnLoad(persistentObjects);

        // if(persistentObjects[objectIndex]==null){
        //     persistentObjects[objectIndex] = gameObject;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else if(persistentObjects[objectIndex] != gameObject){
        //     Destroy(gameObject);
        // }

    }
}
