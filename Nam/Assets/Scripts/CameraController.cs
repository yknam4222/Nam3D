using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera camera;

    public GameObject target;

    private bool View;

    private Vector3 offset;

    private void Awake()
    {
        camera = gameObject.GetComponent<Camera>();
    }

    void Start()
    {
        View = false;
        offset = new Vector3(0.0f, 10.0f, 10.0f);
    }

    void Update()
    {
        View = Input.GetKey(KeyCode.Tab) ? true : false;

        if (View)
        {
            offset = new Vector3(0.0f, 5.0f, -3.0f);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 100.0f, Time.deltaTime);
        }
        else
        {
            offset = new Vector3(0.0f, 10.0f, -10.0f);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 60.0f, Time.deltaTime);
        }

        camera.transform.position = Vector3.Lerp(
            camera.transform.position,
            target.transform.position + offset,
            0.016f);

        camera.transform.LookAt(target.transform.position);
    }
}
