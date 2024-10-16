using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColourToBrown : MonoBehaviour
{
    private Renderer objectRenderer;
    void Start() {
        objectRenderer = GetComponent<Renderer>();

        Color32 colourBrown = new Color32(139, 69, 19, 255);

        objectRenderer.material.color = colourBrown;
    }
}
