using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterController : MonoBehaviour
{

    public List<GameObject> PointList;
    //void Start()
    //{
    //    for (int i = 0; i < 72; ++i)
    //    {
    //        GameObject obj = new GameObject("point");
    //        obj.AddComponent<myGizmo>();

    //        obj.transform.position = new Vector3(
    //            Mathf.Cos((i * 5.0f ) * Mathf.Deg2Rad),
    //            Mathf.Sin((i  * 5.0f) * Mathf.Deg2Rad),
    //            0.0f) * 5.0f;

    //        PointList.Add(obj);
    //    }
    //}

    [Range(0.0f, 90.0f)]
    public float Angle;

    private void Start()
    {
        gameObject.AddComponent<myGizmo>();
        Angle = 5.0f;
    }

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 Movement = new Vector3(hor, 0.0f, ver) * 5.0f * Time.deltaTime;

        //transform.Translate(Movement);

        transform.position = new Vector3(
                Movement.x, Mathf.Sin(Angle * Mathf.Deg2Rad) * 5.0f, Movement.z);
    }
}

