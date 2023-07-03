using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererManager : MonoBehaviour
{
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        Material material = renderer.material;

        material.SetFloat("_Alpha", 0.5f);
    }

    void Update()
    {
        
    }
}
