using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    const int M = 0; //Matrix
    const int T = 1; // Transform
    const int R = 2; // Rotation
    const int S = 3; // Scale

    public Node Target = null;
    public List<Vector3> vertices = new List<Vector3>();

    private float Speed;

    public Material material;

    Vector3 LeftCheck;
    Vector3 RightCheck;

    [Range(0.0f, 180.0f)]
    public float Angle;

    private bool move;

    private void Awake()
    {
        SphereCollider coll = GetComponent<SphereCollider>();
        coll.radius = 0.05f;
        coll.isTrigger = true;

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;

        //Target = GameObject.Find("ParentObject").transform.GetChild(0).GetComponent<Node>();
    }
    private void Start()
    {
        Speed = 5.0f;

        float x = 2.5f;
        float z = 3.5f;

        LeftCheck = transform.position + new Vector3(-x, 0.0f, z);
        RightCheck = transform.position + new Vector3(x, 0.0f, z);

        Angle = 45.0f;

        move = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                MeshFilter meshFilter = hit.transform.gameObject.GetComponent<MeshFilter>();

                Vector3[] verticesPoint = meshFilter.mesh.vertices;

                for(int i = 0;  i < verticesPoint.Length; ++i)
                {
                    if (!vertices.Contains(verticesPoint[i]) 
                        && verticesPoint[i].y < transform.position.y + 0.05f
                        //&& transform.position.y < verticesPoint[i].y + 0.05f
                        )
                    {
                        vertices.Add(verticesPoint[i]);
                    }
                }

            }

            for(int i = 0; i < vertices.Count; ++i)
            {
                GameObject obj = new GameObject(i.ToString());
                obj.transform.position = new Vector3(
                            hit.transform.position.x + vertices[i].x * hit.transform.localScale.x,
                            transform.position.y,
                            hit.transform.position.z + vertices[i].z * hit.transform.localScale.z);

                obj.AddComponent<myGizmo>();

                Matrix4x4[] matrix = new Matrix4x4[4];

                matrix[T] = Matrix.Translate(hit.transform.position);
                matrix[R] = Matrix.Rotate(hit.transform.eulerAngles);
                matrix[S] = Matrix.Scale(hit.transform.localScale); 

                matrix[M] = matrix[T] * matrix[R] * matrix[S];

                Outpot(matrix[M]);

                //Vector3 v = matrix[M].MultiplyPoint(vertices[i]);
                //Debug.Log(v.x + " , " + v.y + " , " + v.z);
            }
        }

        if (Target)
        {
            Vector3 Direction = (Target.transform.position - transform.position).normalized;

            transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(Direction),
                    Time.deltaTime);
            if (move)
            {
                transform.position += Direction * Speed * Time.deltaTime;
            }
            else
            {
                Vector3 targetDir = Target.transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                if (Vector3.Angle(targetDir, transform.forward) < 0.5f)
                    move = true;
            }
        }
    }

    private void FixedUpdate()
    {
        float startAngle = transform.eulerAngles.y - Angle;

        RaycastHit hit;

        Debug.DrawRay(transform.position,
            new Vector3(Mathf.Sin(startAngle * Mathf.Deg2Rad),
            0.0f, Mathf.Cos(startAngle * Mathf.Deg2Rad)) * 2.5f,
            Color.white);

        if (Physics.Raycast(transform.position, LeftCheck, out hit, 5.0f))
        {

        }

        Debug.DrawRay(transform.position,
            new Vector3(Mathf.Sin((Angle + transform.eulerAngles.y) * Mathf.Deg2Rad),
            0.0f, Mathf.Cos((Angle + transform.eulerAngles.y) * Mathf.Deg2Rad)) * 2.5f, Color.green);

        if (Physics.Raycast(transform.position, RightCheck, out hit, 5.0f))
        {

        }

        for (float f = -Angle + 5.0f; f < Angle; f += 5.0f)
        {
            Debug.DrawRay(transform.position,
                new Vector3(Mathf.Sin((f + transform.eulerAngles.y) * Mathf.Deg2Rad), 0.0f,
                Mathf.Cos((f + transform.eulerAngles.y) * Mathf.Deg2Rad)) * 2.5f, Color.red);
        }

    }

    private void LateUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        move = false;

        //if (Target.transform.name == other.transform.name)
        //    Target = Target.Next;
    }

    void Outpot(Matrix4x4 _m)
    {
        Debug.Log("==========================================");
        Debug.Log(_m.m00 + " , " + _m.m01 + " , " + _m.m02 + " , " + _m.m03);
        Debug.Log(_m.m10 + " , " + _m.m11 + " , " + _m.m12 + " , " + _m.m13);
        Debug.Log(_m.m20 + " , " + _m.m21 + " , " + _m.m22 + " , " + _m.m23);
        Debug.Log(_m.m30 + " , " + _m.m31 + " , " + _m.m32 + " , " + _m.m33);
    }
}
