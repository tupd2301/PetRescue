using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionListUIElement : MonoBehaviour
{
    public WinConditionCellUIElement[] m_WinConditionCells;

    void Start()
    {
        List<WinCondition> winConditions = GameManager.GetWinConditions();
        int numConditions = winConditions.Count;

        int totalCells = m_WinConditionCells.Length;
        for ( int i = 0; i < totalCells; i++ )
        {
            WinConditionCellUIElement currCell = m_WinConditionCells[ i ];
            if( currCell )
            {
                if( i < numConditions )
                {
                    currCell.gameObject.SetActive( true );

                    WinCondition currWinCondition = winConditions[ i ];
                    if( currCell && currWinCondition )
                    {
                        currCell.SetWinCondition( currWinCondition );
                    }
                }
                else
                {
                    currCell.gameObject.SetActive( false );
                }
            }
        }
    }

    void Update()
    {
        foreach (WinConditionCellUIElement currWinCondition in m_WinConditionCells)
        {
            if(currWinCondition)
            {
                currWinCondition.UpdateConditionCell();
            }
        }
    }
}
