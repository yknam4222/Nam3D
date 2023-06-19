using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    const int T = 1; // Transform
    const int R = 2; // Rotation
    const int S = 3; // Scale
    const int M = 0; // Matrix

    public Node Target = null;
    public List<Node> BestList = new List<Node>();
    public List<Node> OpenList = new List<Node>();

    private float Speed;

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

        Target = new Node(
            new Vector3(0.0f, 0.0f, 30.0f));

        LeftCheck = transform.position + (new Vector3(-x, 0.0f, z));
        RightCheck = transform.position + (new Vector3(x, 0.0f, z));

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

                    float bastDistance = float.MaxValue;

                    OpenList.Clear();

                    List<Vector3> VertexList = GetVertex(hit.transform.gameObject);

                    /*
                    for (int i = 0; i < VertexList.Count; ++i)
                        VertexList[i] = new Vector3(VertexList[i].x, 0.1f, VertexList[i].z);
                     */

                    Node StartNode = null;

                    foreach (Vector3 element in VertexList)
                    {
                        // ���ؽ� ��ġ�� �����Ͽ� ���� �̵��� ������ ���� ������.
                        Matrix4x4[] matrix = new Matrix4x4[4];

                        matrix[T] = Matrix.Translate(hit.transform.position);
                        matrix[R] = Matrix.Rotate(hit.transform.eulerAngles);
                        matrix[S] = Matrix.Scale(hit.transform.lossyScale * scale);

                        matrix[M] = matrix[T] * matrix[R] * matrix[S];
                        Vector3 v = matrix[M].MultiplyPoint(element);

                        Node node = new Node();
                        node.Position = v;
                        node.Cost = 0.0f;
                        node.Next = null;

                        OpenList.Add(node);

                        // ���� ����� ��Ʈ�� ã�� ����.
                        float curentDistance = Vector3.Distance(transform.position, v);

                        if (curentDistance < bastDistance)
                        {
                            bastDistance = curentDistance;
                            StartNode = node;
                        }
                    }

                    // ������ġ�� 
                    if (StartNode != null)
                        OpenList.Remove(StartNode);

                    BestList = AStar(StartNode, Target);


                    //�ð��� ǥ��
                    GameObject StartPoint = new GameObject("StartNode");
                    StartPoint.transform.position = StartNode.Position;
                    StartPoint.transform.SetParent(parent.transform);
                    myGizmo gizmo = StartPoint.AddComponent<myGizmo>();
                    gizmo.color = Color.red;

                    for (int i = 1; i < BestList.Count; ++i)
                    {
                        GameObject Object = new GameObject("node");
                        Object.transform.position = BestList[i].Position;
                        Object.transform.SetParent(parent.transform);
                        Object.AddComponent<myGizmo>();
                    }
                }
            }
        }


        List<Vector3> GetVertex(GameObject hitObject)
        {
            HashSet<Vector3> set = new HashSet<Vector3>();

            // ���� ������Ʈ�� Ȯ��.
            if (hitObject.transform.childCount != 0)
            {
                // ���� ������Ʈ�� �����Ѵٸ� ��� ����������Ʈ�� Ȯ��.
                for (int i = 0; i < hitObject.transform.childCount; ++i)
                {
                    // ��� ���� ������Ʈ�� ���ؽ��� �޾ƿ�.
                    //  �ߺ� ���� ���� �� ����.
                    set.UnionWith(GetVertex(hitObject.transform.GetChild(i).gameObject));
                }
            }

            List<Vector3> VertexList = new List<Vector3>(set);

            //  ���� ������Ʈ�� MeshFilter �� Ȯ��.
            MeshFilter meshFilter = hitObject.GetComponent<MeshFilter>();

            //  MeshFilter �� ���ٸ� ������ ���ؽ��� �����Ƿ� ����
            if (meshFilter == null)
                return VertexList;

            //  ��� ���ؽ��� ����
            Vector3[] verticesPoint = meshFilter.mesh.vertices;

            //  hit �� ������Ʈ�� ��� ���ؽ� Ȯ��.
            for (int i = 0; i < verticesPoint.Length; ++i)
            {
                //  ���ؽ��� Ȯ���ϴ� ����.
                if (!VertexList.Contains(verticesPoint[i])
                    && verticesPoint[i].y < transform.position.y + 0.05f
                    && transform.position.y < verticesPoint[i].y + 0.05f)
                {
                    //  �ش� ���ؽ� �߰�
                    VertexList.Add(verticesPoint[i]);
                }
            }

            return VertexList;
        }



        List<Node> AStar(Node StartNode, Node EndNode)
        {
            List<Node> bestNodes = new List<Node>();

            // ** (A*)
            Node MainNode = StartNode;
            int Count = 0;


            bestNodes.Add(StartNode);


            Node compair = StartNode;

            while (OpenList.Count != 0)
            {
                Count++;

                if (Count == 100)
                    break;
                //  ������ ��带 ã�´�.
                float OldDistance = float.MaxValue;

                for (int i = 0; i < OpenList.Count; ++i)
                {
                    float Distance = Vector3.Distance(compair.Position, OpenList[i].Position);

                    if (Distance < OldDistance)
                    {
                        OldDistance = Distance;
                        compair = OpenList[i];
                    }
                }

                //if (!bestNodes.Contains(OpenList[index]))
                //{
                //    Node OldNode = bestNodes[bestNodes.Count - 1];
                //    Node curentNode = OpenList[index];

                //    RaycastHit Hit;
                //    // origin(�������), direction(������)
                //    if (Physics.Raycast(OldNode.Position, curentNode.Position, out Hit, OldDistance))
                //    {
                //        Debug.Log(Hit.transform.name);

                //        if (Hit.transform.tag != "Node")
                //        {

                //        }
                //        else
                //        {

                //        }
                //    }

                //    if (Vector3.Distance(EndNode.Position, curentNode.Position) < Vector3.Distance(EndNode.Position, OldNode.Position))
                //    {
                //        OpenList.Remove(curentNode);
                //        bestNodes.Add(curentNode);
                //    }
                //    else
                //        break;
                //}
            }

            bestNodes.Add(EndNode);
            return bestNodes;
        }



        /*
        if (Target)
        {
            Vector3 Direction = (Target.transform.position - transform.position).normalized;

            transform.rotation = Quaternion.Lerp(
                   transform.rotation,
                   Quaternion.LookRotation(Direction),
                   0.016f);

            if (move)
            {
                transform.position += Direction * Speed * Time.deltaTime;
            }
            else
            {
                Vector3 targetDir = Target.transform.position - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                if (Vector3.Angle(targetDir, transform.forward) < 0.1f)
                    move = true;
            }
        }
         */
    }

    private void FixedUpdate()
    {
        float startAngle = (transform.eulerAngles.y - Angle);

        RaycastHit hit;

        Debug.DrawRay(transform.position,
            new Vector3(
                Mathf.Sin(startAngle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(startAngle * Mathf.Deg2Rad)) * 2.5f,
            Color.white);

        if (Physics.Raycast(transform.position, LeftCheck, out hit, 5.0f))
        {

        }

        Debug.DrawRay(transform.position,
             new Vector3(
                 Mathf.Sin((transform.eulerAngles.y + Angle) * Mathf.Deg2Rad), 0.0f, Mathf.Cos((transform.eulerAngles.y + Angle) * Mathf.Deg2Rad)) * 2.5f,
             Color.green);

        if (Physics.Raycast(transform.position, RightCheck, out hit, 5.0f))
        {

        }

        //int Count = (int)((Angle * 2) / 5.0f);

        for (float f = startAngle + 5.0f; f < (transform.eulerAngles.y + Angle - 5.0f); f += 5.0f)
        {
            Debug.DrawRay(transform.position,
                new Vector3(
                    Mathf.Sin(f * Mathf.Deg2Rad), 0.0f, Mathf.Cos(f * Mathf.Deg2Rad)) * 2.5f,
                Color.red);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //move = false;

        /*
        if (Target.transform.name == other.transform.name)
            Target = Target.Next;
         */
    }
}