using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
[CanEditMultipleObjects]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        List<PropertyReplaceInfo> ReplaceInfoList = new List<PropertyReplaceInfo>()
        {
            new PropertyReplaceInfo("m_WinConditions", EditorUtils.MakeCustomArrayWidget),
            new PropertyReplaceInfo("m_SpawnOnStart", EditorUtils.MakeCustomArrayWidget),
            new PropertyReplaceInfo("m_AddToSpawnedUnits", EditorUtils.MakeCustomArrayWidget),
            new PropertyReplaceInfo("m_DeathParticles", EditorUtils.MakeCustomArrayWidget),
        };

        EditorUtils.DrawAllProperties(serializedObject, ReplaceInfoList);

        serializedObject.ApplyModifiedProperties();
    }
}
