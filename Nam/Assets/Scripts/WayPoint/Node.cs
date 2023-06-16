using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class Node //: MonoBehaviour
{
    public Vector3 Position;
    public Node Next;
    public float Cost;

    public Node(Node _node, float _cost)
    {
        Next = _node;
        Cost = _cost;
    }

}
