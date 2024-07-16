using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AIObjectSpawnInfo
{
    public string m_SpawnAtCellId;
    public UnitData m_UnitData;
    public UnitAI m_AssociatedAI;
    public CompassDir m_StartDirection;
    public bool m_bIsATarget;
}

[CreateAssetMenu(fileName = "NewAITeamData", menuName = "TurnBasedTools/PlayerTeamData/Create AITeamData", order = 1)]
public class AITeamData : TeamData
{
    public List<AIObjectSpawnInfo> m_AISpawnUnits;
}
