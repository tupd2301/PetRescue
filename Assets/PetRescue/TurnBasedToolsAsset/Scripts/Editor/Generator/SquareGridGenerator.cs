using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SquareGridGenerator : IGridGenerator<SquareGrid>
{
    [MenuItem("TurnBasedTools/SquareGrid Generator")]
    static void Init()
    {
        SquareGridGenerator window = (SquareGridGenerator)EditorWindow.GetWindow(typeof(SquareGridGenerator));
        window.Show();
    }

    protected override void OnGUI()
    {
        base.OnGUI();
    }

    protected override void DrawCells(SquareGrid levelGrid, Vector3 CellBounds)
    {
        for (int y = 0; y < m_GridSize.y; y++)
        {
            float finalY = y * CellBounds.z;
            for (int x = 0; x < m_GridSize.x; x++)
            {
                float finalX = x * CellBounds.x;
                levelGrid.GenerateCell(new Vector3(finalX, 0.0f, -finalY), new Vector2(x, y));
            }
        }
    }
}
