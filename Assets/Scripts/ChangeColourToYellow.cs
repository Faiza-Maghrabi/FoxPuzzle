using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourToYellow : MonoBehaviour
{
    private Renderer objectRenderer;
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        Color32 colourYellow = new Color32(255, 215, 0, 255);

        objectRenderer.material.color = colourYellow;
    }
}
