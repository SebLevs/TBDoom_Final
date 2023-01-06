using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent), true)]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Raise"))
            {
                ((GameEvent)target).Raise();
            }
        }
        EditorGUI.EndDisabledGroup();
    }
}
