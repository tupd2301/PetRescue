﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealAbilityParam", menuName = "TurnBasedTools/Ability/Parameters/ Create HealAbilityParam", order = 1)]
public class HealParam : AbilityParam
{
    public int m_HealAmount;

    public override void ApplyTo(GridUnit InCaster, GridObject InObject)
    {
        if(InObject)
        {
            Health health = InObject.GetComponent<Health>();
            if(health)
            {
                health.Heal(m_HealAmount);
            }
        }
    }

    public override string GetAbilityInfo()
    {
        return "Heal Target by: " + m_HealAmount;
    }
}
