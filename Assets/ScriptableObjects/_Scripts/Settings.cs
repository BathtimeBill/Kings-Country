using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "Settings", menuName = "BGG/Settings", order = 1)]
public class Settings : ScriptableObject
{
    [Header("Debug Testing - Turn all off for release")]
    public Testing testing;

    [Header("Camera Shakes")]
    public CameraShake cameraShake;

    [Header("Tween Stuff")]
    public Tweening tweening;

    [Header("Colours")]
    public Colours colours;

    [Header("Mini Map")]
    public MiniMap miniMap;

    [Header("Icons")]
    public Icons icons;

    [Header("Audio")]
    public AudioVariables audio;

    [Header("Vibration")]
    public Vibration vibration;

    [Header("General")]
    public General general;

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(Settings))]
    [CanEditMultipleObjects]

    public class SettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Settings settings = (Settings)target;
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.black;
            if (GUILayout.Button("Dark Mode"))
            {
                settings.colours.ChangePanelColour(PanelColourID.Black, null);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.white;
            if (GUILayout.Button("Light Mode"))
            {
                settings.colours.ChangePanelColour(PanelColourID.White, null);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Blue Mode"))
            {
                settings.colours.ChangePanelColour(PanelColourID.Blue, null);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Green Mode"))
            {
                settings.colours.ChangePanelColour(PanelColourID.Green, null);
                EditorUtility.SetDirty(settings);
            }
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Red Mode"))
            {
                settings.colours.ChangePanelColour(PanelColourID.Red, null);
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

[System.Serializable]
public class Colours
{
    public Color highlightedColor;
    public Color cooldownColor;
    public Color upgradeIconsColor;
    public Color titleColor;
    public Color descriptionColor;
    public Color upgradeIncreaseColor;
    public Color upgradeDecreaseColor;
    public Gradient treePercentageGradient;
    [Header("Map")]
    public Color mapUnlockedColor;
    public Color mapUnlockedHighlightColor;
    public Color mapUnlockedSelectedColor;
    public Color mapLockedColor;
    public Color mapLockedHighlightColor;
    public Color mapLockedSelectedColor;
    [Header("Title")]
    public Color levelHasColor;
    public Color levelHasNotColor;
    [Header("UI Panels")]
    public Color darkPanels;
    public Color lightPanels;
    public Color bluePanels;
    public Color greenPanels;
    public Color redPanels;
    [Header("Toggles")]
    public Color toggleIconActiveColor;
    public Color toggleIconInactiveColor;
    public Color toggleIconDisabledColor;
    public Color toggleIconHighlightColor;
    [Header("General")]
    public Color transparentColor;
    [Header("Unit Colours")]
    public Color homeTreeUnitColor;
    public Color hutUnitColor;
    public Color hogyrUnitColor;
    public Color woodcutterTypeColor;
    public Color hunterTypeColor;
    public Color warriorTypeColor;
    public Color specialTypeColor;

    [Header("Unit Colours")]
    [BV.EnumList(typeof(ColorID))]
    public List<ObjectColor> objectColor;

    public string GetIncreaseColorString => ColorX.GetColorHex(upgradeIncreaseColor);
    public string GetDecreaseColorString => ColorX.GetColorHex(upgradeDecreaseColor);

    public Color GetPanelColour(PanelColourID _panelColourID)
    {
        if (_panelColourID == PanelColourID.Black)
            return darkPanels;
        else if (_panelColourID == PanelColourID.White)
            return lightPanels;
        else if(_panelColourID == PanelColourID.Blue)
            return bluePanels;
        else if (_panelColourID == PanelColourID.Green)
            return greenPanels;
        else 
            return redPanels;
    }

    public Color GetUnitColor(UnitData _creatureData)
    {
        Color c = Color.white;
        if (_creatureData.home == BuildingID.HomeTree)
            c = homeTreeUnitColor;
        if(_creatureData.home == BuildingID.Hut)
            c = hutUnitColor;
        if(_creatureData.home == BuildingID.Hogyr)
            c = hogyrUnitColor;
        return c;
    }

    public Color GetUnitColor(EnemyData _humanData)
    {
        Color c = Color.white;
        if (_humanData.type == EnemyType.Woodcutter)
            c = woodcutterTypeColor;
        if (_humanData.type == EnemyType.Hunter)
            c = hunterTypeColor;
        if (_humanData.type == EnemyType.Warrior)
            c = warriorTypeColor;
        if (_humanData.type == EnemyType.Special)
            c = specialTypeColor;
        return c;
    }

    public Color GetUnitColor(BuildingData _buildingData)
    {
        Color c = Color.white;
        if (_buildingData.id == BuildingID.HomeTree)
            c = homeTreeUnitColor;
        if (_buildingData.id == BuildingID.Hut)
            c = hutUnitColor;
        if (_buildingData.id == BuildingID.Hogyr)
            c = hogyrUnitColor;
        return c;
    }

    public string GetColoredUnitName(UnitData _creatureData)
    {
        return "<color=#" + ColorX.GetColorHex(GetUnitColor(_creatureData)) + ">" + _creatureData.name + "</color>";
    }
    public string GetColoredUnitName(EnemyData _humanData)
    {
        return "<color=#" + ColorX.GetColorHex(GetUnitColor(_humanData)) + ">" + _humanData.name + "</color>";
    }

    public void ChangePanelColour(PanelColourID _color, SaveManager _save)
    {
        Color c = GetPanelColour(_color);
#if !UNITY_EDITOR
        _save.SetPanelColour(_color);
#endif
        GameObject[] panels = GameObject.FindGameObjectsWithTag("UIBackgroundPanel");
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].GetComponent<UnityEngine.UI.Image>() != null)
            {
                TweenX.TweenColor(panels[i].GetComponent<UnityEngine.UI.Image>(), c);
#if UNITY_EDITOR
                panels[i].GetComponent<UnityEngine.UI.Image>().color = c;
#endif
            }
        }
    }
}

[System.Serializable]
public class CameraShake
{
    public float shakeDuration = 1.5f;
    public int shakeVibrato = 10;
    public float shakeRandomness = 90f;
    public float fyreShakeIntensity = 2.0f;
    public float stormerShakeIntensity = 3.0f;
}

[System.Serializable]
public class Tweening
{
    public float UIButtonTweenTime = 0.3f;
    public Ease UIButtonTweenEase;
    public float titlePanelTweenTime = 0.5f;
    public Ease titlePanelTweenEase;
    public float errorTweenTime = 0.2f;
    public float errorTweenDuration = 2f;
    public Ease errorTweenEase;
    [Header("In Game Panels")]
    public float UITweenTime = 0.4f;
    public Ease UITweenEase;
    [Header("Overworld Map")]
    public float mapTweenTime = 0.5f;
    public Ease mapTweenEase;
    [Header("Scene Transition")]
    public TransitionType sceneTransitionType;
    public float sceneTransitionTime = 5f;
    public Ease sceneTransitionEase;
    [Header("Game Log")]
    public float logTweenTime = 0.3f;
    public float logTweenDelay = 1.5f;
    public Ease logEase;
}

[Serializable]
public class AudioVariables
{
    public float soundVolume = 1f;
    public bool sound = true;
    public float musicVolume = 1f;
    public bool music = true;
}

[Serializable]
public class Vibration
{
    public bool vibrateOn = true;
    public float[] vibrateStrength; //Perfect, Great, Good
}

[Serializable]
public class Icons
{
    public Sprite emptyIcon;
    public Sprite maegenIcon;
    public Sprite wildlifeIcon;
    public Sprite damageIcon;
    public Sprite healthIcon;
    public Sprite speedIcon;
    public Sprite cooldownIcon;
}

[Serializable]
public class MiniMap
{
    public bool visible = true;
    public bool showIcons = true;
    public int buildingIconSize = 24;
    public int creatureIconSize = 16;
    public int humanIconSize = 16;
}

[Serializable]
public class ObjectColor
{
    public ColorID id;
    public Color color;
}

[Serializable]
public class General
{
    public float introCameraDuration = 5;
    public GameObject maegenPickup;
    public GameObject healObject;
}

[Serializable]
public class Testing
{
    public bool allGlossaryUnlocked = false;
    [Tooltip("Will override the save data values")]
    public bool overrideTutorial;
    [DrawIf("overrideTutorial", true)]
    public bool showTutorial;
}
