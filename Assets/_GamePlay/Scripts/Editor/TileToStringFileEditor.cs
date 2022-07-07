using UnityEngine;
using UnityEditor;
using StackMaker.Core;

[CustomEditor(typeof(TileToStringFile))]
public class TileToStringFileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileToStringFile script = (TileToStringFile)target;
        if(GUILayout.Button("Save Map Data"))
        {
            script.WriteAFile();
        }
    }
}
