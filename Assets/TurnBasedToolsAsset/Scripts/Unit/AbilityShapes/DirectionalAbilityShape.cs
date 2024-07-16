﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityShape", menuName = "TurnBasedTools/Ability/Shapes/Create DirectionalAbilityShape", order = 1)]
public class DirectionalAbilityShape : AbilityShape
{
    [SerializeField]
    bool m_bOnlyMyEnemies;

    public override List<ILevelCell> GetCellList(GridUnit InCaster, ILevelCell InCell, int InRange, bool bAllowBlocked, GameTeam m_EffectedTeam)
    {
        List<ILevelCell> cells = new List<ILevelCell>();

        if(InCell && InRange > 0)
        {
            cells.AddRange(GetCellsInDirection(InCell, InRange, CompassDir.N, bAllowBlocked, m_EffectedTeam));
            cells.AddRange(GetCellsInDirection(InCell, InRange, CompassDir.E, bAllowBlocked, m_EffectedTeam));
            cells.AddRange(GetCellsInDirection(InCell, InRange, CompassDir.NE, bAllowBlocked, m_EffectedTeam));
            cells.AddRange(GetCellsInDirection(InCell, InRange, CompassDir.NW, bAllowBlocked, m_EffectedTeam));
            cells.AddRange(GetCellsInDirection(InCell, InRange, CompassDir.SE, bAllowBlocked, m_EffectedTeam));
            cells.AddRange(GetCellsInDirection(InCell, InRange, CompassDir.S, bAllowBlocked, m_EffectedTeam));
            cells.AddRange(GetCellsInDirection(InCell, InRange, CompassDir.SW, bAllowBlocked, m_EffectedTeam));
            cells.AddRange(GetCellsInDirection(InCell, InRange, CompassDir.W, bAllowBlocked, m_EffectedTeam));
        }

        if ( m_bOnlyMyEnemies )
        {
            List<ILevelCell> enemyCells = new List<ILevelCell>();
            foreach ( var currCell in cells )
            {
                GridUnit unitOnCell = currCell.GetUnitOnCell();
                if ( unitOnCell )
                {
                    GameTeam AffinityToCaster = GameManager.GetTeamAffinity( InCaster.GetTeam(), unitOnCell.GetTeam() );
                    if ( AffinityToCaster == GameTeam.Hostile )
                    {
                        enemyCells.Add( currCell );
                    }
                }
            }

            return enemyCells;
        }
        else
        {
            return cells;
        }
    }

    List<ILevelCell> GetCellsInDirection(ILevelCell StartCell, int InRange, CompassDir Dir, bool bAllowBlocked, GameTeam m_EffectedTeam)
    {
        List<ILevelCell> cells = new List<ILevelCell>();

        if(InRange > 0)
        {
            ILevelCell CurserCell = StartCell.GetAdjacentCell(Dir);

            int Count = 0;
            while (CurserCell)
            {
                if(CurserCell.IsBlocked() && !bAllowBlocked)
                {
                    break;
                }

                GridObject gridObj = CurserCell.GetObjectOnCell();
                if (gridObj)
                {
                    if (m_EffectedTeam == GameTeam.None)
                    {
                        break;
                    }

                    GameTeam ObjAffinity = GameManager.GetTeamAffinity(gridObj.GetTeam(), StartCell.GetCellTeam());
                    if (ObjAffinity == GameTeam.Friendly && m_EffectedTeam == GameTeam.Hostile)
                    {
                        break;
                    }
                }

                cells.Add(CurserCell);
                CurserCell = CurserCell.GetAdjacentCell(Dir);
                Count++;
                if(InRange != -1 && Count >= InRange)
                {
                    break;
                }
            }
        }

        return cells;
    }
}
