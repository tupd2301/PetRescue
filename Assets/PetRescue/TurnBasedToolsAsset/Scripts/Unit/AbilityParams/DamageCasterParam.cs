using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageCasterAbilityParam", menuName = "TurnBasedTools/Ability/Parameters/ Create DamageCasterAbilityParam", order = 1)]
public class DamageCasterParam : AbilityParam
{
    public int m_Damage;
    public bool m_bMagicalDamage;

    public override void ApplyTo(GridUnit InCaster, ILevelCell InObject)
    {
        Health healthComp = InCaster.GetComponent<Health>();
        if (healthComp)
        {
            if (m_bMagicalDamage)
            {
                healthComp.MagicDamage(m_Damage);
            }
            else
            {
                healthComp.Damage(m_Damage);
            }
        }
    }

    public override string GetAbilityInfo()
    {
        return "Damage Self " + (m_bMagicalDamage ? "(Magical)" : "") + " " + m_Damage.ToString();
    }
}
