using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitUpgradeButton : InteractableButton
{
    public UnitUpgrades unitUpgrades;
    public void SetInteractable(bool _interactable)
    {
        if (button == null)
            return;

        button.interactable = _interactable;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        unitUpgrades.ShowStatsUpgrade();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        unitUpgrades.ShowStats();
    }
}
