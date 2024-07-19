using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityParam : ScriptableObject
{
    public virtual void ApplyTo(GridUnit InCaster, GridObject InObject)
    {
        
    }
    
    public virtual void ApplyTo(GridUnit InCaster, ILevelCell InCell)
    {

    }

    public virtual string GetAbilityInfo()
    {
        return name;
    }
}
