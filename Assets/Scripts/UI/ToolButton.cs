using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ToolButton : MonoBehaviour
{
    public Tool tool;
    public Image icon;
    public Image cooldownFill;
    Button button;

    public void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => PressedButton());
        CooldownFill(0);
    }

    void PressedButton()
    {
        GameEvents.ReportOnToolButtonPressed(tool);
    }

    public void SetInteractable(bool _interactable)
    {
        button.interactable = _interactable;
    }

    public void CooldownFill(float _timeRemaining)
    {
        cooldownFill.fillAmount = _timeRemaining;
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
