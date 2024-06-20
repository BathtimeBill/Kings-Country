using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TitleManager : GameBehaviour
{
    [Header("Canvas Groups")]
    public CanvasGroup homeCanvas;
    public CanvasGroup unitCanvas;
    public CanvasGroup mapCanvas;
    //public CanvasGroup settingsCanvas;

    [Header("Virtual Cameras")]
    public GameObject homeCamera;
    public GameObject unitCamera;
    public GameObject mapCamera;

    [Header("Title Buttons")]
    public TitleButton[] titleButtons;
    [Header("Managers")]
    public OverWorldManager overworldManager;
    public SceneManager sceneManager;

    private void Start()
    {
        TurnOffPanels();
        TurnOffCameras();
        homeCamera.SetActive(true);
        FadeX.InstantOpaque(homeCanvas);
    }

    public void ShowHome()
    {
        TurnOffCameras();
        homeCamera.SetActive(true);
        overworldManager.ToggleInMap(false);

        FadeX.FadeIn(homeCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeOut(unitCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeOut(mapCanvas, _DATA.settings.tweening.titlePanelTweenTime);
    }

    public void ShowUnits()
    {
        TurnOffCameras();
        unitCamera.SetActive(true);

        FadeX.FadeOut(homeCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeIn(unitCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeOut(mapCanvas, _DATA.settings.tweening.titlePanelTweenTime);
    }

    public void ShowMap()
    {
        TurnOffCameras();
        mapCamera.SetActive(true);
        overworldManager.ToggleInMap(true);

        FadeX.FadeOut(homeCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeOut(unitCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeIn(mapCanvas, _DATA.settings.tweening.titlePanelTweenTime);
    }

    public void ShowSettings()
    {

    }

    public void TurnOffCameras()
    {
        homeCamera.SetActive(false);
        unitCamera.SetActive(false);
        mapCamera.SetActive(false);
    }

    public void TurnOffPanels()
    {
        FadeX.InstantTransparent(homeCanvas);
        FadeX.InstantTransparent(unitCanvas);
        FadeX.InstantTransparent(mapCanvas);
    }
    public void ToggleTitleText(bool _show)
    {
        for(int i=0; i<titleButtons.Length;i++)
        {
            titleButtons[i].ToggleText(_show);
        }
    }

    public void ChangePanelColour(bool _dark)
    {
        GameObject[] panels = GameObject.FindGameObjectsWithTag("UIBackgroundPanel");
        for(int i=0; i< panels.Length; i++)
        {
            if (panels[i].GetComponent<UnityEngine.UI.Image>() != null)
            {
                TweenX.TweenColor(panels[i].GetComponent<UnityEngine.UI.Image>(), _dark ? _COLOUR.darkModeBackground : _COLOUR.lightModeBackground);
#if UNITY_EDITOR
                panels[i].GetComponent<UnityEngine.UI.Image>().color = _dark ? _COLOUR.darkModeBackground : _COLOUR.lightModeBackground;
#endif
            }


        }
    }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(TitleManager))]
    [CanEditMultipleObjects]

    public class TitleManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TitleManager titleManager = (TitleManager)target;
            DrawDefaultInspector();
            GUILayout.Space(20);
            GUI.backgroundColor = Color.green;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Show Home"))
            {
                titleManager.TurnOffCameras();
                titleManager.TurnOffPanels();
                titleManager.ShowHome();
                FadeX.InstantOpaque(titleManager.homeCanvas);
                EditorUtility.SetDirty(titleManager);
            }
            if (GUILayout.Button("Show Map"))
            {
                titleManager.TurnOffCameras();
                titleManager.TurnOffPanels();
                FadeX.InstantOpaque(titleManager.mapCanvas);
                titleManager.ShowMap();
                EditorUtility.SetDirty(titleManager);
            }
            if (GUILayout.Button("Show Upgrades"))
            {
                titleManager.TurnOffCameras();
                titleManager.TurnOffPanels();
                FadeX.InstantOpaque(titleManager.unitCanvas);
                titleManager.ShowUnits();
                EditorUtility.SetDirty(titleManager);
            }
            
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.blue;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Show Title Labels"))
            {
                titleManager.ToggleTitleText(true);
                EditorUtility.SetDirty(titleManager);
            }
            if (GUILayout.Button("Hide Title Labels"))
            {
                titleManager.ToggleTitleText(false);
                EditorUtility.SetDirty(titleManager);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.black;
            if (GUILayout.Button("Dark Mode"))
            {
                titleManager.ChangePanelColour(true);
                EditorUtility.SetDirty(titleManager);
            }
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Light Mode"))
            {
                titleManager.ChangePanelColour(false);
                EditorUtility.SetDirty(titleManager);
            }
            GUILayout.EndHorizontal();
        }
    }
#endif
    #endregion
}
