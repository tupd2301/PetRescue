using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCell2D : ILevelCell
{
    public override void SetMaterial(CellState InCellState)
    {
        Sprite cellSprite = GetStyleInfo().GetCellStateSprite(InCellState);

        SpriteRenderer spriteRenderer = GetRenderer<SpriteRenderer>();
        if(spriteRenderer)
        {
            spriteRenderer.sprite = cellSprite;
        }

        if (spriteRenderer == null)
        {
            Debug.Log("([TurnBasedTools]::LevelCell2D::SetMaterial) " + name + " doesn't have a SpriteRenderer.");
        }

        if (cellSprite == null)
        {
            Debug.Log("([TurnBasedTools]::LevelCell2D::SetMaterial) " + name + " doesn't have a sprite for: " + InCellState.ToString());
        }
    }

    CellStyleInfo2D GetStyleInfo()
    {
        CellStyleInfo2D cellStyleInfo = gameObject.GetComponent<CellStyleInfo2D>();
        if (cellStyleInfo == null)
        {
            cellStyleInfo = gameObject.AddComponent<CellStyleInfo2D>();
        }

        return cellStyleInfo;
    }
}
