using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [Range(0.0f, 180.0f)]
    public float Angle;

    public Node Target;

    private float Speed;

    private int check;
    private bool move;

    private Camera camera;

    private Vector3 LeftCheck, RightCheck;

    private Vector3 offset;

    private bool View;

    private void Awake()
    {
        camera = Camera.main;

        SphereCollider coll = GetComponent<SphereCollider>();
        coll.radius = 0.05f;
        coll.isTrigger = true;

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;

        offset = new Vector3(0.0f, 10.0f, 0.0f);
        //Target = GameObject.Find("ParentObject").transform.GetChild(0).GetComponent<Node>();
        View = false;
    }

    private void Start()
    {
        Angle = 45.0f;
        move = false;

        Speed = 3.0f;

        float x = 2.5f;
        float z = 3.5f;

        LeftCheck = transform.position + (new Vector3(-x, 0.0f, z));
        RightCheck = transform.position + (new Vector3(x, 0.0f, z));
    }

    private void Update()
    {
        View = (Input.GetKey(KeyCode.Tab)) ? true:false;

        if (View)
        {
            offset = new Vector3(0.0f, 5.0f, -3.0f);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 100.0f, Time.deltaTime);
        }
        else
        {
            offset = new Vector3(0.0f, 10.0f, -10.0f);
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 60.0f, Time.deltaTime);
        }

        camera.transform.position = Vector3.Lerp(
            camera.transform.position,
            transform.position + offset,
            0.016f
            );

        camera.transform.LookAt(transform.position);

        if (Target)
        {
            if (move)
            {
                Vector3 Direction = (Target.transform.position - transform.position).normalized;
                transform.position += Direction * Time.deltaTime * Speed;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                     Quaternion.LookRotation(transform.position, Target.transform.position),
                    Time.deltaTime);
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position, RightCheck, Color.red);
        Debug.DrawRay(transform.position, LeftCheck, Color.red);

        if (Physics.Raycast(transform.position, LeftCheck, out hit, 5.0f))
        {

        }

        if (Physics.Raycast(transform.position, RightCheck, out hit, 5.0f))
        {

        }

        /*
        Debug.DrawRay(transform.position,
            new Vector3(
                Mathf.Sin(Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(Angle * Mathf.Deg2Rad)) * 2.5f,
            Color.green
            );

        Debug.DrawRay(transform.position,
            new Vector3(
                Mathf.Sin(-Angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(Angle * Mathf.Deg2Rad)) * 2.5f,
            Color.green
            );
        */

        for (float f = -Angle; f < Angle; f += 5.0f)
        {
            Debug.DrawLine(
                transform.position,
                transform.position +
                new Vector3(
                    Mathf.Sin((transform.rotation.eulerAngles.y + f) * Mathf.Deg2Rad),
                    0.0f,
                    Mathf.Cos((transform.rotation.eulerAngles.y + f) * Mathf.Deg2Rad)) * 2.5f,
                Color.white
            );
        }
    }

    void function()
    {
        if (move)
            return;

        move = true;
        StartCoroutine(SetMove());
    }

    IEnumerator SetMove()
    {
        float time = 0.0f;

        check = (check == 0) ? 1 : 0;

        while (time < 1.0f)
        {
            time += Time.deltaTime;

            yield return null;

        }
        move = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Target.transform.name == other.transform.name)
            Target = Target.Next;
    }
}
