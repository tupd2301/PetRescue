using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnEndedUIElement : MonoBehaviour
{
    public GameObject m_Screen;
    public RawImage m_Image;
    public Texture2D m_FriendlyTurn;
    public Texture2D m_HostileTurn;

    bool m_bShowing = false;
    Queue<GameTeam> TeamQueue = new Queue<GameTeam>();

    void Start()
    {
        GameManager.Get().OnTeamWon.AddListener(HandleGameDone);

        GameRules gameRules = GameManager.GetRules();
        if(gameRules)
        {
            gameRules.OnTeamTurnBegin.AddListener(HandleTeamChanged);
        }

        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 0.0f);
    }

    void HandleTeamChanged(GameTeam InTeam)
    {
        if(InTeam == GameTeam.Friendly || InTeam == GameTeam.Hostile)
        {
            if(m_bShowing)
            {
                TeamQueue.Enqueue(InTeam);
            }
            else
            {
                m_Image.texture = InTeam == GameTeam.Friendly ? m_FriendlyTurn : m_HostileTurn;
                m_bShowing = true;
                StartCoroutine(HandleShowGraphic());
            }
        }
    }

    IEnumerator HandleShowGraphic()
    {
        m_bShowing = true;

        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 1.0f);

        yield return new WaitForSeconds(0.5f);

        m_Image.color = new Color(m_Image.color.r, m_Image.color.g, m_Image.color.b, 0.0f);

        m_bShowing = false;

        if (TeamQueue.Count > 0)
        {
            HandleTeamChanged(TeamQueue.Dequeue());
        }
    }


    void HandleGameDone(GameTeam InWinningTeam)
    {
        m_Screen.SetActive(false);
    }
}
