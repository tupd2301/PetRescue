using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Canvas canvas;
    private WinPopupUI _winPopupUI; 
    private LosePopupUI _losePopupUI;
    private IngameTopContentUI _ingameTopContentUI;
    [SerializeField] private Animator _cloudAnimator;

    public void UpdateTopContentUI()
    {
        if(_ingameTopContentUI)
            _ingameTopContentUI.UpdateUI();
    }

    public IEnumerator ShowWinPopup(float delay = 0)
    {
        for (int i = 0; i < delay*1000; i+=300)
        {
            Handheld.Vibrate();
        }
        yield return new WaitForSeconds(delay);
        if(_winPopupUI == null)
        _winPopupUI = Instantiate(Resources.Load<WinPopupUI>("Prefabs/UIPrefabs/WinPopup"), canvas.transform).GetComponent<WinPopupUI>();
        _winPopupUI.Show();
    }
    public void HideWinPopup()
    {
        if(_winPopupUI)
        _winPopupUI.Show(false);
    }
    public IEnumerator ShowLosePopup(float delay = 0)
    {
        for (int i = 0; i < delay*1000; i+=300)
        {
            Handheld.Vibrate();
        }
        yield return new WaitForSeconds(delay);
        if(_losePopupUI == null)
        _losePopupUI = Instantiate(Resources.Load<LosePopupUI>("Prefabs/UIPrefabs/LosePopup"), canvas.transform).GetComponent<LosePopupUI>();
        _losePopupUI.Show();
    }
    public void HideLosePopup()
    {
        if(_losePopupUI)
        _losePopupUI.Show(false);
    }

    public IEnumerator ShowIngameTopContent(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        if(_ingameTopContentUI == null)
        _ingameTopContentUI = Instantiate(Resources.Load<IngameTopContentUI>("Prefabs/UIPrefabs/IngameTopContent"), canvas.transform).GetComponent<IngameTopContentUI>();
        _ingameTopContentUI.Show();
    }
    public void HideIngameTopContent()
    {
        if(_ingameTopContentUI)
        _ingameTopContentUI.Show(false);
    }
    public void ShowCloud()
    {
        if(_cloudAnimator == null)
        {
            _cloudAnimator = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UIPrefabs/Clouds"),canvas.transform).GetComponent<Animator>();
        }
        _cloudAnimator.Play("Start");
    }
    public void HideCloud()
    {
        if(_cloudAnimator == null)
        {
            _cloudAnimator = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UIPrefabs/Clouds"),canvas.transform).GetComponent<Animator>();
        }
        _cloudAnimator.Play("End");
        Destroy(_cloudAnimator.gameObject, 5f);
    }
}
