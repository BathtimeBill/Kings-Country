using DG.Tweening;
using TMPro;
using UnityEngine;

public class UnitPanel : GameBehaviour
{
    [Header("Toggles")]
    public GameObject homeTreeButton;
    public GameObject hutButton;
    public GameObject horgrButton;

    [Header("Unit Buttons")]
    public UnitButton[] unitButtons;
    [Header("Description Panel")]
    public TMP_Text description;
    public TMP_Text healthStat;
    public TMP_Text damageStat;
    public TMP_Text speedStat;
    public string defaultText = "";
    public CanvasGroup maegenTotal;

    private float cooldownTimeLeft = 0;
    [Header("Tweening")]
    public float showPos = 30f;
    public float hidePos = -316f;
    private Tweener panelTweener;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = myRectTransform;
        homeTreeButton.SetActive(_DATA.currentLevel.availableBuildings.Contains(SiteID.HomeTree));
        hutButton.SetActive(_DATA.currentLevel.availableBuildings.Contains(SiteID.Hut));
        horgrButton.SetActive(_DATA.currentLevel.availableBuildings.Contains(SiteID.Horgr));
        ShowUnitPanel(false);
    }

    public void StartCooldowns()
    {
        cooldownTimeLeft = _GAME.treeCooldown;
        for (int i = 0; i < unitButtons.Length; i++)
        {
            unitButtons[i].button.interactable = false;
        }
    }

    void StopCooldowns()
    {
        for (int i = 0; i < unitButtons.Length; i++)
        {
            unitButtons[i].button.interactable = unitButtons[i].guardianData.cost <= _GAME.maegen;
        }
    }

    void Update()
    {
        if (cooldownTimeLeft >= 0)
        {
            cooldownTimeLeft -= Time.deltaTime;
            for (int i = 0; i < unitButtons.Length; i++)
            {
                unitButtons[i].Cooldown(cooldownTimeLeft);
            }
        }
        else
            StopCooldowns();
    }

    public void PointerEnter(UnitButton _unitButton)
    {
        for(int i = 0; i < unitButtons.Length; i++)
        {
            if(_unitButton == unitButtons[i])
            {
                unitButtons[i].FadeInButton();
                description.text = _unitButton.guardianData.description;
                healthStat.text = _unitButton.guardianData.health.ToString();
                damageStat.text = _unitButton.guardianData.damage.ToString();
                speedStat.text = _unitButton.guardianData.speed.ToString();
            }
            else
            {
                unitButtons[i].FadeOutButton();
            }
        }
    }

    public void PointerExit()
    {
        for (int i = 0; i < unitButtons.Length; i++)
        {
            unitButtons[i].FadeInButton();
            description.text = defaultText;
            healthStat.text = "-";
            damageStat.text = "-";
            speedStat.text = "-";
        }
    }

    public void ShowPanel(SiteID _baseID)
    {
        switch (_baseID)
        {
            case SiteID.HomeTree:
                unitButtons[0].SetupButton(_DATA.GetUnit(GuardianID.Satyr));
                unitButtons[1].SetupButton(_DATA.GetUnit(GuardianID.Orcus));
                unitButtons[2].SetupButton(_DATA.GetUnit(GuardianID.Leshy));
                break;
            case SiteID.Hut:
                unitButtons[0].SetupButton(_DATA.GetUnit(GuardianID.Goblin));
                unitButtons[1].SetupButton(_DATA.GetUnit(GuardianID.Skessa));
                unitButtons[2].SetupButton(_DATA.GetUnit(GuardianID.Fidhain));
                break;
            case SiteID.Horgr:
                unitButtons[0].SetupButton(_DATA.GetUnit(GuardianID.Huldra));
                unitButtons[1].SetupButton(_DATA.GetUnit(GuardianID.Mistcalf));
                unitButtons[2].SetupButton(_DATA.GetUnit(GuardianID.Unknown));
                break;
        }
        TweenPanel(true);
        _SM.PlaySound(_SM.openMenuSound);
    }

    void TweenPanel(bool _show)
    {
        TweenX.KillTweener(panelTweener);
        panelTweener = rectTransform.DOAnchorPosY(_show ? showPos : hidePos, _TWEENING.UITweenTime).SetEase(_TWEENING.UITweenEase).SetUpdate(true);
        maegenTotal.DOFade(_show ? 1 : 0, _TWEENING.UITweenTime);
    }

    public void ShowUnitPanel(bool _show)
    {
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, _show ? showPos : hidePos);
        maegenTotal.alpha = _show ? 1 : 0;
    }

    #region Events

    private void OnHomeTreeSelected()
    {
        ShowPanel(SiteID.HomeTree);
    }
    private void OnHutSelected()
    {
        ShowPanel(SiteID.Hut);
    }
    private void OnHorgrSelected()
    {
        ShowPanel(SiteID.Horgr);
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
        GameEvents.OnHomeTreeSelected -= OnHomeTreeSelected;
        GameEvents.OnHutSelected -= OnHutSelected;
        GameEvents.OnHorgrSelected -= OnHorgrSelected;
        GameEvents.OnGroundClicked -= OnGroundClicked;
    }
    #endregion
}
