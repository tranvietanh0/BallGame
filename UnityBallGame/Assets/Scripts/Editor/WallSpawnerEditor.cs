using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WallSpawner))]
public class WallSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WallSpawner spawner = (WallSpawner)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Spawn Walls", GUILayout.Height(40)))
        {
            spawner.SpawnWallsManually();
        }

        if (GUILayout.Button("Clear Walls", GUILayout.Height(30)))
        {
            spawner.ClearWalls();
        }
    }
}