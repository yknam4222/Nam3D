using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    private Camera camera;
    private Vector3 direction;
    private Transform target;
    private float distance;

    private void Awake()
    {
        camera = Camera.main;

        target = GameObject.Find("Player").transform;
    }

    private void Start()
    {
        direction = (target.position - transform.position).normalized;

        distance = Vector3.Distance(target.position, transform.position);
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + direction * distance, Color.green);
        Ray ray = new Ray(transform.position, direction);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, distance))
        {
            if(hit.transform != null)
            {
                if(hit.transform.name != "Cube")
                {

                }
                Debug.Log(hit.transform.name);
            }
        }
    }
}
