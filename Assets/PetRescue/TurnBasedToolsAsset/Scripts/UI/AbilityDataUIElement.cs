using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AbilityConnections
{
    public RawImage m_AbilityImage;

    [Header("Range")]
    public RawImage m_RangeImage;
    public Text m_RangeNumText;

    [Header("Action Points")]
    public RawImage m_APNumImage;
    public Text m_APNumText;

    [Header("Ability Pop-up")]
    public GameObject m_AbilityPopup;
    public Text m_AbilityNameText;
    public Text m_AbilityInfoText;
}

public class AbilityDataUIElement : MonoBehaviour
{
    public UnitAbility m_Ability;
    int m_Index;
    public AbilityConnections m_Connections;

    AbilityListUIElement m_Owner;

    void Start()
    {
        SetPopupVisibility(false);
        SetupUIElement();

        GameManager.Get().OnTeamWon.AddListener(HandleGameDone);
    }

    public void SetOwner(AbilityListUIElement InListUIElem)
    {
        m_Owner = InListUIElem;
    }

    public void SetAbility(UnitAbility InAbility, int InIndex)
    {
        m_Ability = InAbility;
        m_Index = InIndex;
        SetupUIElement();
    }

    public void ClearAbility()
    {
        m_Ability = null;

        m_Connections.m_AbilityImage.texture = m_Owner.BlankTexture;

        m_Connections.m_RangeNumText.text = "";
        m_Connections.m_APNumText.text = "";

        m_Connections.m_AbilityNameText.text = "";
        m_Connections.m_AbilityInfoText.text = "";
    }

    void SetupUIElement()
    {
        if(m_Ability)
        {
            m_Connections.m_AbilityImage.texture = m_Ability.GetIcon();

            m_Connections.m_RangeNumText.text = m_Ability.GetRadius().ToString();
            m_Connections.m_APNumText.text = m_Ability.GetActionPointCost().ToString();

            m_Connections.m_AbilityNameText.text = m_Ability.GetAbilityName();
            m_Connections.m_AbilityInfoText.text = GenerateAbilityInfo();
        }
    }

    string GenerateAbilityInfo()
    {
        if(!m_Ability)
        {
            return "";
        }

        string abilityInfo = "";

        List<AbilityParam> AbilityParams = m_Ability.GetParameters();
        foreach ( AbilityParam param in AbilityParams )
        {
            if( param )
            {
                abilityInfo += param.GetAbilityInfo() + "\n";
            }
        }

        List<Ailment> AbilityAilments = m_Ability.GetAilments();
        foreach ( Ailment ailment in AbilityAilments )
        {
            if ( ailment )
            {
                abilityInfo += "-" + ailment.m_Description + "\n";
            }
        }

        return abilityInfo;
    }

    void SetPopupVisibility(bool bInVisible)
    {
        if(bInVisible)
        {
            if(!m_Ability)
            {
                return;
            }
        }

        m_Connections.m_AbilityPopup.SetActive(bInVisible);
        if(bInVisible)
        {
            SetupUIElement();
        }
    }

    void Update()
    {

    }

    public void OnClicked()
    {
        if(m_Ability)
        {
            if(m_Owner)
            {
                m_Owner.HandleAbilitySelected(m_Index);
            }
        }
    }

    public void OnHover(bool bStart)
    {
        SetPopupVisibility(bStart);
    }

    void HandleGameDone(GameTeam InWinningTeam)
    {
        gameObject.SetActive(false);
    }
}
