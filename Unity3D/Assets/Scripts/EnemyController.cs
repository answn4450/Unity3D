using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    const int M = 0; // Matrix
    const int T = 1; // Transform
    const int R = 2; // Rotation
    const int S = 3; // Scale

    public Node Target;
    public List<Vector3> vertices = new List<Vector3>();

    private float Speed;

    public Material material;

    Vector3 LeftCheck;
    Vector3 RightCheck;


    [Range(0.0f, 180.0f)]
    public float Angle;

    private bool move;

    [Range(1.0f, 2.0f)]
    public float scale;

    private void Awake()
    {
        SphereCollider coll = GetComponent<SphereCollider>();
        coll.radius = 0.05f;
        coll.isTrigger = true;

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.useGravity = false;

        //Target = GameObject.Find("ParentObject").transform.GetChild(0).GetComponent<Node>();
        Target = null;
    }

    private void Start()
    {
        material.color = Color.red;

        Speed = 5.0f;

        float x = 2.5f;
        float z = 3.5f;

        LeftCheck = transform.position + (new Vector3(-x, 0.0f, z));
        RightCheck = transform.position + (new Vector3(x, 0.0f, z));

        Angle = 45.0f;

        move = false;

        scale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                MeshFilter meshFilter = hit.transform.gameObject.GetComponent<MeshFilter>();
                Vector3[] verticesPoint = meshFilter.mesh.vertices;

                List<Vector3> temp = new List<Vector3>();
                
                for (int i = 0; i < verticesPoint.Length; ++i)
                {
                    if (!vertices.Contains(verticesPoint[i]) 
                        && verticesPoint[i].y < transform.position.y + 0.05f
                        && verticesPoint[i].y > transform.position.y - 0.05f)

                    {
                        temp.Add(verticesPoint[i]);
                    }
                }

                for (int i = 0; i < temp.Count; ++i)
                {
                    temp[i] = new Vector3(
                        temp[i].x,
                        0.1f,
                        temp[i].z
                        );
                }

                vertices.Clear();

                for (int i = 0; i < temp.Count; ++i)
                {
                    GameObject obj = new GameObject(i.ToString());
                    
                    Matrix4x4[] matrix = new Matrix4x4[4];

                    matrix[T] = Matrix.Translate(hit.transform.position);
                    matrix[R] = Matrix.Rotate(hit.transform.eulerAngles);
                    matrix[S] = Matrix.Scale(hit.transform.localScale * scale);

                    matrix[M] = matrix[T] * matrix[R] * matrix[S];

                    Vector3 v = matrix[M].MultiplyPoint(temp[i]);
                    vertices.Add(v);

                    obj.transform.position = v;
                    obj.AddComponent<MyGizmo>();
                    obj.GetComponent<MyGizmo>().color = Color.black;

                    //Debug.Log("-----------------------------------");
                    //Outpot(matrix[M]);
                    //Vector3 v = matrix[M].MultiplyPoint(vertices[i]);
                    //Debug.Log(v.x + " ," + v.y + " ," + v.z);

                }
            }
        }

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

    /*
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

        while (time < 1.0f)
        {

            time += Time.deltaTime;

            yield return null;
        }

        move = false;
    }
     */

    private void OnTriggerEnter(Collider other)
    {
        move = false;

        //if (Target.transform.name == other.transform.name)
        //    Target = Target.Next;
    }

    void Outpot(Matrix4x4 _m)
    {
        Debug.Log(_m.m00 + ", " + _m.m01 + ", " + _m.m02 + ", " + _m.m03 + ", ");
        Debug.Log(_m.m10 + ", " + _m.m11 + ", " + _m.m12 + ", " + _m.m13 + ", ");
        Debug.Log(_m.m20 + ", " + _m.m21 + ", " + _m.m22 + ", " + _m.m23 + ", ");
        Debug.Log(_m.m30 + ", " + _m.m31 + ", " + _m.m32 + ", " + _m.m33 + ", ");
    }
}
