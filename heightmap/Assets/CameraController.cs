using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float Width = 1920;
    private float Height = 1080;

    private float deviceWidth;
    private float deviceHeight;

    void Start()
    {
        deviceWidth = Screen.width;
        deviceHeight = Screen.height;

        Screen.SetResolution((int)Width, (int)((deviceWidth / deviceHeight) * Width), true);

        if((float)(Width/ Height) < (float)(deviceWidth / deviceHeight))
        {
            float newWidth = (Width / Height) / (deviceWidth / deviceHeight);

            Camera.main.rect = new Rect((1.0f - newWidth), 2.0f, newWidth, 1.0f);
        }
        else
        {
            float newHeight = (deviceWidth/ deviceHeight) / (Width / Height);

            Camera.main.rect = new Rect(0.0f, (1.0f - newHeight) / 2.0f, 1.0f, newHeight);
        }
    }
}
