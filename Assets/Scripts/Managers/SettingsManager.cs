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
    }

    #region Audio
    public void SetVolume(float sliderValue)
    {
        float newVolume = sliderValue * 0.1f;
        musicAudioMixer.SetFloat("MusicVolume", newVolume);
        _SAVE.SetMusicVolume(newVolume);
    }

    public void SetVolumeSFX(float sliderValue)
    {
        float newVolume = sliderValue * 0.1f;
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
        _SETTINGS.colours.ChangePanelColour(_color, _COLOUR, _SAVE);
    }
    #endregion

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(SettingsManager))]
    [CanEditMultipleObjects]

    public class SettingsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SettingsManager settings = (SettingsManager)target;
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.black;
            if (GUILayout.Button("Dark Mode"))
            {
                settings.ChangePanelColour(PanelColourID.Black);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Light Mode"))
            {
                settings.ChangePanelColour(PanelColourID.White);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Blue Mode"))
            {
                settings.ChangePanelColour(PanelColourID.Blue);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Green Mode"))
            {
                settings.ChangePanelColour(PanelColourID.Green);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Red Mode"))
            {
                settings.ChangePanelColour(PanelColourID.Red);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            DrawDefaultInspector();
        }
    }
#endif
    #endregion
}
