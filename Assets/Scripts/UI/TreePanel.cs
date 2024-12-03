using UnityEngine;
using TMPro;

public class TreePanel : InGamePanel
{
    public TMP_Text maegenPriceText;
    public TMP_Text wildlifePriceText;

    public override void PointerEnter(TreeButton _treeButton)
    {
        base.PointerEnter(_treeButton);
        maegenPriceText.text = _DATA.GetTree(_treeButton.treeID).maegenPrice.ToString();
        wildlifePriceText.text = _DATA.GetTree(_treeButton.treeID).wildlifePrice.ToString();
    }
}
