using UnityEngine;
using TMPro;
using DG.Tweening;

public class TogglePanel : GameBehaviour
{
    public GameObject unitPanel;

    public CanvasGroup homeTreePanel;
    public CanvasGroup hutPanel;
    public CanvasGroup horgrPanel;

    public float showPos = 30f;
    public float hidePos = -316f;
    public float tweenTime = 0.2f;
    public Ease ease;
    Tweener panelTweener;

    private void Start()
    {
        unitPanel.transform.DOMoveY(hidePos, 0);
    }

    public void ShowPanel(BuildingID _baseID)
    {
        switch(_baseID) 
        {
            case BuildingID.HomeTree:
                ShowPanel(homeTreePanel, true);
                ShowPanel(hutPanel, false);
                ShowPanel(horgrPanel, false);
                break;
            case BuildingID.Hut:
                ShowPanel(homeTreePanel, false);
                ShowPanel(hutPanel, true);
                ShowPanel(horgrPanel, false);
                break;
            case BuildingID.Hogyr:
                ShowPanel(homeTreePanel, false);
                ShowPanel(hutPanel, false);
                ShowPanel(horgrPanel, true);
                break;
        }
        TweenPanel(true);
        _SM.PlaySound(_SM.openMenuSound);
    }

    void TweenPanel(bool _show)
    {
        KillTweener(panelTweener);
        panelTweener = unitPanel.transform.DOMoveY(_show ? showPos : hidePos, tweenTime).SetEase(ease).SetUpdate(true);
    }

    void ShowPanel(CanvasGroup _canvas, bool _show)
    {
        _canvas.alpha = _show ? 1 : 0;
        _canvas.interactable = _show;
        _canvas.blocksRaycasts = _show;
    }

    private void OnHomeTreeSelected()
    {
        ShowPanel(BuildingID.HomeTree);
    }
    private void OnHutSelected()
    {
        ShowPanel(BuildingID.Hut);
    }
    private void OnHorgrSelected()
    {
        ShowPanel(BuildingID.Hogyr);
    }
    private void OnGroundClicked()
    {
        _SM.PlaySound(_SM.closeMenuSound);
        TweenPanel(false);
    }

    private void OnEnable()
    {
        GameEvents.OnHomeTreeSelected += OnHomeTreeSelected;
        GameEvents.OnHutSelected += OnHutSelected;
        GameEvents.OnHorgrSelected += OnHorgrSelected;
        GameEvents.OnGroundClicked += OnGroundClicked;
    }

    private void OnDisable()
    {
        GameEvents.OnHomeTreeSelected += OnHomeTreeSelected;
        GameEvents.OnHutSelected -= OnHutSelected;
        GameEvents.OnHorgrSelected -= OnHorgrSelected;
        GameEvents.OnGroundClicked -= OnGroundClicked;
    }

}
