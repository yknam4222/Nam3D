using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererManager : MonoBehaviour
{
    private Renderer renderer;

    private const string path = "Legacy Shaders/Transparent/Specular";

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    IEnumerator SetColor(Renderer renderer)
    {
        Material material = new Material(Shader.Find(path));

        while (true)
        {
            yield return null;

            renderer.material = material;

            Color color = renderer.material.color;

            color.a = 0.5f;
            renderer.material.color = color;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (renderer != null)
            {
                StartCoroutine(SetColor(renderer));
            }
        }
    }
}
