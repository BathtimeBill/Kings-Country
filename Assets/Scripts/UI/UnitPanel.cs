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

    private void Start()
    {
        homeTreeButton.SetActive(_DATA.currentLevel.availableBuildings.Contains(BuildingID.HomeTree));
        hutButton.SetActive(_DATA.currentLevel.availableBuildings.Contains(BuildingID.Hut));
        horgrButton.SetActive(_DATA.currentLevel.availableBuildings.Contains(BuildingID.Hogyr));
        //TODO hacky workaround. Revisit 
        ExecuteAfterFrames(2, ()=>
        {
            ShowUnitPanel(false);
        });
    }

    public void StartCooldowns()
    {
        cooldownTimeLeft = _GM.treeCooldown;
        for (int i = 0; i < unitButtons.Length; i++)
        {
            unitButtons[i].button.interactable = false;
        }
    }

    void StopCooldowns()
    {
        for (int i = 0; i < unitButtons.Length; i++)
        {
            unitButtons[i].button.interactable = unitButtons[i].unitData.cost <= _GM.maegen;
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
                description.text = _unitButton.unitData.description;
                healthStat.text = _unitButton.unitData.health.ToString();
                damageStat.text = _unitButton.unitData.damage.ToString();
                speedStat.text = _unitButton.unitData.speed.ToString();
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

    public void ShowPanel(BuildingID _baseID)
    {
        switch (_baseID)
        {
            case BuildingID.HomeTree:
                unitButtons[0].SetupButton(_DATA.GetUnit(CreatureID.Satyr));
                unitButtons[1].SetupButton(_DATA.GetUnit(CreatureID.Orcus));
                unitButtons[2].SetupButton(_DATA.GetUnit(CreatureID.Leshy));
                break;
            case BuildingID.Hut:
                unitButtons[0].SetupButton(_DATA.GetUnit(CreatureID.Goblin));
                unitButtons[1].SetupButton(_DATA.GetUnit(CreatureID.Skessa));
                unitButtons[2].SetupButton(_DATA.GetUnit(CreatureID.Fidhain));
                break;
            case BuildingID.Hogyr:
                unitButtons[0].SetupButton(_DATA.GetUnit(CreatureID.Huldra));
                unitButtons[1].SetupButton(_DATA.GetUnit(CreatureID.Mistcalf));
                unitButtons[2].SetupButton(_DATA.GetUnit(CreatureID.Unknown));
                break;
        }
        TweenPanel(true);
        _SM.PlaySound(_SM.openMenuSound);
    }

    void TweenPanel(bool _show)
    {
        TweenX.KillTweener(panelTweener);
        panelTweener = transform.DOMoveY(_show ? showPos : hidePos, _TWEENING.UITweenTime).SetEase(_TWEENING.UITweenEase).SetUpdate(true);
        maegenTotal.DOFade(_show ? 1 : 0, _TWEENING.UITweenTime);
    }

    public void ShowUnitPanel(bool _show)
    {
        transform.position = new Vector3(transform.position.x, _show ? showPos : hidePos, transform.position.z);
        maegenTotal.alpha = _show ? 1 : 0;
    }

    #region Events

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
    #endregion
}
