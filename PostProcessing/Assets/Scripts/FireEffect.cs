using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireEffect : MonoBehaviour
{
    private Light pointLight;
    private bool pointLightCheck;
    private float pointLightTime;

    void Start()
    {
        pointLight = GetComponent<Light>();

        pointLight.range = 13.0f;

        pointLightCheck = false;

        pointLightTime = 0.15f;
    }

    void Update()
    {
        if(pointLightCheck)
        {
            pointLight.range = 14.0f;

            pointLightTime -= Time.deltaTime;
            if(pointLightTime < 0.0f)
            {
                pointLightTime = 0.1f;
                pointLightCheck = false;
            }   
        }
        else 
        {
            pointLight.range = 11.5f;

            pointLightTime -= Time.deltaTime;
            if (pointLightTime < 0.0f)
            {
                pointLightTime = 0.1f;
                pointLightCheck = true;
            }
        }
    }
}
