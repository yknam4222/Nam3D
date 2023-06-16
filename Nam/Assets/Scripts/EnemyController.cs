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
    public List<Node> vertices = new List<Node>();
    public List<Vector3> bestList = new List<Vector3>();
    public List<Node> OpenList = new List<Node>();


    private float Speed;

    public Material material;

    Vector3 LeftCheck;
    Vector3 RightCheck;

    [Range(0.0f, 180.0f)]
    public float Angle;

    private bool getNode;

    [Range(1.0f, 2.0f)]
    public float scale;

    private GameObject parent;

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
        parent = new GameObject("Nodes");

        Speed = 5.0f;

        float x = 2.5f;
        float z = 3.5f;

        LeftCheck = transform.position + new Vector3(-x, 0.0f, z);
        RightCheck = transform.position + new Vector3(x, 0.0f, z);

        Angle = 45.0f;

        getNode = false;
        scale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag != "Node")
                {
                    getNode = true;

                    float bestDistance = float.MaxValue;
                    bool isInitial = true;

                    OpenList.Clear();
                    vertices.Clear();

                    List<Vector3> VertetList = GetVertex(hit.transform.gameObject);

                    foreach (Vector3 element in VertetList)
                    {
                        Matrix4x4[] matrix = new Matrix4x4[4];

                        matrix[T] = Matrix.Translate(hit.transform.position);
                        matrix[R] = Matrix.Rotate(hit.transform.eulerAngles);
                        matrix[S] = Matrix.Scale(hit.transform.lossyScale * scale);

                        matrix[M] = matrix[T] * matrix[R] * matrix[S];
                        Vector3 v = matrix[M].MultiplyPoint(element);

                        float currentDistance = Vector3.Distance(transform.position, v);

                        //GameObject Obj = new GameObject("Node");
                        Node node = new Node();
                        node.Position = v;
                        //Obj.transform.SetParent(parent.transform);
                        //Obj.transform.position = v;
                        //Obj.AddComponent<myGizmo>();

                        if (isInitial)
                        {
                            bestDistance = currentDistance;
                            isInitial = false;
                            vertices.Add(node);
                        }
                        else if (currentDistance < bestDistance)
                            bestDistance = currentDistance;
                        else
                            vertices.Add(node);
                    }

                    Node Mainnode = OpenList[0];
                    Mainnode.Cost = 0.0f;

                    while (vertices.Count != 0)
                    {
                        float OldDistance = 1000000.0f;
                        int index = 0;

                        for (int i = 0; i < vertices.Count; ++i)
                        {
                            float Distance = Vector3.Distance(OpenList[0].transform.position,
                                vertices[i].transform.position);

                            if (Distance < OldDistance)
                            {
                                OldDistance = Distance;
                                Node Nextnode = vertices[i].GetComponent<Node>();
                                Nextnode.Cost = Mainnode.Cost + Distance;
                                index = i;
                            }
                        }

                        if (!OpenList.Contains(vertices[index].GetComponent<Node>()))
                        {
                            Node OldNode = OpenList[OpenList.Count - 1];
                            Node current = vertices[index].GetComponent<Node>();

                            RaycastHit Hit;

                            if (Physics.Raycast(OldNode.transform.position, current.transform.position, out hit, OldDistance))
                            {
                                Debug.DrawRay(OldNode.transform.position, current.transform.position, Color.green, 100.0f);
                                Debug.Log(hit.transform.name);
                                if (hit.transform.tag != "Node")
                                {
                                }
                                else
                                {
                                }
                            }

                            OpenList.Add(vertices[index].GetComponent<Node>());
                            vertices[index].GetComponent<Node>();

                            vertices.Remove(vertices[index]);
                        }
                    }
                }
            }
        }

        List<Vector3> GetVertex(GameObject hitObject)
        {
            HashSet<Vector3> set = new HashSet<Vector3>();
            // 하위 오브젝트를 확인
            if (hitObject.transform.childCount != 0)
            {
                // 하위 오브젝트가 존재한다면 모든 하위 오브젝트를 확인.
                for (int i = 0; i < hitObject.transform.childCount; ++i)
                {
                    // 모든 하위 오브젝트의 버텍스를 받아옴.
                    // 중복 원소 제거 후 삽입.
                    set.UnionWith(GetVertex(hitObject.transform.GetChild(i).gameObject));
                    //VertexList.AddRange(GetVertex(hitObject.transform.GetChild(i).gameObject));
                }
            }
            List<Vector3> VertexList = new List<Vector3>(set);

            // 현재 오브젝트의 MeshFilter를 확인.
            MeshFilter meshFilter = hitObject.GetComponent<MeshFilter>();

            // MeshFilter가 없다면 참조할 버텍스가 없으므로 종료.
            if (meshFilter == null)
                return VertexList;

            // 모든 버텍스를 참조
            Vector3[] verticesPoint = meshFilter.mesh.vertices;

            // hit된 오브젝트의 모든 버텍스를 확인.
            for (int i = 0; i < verticesPoint.Length; ++i)
            {
                // 버텍스를 확인하는 조건.
                if (!VertexList.Contains(verticesPoint[i])
                    && verticesPoint[i].y < transform.position.y + 0.05f
                    && transform.position.y < verticesPoint[i].y + 0.05f
                    )
                {
                    // 해당 버텍스 추가
                    VertexList.Add(verticesPoint[i]);
                }
            }

            return VertexList;
        }

        if (Target)
        {
            Vector3 Direction = (Target.transform.position - transform.position).normalized;

            transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(Direction),
                    Time.deltaTime);
            if (getNode)
            {
                transform.position += Direction * Speed * Time.deltaTime;
            }
            else
            {
                Vector3 targetDir = Target.transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                if (Vector3.Angle(targetDir, transform.forward) < 0.5f)
                    getNode = true;
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
        getNode = false;

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
