using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterController : MonoBehaviour
{
    public List<GameObject> PointList;

    [Range(-90.0f, 90.0f)]
    public float Angle;

    public float Speed;

    void Start()
    {
        gameObject.AddComponent<MyGizmo>();

        Speed = 5.0f;
    }

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 Movement = new Vector3(hor, 0.0f, ver) * 5.0f * Time.deltaTime;
        transform.Translate(Movement);
        //transform.position = new Vector3(
          //  0.0f, Mathf.Sin(Angle * Mathf.Deg2Rad), 0.0f) * Speed;
    }
}
