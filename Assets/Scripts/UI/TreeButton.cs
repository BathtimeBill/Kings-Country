using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TreeButton : InteractableButton
{
    public TreeID treeID;
    public Image cooldownFill;
    public TMP_Text meagenPrice;
    public TreePanel treePanel;

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
        GameEvents.ReportOnTreeButtonPressed(treeID);
    }
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (treePanel == null)
            return;
        treePanel.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (treePanel == null)
            return;
        treePanel.PointerExit(eventData);
    }
    public override void SetupButton()
    {
        icon.sprite = _DATA.GetTree(treeID).icon;
        //name = tool.toolName + "ToolButton";
    }
    #endregion
}
