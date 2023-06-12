using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
   

    [Range(0.0f, 1.0f)]
    public float dis;

    private Vector3 temp;
    private Vector3 dest;

    private int check;
    private bool move;

    private void Start()
    {
        dis = 0.0f;

        temp = new Vector3(0.0f, 0.0f, 0.0f);
        dest = new Vector3(100.0f, 0.0f, 0.0f);
        check = 0;

        move = false;
    }

    void Update()
    {
        // transform.eulerAngles = new Vector3(0.0f, Angle, 0.0f);


        Quaternion rotation = transform.rotation;

        // ** rotation 각의 변경

        // transform.rotation = Quaternion.Lerp(transform.rotation, rotation.)

        transform.rotation = rotation;

        if (Input.GetMouseButtonDown(0))
        {
            function();
        }


        //transform.position = Vector3.Lerp(transform.position, new Vector3(10.0f, 0.0f, 0.0f), 0.016f);


       
    }

    void function()
    {
        if (move)
            return;

        move = true;
        StartCoroutine(SetMove());
    }

    IEnumerator SetMove()
    {
        float time = 0.0f;

        check = (check == 0) ? 1 : 0;

        while (time < 1.0f)
        {
            if (check == 0)
            {

                transform.position = Vector3.Lerp(
                    dest,
                    temp,
                    time);
            }
            else
            {
                transform.position = Vector3.Lerp(
                    temp,
                    dest,
                    time);
            }

            time += Time.deltaTime;

            yield return null;
        }

        move = false;
    }
}