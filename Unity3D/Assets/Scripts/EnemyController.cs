using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    public Node Target;

    private float Speed;

    private Vector3 LeftCheck, RightCheck;

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
        Speed = 3.0f;

        float x = 2.5f;
        float z = 3.5f;

        LeftCheck = transform.position + (new Vector3(-x, 0.0f, z));
        RightCheck = transform.position + (new Vector3(x, 0.0f, z));
    }

    private void Update()
    {
        if (Target)
        {
            Vector3 Direction = (Target.transform.position - transform.position).normalized;
            //transform.Translate(Direction * Time.deltaTime * Speed);
            transform.position += Direction * Time.deltaTime * Speed;

            Ray ray = new Ray(transform.position, Direction);

            RaycastHit hit;

            //transform.LookAt(Target.transform);

            //Debug.DrawRay(transform.position, Direction * 5.0f, Color.red);
            Debug.DrawRay(transform.position, RightCheck, Color.red);
            Debug.DrawRay(transform.position, LeftCheck, Color.red);

            if (Physics.Raycast(transform.position, LeftCheck, out hit, 5.0f))
            {

            }

            if (Physics.Raycast(transform.position, RightCheck, out hit, 5.0f))
            {

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Target.transform.name == other.transform.name)
            Target = Target.Next;
    }
}
