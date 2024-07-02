using UnityEngine;
using TMPro;

public class TaskLine : GameBehaviour
{
    public TutorialID taskID;
    public TMP_Text text;
    public GameObject checkFill;

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
        TweenX.TweenColor(text, _SETTINGS.colours.descriptionColor);
    }

    public void CheckOffTask()
    {
        _SM.PlaySound(_SM.taskCompleteSound);
        checkFill.SetActive(true);
    }
}
