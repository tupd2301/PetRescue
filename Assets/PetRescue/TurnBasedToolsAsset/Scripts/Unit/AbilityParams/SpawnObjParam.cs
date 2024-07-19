﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnObjectParam", menuName = "TurnBasedTools/Ability/Parameters/ Create SpawnObjectAbilityParam", order = 1)]
public class SpawnObjParam : AbilityParam
{
    public GameObject m_Object;
    public Vector3 m_Offset;

    public override void ApplyTo(GridUnit InCaster, ILevelCell InCell)
    {
        if(!InCell.IsObjectOnCell())
        {
            GameManager.SpawnObjectOnCell(m_Object, InCell, m_Offset);
        }
    }

    public override string GetAbilityInfo()
    {
        return "Spawn: " + m_Object.name;
    }
}
