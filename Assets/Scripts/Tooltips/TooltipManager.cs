using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : Singleton<TooltipManager>
{
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI titleComponent;
    public GameObject tooltipBox;
    public GameObject tooltipBoxTop;
    public TextMeshProUGUI textComponentTop;
    public TextMeshProUGUI titleComponentTop;
    public Image levelImage;

    void Start()
    {
        Cursor.visible = true;
        tooltipBox.SetActive(false);
        tooltipBoxTop.SetActive(false);

    }

    void Update()
    {
        tooltipBox.transform.position = Input.mousePosition;
        tooltipBoxTop.transform.position = Input.mousePosition;
    }

    public void SetAndShowTooltip(string message, string title)
    {
        tooltipBox.SetActive(true);
        textComponent.text = message;
        titleComponent.text = title;
    }
    public void SetAndShowTooltipTop(string message, string title)
    {
        tooltipBoxTop.SetActive(true);
        textComponentTop.text = message;
        titleComponentTop.text = title;
    }
    public void SetLevelImage(Sprite sprite)
    {
        levelImage.sprite = sprite;
    }

    public void SetAndShowPopulousTooltip()
    {
        tooltipBox.SetActive(true);
        if(_DATA.HasPerk(PerkID.Populous) == false)
        {
            textComponent.text = "This increases the population cap by +5. Cost is 1200 Maegen.";
            titleComponent.text = "Populous Upgrade";
        }
    }

    public void HideTooltip()
    {
        tooltipBox.SetActive(false);
        textComponent.text = string.Empty;
        titleComponent.text = string.Empty;
    }
    public void HideTooltipTop()
    {
        tooltipBoxTop.SetActive(false);
        textComponentTop.text = string.Empty;
        titleComponentTop.text = string.Empty;
    }
}
