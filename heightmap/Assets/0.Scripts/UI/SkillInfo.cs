using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    public float coolDown;

    private void Start()
    {
        coolDown = 1.0f;
    }
}
