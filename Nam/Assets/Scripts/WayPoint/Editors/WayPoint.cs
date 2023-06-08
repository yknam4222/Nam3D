using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    private GameObject parent;

    void Start()
    {
        parent = GameObject.Find("ParentObject");
    }

    void Update()
    {
        for(int i  = 0; i < parent.transform.childCount; ++i)
        {
            Node node = parent.transform.GetChild(i).GetComponent<Node>();

            Debug.DrawLine(node.transform.position, node.Next.transform.position);
        }
    }
}
