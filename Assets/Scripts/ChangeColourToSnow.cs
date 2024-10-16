using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourToSnow : MonoBehaviour
{
    private Renderer objectRenderer;
    void Start() {
        objectRenderer = GetComponent<Renderer>();

        Color32 colourSnow = new Color32(240, 248, 255, 255);

        objectRenderer.material.color = colourSnow;
    }
}
