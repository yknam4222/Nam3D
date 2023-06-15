using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Node : MonoBehaviour
{

    [HideInInspector] public Node Next;
    public float Cost;

    private void Start()
    {
        SphereCollider coll = GetComponent<SphereCollider>();
        coll.radius = 0.2f;
        coll.isTrigger = true;
    }

}
