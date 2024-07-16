using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityShape", menuName = "TurnBasedTools/Ability/Shapes/Create AllTeammatesAbilityShape", order = 1)]
public class AllTeammatesAbilityShape : AbilityShape
{
    public override List<ILevelCell> GetCellList(GridUnit InCaster, ILevelCell InCell, int InRange, bool bAllowBlocked, GameTeam m_EffectedTeam)
    {
        List<GridUnit> TeamUnits = GameManager.GetUnitsOnTeam( InCaster.GetTeam() );

        List<ILevelCell> cells = new List<ILevelCell>();
        foreach ( var unit in TeamUnits )
        {
            if ( unit )
            {
                ILevelCell unitCell = unit.GetCell();
                if ( unitCell )
                {
                    cells.Add( unitCell );
                }
            }
        }

        return cells;
    }
}
