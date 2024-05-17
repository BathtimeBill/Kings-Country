using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ToolButton : InteractableButton
{
    public ToolID toolID;
    public Image icon;
    public Image cooldownFill;
    public TMP_Text meagenPrice;
    public ToolPanel toolPanel;

    public override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => PressedButton());
        CooldownFill(0);
    }

    void PressedButton()
    {
        GameEvents.ReportOnToolButtonPressed(toolID);
    }

    public void SetInteractable(bool _interactable)
    {
        if (button == null)
            return;

        button.interactable = _interactable;

        if(GetComponent<Coffee.UIExtensions.ShinyEffectForUGUI>() != null)
            GetComponent<Coffee.UIExtensions.ShinyEffectForUGUI>().enabled = _interactable;
    }

    public void CooldownFill(float _timeRemaining)
    {
        cooldownFill.fillAmount = _timeRemaining;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (toolPanel == null)
            return;
        toolPanel.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (toolPanel == null)
            return;
        toolPanel.PointerExit();
    }

    public void SetupButton()
    {
        icon.sprite = _TOOL.GetTool(toolID).toolIcon;
        //name = tool.toolName + "ToolButton";
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
