using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PerkButton : InteractableButton
{
    public PerkData perkData;
    public PerkPanel perkPanel;
    public override void Start()
    {
        base.Start();
    }

    public void SetUpgrade(PerkID _id)
    {
        perkData = _DATA.GetPerk(_id);
        perkPanel.perkOptions.Add(_id);
        icon.sprite = perkData.icon;
    }

    #region overrides
    public override void ClickedButton()
    {
        perkPanel.SendPerkId(perkData.id);
        GameEvents.ReportOnUpgradeSelected(perkData.id);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        perkPanel.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        perkPanel.PointerExit();
    }
    #endregion
}
