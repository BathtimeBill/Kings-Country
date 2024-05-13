using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour
{
    public Tool tool;
    public Image icon;
    Button button;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => PressedButton());
    }

    void PressedButton()
    {
        GameEvents.ReportOnToolButtonPressed(tool);
    }

    public void SetupButton()
    {
        icon.sprite = tool.toolIcon;
        name = tool.toolName + "ToolButton";
    }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(ToolButton))]
    [CanEditMultipleObjects]

    public class ToolButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ToolButton tool = (ToolButton)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Setup"))
            {
                tool.SetupButton();
                EditorUtility.SetDirty(tool);
            }
            GUILayout.EndHorizontal();
           
            DrawDefaultInspector();
        }
    }
#endif
    #endregion
}
