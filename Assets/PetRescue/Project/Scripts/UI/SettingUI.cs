using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public System.Action OnShowPopup;
    public System.Action OnCancelClicked;
    public System.Action OnSaveClicked;
    public GameObject guiMain;

    [SerializeField]
    private EventTrigger _backgroundTrigger;

    [SerializeField]
    private Slider _bgmSlider;

    [SerializeField]
    private Slider _sfxSlider;

    [SerializeField]
    private Toggle _vibrateToggle;

    [SerializeField]
    private Button _cancelButton;

    [SerializeField]
    private Button _saveButton;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        OnShowPopup = () =>
        {
            Show();
        };
        OnCancelClicked = () =>
        {
            Show();
        };

        OnSaveClicked = () =>
        {
            SaveValue();
            Show();
        };
        _cancelButton.onClick.AddListener(Cancel);
        _saveButton.onClick.AddListener(Save);

        _backgroundTrigger.triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(
            (data) =>
            {
                OnShowPopup?.Invoke();
            }
        );
        _backgroundTrigger.triggers.Add(entry);
    }

    public void LoadUI()
    {
        _bgmSlider.value = PlayerPrefs.GetFloat("BGM_VOLUME_KEY", 1f);
        _sfxSlider.value = PlayerPrefs.GetFloat("SFX_VOLUME_KEY", 1f);
        _vibrateToggle.isOn = PlayerPrefs.GetInt("VIBRATE_KEY", 1) == 1;
    }

    public void SaveValue()
    {
        PlayerPrefs.SetFloat("BGM_VOLUME_KEY", _bgmSlider.value);
        PlayerPrefs.SetFloat("SFX_VOLUME_KEY", _sfxSlider.value);
        PlayerPrefs.SetInt("VIBRATE_KEY", _vibrateToggle.isOn ? 1 : 0);
        LoadUI();
    }

    public void Show()
    {
        guiMain.SetActive(guiMain.activeSelf ? false : true);
        LoadUI();
    }

    public void Cancel()
    {
        OnCancelClicked?.Invoke();
    }

    public void Save()
    {
        OnSaveClicked?.Invoke();
    }
}
