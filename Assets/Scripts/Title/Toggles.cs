using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Toggles : GameBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Image highlight;
    Toggle toggle;
    public void Start()
    {
        toggle = GetComponent<Toggle>();
        if (toggle != null)
            toggle.onValueChanged.AddListener((bool on) => PressedToggle(on));
        ExecuteNextFrame(()=>PressedToggle(toggle.isOn));
    }

    public virtual void PressedToggle(bool _on)
    {
        if (!toggle.interactable) 
            return;

        if (!_on)
        {
            TweenX.TweenColor(icon, _COLOUR.toggleIconInactiveColor, _TWEENING.UIButtonTweenTime);
            TweenX.TweenColor(highlight, _COLOUR.transparentColor, _TWEENING.UIButtonTweenTime);
            return;
        }

        TweenX.TweenColor(icon, _COLOUR.toggleIconActiveColor, _TWEENING.UIButtonTweenTime);
        TweenX.TweenColor(highlight, _COLOUR.toggleIconHighlightColor, _TWEENING.UIButtonTweenTime);
    }

    public virtual void SetInteractable(bool _interactable)
    {
        if (toggle != null)
            toggle.interactable = _interactable;

        if(!_interactable)
        {
            icon.color = _COLOUR.toggleIconDisabledColor;
            highlight.color = _COLOUR.toggleIconDisabledColor;
        }
        else
        {
            icon.color = _COLOUR.toggleIconInactiveColor;
            highlight.color = _COLOUR.transparentColor;
        }

        if (GetComponent<Coffee.UIExtensions.ShinyEffectForUGUI>() != null)
            GetComponent<Coffee.UIExtensions.ShinyEffectForUGUI>().enabled = _interactable;
    }

    public virtual void OnPointerEnter(PointerEventData eventData) { }
    public virtual void OnPointerExit(PointerEventData eventData) { }
}
