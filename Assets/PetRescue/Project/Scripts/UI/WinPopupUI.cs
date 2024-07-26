using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPopupUI : MonoBehaviour
{
    [SerializeField] private GameObject _winPopup;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _totalTargetText;
    [SerializeField] private Text _currentTargetText;
    void Start()
    {
        _nextButton.onClick.AddListener(Next);
        _restartButton.onClick.AddListener(Restart);
    }
    public void Show(bool isShow = true)
    {
        _winPopup.SetActive(isShow);
        _levelText.text = "Level: " + GamePlay.Instance.currentLevelData.level.ToString();
        _totalTargetText.text = "/" + GamePlay.Instance.petManager.GetTotalPetCount().ToString();
        _currentTargetText.text = GamePlay.Instance.petManager.GetPetCount(true).ToString();
    }

    public void Next()
    {
        GamePlay.Instance.NextLevel();
        Show(false);
    }
    public void Restart()
    {
        GamePlay.Instance.Restart();
        Show(false);
    }
}
