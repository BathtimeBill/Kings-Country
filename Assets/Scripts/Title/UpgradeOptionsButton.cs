using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeOptionsButton : InteractableButton
{
    public UpgradeManager upgradeManager;
    public UpgradeCategoryID upgradeCategory;
    public override void Start()
    {
        base.Start();
    }

    public override void ClickedButton()
    {
        upgradeManager.ChangeCategory(upgradeCategory);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
    }
    public override void OnPointerUp(PointerEventData eventData)
    {

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
    }
}
