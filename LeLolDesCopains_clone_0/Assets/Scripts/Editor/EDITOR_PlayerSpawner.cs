using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerSpawner))]
public class EDITOR_PlayerSpawner : Editor
{
    private PlayerSpawner targetScript;

    private void OnEnable()
    {
        targetScript = (PlayerSpawner)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Populate SpawnPoints array"))
            targetScript.PopulateSpawnPoints();

    }
}
