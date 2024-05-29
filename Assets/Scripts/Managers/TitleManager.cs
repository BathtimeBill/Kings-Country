using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TitleManager : GameBehaviour
{
    [Header("Canvas Groups")]
    public CanvasGroup homeCanvas;
    public CanvasGroup unitCanvas;
    public CanvasGroup perkCanvas;
    //public CanvasGroup settingsCanvas;

    [Header("Virtual Cameras")]
    public GameObject homeCamera;
    public GameObject unitCamera;
    public GameObject perkCamera;

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

        FadeX.FadeInPanel(homeCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeOutPanel(unitCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeOutPanel(perkCanvas, _DATA.settings.tweening.titlePanelTweenTime);
    }

    public void ShowUnits()
    {
        TurnOffCameras();
        unitCamera.SetActive(true);

        FadeX.FadeOutPanel(homeCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeInPanel(unitCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeOutPanel(perkCanvas, _DATA.settings.tweening.titlePanelTweenTime);
    }

    public void ShowPerks()
    {
        TurnOffCameras();
        perkCamera.SetActive(true);

        FadeX.FadeOutPanel(homeCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeOutPanel(unitCanvas, _DATA.settings.tweening.titlePanelTweenTime);
        FadeX.FadeInPanel(perkCanvas, _DATA.settings.tweening.titlePanelTweenTime);
    }

    public void ShowSettings()
    {

    }

    public void TurnOffCameras()
    {
        homeCamera.SetActive(false);
        unitCamera.SetActive(false);
        perkCamera.SetActive(false);
    }

    public void TurnOffPanels()
    {
        FadeX.InstantTransparent(homeCanvas);
        FadeX.InstantTransparent(unitCanvas);
        FadeX.InstantTransparent(perkCanvas);
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
            if (GUILayout.Button("Show Units"))
            {
                titleManager.TurnOffCameras();
                titleManager.TurnOffPanels();
                FadeX.InstantOpaque(titleManager.unitCanvas);
                titleManager.ShowUnits();
                EditorUtility.SetDirty(titleManager);
            }
            if (GUILayout.Button("Show Perks"))
            {
                titleManager.TurnOffCameras();
                titleManager.TurnOffPanels();
                FadeX.InstantOpaque(titleManager.perkCanvas);
                titleManager.ShowPerks();
                EditorUtility.SetDirty(titleManager);
            }
            GUILayout.EndHorizontal();
        }
    }
#endif
    #endregion
}
