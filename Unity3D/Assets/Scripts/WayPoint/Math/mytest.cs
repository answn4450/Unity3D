using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mytest : MonoBehaviour
{
    void Start()
    {
        gameObject.AddComponent<MyGizmo>();
        gameObject.GetComponent<MyGizmo>().color = Color.yellow;
    }
}
