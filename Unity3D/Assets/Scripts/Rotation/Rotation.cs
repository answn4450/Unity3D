using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{

    [Range(0.0f, 1.0f)]
    public float dis;

    private Vector3 temp;
    private Vector3 dest;

    private int check;
    private bool move;

    void Start()
    {
        dis = 1.0f;

        temp = new Vector3(0.0f, 0.0f, 0.0f);
        dest = new Vector3(100.0f, 0.0f, 0.0f);

        check = 0;
    }

    void Update()
    {
        transform.Rotate(0.0f, Input.GetAxis("Horizontal") * Time.deltaTime * 50.0f, 0.0f, Space.Self);

        if (Input.GetMouseButtonDown(0))
        {
            function();
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
            if (check == 0)
            {
                transform.position = Vector3.Lerp(
                    temp,
                    dest,
                    time
                    );
            }
            else
            {
                transform.position = Vector3.Lerp(
                    dest,
                    temp,
                    time
                    );
            }

            time += Time.deltaTime;

            yield return null;

        }
        move = false;
    }
}