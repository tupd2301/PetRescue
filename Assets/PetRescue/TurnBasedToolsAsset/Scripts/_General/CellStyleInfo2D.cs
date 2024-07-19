using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellStyleInfo2D : MonoBehaviour
{
    [SerializeField]
    Sprite m_HoverSprite;

    [SerializeField]
    Sprite m_PositiveSprite;

    [SerializeField]
    Sprite m_NegativeSprite;

    [SerializeField]
    Sprite m_MovementSprite;

    Sprite m_NormalSprite;

    void Awake()
    {
        m_NormalSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public Sprite GetCellStateSprite(CellState InCellState)
    {
        switch (InCellState)
        {
            case CellState.eNormal:
                return m_NormalSprite;
            case CellState.eHover:
                return m_HoverSprite;
            case CellState.ePositive:
                return m_PositiveSprite;
            case CellState.eNegative:
                return m_NegativeSprite;
            case CellState.eMovement:
                return m_MovementSprite;
        }

        return m_NormalSprite;
    }
}
