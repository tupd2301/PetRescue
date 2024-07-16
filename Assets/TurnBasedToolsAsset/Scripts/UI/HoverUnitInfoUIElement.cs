using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverUnitInfoUIElement : MonoBehaviour
{
    public GameObject m_ScreenObject;

    [Space(5)]

    public Text m_UnitName;

    [Space(5)]

    public Text m_HealthText;
    public Text m_ArmorText;
    public Text m_MagicArmorText;
    public Text m_APText;

    [Space(5)]

    public Slider m_HealthSlider;
    public Slider m_ArmorSlider;
    public Slider m_MagicArmorSlider;
    public Slider m_APSlider;

    GridUnit m_CurrUnit = null;

    bool bEnabled = true;

    private void Awake()
    {
        SetupScreen();
    }

    void Start()
    {
        GameManager.Get().OnUnitHover.AddListener(HandleUnitHover);
        GameManager.Get().OnTeamWon.AddListener(HandleGameDone);
    }

    void Update()
    {
        SetupScreen();
    }

    void HandleUnitHover(GridUnit InUnit)
    {
        if ( m_CurrUnit )
        {
            Health hpComp = m_CurrUnit.GetComponent<Health>();
            if ( hpComp )
            {
                hpComp.OnHealthDepleted.RemoveListener( HandleUnitDeath );
            }
        }

        m_CurrUnit = InUnit;

        if ( m_CurrUnit )
        {
            Health hpComp = m_CurrUnit.GetComponent<Health>();
            if ( hpComp )
            {
                hpComp.OnHealthDepleted.AddListener( HandleUnitDeath );
            }
        }

        SetupScreen();
    }

    void HandleUnitDeath()
    {
        HandleUnitHover( null );
    }

    void SetupScreen()
    {
        if(m_CurrUnit && bEnabled)
        {
            m_ScreenObject.SetActive(true);

            UnitData unitData = m_CurrUnit.GetUnitData();

            int CurrAbilityPoints = m_CurrUnit.GetCurrentAbilityPoints();
            int TotalAbilityPoints = unitData.m_AbilityPoints;

            m_UnitName.text = unitData.m_UnitName;
            m_APText.text = CurrAbilityPoints.ToString();

            float APPercentage = ( (float)CurrAbilityPoints ) / ( (float)TotalAbilityPoints );
            m_APSlider.value = APPercentage;

            Health healthComp = m_CurrUnit.GetComponent<Health>();
            if(healthComp)
            {
                m_HealthText.text = healthComp.GetHealth().ToString();
                m_ArmorText.text = healthComp.GetArmor().ToString();
                m_MagicArmorText.text = healthComp.GetMagicArmor().ToString();

                m_HealthSlider.value = healthComp.GetHealthPercentage();
                m_ArmorSlider.value = healthComp.GetArmorPercentage();
                m_MagicArmorSlider.value = healthComp.GetMagicArmorPercentage();
            }
        }
        else
        {
            m_ScreenObject.SetActive(false);
        }
    }


    void HandleGameDone(GameTeam InWinningTeam)
    {
        m_ScreenObject.SetActive(false);
        bEnabled = false;
    }
}
