﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPushParam", menuName = "TurnBasedTools/Ability/Parameters/ Create PushAbilityParam", order = 1)]
public class PushAbilityParam : AbilityParam
{
    public int m_Distance;

    public override void ApplyTo(GridUnit InCaster, ILevelCell InCell)
    {
        GridUnit TargetUnit = InCell.GetUnitOnCell();

        if (TargetUnit && InCaster)
        {
            CompassDir PushDirection = InCaster.GetCell().GetDirectionToAdjacentCell( InCell );

            ILevelCell targetCell = TargetUnit.GetCell();
            for (int i = 0; i < m_Distance; i++)
            {
                ILevelCell dirCell = targetCell.GetAdjacentCell(PushDirection);
                if(dirCell && dirCell.IsCellAccesible())
                {
                    targetCell = dirCell;
                }
            }

            TargetUnit.MoveTo(targetCell);
        }
    }

    public override string GetAbilityInfo()
    {
        return "Push Back: " + m_Distance.ToString() + " Space" + ((m_Distance > 1) ? "s" : "");
    }
}
