using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ToolButton : InteractableButton
{
    public ToolID toolID;
    public Image cooldownFill;
    public TMP_Text meagenPrice;
    public ToolPanel toolPanel;

    public override void Start()
    {
        base.Start();
        CooldownFill(0);
    }

    public void CooldownFill(float _timeRemaining)
    {
        cooldownFill.fillAmount = _timeRemaining;
    }

    #region overrides
    public override void ClickedButton()
    {
        GameEvents.ReportOnToolButtonPressed(toolID);
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
        toolPanel.PointerExit(eventData);
    }

    #endregion

    #region Editor
    public override void SetupButton()
    {
        icon.sprite = _DATA.GetTool(toolID).icon;
        //name = tool.toolName + "ToolButton";
    }
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
