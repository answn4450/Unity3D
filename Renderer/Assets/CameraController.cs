using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    private Camera camera;
    private Vector3 direction;
    private float distance;
    public LayerMask mask;

    public Renderer prev_renderer;
    public Renderer renderer;

    private List<Renderer> whiteList = new List<Renderer>();
    private List<Renderer> blackList = new List<Renderer>();
    private Dictionary<Renderer, bool> renderStat = new Dictionary<Renderer, bool>();
    
    private bool Check;

    private const string path = "Legacy Shaders/Transparent/Specular";

    private void Awake()
    {
        camera = Camera.main;

        target = GameObject.Find("Player").transform;
        distance = 10;
        Check = false;
    }

    private void Start()
    {
        direction = (target.position - transform.position).normalized;
    }

    private void Update()
    {
        Debug.DrawLine(
            transform.position,
            transform.position + direction * distance,
            Color.green
            );
        Ray ray = new Ray(transform.position, direction);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, mask))
        {
            if (hit.transform != null)
            {
                if (Check == false || true)
                {
                    print("sd");
                    Check = true;
                    renderer = hit.transform.GetComponent<Renderer>();

                    if (renderer != null)
                    {
                        if (!renderStat.ContainsKey(renderer))
                            renderStat.Add(renderer, false);
                        StartCoroutine(SetColor(renderer));
                    }
                }
                Debug.Log(hit.transform.name);
            }
        }
        else if (renderer != null)
        {
            renderStat[renderer] = true;
            StartCoroutine(SetColor(renderer));
        }
    }

    IEnumerator SetColor(Renderer renderer)
    {
        // ** Color값 변경이 가능한 Shader로 변경.
        Material material = new Material(Shader.Find(path));

        renderer.material = material;
        
        // ** 변경된 Shader의 Color 값을 받아옴.
        Color color = renderer.material.color;

        // ** color.a 값이 0.5f 보다 큰 경우에만 반복.

        bool back = renderStat[renderer];
        bool aInRange = (!back && 0.5f < color.a) || (back && 1.0f > color.a);
        bool renderInList = !back && whiteList.Contains(renderer) || back && blackList.Contains(renderer);
        
        while (aInRange)
        {
            yield return null;

            // ** Alpha(1) -= Time.deltaTime;
            if (back)
                color.a += Time.deltaTime;
            else
                color.a -= Time.deltaTime;

            renderer.material.color = color;
        }
    }
}
