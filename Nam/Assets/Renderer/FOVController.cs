using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class FOVController : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = mesh;

        List<Vector3> vertices = new List<Vector3>();

        float Angle = 0.0f;

        vertices.Add(Vector3.zero);

        for (int i = 0; i < 72; ++i)
        {
            vertices.Add(new Vector3(
                Mathf.Sin(Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(Angle * Mathf.Deg2Rad)) * 5.0f);

            Angle = i * 5.0f;
        }

        int max = (vertices.Count -1) * 3;

        int[] triangles = new int[max];
        
        for(int i = 0; i < vertices.Count - 2; ++i)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2 ;
        }

        triangles[3 * 71] = 0;
        triangles[3 * 71 + 1] = 71;
        triangles[3 * 71 + 2] = 1;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void Update()
    {
        
    }
}
