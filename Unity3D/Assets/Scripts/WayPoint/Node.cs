using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SphereCollider))]
public class Node : MonoBehaviour
{
    [HideInInspector] public Node Next;
}
