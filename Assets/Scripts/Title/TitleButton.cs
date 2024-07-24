using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButton : InteractableButton
{
    public GameObject buttonTextLabel;
    public bool hideUntilTutorialComplete = false;
    private bool isAcive;
    public override void Start()
    {
        base.Start();
        Vector3 temp = buttonTextLabel.transform.localScale;
        temp.x = 0;
        buttonTextLabel.transform.localScale = temp;

        isAcive = !_SAVE.GetTutorialStatus && hideUntilTutorialComplete ? false : true;
        SetInteractable(isAcive);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        _SM.PlaySound(_SM.buttonClickSound);
        if(isAcive)
            TweenX.TweenTextScale(buttonTextLabel.transform, 1, _TWEENING.UIButtonTweenTime, _TWEENING.UIButtonTweenEase);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (isAcive)
            TweenX.TweenTextScale(buttonTextLabel.transform, 0, _TWEENING.UIButtonTweenTime, _TWEENING.UIButtonTweenEase);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        _SM.PlaySound(_SM.buttonClickSound);
    }

    public void TweenText(bool _in)
    {

    }

    public void ToggleText(bool _show)
    {
        Vector3 temp = buttonTextLabel.transform.localScale;
        temp.x = _show ? 1 : 0;
        buttonTextLabel.transform.localScale = temp;
    }
}
