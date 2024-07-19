using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageAbilityParam", menuName = "TurnBasedTools/Ability/Parameters/ Create DamageAbilityParam", order = 1)]
public class DamageAbilityParam : AbilityParam
{
    public int m_Damage;
    public bool m_bMagicalDamage;

    public override void ApplyTo(GridUnit InCaster, GridObject InObject)
    {
        Health healthComp = InObject.GetComponent<Health>();
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
        return "Damage" + (m_bMagicalDamage ? "(Magical)" : "") + " " + m_Damage.ToString();
    }
}
