using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityListUIElement : MonoBehaviour
{
    GridUnit m_SelectedUnit;

    public Texture2D BlankTexture;
    public GameObject AbilityList;

    public List<AbilityDataUIElement> m_UIAbilities;

    void Start()
    {
        ClearAbilityList();
        GameManager.Get().OnUnitSelected.AddListener(HandleUnitSelected);
        AbilityList.SetActive(false);
    }

    public void HandleUnitSelected(GridUnit InUnit)
    {
        if(InUnit)
        {
            AbilityList.SetActive(true);
            m_SelectedUnit = InUnit;
            SetupAbilityList();
        }
        else
        {
            ClearAbilityList();
            m_SelectedUnit = null;
            AbilityList.SetActive(false);
        }
    }

    void SetupAbilityList()
    {
        if(m_SelectedUnit)
        {
            int currIndex = 0;
            foreach (UnitAbilityPlayerData abilityData in m_SelectedUnit.GetAbilities())
            {
                if(abilityData.unitAbility)
                {
                    m_UIAbilities[currIndex].SetOwner(this);
                    m_UIAbilities[currIndex].SetAbility(abilityData.unitAbility, currIndex);
                }
                currIndex++;
            }
        }
    }

    void ClearAbilityList()
    {
        foreach (AbilityDataUIElement abilityUI in m_UIAbilities)
        {
            if (abilityUI)
            {
                abilityUI.SetOwner(this);
                abilityUI.ClearAbility();
            }
        }
    }

    public void HandleAbilitySelected(int InIndex)
    {
        if(m_SelectedUnit)
        {
            m_SelectedUnit.SetupAbility(InIndex);
        }
    }
}
