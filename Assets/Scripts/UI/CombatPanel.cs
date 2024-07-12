using UnityEngine.EventSystems;

public class CombatPanel : InGamePanel
{
    public override void PointerEnter(CombatButton _combatButton)
    {
        base.PointerEnter(_combatButton);
    }

    public override void PointerExit(PointerEventData eventData)
    {
        base.PointerExit(eventData);
    }
}
