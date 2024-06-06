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
        if(combatID == CombatID.Attack)
        {
            _SM.PlaySound(_SM.attackSound);
            GameEvents.ReportOnAttackSelected();
        }
        if(combatID == CombatID.Defend)
        {
            _SM.PlaySound(_SM.defendSound);
            GameEvents.ReportOnDefendSelected();
        }
        if (combatID == CombatID.Formation)
        {
            _SM.PlaySound(_SM.formationSound);
            GameEvents.ReportOnFormationSelected();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        combatPanel.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        combatPanel.PointerExit();
    }
}
