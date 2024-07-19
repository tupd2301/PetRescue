using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinConditionCellUIElement : MonoBehaviour
{
    public Text m_WinConditionName;
    public Text m_DescriptionText;
    public Text m_ConditionStateText;

    [Space(10)]

    public RawImage m_Icon;

    WinCondition m_CurrentWinCondition;

    private void Start()
    {
        GameManager.Get().OnTeamWon.AddListener(HandleGameDone);
    }

    private void Update()
    {
        
    }

    public void SetWinCondition(WinCondition InWinCondition)
    {
        m_CurrentWinCondition = InWinCondition;
        UpdateConditionCell();
    }

    public void UpdateConditionCell()
    {
        if(m_CurrentWinCondition)
        {
            m_WinConditionName.text = m_CurrentWinCondition.m_ConditionName;
            m_DescriptionText.text = m_CurrentWinCondition.m_Description;
            m_ConditionStateText.text = m_CurrentWinCondition.GetConditionStateString();

            m_Icon.texture = m_CurrentWinCondition.m_Icon;
        }
    }


    void HandleGameDone(GameTeam InWinningTeam)
    {
        gameObject.SetActive(false);
    }
}
