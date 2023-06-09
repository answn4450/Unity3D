using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    public Color color = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
