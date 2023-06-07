using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class WayPointEditor : Editor
{
    //SerializedObject Object = 
    
    //GUILayout

    private void OnSceneGUI()
    {
        if(GUILayout.Button("zzz"))
        {

        }

        WayPoint Target = (WayPoint)target;
        Debug.Log(Target.number);
    }
}
