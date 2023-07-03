using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    private Camera camera;
    private Vector3 direction;
    private Transform target;
    private float distance;

    public LayerMask mask;

    private bool Check;

    private const string path = "Legacy Shaders/Transparent/Specular";

    private void Awake()
    {
        camera = Camera.main;

        target = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        direction = (target.position - transform.position).normalized;

        distance = Vector3.Distance(target.position, transform.position);

        Check = false;
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + direction * distance, Color.green);
        Ray ray = new Ray(transform.position, direction);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, mask))
        {
            if (hit.transform != null)
            {
                Debug.Log(hit.transform.name);
                if (Check == false)
                {
                    Check = true;
                    Renderer renderer = hit.transform.GetComponent<Renderer>();

                    if (renderer != null)
                        StartCoroutine(SetColor(renderer));
                }
            }
            else if(hit.transform == null)
            {
                Check = false;

                Debug.Log("zzzzzzzzz");
            }
            /*
            else
            {
                Check = false;
                renderer = null;
            }
             */
        }


    }

    IEnumerator SetColor(Renderer renderer)
    {
        // Color�� ������ ������ Shader�� ����
        Material material = new Material(Shader.Find(path));

        //����� Shader�� color���� �޾ƿ�.
        Color color = renderer.material.color;

        //color.a ���� 0.5f ���� ū ��쿡�� �ݺ�.
        while (0.5f < color.a)
        {
            yield return null;

            // Alpha = 1 : Alpha -= Time.deltaTime
            color.a -= Time.deltaTime;
            renderer.material.color = color;
        }
    }
}
