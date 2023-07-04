using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraController2 : MonoBehaviour
{
    private Camera camera;
    private Vector3 direction;
    private Transform target;
    private float distance;

    public LayerMask mask;

    private bool Check;

    private const string path = "Legacy Shaders/Transparent/Specular";

    public List<Renderer> objectRenderers = new List<Renderer>();
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

        // ��� �浹�� ����
        List<RaycastHit> hits = Physics.RaycastAll(transform.position, direction, distance, mask).ToList();

        List<Renderer> renderers = hits.Select(hit => hit.transform.GetComponent<Renderer>())
            .Where(renderer => renderer != null).ToList();

        // renderers.Select(renderer => objectRenderers.Contains(renderer));

        //objectRenderers.Clear();

        /*
        //hits �迭�� ��� ���Ҹ� Ȯ��
        foreach (RaycastHit hit in hits)
        {
            //ray�� �浹�� ������ object�� renderer�� �޾ƿ�
            Renderer renderer = hit.transform.GetComponent<Renderer>();

            //renderer == null �̶�� ���� ���Ҹ� Ȯ��
            if (renderer == null)
                continue;

            //���� ����Ʈ �߿� ������ ���Ұ� ���ԵǾ� �ִ��� Ȯ��
            if (!objectRenderers.Contains(renderer))
                //���Ե��� �ʾҴٸ�
                objectRenderers.Add(renderer);

            foreach (Renderer element in objectRenderers)
            {
                if (HitList.Contains(element))
                {
                    StartCoroutine(SetFadeOut(element));
                }
            }

            if (renderer != null)
            {
                if (!objectRenderers.Contains(renderer))
                    StartCoroutine(SetFadeOut(renderer));
            }
        }
         */

        if (objectRenderers != null)
        {

        foreach(Renderer renderer in objectRenderers)
        {
                //����� Shader�� color���� �޾ƿ�.
                Color color = renderer.material.color;

                color.a = 1.0f;
                objectRenderers.Remove(renderer);
        }

        }
        //foreach (RaycastHit hit in hits)
        //{
        //    Renderer renderer = hit.transform.GetComponent<Renderer>();

        //    if (!objectRenderers.Contains(renderer))
        //    {
        //        objectRenderers.Remove(renderer);
        //    }
        //}

        // 1ȸ�� ����ȴٸ� ������ �浹�� �ִٴ� ��.
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.transform.GetComponent<Renderer>();

            // �浹�� �ִٸ� Renderer�� Ȯ��.
            if (renderer != null)
            {
                // List�� �̹� ���Ե� Renderer���� Ȯ��
                if (!objectRenderers.Contains(renderer))
                {
                    objectRenderers.Add(renderer);
                    StartCoroutine(SetFadeOut(renderer));

                }
            }
        }
        //Ȯ�ε� ��� Renderer�� ����ȭ �۾��� ����
        // foreach (Renderer element in objectRenderers)
        //StartCoroutine(SetFadeOut(renderer));
    }

    IEnumerator SetFadeOut(Renderer renderer)
    {
        // Color�� ������ ������ Shader�� ����
        renderer.material = new Material(Shader.Find(path));

        //����� Shader�� color���� �޾ƿ�.
        Color color = renderer.material.color;

        //color.a ���� 0.5f ���� ū ��쿡�� �ݺ�.
        while (0.5f < color.a)
        {
            yield return null;

            // Alpha = 1 : Alpha -= Time.deltaTime
            color.a -= Mathf.Clamp(Time.deltaTime * 10, 0.1f, 0.5f);

            renderer.material.color = color;
        }
    }

    IEnumerator SetFadeIn(Renderer renderer)
    {
        //����� Shader�� color���� �޾ƿ�.
        Color color = renderer.material.color;

        //color.a ���� 0.5f ���� ū ��쿡�� �ݺ�.
            yield return null;

            // Alpha = 1 : Alpha -= Time.deltaTime
            color.a = 1.0f;


    }


    /*
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
     */
}
