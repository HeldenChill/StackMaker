using UnityEngine;
using UnityEditor;
using StackMaker.Core;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Level script = (Level)target;
        if(GUILayout.Button("Normalize Position"))
        {
            script.NormalizeStackPosition();
        }
    }
}
