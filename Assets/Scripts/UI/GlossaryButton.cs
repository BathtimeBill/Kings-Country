using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GlossaryButton : GameBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GlossaryID glossaryID;
    private Button button;
    private TMP_Text buttonText;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();
        button.onClick.AddListener(() => OnClick());
    }

    public void Setup()
    {
        buttonText.text = _GLOSSARY.GetGlossaryItem(glossaryID).title;
        //TODO if not unlocked through the tutorial, then text is ??? and grayed out
    }

    private void OnClick()
    {

        _GLOSSARY.ShowGlossaryItem(glossaryID);
    }

    public void HighlightButton(bool _highlight)
    {
        TweenX.TweenColor(buttonText, _highlight ? _COLOUR.titleColor : _COLOUR.descriptionColor);
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
