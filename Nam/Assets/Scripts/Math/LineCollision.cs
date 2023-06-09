using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;
}

public class LineCollision : MonoBehaviour
{
    public List<Line> LineList = new List<Line>();

    [SerializeField] private float Width;
    [SerializeField] private float Height;
    float w;
    float h;
    void Start()
    {
        //float fX = Random.Range(0.0f, 0.0f);
        //float fY = Random.Range(0.0f, 0.0f);

        Vector3 OldPoint= new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < 10; ++i)
        {
            Line line = new Line();

            line.StartPoint = OldPoint;

            float fY = 0.0f;
            while(true)
            {
                fY = Random.Range(-5.0f, 5.0f);
                if (fY != 0.0f) break;
            }

            OldPoint = new Vector3(
                OldPoint.x + Random.Range(1.0f, 1.0f), 
                OldPoint.y + fY, 
                0.0f);

            line.EndPoint = OldPoint;

            LineList.Add(line);
        }
        Height = 1.0f;
        Width = 1.0f;
    }

    void Update()
    {
        foreach(Line element in LineList)
        {
            Debug.DrawLine(element.StartPoint, element.EndPoint, Color.green);

            Width = element.EndPoint.x - element.StartPoint.x;
            Height = element.EndPoint.y - element.StartPoint.y;

            

            if (Width != 0)
                w = Width;
            if (Height != 0)
                h = Height;

            if(element.StartPoint.x < transform.position.x && transform.position.x < element.EndPoint.x)
                transform.position = new Vector3(transform.position.x, Height / Width * (transform.position.x - element.StartPoint.x)  + element.StartPoint.y, 0.0f) ;
            

            Debug.Log(Height);
            Debug.Log("start.x = " +  element.StartPoint.x);
            Debug.Log("start.y = " + element.StartPoint.y);
            Debug.Log("end.x = " + element.EndPoint.x);
            Debug.Log("end.y = " + element.EndPoint.y);
        }

        if (Input.GetKeyDown(KeyCode.Space))
            transform.position += new Vector3(0.01f, 0.0f, 0.0f);

        float Hor = Input.GetAxis("Horizontal");
        float Ver = Input.GetAxis("Vertical");

    }
}
