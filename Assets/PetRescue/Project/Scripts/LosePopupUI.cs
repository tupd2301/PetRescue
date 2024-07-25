using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePopupUI : MonoBehaviour
{
    [SerializeField] private GameObject _popup;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Text _levelText;
    void Start()
    {
        _restartButton.onClick.AddListener(Restart);
    }
    public void Show(bool isShow = true)
    {
        _popup.SetActive(isShow);
        _levelText.text = GamePlay.Instance.currentLevelData.level.ToString();
    }
    public void Restart()
    {
        GamePlay.Instance.Restart();
        Show(false);
    }
}
