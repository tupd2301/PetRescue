using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnUIElement : MonoBehaviour
{
    public Button m_Button;

    private void Start()
    {
        GameManager.Get().OnTeamWon.AddListener(HandleGameDone);
    }

    void Update()
    {
        m_Button.interactable = GameManager.CanFinishTurn();
    }
    
    public void EndTurn()
    {
        GameManager.FinishTurn();
    }

    void HandleGameDone(GameTeam InWinningTeam)
    {
        gameObject.SetActive(false);
    }
}
