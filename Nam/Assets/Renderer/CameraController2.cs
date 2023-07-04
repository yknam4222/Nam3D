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

        // 모든 충돌을 감지
        List<RaycastHit> hits = Physics.RaycastAll(transform.position, direction, distance, mask).ToList();

        List<Renderer> renderers = hits.Select(hit => hit.transform.GetComponent<Renderer>())
            .Where(renderer => renderer != null).ToList();

        // renderers.Select(renderer => objectRenderers.Contains(renderer));

        //objectRenderers.Clear();

        /*
        //hits 배열의 모든 원소를 확인
        foreach (RaycastHit hit in hits)
        {
            //ray의 충돌이 감지된 object의 renderer을 받아옴
            Renderer renderer = hit.transform.GetComponent<Renderer>();

            //renderer == null 이라면 다음 원소를 확인
            if (renderer == null)
                continue;

            //이전 리스트 중에 동일한 원소가 포함되어 있는지 확인
            if (!objectRenderers.Contains(renderer))
                //포함되지 않았다면
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
                //변경된 Shader의 color값들 받아옴.
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

        // 1회라도 실행된다면 감지된 충돌이 있다는 것.
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.transform.GetComponent<Renderer>();

            // 충돌이 있다면 Renderer을 확인.
            if (renderer != null)
            {
                // List에 이미 포함된 Renderer인지 확인
                if (!objectRenderers.Contains(renderer))
                {
                    objectRenderers.Add(renderer);
                    StartCoroutine(SetFadeOut(renderer));

                }
            }
        }
        //확인된 모든 Renderer의 투명화 작업을 진행
        // foreach (Renderer element in objectRenderers)
        //StartCoroutine(SetFadeOut(renderer));
    }

    IEnumerator SetFadeOut(Renderer renderer)
    {
        // Color값 변경이 가능한 Shader로 변경
        renderer.material = new Material(Shader.Find(path));

        //변경된 Shader의 color값들 받아옴.
        Color color = renderer.material.color;

        //color.a 값이 0.5f 보다 큰 경우에만 반복.
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
        //변경된 Shader의 color값들 받아옴.
        Color color = renderer.material.color;

        //color.a 값이 0.5f 보다 큰 경우에만 반복.
            yield return null;

            // Alpha = 1 : Alpha -= Time.deltaTime
            color.a = 1.0f;


    }


    /*
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
     */
}
