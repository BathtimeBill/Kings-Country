using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolPanel : InGamePanel
{
    public TMP_Text maegenPriceText;
    public TMP_Text wildlifePriceText;

    public override void PointerEnter(ToolButton _toolButton)
    {
        base.PointerEnter();
        maegenPriceText.text = _DATA.GetTool(_toolButton.toolID).maegenPrice.ToString();
        wildlifePriceText.text = _DATA.GetTool(_toolButton.toolID).wildlifePrice.ToString();
    }

    public override void PointerExit()
    {
        base.PointerExit();
    }
}
