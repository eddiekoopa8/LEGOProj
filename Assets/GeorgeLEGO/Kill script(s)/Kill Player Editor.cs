using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(KillPlayer))]
public class KillPlayerEditor : Editor {
    private void OnEnable() {
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        WhatToDoWhen();
        KillPlayerButton();
        serializedObject.ApplyModifiedProperties();
    }
    void WhatToDoWhen() {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onPlayerDeath"));
        switch (((KillPlayer)target).onPlayerDeath) {
            case KillPlayer.OnPlayerDeath.changeScene:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("sceneToChangeTo"));
                break;
            
        }
    }
    private void KillPlayerButton() {
        if (Application.isPlaying)
            serializedObject.FindProperty("killPlayer").boolValue = GUILayout.Button("Kill the player immediately");
    }
}
