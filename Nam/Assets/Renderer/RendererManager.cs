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
        // Color값 변경이 가능한 Shader로 변경
        Material material = new Material(Shader.Find(path));

        //변경된 Shader의 color값들 받아옴.
        Color color = renderer.material.color;

        //color.a 값이 0.5f 보다 큰 경우에만 반복.
        while (0.5f < color.a)
        {
            yield return null;

            // Alpha = 1 : Alpha -= Time.deltaTime
            color.a -= Time.deltaTime;
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
