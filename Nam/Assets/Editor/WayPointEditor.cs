using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WayPoint))]
public class WayPointEditor : Editor
{
    private void OnSceneGUI()
    {
        //SerializedObject Object = 
        //GUILayout.

        
        WayPoint Target = (WayPoint)target;
        Debug.Log(Target.number);
    }
}
