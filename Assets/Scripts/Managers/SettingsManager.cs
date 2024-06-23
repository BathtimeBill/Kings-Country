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
    void Start()
    {
        ChangePanelColour(_SAVE.GetPanelColour());

        musicSlider.onValueChanged.AddListener((float _value) => SetVolume(_value));
        musicSlider.value = _SAVE.GetMusicVolume * 10;
        sfxSlider.onValueChanged.AddListener((float _value) => SetVolumeSFX(_value));
        sfxSlider.value = _SAVE.GetSFXVolume * 10;

        colourBlackToggle.onValueChanged.AddListener((bool _on) => ChangePanelBlack(_on));
        colourWhiteToggle.onValueChanged.AddListener((bool _on) => ChangePanelWhite(_on));
        colourBlueToggle.onValueChanged.AddListener((bool _on) => ChangePanelBlue(_on));
        colourGreenToggle.onValueChanged.AddListener((bool _on) => ChangePanelGreen(_on));
        colourRedToggle.onValueChanged.AddListener((bool _on) => ChangePanelRed(_on));
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
    public void ChangePanelBlack(bool _on)
    {
        ChangePanelColour(_COLOUR.darkPanels);
        _SAVE.SetPanelColour(_COLOUR.darkPanels);
    }
    public void ChangePanelWhite(bool _on)
    {
        ChangePanelColour(_COLOUR.lightPanels);
        _SAVE.SetPanelColour(_COLOUR.lightPanels);
    }
    public void ChangePanelBlue(bool _on)
    {
        ChangePanelColour(_COLOUR.bluePanels);
        _SAVE.SetPanelColour(_COLOUR.bluePanels);
    }
    public void ChangePanelGreen(bool _on)
    {
        ChangePanelColour(_COLOUR.greenPanels);
        _SAVE.SetPanelColour(_COLOUR.greenPanels);
    }
    public void ChangePanelRed(bool _on)
    {
        ChangePanelColour(_COLOUR.redPanels);
        _SAVE.SetPanelColour(_COLOUR.redPanels);
    }

    public void ChangePanelColour(Color _color)
    {
        GameObject[] panels = GameObject.FindGameObjectsWithTag("UIBackgroundPanel");
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].GetComponent<Image>() != null)
            {
                TweenX.TweenColor(panels[i].GetComponent<Image>(), _color);
#if UNITY_EDITOR
                panels[i].GetComponent<Image>().color = _color;
#endif
            }
        }
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
                settings.ChangePanelColour(settings._COLOUR.darkPanels);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Light Mode"))
            {
                settings.ChangePanelColour(settings._COLOUR.lightPanels);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Blue Mode"))
            {
                settings.ChangePanelColour(settings._COLOUR.bluePanels);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Green Mode"))
            {
                settings.ChangePanelColour(settings._COLOUR.greenPanels);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Red Mode"))
            {
                settings.ChangePanelColour(settings._COLOUR.redPanels);
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
