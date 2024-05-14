using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolPanelManager : MonoBehaviour
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
        titleText.text = _toolButton.tool.toolName;
        descriptionText.text = _toolButton.tool.toolDescription;
        maegenPriceText.text = _toolButton.tool.cost.ToString();
    }

    public void PointerExit()
    {
        canvasGroup.gameObject.SetActive(false);
    }
}
