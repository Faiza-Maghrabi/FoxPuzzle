using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourToBlue : MonoBehaviour
{
    private Renderer objectRenderer;
    void Start() {

        objectRenderer = GetComponent<Renderer>();

        Color32 colourBlue = new Color32(25, 25, 112, 255);

        objectRenderer.material.color = colourBlue;
    }
}
