﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeamTurnChangeEvent : UnityEvent<GameTeam> { }

[System.Serializable]
public struct GameplayData
{
    // If this is true, then hit animations will play if you get damaged while moving.
	[SerializeField]
    public bool bShowHitAnimOnMove;
}

public class GameRules : ScriptableObject
{
    public GameTeam m_StartingTeam;

    [SerializeField]
    GameplayData m_GameplayData;

    GameTeam m_CurrentTeam;
    int m_TurnNumber = 0;

    public TeamTurnChangeEvent OnTeamTurnBegin = new TeamTurnChangeEvent();

    protected virtual void Init()
    {
        StartGame();
    }

    public GameTeam GetCurrentTeam()
    {
        return m_CurrentTeam;
    }

    public int GetTurnNumber()
    {
        return m_TurnNumber;
    }

    public void InitalizeRules()
    {
        m_TurnNumber = 0;
        m_CurrentTeam = GameTeam.All;
        Init();
    }

    public GameplayData GetGameplayData()
    {
        return m_GameplayData;
    }

    protected void StartGame()
    {
        m_CurrentTeam = m_StartingTeam;
        m_TurnNumber = 0;
        GameManager.HandleGameStarted();
        BeginTeamTurn(m_CurrentTeam);
    }

    public void EndTurn()
    {
        EndTeamTurn(m_CurrentTeam);
        
        if(m_CurrentTeam == GameTeam.Friendly)
        {
            m_CurrentTeam = GameTeam.Hostile;
            m_TurnNumber++;
        }
        else if (m_CurrentTeam == GameTeam.Hostile)
        {
            m_CurrentTeam = GameTeam.Friendly;
        }

        if (!GameManager.GetTeamList().Contains(m_CurrentTeam))
        {
            EndTurn();
        }
        else
        {
            OnTeamTurnBegin.Invoke(m_CurrentTeam);
            BeginTeamTurn(m_CurrentTeam);
        }
    }
    
    public virtual void Update()
    {
        
    }

    public virtual GridUnit GetSelectedUnit()
    {
        return null;
    }

    public virtual void BeginTeamTurn(GameTeam InTeam)
    {
        
    }

    public virtual void EndTeamTurn(GameTeam InTeam)
    {

    }

    public virtual void HandlePlayerSelected(GridUnit InPlayerUnit)
    {

    }

    public virtual void HandleEnemySelected(GridUnit InEnemyUnit)
    {

    }
    
    public virtual void HandleCellSelected(ILevelCell InCell)
    {

    }

    public virtual void HandleNumPressed(int InNumPressed)
    {
        
    }

    public virtual void HandleTeamWon(GameTeam InTeam)
    {
        
    }
}
