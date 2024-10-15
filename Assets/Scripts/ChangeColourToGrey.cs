using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourToGrey : MonoBehaviour
{
    private Renderer objectRenderer;
    void Start() {
        objectRenderer = GetComponent<Renderer>();

        Color32 colourGrey = new Color32(50, 50, 50, 255);

        objectRenderer.material.color = colourGrey;
    }
}
