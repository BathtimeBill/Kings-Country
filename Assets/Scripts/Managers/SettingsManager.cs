using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : GameBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer musicAudioMixer;
    [SerializeField] private AudioMixer SFXAudioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [Header("Units")]
    [SerializeField] private Toggle unitOutlinesToggle;
    [SerializeField] private Toggle unitHealthBarToggle;
    [Header("Mini Map")]
    [SerializeField] private Toggle showMiniMapToggle;
    [SerializeField] private Toggle showMiniMapIconsToggle;
    [SerializeField] private Toggle miniMapRotationToggle;
    [Header("Aesthetics")]
    [SerializeField] private Toggle colourBlackToggle;
    [SerializeField] private Toggle colourWhiteToggle;
    [SerializeField] private Toggle colourBlueToggle;
    [SerializeField] private Toggle colourGreenToggle;
    [SerializeField] private Toggle colourRedToggle;
    [SerializeField] private ToggleGroup colourToggles;
    [Header("Data")]
    [SerializeField] private CanvasGroup deleteDataPopup;
    [SerializeField] private CanvasGroup blackoutImage;

    void Awake()
    {
        //Audio
        musicSlider.onValueChanged.AddListener((float _value) => SetVolume(_value));
        musicSlider.value = _SAVE.GetMusicVolume * 10;
        sfxSlider.onValueChanged.AddListener((float _value) => SetVolumeSFX(_value));
        sfxSlider.value = _SAVE.GetSFXVolume * 10;

        //Units
        unitOutlinesToggle.onValueChanged.AddListener((bool _show) => GameEvents.ReportOnUnitOutlines(_show));
        unitOutlinesToggle.isOn = _SAVE.GetUnitOutline;
        unitHealthBarToggle.onValueChanged.AddListener((bool _show) => GameEvents.ReportOnUnitHealthBars(_show));
        unitHealthBarToggle.isOn = _SAVE.GetUnitHealthBars;

        //Mini Map
        showMiniMapToggle.onValueChanged.AddListener((bool _show) => GameEvents.ReportOnMiniMapShow(_show));
        showMiniMapToggle.isOn = _SAVE.GetMiniMapShow;
        showMiniMapIconsToggle.onValueChanged.AddListener((bool _show) => GameEvents.ReportOnMiniMapIcons(_show));
        showMiniMapIconsToggle.isOn = _SAVE.GetMiniMapIcons;
        miniMapRotationToggle.onValueChanged.AddListener((bool _show) => GameEvents.ReportOnMiniMapRotation(_show));
        miniMapRotationToggle.isOn = _SAVE.GetMiniMapRotation;

        //Aesthetics
        colourBlackToggle.onValueChanged.AddListener((bool _on) => ChangePanelColour(PanelColourID.Black));
        colourWhiteToggle.onValueChanged.AddListener((bool _on) => ChangePanelColour(PanelColourID.White));
        colourBlueToggle.onValueChanged.AddListener((bool _on) => ChangePanelColour(PanelColourID.Blue));
        colourGreenToggle.onValueChanged.AddListener((bool _on) => ChangePanelColour(PanelColourID.Green));
        colourRedToggle.onValueChanged.AddListener((bool _on) => ChangePanelColour(PanelColourID.Red));

        PanelColourID currentColour = _SAVE.GetPanelColour();
        //ChangePanelColour(currentColour);
        colourToggles.SetAllTogglesOff(true);
        switch (currentColour)
        {
            case PanelColourID.Black:
                colourBlackToggle.isOn = true;
                break;
            case PanelColourID.White:
                colourWhiteToggle.isOn = true;
                break;
            case PanelColourID.Blue:
                colourBlueToggle.isOn = true;
                break;
            case PanelColourID.Green:
                colourGreenToggle.isOn = true;
                break;
            case PanelColourID.Red:
                colourRedToggle.isOn = true;
                break;
        }

        //Data
        FadeX.InstantTransparent(deleteDataPopup);
        FadeX.InstantTransparent(blackoutImage);
    }

    #region Audio
    public void SetVolume(float sliderValue)
    {
       
        float newVolume = 7 * sliderValue - 60;
        print(newVolume);
        musicAudioMixer.SetFloat("MusicVolume", newVolume);
        _SAVE.SetMusicVolume(newVolume);
    }

    public void SetVolumeSFX(float sliderValue)
    {
        
        float newVolume = 7 * sliderValue - 60;
        SFXAudioMixer.SetFloat("SFXVolume", newVolume);
        _SAVE.SetSFXVolume(newVolume);
    }
    #endregion

    #region Units

    #endregion

    #region Mini Map

    #endregion

    #region Aesthetics

    public void ChangePanelColour(PanelColourID _color)
    {
        _SETTINGS.colours.ChangePanelColour(_color, _SAVE);
    }
    #endregion

    #region Data
    public void ToggleDeletePopup(bool _on)
    {
        if (_on)
        {
            FadeX.FadeIn(deleteDataPopup);
            FadeX.FadeIn(blackoutImage);
        }
        else
        {
            FadeX.FadeOut(deleteDataPopup);
            FadeX.FadeOut(blackoutImage);
        }
    }
    public void DeleteData()
    {
        _SAVE.DeleteData();
    }

    #endregion
}
