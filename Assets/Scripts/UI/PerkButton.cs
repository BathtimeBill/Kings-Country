using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PerkButton : InteractableButton
{
    public PerkData perkData;
    public Image icon;
    public PerkPanel perkPanel;
    public override void Start()
    {
        base.Start();
        button.onClick.AddListener(() => PressedButton());
    }

    void PressedButton()
    {
        perkPanel.SendPerkId(perkData.id);
        GameEvents.ReportOnUpgradeSelected(perkData.id);
    }

    public void SetUpgrade(PerkID _id)
    {
        perkData = _PERK.GetPerk(_id);
        perkPanel.perkOptions.Add(_id);
        icon.sprite = perkData.icon;
    }

    public void SetInteractable(bool _interactable)
    {
        if(button !=null)
            button.interactable = _interactable;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        perkPanel.PointerEnter(this);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        perkPanel.PointerExit();
    }
}
