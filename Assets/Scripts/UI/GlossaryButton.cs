using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GlossaryButton : GameBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GlossaryID glossaryID;
    private Button button;
    private TMP_Text buttonText;
    public bool unlocked = false;

    private void SetInteractable(bool _interactable) => button.interactable = _interactable;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();
        button.onClick.AddListener(() => OnClick());
    }

    public void Setup()
    {
        unlocked = _SAVE.GetGlossaryItemUnlocked(glossaryID);

        if(_TESTING.allGlossaryUnlocked)
            unlocked = true;

        Unlock(unlocked);
    }

    public void Unlock(bool _unlock)
    {
        unlocked = _unlock;
        buttonText.text = unlocked ? _GLOSSARY.GetGlossaryItem(glossaryID).title : "?????????";
        SetInteractable(unlocked);
    }


    private void OnClick()
    {
        _GLOSSARY.ShowGlossaryItem(glossaryID);
    }

    public void HighlightButton(bool _highlight)
    {
        buttonText.color = _highlight ? _COLOUR.titleColor : _COLOUR.descriptionColor;
       // TweenX.TweenColor(buttonText, _highlight ? _COLOUR.titleColor : _COLOUR.descriptionColor);
    }

    public void OnPointerEnter(PointerEventData eventData) 
    {
        HighlightButton(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        HighlightButton(false);
    }
    public void OnPointerDown(PointerEventData eventData) { }
    public void OnPointerUp(PointerEventData eventData) { }
}
