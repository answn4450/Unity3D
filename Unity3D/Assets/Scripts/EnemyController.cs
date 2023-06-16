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
    public List<Node> Vertices = new List<Node>();
    public List<Node> BestList = new List<Node>();
    public List<Node> OpenList = new List<Node>();

    private float Speed;

    Vector3 LeftCheck;
    Vector3 RightCheck;

    [Range(0.0f, 180.0f)]
    public float Angle;

    private bool move;

    [Range(1.0f, 2.0f)]
    public float scale;

    public Vector3 EndPoint;

    private GameObject parent;

    public bool getNode;

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

        LeftCheck = transform.position + (new Vector3(-x, 0.0f, z));
        RightCheck = transform.position + (new Vector3(x, 0.0f, z));

        Angle = 45.0f;

        move = false;

        scale = 1.0f;

        EndPoint = GameObject.Find("EndPoint").transform.position;

        getNode = false;
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

                    bool initial = true;
                    float bestDistance = 1000000.0f;
                    float currentDistance;

                    OpenList.Clear();
                    Vertices.Clear();

                    Node StartNode = new Node();

                    foreach (Vector3 element in GetVertex(hit.transform.gameObject))
                    {
                        Matrix4x4[] matrix = new Matrix4x4[4];

                        matrix[T] = Matrix.Translate(hit.transform.position);
                        matrix[R] = Matrix.Rotate(hit.transform.eulerAngles);
                        matrix[S] = Matrix.Scale(hit.transform.lossyScale * scale);

                        matrix[M] = matrix[T] * matrix[R] * matrix[S];

                        Vector3 v = matrix[M].MultiplyPoint(element);

                        // ** 제일 가까운 노드를 찾기 위함.
                        currentDistance = Vector3.Distance(transform.position, v);

                        Node node = new Node();
                        node.Position = v;

                        if (initial)
                        {
                            initial = false;
                            bestDistance = currentDistance;
                            StartNode = node;
                            BestList.Add(node);
                        }
                        else if (currentDistance < bestDistance)
                        {
                            bestDistance = currentDistance;
                            StartNode = node;
                        }
                        else
                            BestList.Add(node);

                    }

                    // ** 시각적 표현

                    GameObject StartPoint = new GameObject("StarNode");
                    StartPoint.transform.SetParent(parent.transform);
                    StartPoint.transform.position = StartNode.Position;
                    MyGizmo gizmo = StartPoint.AddComponent<MyGizmo>();
                    gizmo.color = Color.red;

                    if (StartNode != null)
                        OpenList.Add(StartNode);

                    for (int i = 1; i <BestList.Count; ++i)
                    {
                        GameObject Object = new GameObject("node");
                        StartPoint.transform.SetParent(parent.transform);
                        StartPoint.transform.position = BestList[i].Position;
                        StartPoint.AddComponent<MyGizmo>();
                    }

                    Node EndNode = new Node(GameObject.Find("EndPoint").transform.position);
                    BestList = AStar(StartNode, EndNode);

                }
            }
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
        MyTest();
    }


    private List<Vector3> GetVertex(GameObject hitObject)
    {
        List<Vector3> VertexList = new List<Vector3>();
        HashSet<Vector3> set = new HashSet<Vector3>();

        // ** 하위 오브젝트를 확인
        if (hitObject.transform.childCount != 0)
        {
            // ** 하위 오브젝트가 존재한다면 모든 하위오브젝트를 확인
            for (int i = 0; i < hitObject.transform.childCount; ++i)
            {
                // ** 모든 하위 오브젝트의 버텍스를 받아옴.
                set = new HashSet<Vector3>(VertexList);

                // ** 중복원소 제거후 삽입
                set.UnionWith(GetVertex(hitObject.transform.GetChild(i).gameObject));
            }
        }

        // ** 현재 오브젝트의 MeshFilter를 확인.
        MeshFilter meshFilter = hitObject.transform.gameObject.GetComponent<MeshFilter>();

        // ** MeshFilter가 없다면 참조할 버텍스가 없으므로 종료
        // ** 모든 버텍스를 참조
        Vector3[] verticesPoint = meshFilter.mesh.vertices;

        // ** hit 된 오브젝트의 모든 버텍스 확인.
        for (int i = 0; i < verticesPoint.Length; ++i)
        {
            // ** 버텍스를 확인하는 조건.
            if (!VertexList.Contains(verticesPoint[i])
                && verticesPoint[i].y < transform.position.y + 0.05f
                && transform.position.y < verticesPoint[i].y + 0.05f)
            {
                // ** 버텍스의 y 좌표를 ground의 Y 좌표보다 조금 높은 위치(0.1f)로 변경
                verticesPoint[i].y = 0.1f;
                // ** 해당 버텍스 추가
                VertexList.Add(verticesPoint[i]);
            }
        }

        return VertexList;
    }


    private List<Node> AStar(Node StartNode, Node EndNode)
    {
        Node compare;
        int Count = 0;

        float OldDistance = float.MaxValue;

        while (OpenList.Count != 0)
        {
            if (++Count > 100)
                break;

            int index = 0;

            for (int i = 0; i < Vertices.Count; ++i)
            {
                if (index == i)
                    continue;

                float Distance = Vector3.Distance(OpenList[index].Position, Vertices[i].Position);

                if (Distance < OldDistance && !OpenList.Contains(OpenList[index]))
                {
                    OldDistance = Distance;
                    Node Nextnode = Vertices[i];
                    //Nextnode.Cost = MainNode.Cost + Distance;
                    index = i;
                }
            }

            if (!BestList.Contains(OpenList[index]))
            {
                Node OldNode = OpenList[OpenList.Count - 1];
                Node currentNode = Vertices[index];

                RaycastHit Hit;

                if (Physics.Raycast(OldNode.Position, currentNode.Position, out Hit, OldDistance))
                {
                    Debug.Log(Hit.transform.position);

                    if (Hit.transform.tag != "Node")
                    {

                    }
                    else
                    {

                    }
                }

                if (Vector3.Distance(EndNode.Position, currentNode.Position) < Vector3.Distance(EndNode.Position, OldNode.Position))
                {

                }
                else
                {
                    break;
                }

                OpenList.Remove(Vertices[index]);
                //best.Add(index);
            }
        }

        return new List<Node>();
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
        move = false;
        
        /*
        if (Target.transform.name == other.transform.name)
            Target = Target.Next;
         */
    }


    void Outpot(Matrix4x4 _m)
    {
        Debug.Log("==============================================");
        Debug.Log(_m.m00 + ", " + _m.m01 + ", " + _m.m02 + ", " + _m.m03);
        Debug.Log(_m.m10 + ", " + _m.m11 + ", " + _m.m12 + ", " + _m.m13);
        Debug.Log(_m.m20 + ", " + _m.m21 + ", " + _m.m22 + ", " + _m.m23);
        Debug.Log(_m.m30 + ", " + _m.m31 + ", " + _m.m32 + ", " + _m.m33);
    }

    private void MyTest()
    {
        for (int i = 0; i < BestList.Count - 1; ++i)
            Debug.DrawLine(BestList[i].Position, BestList[i + 1].Position, Color.black);
    }
}