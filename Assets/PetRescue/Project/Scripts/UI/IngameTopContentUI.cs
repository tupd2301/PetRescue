using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameTopContentUI : MonoBehaviour
{
    [SerializeField] private GameObject _topContent;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _movesText;
    [SerializeField] private Text _totalTargetText;
    [SerializeField] private Text _currentTargetText;

    void Awake()
    {
        _settingButton.onClick.AddListener(()=>GamePlay.Instance.ShowCheatInput());
    }
    public void UpdateUI()
    {
        SetLevel();
        SetMoves();
        SetTotalTarget();
        SetCurrentTarget();
    }

    public void Show(bool isShow = true)
    {
        _topContent.SetActive(isShow);
    }

    public void SetLevel()
    {
        _levelText.text = "Level: " + GamePlay.Instance.currentLevelData.level.ToString();
    }

    public void SetMoves()
    {
        _movesText.text = GamePlay.Instance.move.ToString();
    }

    public void SetTotalTarget()
    {
        _totalTargetText.text = "/" + GamePlay.Instance.petManager.GetTotalPetCount().ToString();
    }

    public void SetCurrentTarget()
    {
        _currentTargetText.text = GamePlay.Instance.petManager.GetPetCount(true).ToString();
    }
}
