using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(SphereCollider))]
public class Node //: MonoBehaviour
{
    //[HideInInspector] 
    public Vector3 Position;
    public Node Next;
    public float Cost;

    public Node()
    {
        Next = null;
        Cost = 0.0f;
        Position = Vector3.zero;
    }

    public Node(Vector3 _position)
    {
        Next = null;
        Cost = 0.0f;
        Position = _position;
    }

    public Node(Node _node, float _cost)
    {
        Next = _node;
        Cost = _cost;
        Position = Vector3.zero;
    }

    public Node(Node _node , float _cost, Vector3 _position)
    {
        Next = _node;
        Cost = _cost;
        Position = _position;
    }
}
