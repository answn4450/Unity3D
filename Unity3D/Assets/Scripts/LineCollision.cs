using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Line
{
    public Vector3 StartPoint;
    public Vector3 EndPoint;
}

public class LineCollision : MonoBehaviour
{
    public List<Line> LineList = new List<Line>();

    [SerializeField] private float Width;
    [SerializeField] private float Height;

    void Start()
    {
        //float fx = Random.Range(0.0f, 0.0f);
        //float fy = Random.Range(0.0f, 0.0f);

        Vector3 OldPoint = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < 5; ++i)
        {
            Line line = new Line();

            line.StartPoint = OldPoint;

            float fy = 0.0f;

            while (true)
            {
                fy = Random.Range(-5.0f, 5.0f);
                if (fy != 0.0f) break;
            }

            line.EndPoint = new Vector3(0.0f, 0.0f, 0.0f);

            OldPoint = new Vector3(
                OldPoint.x + Random.Range(1.0f, 5.0f),
                OldPoint.y + Random.Range(-5.0f, 5.0f),
                0.0f
                );

            /*
            line.EndPoint = new Vector3(
                OldPoint.x,
                OldPoint.y,
                0.0f);
            */
            line.EndPoint = OldPoint;

            LineList.Add(line);
        }
    }

    void Update()
    {
        float Hor = Input.GetAxis("Horizontal") * Time.deltaTime;

        foreach(Line element in LineList)
        {
            if (transform.position.x >= element.StartPoint.x && transform.position.x <= element.EndPoint.x)
            {
                float ratio = (element.EndPoint.y - element.StartPoint.y) / (element.EndPoint.x - element.StartPoint.x);
                transform.position = new Vector3(
                    transform.position.x,
                    element.StartPoint.y + ratio * (transform.position.x - element.StartPoint.x),
                    0.0f);
                Debug.DrawLine(element.StartPoint, element.EndPoint, Color.red);
            }
            else
                Debug.DrawLine(element.StartPoint, element.EndPoint, Color.green);
        }

        //if (transform.position.x == 0)
        //    transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        /*
        transform.position = new Vector3(
            transform.position.x,
            (Height / Width) * (transform.position.x - StartPoint.x),
            0.0f
            );
         */ 
    }
}
