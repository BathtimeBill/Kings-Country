public class StartLevelButton : HoldButton
{
    private TitleManager titleManager;
    public TMPro.TMP_Text text;

    private void Awake()
    {
        titleManager = FindObjectOfType<TitleManager>();
    }

    public override void OnButtonFilled()
    {
        base.OnButtonFilled();
        SetInteractable(false);
        titleManager.sceneManager.LoadScene(_DATA.GetLevel(titleManager.overworldManager.selectedLevel).levelScene);
        GameEvents.ReportOnLevelStart();
    }

    public override void SetInteractable(bool _interactable)
    {
        base.SetInteractable(false);
        text.text = _interactable ? "Play" : "Locked";
        text.color = _interactable ? _COLOUR.highlightedColor : _COLOUR.cooldownColor;
    }
}
