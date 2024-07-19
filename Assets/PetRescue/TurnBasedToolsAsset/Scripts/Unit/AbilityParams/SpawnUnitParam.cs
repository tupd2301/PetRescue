using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnUnitParam", menuName = "TurnBasedTools/Ability/Parameters/ Create SpawnUnitParam", order = 1)]
public class SpawnUnitParam : AbilityParam
{
    public UnitData m_UnitToSpawn;

    public override void ApplyTo(GridUnit InCaster, ILevelCell InCell)
    {
        GridUnit SpawnedUnit = GameManager.SpawnUnit(m_UnitToSpawn, InCaster.GetTeam(), InCell.GetIndex());
        SpawnedUnit.HandleTurnStarted();
    }

    public override string GetAbilityInfo()
    {
        return "Spawn: " + m_UnitToSpawn.m_UnitName;
    }
}
