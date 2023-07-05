using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class CameraController : MonoBehaviour
{
    private Vector3 direction;
    private Transform target;
    public LayerMask mask;
    public float distance;

    private const string path = "Custom/Transparent/MyShader";

    public List<Renderer> objectRenderers = new List<Renderer>();
    
    private Vector3[] outCorners = new Vector3[4];

    [Header("Invisibility")]
    public bool FrustumList;

    [Range(0.0f, 1.0f)]
    public float X = 0.45f;

    [Range(0.0f, 1.0f)]
    public float Y = 0.45f;
    
    [Range(0.0f, 1.0f)]
    public float W = 0.45f;

    [Range(0.0f, 1.0f)]
    public float H = 0.45f;

    private void Awake()
    {
        target = GameObject.Find("Player").transform;
    }

    private void Start()
    {
#if UNITY_EDITOR
        if (!target)
		{
            EditorApplication.ExitPlaymode();
            Debug.Log("Target could not be loaded.");
		}

        if (path.Length <= 0)
		{
            EditorApplication.ExitPlaymode();
		}
#endif

        X = 0.48f;
        Y = 0.35f;
        W = 0.04f;
        H = 0.3f;

        FrustumList = true;
        direction = (target.position - transform.position).normalized;
        distance = Vector3.Distance(target.position, transform.position);
        
        Camera.main.CalculateFrustumCorners(
            new Rect(0, 0, 1, 1),
            Camera.main.farClipPlane,
            Camera.main.stereoActiveEye,
            outCorners
            );
    }

    private void Update()
    {
        if (FrustumList)
		{
            Camera.main.CalculateFrustumCorners(
                new Rect(X,Y,W,H),
                distance,
                Camera.main.stereoActiveEye,
                outCorners
                );

            Debug.DrawLine(transform.position, transform.position + direction * distance, Color.green);

            for (int i = 0; i < 4; ++i)
                Debug.DrawLine(transform.position, (outCorners[i] - transform.position).normalized * distance, Color.blue);
		}

        List<RaycastHit>[] hits = new List<RaycastHit>[4];
        List<Renderer> renderers = new List<Renderer>();

        // ** ��� �浹�� ����.
        for (int i = 0; i < 4; ++i)
		{
            hits[i] = Physics.RaycastAll(transform.position, direction, distance, mask).ToList();

            // ** �浹�� ��� ���ҵ� �߿� Renderer�� ������ ���ο� ����Ʈ�� ����.
            renderers.AddRange(hits[i].Select(hit => hit.transform.GetComponent<Renderer>()).Where(renderer => renderer != null).ToList());
        }

        // ** ���� ����Ʈ���� ���ԵǾ����� ���� ray�� ������ ����Ʈ���� ���� Renderer
        List<Renderer> extractionList = objectRenderers.Where(renderer => !renderers.Contains(renderer)).ToList();

        // ** ������ �Ϸ�� Renderer�� ���� ���İ����� �ǵ�����. 
        // ** �׸��� ����.
        foreach (Renderer renderer in extractionList)
        {
            StartCoroutine(SetFadeIn(renderer));
            objectRenderers.Remove(renderer);
        }

        for (int i = 0; i< 4; ++i)
		{
            // ** hits �迭�� ��� ���Ҹ� Ȯ��.
            foreach(RaycastHit hit in hits[i])
			{
                // ** ray�� �浹�� ������ Object�� Renderer�� �޾ƿ�.
                Renderer renderer = hit.transform.GetComponent<Renderer>();

                // ** �浹�� �ִٸ� Renderer�� Ȯ��.
                if (renderer!=null)
				{
                    // ** List�� �̹� ���Ե� Renderer���� Ȯ��.
                    if (!objectRenderers.Contains(renderer))
					{
                        StartCoroutine(SetFadeOut(renderer));
					}
				}
			}
		}
    }


    IEnumerator SetFadeOut(Renderer renderer)
	{
        // ** Color�� ������ ������ Shader�� ����.
        objectRenderers.Add(renderer); // ** �߰�

        renderer.material = new Material(Shader.Find(path));

        // ** ����� Shader�� Color���� �޾ƿ�.
        Color color = renderer.material.color;

        // ** color.a �� ���� 0.5f ���� ū ��쿡�� �ݺ�.
        while (0.5f < color.a)
		{
            yield return null;

            // ** Alpha(1) -= Time.deltatime;
            color.a -= Mathf.Clamp(Time.deltaTime * 10, 0.1f, 0.5f);

            renderer.material.color = color;
		}
	}


    IEnumerator SetFadeIn(Renderer renderer)
    {
        objectRenderers.Remove(renderer);

        // ** ����� Shader�� Color ���� �޾ƿ�.
        Color color = renderer.material.color;

        // ** color.a ���� 0.9f ���� ���� ��쿡�� �ݺ�
        while (color.a < 0.9f)
        {
            yield return null;
            // ** Alpha(1) -= Time.deltaTime;
            color.a += Mathf.Clamp(Time.deltaTime * 10, 0.1f, 0.5f);

            renderer.material.color = color;
        }

        color.a = 1.0f;
        renderer.material.color = color;
        objectRenderers.Remove(renderer);
    }
}