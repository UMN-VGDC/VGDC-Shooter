using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DistanceFromPlayerButton))]
public class DistanceFromPlayer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DistanceFromPlayerButton button = (DistanceFromPlayerButton)target;
        if (GUILayout.Button("Get Distance"))
        {
            button.GetPlayerDistance();
        }
    }
}
