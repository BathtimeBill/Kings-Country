using UnityEngine;
using TMPro;
using DG.Tweening;

public class TaskLine : GameBehaviour
{
    public TutorialID taskID;
    public TMP_Text text;
    public GameObject checkFill;
    private Tweener textTweener;

    private void Start()
    {
        text.alpha = 0.1f;
        checkFill.SetActive(false);
    }

    public void SetText(string _text)
    {
        text.text = _text;
    }

    public void ActivateTask()
    {
        TweenX.TweenColor(text, _SETTINGS.colours.titleColor);
        PulseTask();
    }

    public void CheckOffTask()
    {
        _SM.PlaySound(_SM.taskCompleteSound);
        checkFill.transform.localScale = Vector3.one * 2;
        checkFill.SetActive(true);
        checkFill.transform.DOScale(Vector3.one, 1.5f).SetEase(Ease.OutBack);
        TweenX.TweenColor(text, _SETTINGS.colours.descriptionColor);
    }

    public void PulseTask()
    {
        TweenX.KillTweener(textTweener);
        textTweener = text.transform.DOScale(Vector3.one * 1.2f, _TWEENING.UITweenTime).SetEase(Ease.OutBack).OnComplete(()=> text.transform.DOScale(Vector3.one, _TWEENING.UITweenTime).SetEase(Ease.InBack).SetDelay(0.5f));
    }
}
