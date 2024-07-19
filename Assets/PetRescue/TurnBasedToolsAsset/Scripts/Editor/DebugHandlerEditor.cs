using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugHandler))]
public class DebugHandlerEditor : Editor
{
    bool m_bKillUnitFoldout = false;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ResetFriendlyTeam();

        m_bKillUnitFoldout = EditorGUILayout.Foldout(m_bKillUnitFoldout, "Kill Unit");
        if(m_bKillUnitFoldout)
        {
            DrawKillUnit();
        }
    }

    void ResetFriendlyTeam()
    {
        GUIStyle ButtonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter,
            stretchWidth = false,
            fontSize = 10,
        };

        if (GUILayout.Button("Reset Friendly Team", ButtonStyle))
        {
            if (Application.isPlaying)
            {
                List<GridUnit> units = GameManager.GetUnitsOnTeam(GameTeam.Friendly);
                foreach (GridUnit currUnit in units)
                {
                    if (currUnit)
                    {
                        currUnit.HandleTurnStarted();
                    }
                }
            }
        }
    }

    #region KillUnit

    GameTeam m_KillUnitTeam = GameTeam.Friendly;
    int m_UnitIndex = 0;
    void DrawKillUnit()
    {
        GUIStyle BorderStyle = new GUIStyle(EditorStyles.helpBox)
        {
            margin = new RectOffset(5, 5, 5, 5)
        };

        GUIStyle ButtonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter,
            stretchWidth = false,
            fontSize = 20,
        };
        ButtonStyle.normal.textColor = Color.white;

        GUILayout.BeginVertical(BorderStyle);

        m_KillUnitTeam = (GameTeam)EditorGUILayout.EnumPopup("Team: ", m_KillUnitTeam);
        m_UnitIndex = EditorGUILayout.IntField("Index: ", m_UnitIndex);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GUI.backgroundColor = Color.red;
        ButtonStyle.normal.textColor = Color.white;
        if (GUILayout.Button("Kill", ButtonStyle, GUILayout.Width(50)))
        {
            if (Application.isPlaying)
            {
                List<GridUnit> unitsOnTeam = GameManager.GetUnitsOnTeam(m_KillUnitTeam);
                if (unitsOnTeam.Count > m_UnitIndex)
                {
                    GridUnit gridUnit = unitsOnTeam[m_UnitIndex];
                    if (gridUnit)
                    {
                        if (!gridUnit.IsDead())
                        {
                            gridUnit.Kill();
                        }
                    }
                }
            }
        }
        GUI.backgroundColor = Color.white;

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    #endregion

}
