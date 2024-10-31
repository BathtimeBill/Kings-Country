using UnityEngine.EventSystems;

public class CombatButton : InteractableButton
{
    public CombatID combatID;
    public string title;
    public string description;
    public CombatPanel combatPanel;
    public override void Start()
    {
        base.Start();
    }

    public override void ClickedButton()
    {
        GameEvents.ReportCombatButton(combatID);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        combatPanel.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        combatPanel.PointerExit(eventData);
    }
}
