﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    GridUnit m_AssocaitedUnit = null;
    
    void Update()
    {
        if( m_AssocaitedUnit == null )
        {
            m_AssocaitedUnit = GetComponentInParent<GridUnit>();
            if( m_AssocaitedUnit )
            {
                bool bIsTarget = GameManager.GetTeamTargets(m_AssocaitedUnit.GetTeam()).Contains(m_AssocaitedUnit);
                if(bIsTarget)
                {
                    transform.localPosition = new Vector3(0, m_AssocaitedUnit.GetBounds().y, 0);
                    GetComponentInChildren<Renderer>().enabled = m_AssocaitedUnit.IsVisible();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
