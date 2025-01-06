using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    //rotates the skybox slowly
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.fixedTime * speed);
    }
}
