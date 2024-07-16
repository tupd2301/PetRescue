using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUIElement : MonoBehaviour
{
    public GameObject m_EndScreen;

    public RawImage m_EndTeamImage;

    public Texture2D m_FriendlyWinTexture;
    public Texture2D m_HostileWinTexture;

    void Start()
    {
        m_EndScreen.SetActive(false);
        GameManager.Get().OnTeamWon.AddListener(HandleTeamWin);
    }
    
    void HandleTeamWin(GameTeam InTeam)
    {
        ShowEndScreen(InTeam == GameTeam.Friendly);
    }

    void ShowEndScreen(bool bIsFriendlyWin)
    {
        m_EndScreen.SetActive(true);
        m_EndTeamImage.texture = bIsFriendlyWin ? m_FriendlyWinTexture : m_HostileWinTexture;
    }
}
