using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Node : MonoBehaviour
{

    [HideInInspector] public Node Next;

}
