using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _winPopupPrefab;
    [SerializeField] private GameObject _losePopupPrefab;
    public Canvas canvas;
    private WinPopupUI _winPopupUI; 
    private LosePopupUI _losePopupUI;

    public IEnumerator ShowWinPopup(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if(_winPopupUI == null)
        _winPopupUI = Instantiate(_winPopupPrefab, canvas.transform).GetComponent<WinPopupUI>();
        _winPopupUI.Show();
    }
    public void HideWinPopup()
    {
        if(_winPopupUI)
        _winPopupUI.Show(false);
    }
    public IEnumerator ShowLosePopup(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if(_losePopupUI == null)
        _losePopupUI = Instantiate(_losePopupPrefab, canvas.transform).GetComponent<LosePopupUI>();
        _losePopupUI.Show();
    }
    public void HideLosePopup()
    {
        if(_losePopupUI)
        _losePopupUI.Show(false);
    }
}
