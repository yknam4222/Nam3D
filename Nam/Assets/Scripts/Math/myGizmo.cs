using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myGizmo : MonoBehaviour
{
    public Color color;

    private void Awake()
    {
        color = Color.green;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
