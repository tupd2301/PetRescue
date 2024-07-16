using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SquareGrid))]
[CanEditMultipleObjects]
public class SquareLevelGridEditor : Editor
{
    SerializedProperty m_CellPalette;
    SerializedProperty m_bCanMoveDiagonal;

    private void OnEnable()
    {
        m_CellPalette = serializedObject.FindProperty("m_CellPalette");
        m_bCanMoveDiagonal = serializedObject.FindProperty("m_bCanMoveDiagonal");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(m_CellPalette, new GUIContent(m_CellPalette.displayName));
        EditorGUILayout.PropertyField(m_bCanMoveDiagonal, new GUIContent(m_bCanMoveDiagonal.displayName));

        serializedObject.ApplyModifiedProperties();
    }
}
