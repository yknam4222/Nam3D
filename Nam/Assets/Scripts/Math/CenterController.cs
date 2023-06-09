using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterController : MonoBehaviour
{

    //public List<GameObject> PointList;
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

    private GameObject Under;
    private GameObject Up;
    

    [Range(0.0f, 90.0f)]
    public float Angle;

    private void Start()
    {
        gameObject.AddComponent<myGizmo>();
        Angle = 5.0f;

        Under = GameObject.Find("under");
        Up = GameObject.Find("up");

    }

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        float x = Up.transform.position.x - Under.transform.position.x;
        float y = Up.transform.position.y - Under.transform.position.y;
        float t = y / x;

        Vector3 Movement = new Vector3(hor, ver, 0.0f) * 5.0f * Time.deltaTime;
        if (transform.position.x > Under.transform.position.x && transform.position.x < Up.transform.position.x)
           transform.position = new Vector3 (transform.position.x, (transform.position.x - Under.transform.position.x) * t, transform.position.z);

        Debug.DrawLine(Under.transform.position, Up.transform.position, Color.green);
        transform.Translate(Movement);
    }
}

