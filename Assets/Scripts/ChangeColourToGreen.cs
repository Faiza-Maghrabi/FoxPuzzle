using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourToGreen : MonoBehaviour
{
    private Renderer objectRenderer;
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        Color32 colourGreen = new Color32(34, 139, 34, 255);

        objectRenderer.material.color = colourGreen;
    }
}
