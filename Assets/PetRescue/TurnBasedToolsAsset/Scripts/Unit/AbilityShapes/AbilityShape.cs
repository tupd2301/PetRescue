using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityShape : ScriptableObject
{
    public virtual List<ILevelCell> GetCellList(GridUnit InCaster, ILevelCell InCell, int InRange, bool bAllowBlocked = true, GameTeam m_EffectedTeam = GameTeam.None)
    {
        return new List<ILevelCell>();
    }
}
