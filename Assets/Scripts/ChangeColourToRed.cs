using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourToRed : MonoBehaviour
{
    private Renderer objectRenderer;
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        Color32 colourRed = new Color32(200, 16, 46, 255);

        objectRenderer.material.color = colourRed;
    }
}
