using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererManager : MonoBehaviour
{
    public Renderer renderer;

    private const string path = "Legacy Shaders/Transparent/Specular";
    
    void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }

    IEnumerator SetColor(Renderer renderer)
    {
        // ** Color�� ������ ������ Shader�� ����.
        Material material = new Material(Shader.Find(path));

        // ** ����� Shader�� Color ���� �޾ƿ�.
        Color color = renderer.material.color;

        // ** color.a ���� 0.5f ���� ū ��쿡�� �ݺ�.
        while (0.5f < color.a)
        {
            yield return null;

            // ** Alpha(1) -= Time.deltaTime;
            color.a -= Time.deltaTime;
            
            renderer.material.color = color;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (renderer != null)
            {
                StartCoroutine(SetColor(renderer));
            }
        }
    }
}
