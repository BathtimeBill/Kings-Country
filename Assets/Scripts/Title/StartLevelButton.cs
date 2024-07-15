public class StartLevelButton : HoldButton
{
    private TitleManager titleManager;
    public TMPro.TMP_Text text;

    public override void Awake()
    {
        base.Awake();
        titleManager = FindObjectOfType<TitleManager>();
    }

    public override void OnButtonFilled()
    {
        base.OnButtonFilled();
        SetInteractable(false);
        titleManager.sceneManager.LoadScene(_DATA.GetLevel(titleManager.overworldManager.selectedLevel).levelReference);
        GameEvents.ReportOnLevelStart();
    }

    public override void SetInteractable(bool _interactable)
    {
        base.SetInteractable(_interactable);
        text.text = _interactable ? "Play" : "Locked";
        text.color = _interactable ? _COLOUR.highlightedColor : _COLOUR.cooldownColor;
    }
}
