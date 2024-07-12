using TMPro;
using UnityEngine.EventSystems;

public class ToolPanel : InGamePanel
{
    public TMP_Text maegenPriceText;
    public TMP_Text wildlifePriceText;

    public override void PointerEnter(ToolButton _toolButton)
    {
        base.PointerEnter(_toolButton);
        maegenPriceText.text = _DATA.GetTool(_toolButton.toolID).maegenPrice.ToString();
        wildlifePriceText.text = _DATA.GetTool(_toolButton.toolID).wildlifePrice.ToString();
    }

    public override void PointerExit(PointerEventData eventData)
    {
        base.PointerExit(eventData);
    }
}
