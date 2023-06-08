using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    public Node Target;

    private float Speed;

    Vector3 LeftCheck;
    Vector3 RightCheck;

    private void Awake()
    {
        SphereCollider coll = GetComponent<SphereCollider>();
        coll.radius = 0.05f;
        coll.isTrigger = true;

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;

        Target = GameObject.Find("ParentObject").transform.GetChild(0).GetComponent<Node>();
    }
    private void Start()
    {
        Speed = 5.0f;

        float x = 2.5f;
        float z = 3.5f;

        LeftCheck = transform.position + new Vector3(-x, 0.0f, z);
        RightCheck = transform.position + new Vector3(x, 0.0f, z);
    }
    private void Update()
    {
        if (Target)
        {
            Vector3 Direction = (Target.transform.position - transform.position).normalized;
            transform.position += Direction * Speed * Time.deltaTime;

            RaycastHit hit;

            transform.LookAt(Target.transform);

            Debug.DrawRay(transform.position, LeftCheck * 5.0f, Color.red);
            if(Physics.Raycast(transform.position, LeftCheck, out hit, 5.0f))
            {

            }

            Debug.DrawRay(transform.position, RightCheck * 5.0f, Color.red);
            if (Physics.Raycast(transform.position, RightCheck, out hit, 5.0f))
            {

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Target.transform.name == other.transform.name)
            Target = Target.Next;
    }
}
