using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ToolButton : InteractableButton
{
    public Tool tool;
    public Image icon;
    public Image cooldownFill;
    public TMP_Text meagenPrice;
    public ToolPanel toolButtonManager;

    public override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => PressedButton());
        CooldownFill(0);
    }

    void PressedButton()
    {
        GameEvents.ReportOnToolButtonPressed(tool);
    }

    public void SetInteractable(bool _interactable)
    {
        if(button != null)
            button.interactable = _interactable;
    }

    public void CooldownFill(float _timeRemaining)
    {
        cooldownFill.fillAmount = _timeRemaining;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (toolButtonManager == null)
            return;
        toolButtonManager.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (toolButtonManager == null)
            return;
        toolButtonManager.PointerExit();
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
