using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "Settings", menuName = "BGG/Settings", order = 1)]
public class Settings : ScriptableObject
{
    [Header("CSV Files")]
    public TextAsset experienceFile;

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

    [Header("Experience")]
    public List<Experience> experience;

    #region Editor
#if UNITY_EDITOR
    public void LoadDataFromFile()
    {
        string[,] grid = CSVReader.GetCSVGrid("/Assets/_Balancing/" + experienceFile.name + ".csv");
        experience.Clear();
        experience = new List<Experience>();
        List<string> keys = new List<string>();

        //First create a list for holding our key values
        for (int y = 0; y < grid.GetUpperBound(0); ++y)
        {
            keys.Add(grid[y, 0]);
        }

        //Loop through the rows, adding the value to the appropriate key
        for (int x = 1; x < grid.GetUpperBound(1); x++)
        {
            Dictionary<string, string> columnData = new Dictionary<string, string>();
            for (int k = 0; k < keys.Count; k++)
            {
                columnData.Add(keys[k], grid[k, x]);
                //Debug.Log("Key: " + keys[k] + ", Value: " + grid[x, k]);
            }

            Experience exp = new Experience();
            //Loop through the dictionary using the key values
            foreach (KeyValuePair<string, string> item in columnData)
            {
                //Gets a experience data based off the ID and updates it
                if (item.Key.Contains("Level"))
                    exp.level = int.TryParse(item.Value, out exp.level) ? exp.level : 0;

                if (item.Key.Contains("Requirement"))
                    exp.requirement = int.TryParse(item.Value, out exp.requirement) ? exp.requirement : 0;

                if (item.Key.Contains("Title"))
                    exp.title = item.Value;
            }
            experience.Add(exp);

            //flag the object as "dirty" in the editor so it will be saved
            EditorUtility.SetDirty(this);

            // Prompt the editor database to save dirty assets, committing your changes to disk.
            AssetDatabase.SaveAssets();
        }
    }


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
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Load Data from file?"))
            {
                if (EditorUtility.DisplayDialog("Load Spreadsheet Data", "Are you sure you want to load experience data? This will overwrite any existing data", "Yes", "No"))
                {
                    settings.LoadDataFromFile();
                    EditorUtility.SetDirty(settings);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
            GUI.backgroundColor = Color.white;
            DrawDefaultInspector();
        }
    }
#endif
    #endregion
}

[System.Serializable]
public class Colours
{
    [Header("Object Colours")]
    //[BV.EnumList(typeof(ObjectID))]
    public List<ObjectColor> objectColor;

    [Header("In Game")]
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

    /// <summary>
    /// Gets the name of a key object/unit formatted within it's colour
    /// </summary>
    /// <param name="_object">The object</param>
    /// <param name="_plural">Whether it is a plural or not</param>
    /// <param name="_toUpper">Whether it should be uppercase (default)</param>
    /// <returns></returns>
    public string GetName(ObjectID _object, bool _plural = false, bool _toUpper = true)
    {
        ObjectColor oc = objectColor.Find(x => x.id == _object);
        if (oc == null)
            return _object.ToString();

        _toUpper = false;
        string c = ColorX.GetColorHex(oc.color);
        string n = oc.id.ToString();
        if (oc.id == ObjectID.Hut)  //Exceptions here
            n = "Witch's Hut";
        n = _toUpper ? (n.ToUpper() + (_plural ? "S" : "")) : (n + (_plural ? "s" : ""));
        return "<color=#" + c + ">" + n + "</color>";
    }

    public Color GetColour(ObjectID _id)
    {
        ObjectColor oc = objectColor.Find(x => x.id == _id);
        if (oc == null)
            return Color.white;

        return oc.color;
    }

    public Color GetColour(CreatureID _id) => GetColour(EnumX.ToEnum<ObjectID>(_id.ToString()));
    public Color GetColour(HumanID _id) => GetColour(EnumX.ToEnum<ObjectID>(_id.ToString()));
    public Color GetColour(ToolID _id) => GetColour(EnumX.ToEnum<ObjectID>(_id.ToString()));
    public Color GetColour(BuildingID _id) => GetColour(EnumX.ToEnum<ObjectID>(_id.ToString()));


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

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ObjectColor))]
    public class ObjectColorDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var idRect = new Rect(position.x, position.y, 150, position.height);
            var colorRect = new Rect(position.x + 155, position.y, 100, position.height);

            // Draw fields - pass GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(idRect, property.FindPropertyRelative("id"), GUIContent.none);
            EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("color"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
#endif
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
    public float blackoutPanelFade = 0.8f;
    public float blackoutPanelTime = 0.3f;
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
    [Header("Effects")]
    public float effectsTweenTime = 1f;
    [Header("ExperienceMeter")]
    public Ease experienceEase;
    public float experienceDelay = 1f;
    public float experienceDuration = 2f;
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

    [Header("Input")]
    public Sprite mouseZoom;
    public Sprite mouseRotate;
    public Sprite mouseLeftClick;
    public Sprite mouseRightClick;
    public Sprite keyboardMoveIcon;
    public Sprite keyboardShiftIcon;

    public string GetTMPIcon(Sprite _icon) => $"<sprite name={_icon.name}>";

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
    public ObjectID id;
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
    [BV.DrawIf("overrideTutorial", true)]
    public bool showTutorial;
}

[Serializable]
public class Experience
{
    public int level;
    public int requirement;
    public string title;
}