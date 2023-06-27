using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointController : MonoBehaviour
{
    private Transform Parent;

    public GameObject HitPoint;

    [Range(-1.0f, 1.0f)]
    public float offsetX;

    [Range(0.0f, 1.0f)]
    public float offsetY;


    private void Awake()
    {
        HitPoint = Instantiate( (GameObject)Resources.Load("HitPoint"));

        Parent = GameObject.Find("HitPointbarCanvas").transform;
    }

    private void Start()
    {
        HitPoint.transform.SetParent(Parent);

        offsetX = 0.0f;
        offsetY = 0.0f;

    }

    private void Update()
    {
        HitPoint.transform.position = Camera.main.WorldToScreenPoint(
            new Vector3(
                Camera.main.transform.position.x + offsetX,
                Camera.main.transform.position.y + offsetY,
                0.0f) + transform.position);
    }
}
