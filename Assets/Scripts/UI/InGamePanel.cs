using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;
using UnityEngine.EventSystems;

public class InGamePanel : GameBehaviour
{
    public List<Button> buttons;
    [Header("Info Box")]
    public CanvasGroup canvasGroup;
    public TMP_Text titleText;
    public TMP_Text descriptionText;

    private void Start()
    {
        canvasGroup.gameObject.SetActive(false);
    }

    public virtual void PointerEnter(CombatButton _combatButton)
    {
        canvasGroup.gameObject.SetActive(true);
        titleText.text = _combatButton.title;
        descriptionText.text = _combatButton.description;
    }
    
    public virtual void PointerEnter(TreeButton _treeButton)
    {
        canvasGroup.gameObject.SetActive(true);
        titleText.text = _DATA.GetTree(_treeButton.treeID).name;
        descriptionText.text = _DATA.GetTree(_treeButton.treeID).description;
    }

    public virtual void PointerEnter(ToolButton _toolButton)
    {
        canvasGroup.gameObject.SetActive(true);
        titleText.text = _DATA.GetTool(_toolButton.toolID).name;
        descriptionText.text = _DATA.GetTool(_toolButton.toolID).description;
    }

    public virtual void PointerEnter(PointerEventData eventData)
    {
        canvasGroup.gameObject.SetActive(true);
    }

    public virtual void PointerExit(PointerEventData eventData)
    {
        canvasGroup.gameObject.SetActive(false);
    }

    public void ToggleShiny(bool _on)
    {
        for(int i=0; i<buttons.Count; i++)
        {
            if(buttons[i].GetComponent<ShinyEffectForUGUI>() != null)
                buttons[i].GetComponent<ShinyEffectForUGUI>().enabled = _on;
        }
    }

    public void ToggleOnActiveShiny()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].GetComponent<ShinyEffectForUGUI>() != null && buttons[i].interactable)
                buttons[i].GetComponent<ShinyEffectForUGUI>().enabled = true;
        }
    }
}
