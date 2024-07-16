﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexagonGrid))]
[CanEditMultipleObjects]
public class HexagonLevelGridEditor : Editor
{
    SerializedProperty m_CellPalette;

    private void OnEnable()
    {
        m_CellPalette = serializedObject.FindProperty("m_CellPalette");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(m_CellPalette, new GUIContent(m_CellPalette.displayName));

        serializedObject.ApplyModifiedProperties();
    }
}
