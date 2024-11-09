using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        GridGenerator grid = (GridGenerator)target;
        if (GUILayout.Button("Generate Grid")) {
            grid.GenerateGrid();
        }
    }
}
