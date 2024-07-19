using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityParticle : MonoBehaviour
{
    public float DeleteAfterTime;

    public virtual void Setup(UnitAbility InAbility, GridUnit InCaster, ILevelCell InTarget)
    {
        Destroy(gameObject, DeleteAfterTime);
    }
}
