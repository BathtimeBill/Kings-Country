using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolPanel : GameBehaviour
{
    [Header("Info Box")]
    public CanvasGroup canvasGroup;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text maegenPriceText;
    public TMP_Text wildlifePriceText;

    private void Start()
    {
        canvasGroup.gameObject.SetActive(false);
    }

    public void PointerEnter(ToolButton _toolButton)
    {
        canvasGroup.gameObject.SetActive(true);
        titleText.text = _TOOL.GetTool(_toolButton.toolID).toolName;
        descriptionText.text = _TOOL.GetTool(_toolButton.toolID).toolDescription;
        maegenPriceText.text = _TOOL.GetTool(_toolButton.toolID).maegenPrice.ToString();
        wildlifePriceText.text = _TOOL.GetTool(_toolButton.toolID).wildlifePrice.ToString();
    }

    public void PointerExit()
    {
        canvasGroup.gameObject.SetActive(false);
    }
}
