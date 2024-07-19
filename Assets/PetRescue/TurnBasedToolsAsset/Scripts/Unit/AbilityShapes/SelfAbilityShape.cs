using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSelfAbilityShape", menuName = "TurnBasedTools/Ability/Shapes/Create SelfAbilityShape", order = 1)]
public class SelfAbilityShape : AbilityShape
{
    public override List<ILevelCell> GetCellList(GridUnit InCaster, ILevelCell InCell, int InRange, bool bAllowBlocked = true, GameTeam m_EffectedTeam = GameTeam.None)
    {
        return new List<ILevelCell> { InCaster.GetCell() };
    }
}
