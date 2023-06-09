using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterController : MonoBehaviour
{
    public List<GameObject> PointList;

    [Range(-90.0f, 90.0f)]
    public float Angle;

    public float Speed;

    // 경사면을 타고 올라감
    Vector2 slope_p1 = new Vector2(0.0f, 0.0f);
    Vector2 slope_p2 = new Vector2(13.0f, 9.0f);
    float slope_ratio;
    //float slope_y = 0.0f;

    // 경사면을 고려한 점프
    bool isJumping = false;
    float flightTime = 0.0f;
    
    void Start()
    {
        gameObject.AddComponent<MyGizmo>();

        Speed = 5.0f;

        slope_ratio = (slope_p2.y - slope_p1.y) / (slope_p2.x - slope_p1.x);

        for (int x = (int)slope_p1.x; x <slope_p2.x;++x)
        {
            GameObject point = new GameObject();
            point.AddComponent<MyGizmo>();
            point.GetComponent<MyGizmo>().color = Color.red;
            point.transform.position = new Vector3(
                x,
                slope_ratio * x,
                0.0f
                );
        }
    }

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        Vector3 Movement = new Vector3(hor, 0.0f, 0.0f) * 5.0f * Time.deltaTime;
        transform.position += Movement;

        if (Input.GetKey(KeyCode.Space) && !isJumping)
            isJumping = true;

        if (isJumping)
            SlopeJump();
        else
            SwimSlope();
    }

    private void Follow()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 Movement = new Vector3(hor, 0.0f, ver) * 5.0f * Time.deltaTime;
        transform.Translate(Movement);
        //transform.position = new Vector3(
        //  0.0f, Mathf.Sin(Angle * Mathf.Deg2Rad), 0.0f) * Speed;
    }

    private void SwimSlope()
    {
        if (transform.position.x >= slope_p1.x && transform.position.x <= slope_p2.x)
        {
            transform.position = new Vector3(
                transform.position.x,
                slope_ratio * (transform.position.x - slope_p1.x),
                0.0f
                );
        }
        else if (transform.position.x <= slope_p1.x)
        {
            transform.position = new Vector3(
                    transform.position.x,
                    slope_p1.y,
                    0.0f);
        }
        else if (transform.position.x >= slope_p2.x)
        {
            transform.position = new Vector3(
                    transform.position.x,
                    slope_p2.y,
                    0.0f);
        }
    }

    
    private void SlopeJump()
    {
        if (isJumping)
        {
            // ** 누르는 힘
            flightTime += Time.deltaTime*3;
            float result = (flightTime * flightTime * 0.98f);
            float jumpHeight = 1.0f;
            // ** 점프
            transform.position += new Vector3(
                0.0f,
                (jumpHeight - result)*Time.deltaTime,
                0.0f
                );

            // ** 최초의 점프 위치를 벗어나면 최초위치로 셋팅.
            //if (oldY < transform.position.y)
            if ((transform.position.y <= slope_p1.y && transform.position.x <= slope_p1.x) ||
                (transform.position.y <= slope_p2.y && transform.position.x >= slope_p2.x) ||
                (transform.position.y <= slope_ratio * (transform.position.x - slope_p1.x))
                )
            {
                flightTime = 0.0f;
                isJumping = false;
                SwimSlope();
            }
        }
    }
}
