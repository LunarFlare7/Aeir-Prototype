using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[CustomEditor(typeof(ArenaController)), CanEditMultipleObjects]
public class SpawnLocationEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        ArenaController ac = (ArenaController)target;

        for (int i = 0; i < ac.waves.Count; i++)
        {
            for (int j = 0; j < ac.waves[i].spawners.Count; j++)
            {
                for (int a = 0; a < ac.waves[i].spawners[j].spawnPositions.Count; a++)
                {
                    EditorGUI.BeginChangeCheck();
                    Vector3 newPosition = Handles.PositionHandle(ac.waves[i].spawners[j].spawnPositions[a], Quaternion.identity);
                    Handles.color = Color.HSVToRGB(i * 0.1f, 1, 1);
                    Handles.DrawWireDisc(newPosition, Vector3.forward, 1f);
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(ac, "Change spawn location");
                        ac.waves[i].spawners[j].spawnPositions[a] = newPosition;
                    }
                }
            }
        }
    }
}

