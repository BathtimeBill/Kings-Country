using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    public TMP_Text maegenText;
    public TMP_Text treesText;
    public TMP_Text wildlifeText;
    public TMP_Text populousText;
    public TMP_Text waveText;
    public TMP_Text enemyProgressText;

    [Header("Error")]
    public TMP_Text errorText;
    Animator errorAnim;

    [Header("Bottom")]
    public TMP_Text unitPanelMaegenText;
    public GameObject combatOptionsPanel;
    public bool mouseOverCombatOptions;
    public GameObject menuPanel;
    public GameObject homeTreePanel;
    public GameObject homeTreeUnitPanel;
    public GameObject homeTreeUpgradePanel;
    public GameObject horgrPanel;
    public GameObject hutPanel;

    [Header("Pause")]
    public CanvasGroup inGameCanvas;
    public CanvasGroup pauseBlackoutPanel;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    [HideInInspector]
    public GameObject warningPanel; //Used for escape pausing. Auto set.
    public GameObject unitsPanel;
    
    public GameObject eldyrButton;
    public GameObject ObjectiveText;

    public GameObject transformText;

    public AudioSource audioSource;
    public AudioSource warningAudioSource;
    public GameObject deathCameraRotator;

    [Header("Tools")]
    public ToolButton fyreTool;
    public int fyreCost;
    public float fyreTimeLeft;
    public bool fyreAvailable;

    public ToolButton stormerTool;
    public int stormerCost;
    public float stormerTimeLeft;
    public bool stormerAvailable;

    public ToolButton runeTool;
    public float runeTimeLeft;
    public bool runeAvailable;

    public ToolButton treeTool;
    public CanvasGroup treePercentageModifier;
    public TMP_Text treePercentageText;
    public TMP_Text treeResultText;
    public TMP_Text treeCostText;

    [Header("Waves")]
    public TMP_Text waveTitleText;
    
    public Animator waveTextAnimator;
    public Button dayNightButton;
    public Button treetoolButton;

    [Header("Day End Stats")]
    public CanvasGroup waveResultsPanel;
    public CanvasGroup treeResultCanvas;
    public CanvasGroup maegenBonusCanvas;
    public CanvasGroup maegenTotalCanvas;
    public CanvasGroup wildlifeResultCanvas;
    public int totalMaegen;
    public int totalTrees;
    public int totalMaegenDrops;
    public TMP_Text totalMaegenText;
    public TMP_Text totalTreesText;
    public TMP_Text treeBonusText;
    public TMP_Text totalMaegenDropsText;
    public TMP_Text penaltyText;
    public Button continueButton;

    [Header("Win Phase Panels")]
    public GameObject wavePanel;
    public GameObject winPanel;
    public GameObject finalScorePanel;
    public GameObject gameOverPanel;
    public TMP_Text winMaegenText;
    public TMP_Text winTreesText;
    public TMP_Text winWildlifeText;
    public TMP_Text winScoreText;
    public TMP_Text winHighScoreText;

    [Header("Upgrade")]
    public PerkPanel upgradePanel;
    public PerkButton upgradeButton1;
    public PerkButton upgradeButton2;

    [Header("Current Perk Panel")]
    public GameObject barkSkin;
    public GameObject flyFoot;
    public GameObject power;
    public GameObject tower;
    public GameObject rune;
    public GameObject fyre;
    public GameObject stormer;
    public GameObject tree;
    public GameObject fertile;
    public GameObject populous;
    public GameObject windfall;
    public GameObject homeTree;

    [Header("Mouse Over UI")]
    public bool mouseOverUI;
    public List<GameObject> elements;
    [Header("Formations")]
    public GameObject formationObject;
    private bool largeFormationSelected;
    public Image formationButtonImage;
    public Sprite smallFormation;
    public Sprite bigFormation;
    [Header("Unit Prices")]
    public TMP_Text satyrPriceText;
    public TMP_Text orcusPriceText;
    public TMP_Text leshyPriceText;
    public TMP_Text skessaPriceText;
    public TMP_Text goblinPriceText;
    public TMP_Text huldraPriceText;
    public TMP_Text mistcalfPriceText;
    public TMP_Text fidhainPriceText;

    [Header("Panel Tweening")]
    public Ease panelEase = Ease.InOutSine;
    public Ease boxEase = Ease.InExpo;
    public Ease maegenEase = Ease.OutExpo;
    public float maegenTweenTime = 1f;
    public float statsinTweenTime = 0.5f;
    public float canvasFadeTime = 0.2f;
    public float panelStartPositionX;
    public float panelEndPositionX;
    public float panelShowPositionX;
    public float buttonPulseScale = 1.2f;
    public float buttonPulseSpeed = 1f;

    Tweener waveScoreTweener;
    Tweener upgradeButtonTweener;
    Tweener continueTweener;
    Tweener winPhasePanelTweener;
    Tweener inGameCanvasTweener;
    Tweener blackoutCanvasTweener;
    Tweener fadeTweener;
    Tweener maegenTweener, maegenColourTweener;
    Tweener errorTweener;

    public string dayMessage = "Day";
    public string nightMessage = "Night";
    public string dayProgressMessage = "Humans Vanquished: ";

    private new void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        CheckTreeUI();
        CheckWildlifeUI();
        CheckPopulousUI();
        CheckWave();
        CheckUnitPrices();
        fyreTimeLeft = 0;
        stormerTimeLeft = 0;

        fyreTool.SetInteractable(_GM.fyreAvailable);
        stormerTool.SetInteractable(_GM.stormerAvailable);
        runeTool.SetInteractable(_GM.runesAvailable);

        formationObject = GameObject.FindGameObjectWithTag("Destination");

        ResetPanel(wavePanel);
        ResetPanel(winPanel);
        ResetPanel(finalScorePanel);
        ResetPanel(gameOverPanel);
        TurnOffIcons();
        waveText.text = nightMessage;

        treePercentageModifier.alpha = 0f;
        treePercentageModifier.gameObject.SetActive(false);

        errorAnim = errorText.GetComponent<Animator>();
        errorText.DOFade(0, 0);

        _SETTINGS.colours.ChangePanelColour(_SAVE.GetPanelColour(), _SAVE);
    }

    private void TurnOffIcons()
    {
        barkSkin.SetActive(false);
        flyFoot.SetActive(false);
        power.SetActive(false);
        tower.SetActive(false);
        rune.SetActive(false);
        fyre.SetActive(false);
        stormer.SetActive(false);
        tree.SetActive(false);
        fertile.SetActive(false);
        populous.SetActive(false);
        windfall.SetActive(false);
        homeTree.SetActive(false);
    }


    void Update()
    {
        FyreCheck();
        StormerCheck();
        RuneCheck();
    }

    public void UpdateMaegenText(int _oldValue, int _newValue)
    {
        TweenX.KillTweener(maegenTweener);
        TweenX.KillTweener(maegenColourTweener);
        int total = _oldValue;
        maegenTweener = DOTween.To(() => total, x => total = x, _newValue, maegenTweenTime).SetEase(maegenEase).OnUpdate(() =>
        {
            maegenText.text = total.ToString();
            unitPanelMaegenText.text = total.ToString();
        });
        maegenText.color = Color.green;
        unitPanelMaegenText.color = Color.green;
        maegenText.DOColor(Color.white, maegenTweenTime).SetEase(maegenEase);
        unitPanelMaegenText.DOColor(Color.white, maegenTweenTime).SetEase(maegenEase);
    }


    private void OnGameStateChanged(GameState _gameState)
    {
        switch(_gameState)
        {
            case GameState.Pause:
                TweenX.KillTweener(inGameCanvasTweener);
                inGameCanvas.interactable = false;
                inGameCanvasTweener = inGameCanvas.DOFade(0, 0.2f).SetUpdate(true);
                TweenX.KillTweener(blackoutCanvasTweener);
                blackoutCanvasTweener = pauseBlackoutPanel.DOFade(0.5f, 0.2f).SetUpdate(true);
                break;
            case GameState.Play:
                dayNightButton.interactable = false;
                TweenX.KillTweener(inGameCanvasTweener);
                inGameCanvas.interactable = true;
                inGameCanvasTweener = inGameCanvas.DOFade(1, 0.2f).SetUpdate(true);
                TweenX.KillTweener(blackoutCanvasTweener);
                blackoutCanvasTweener = pauseBlackoutPanel.DOFade(0, 0.2f).SetUpdate(true);
                break;
            case GameState.Build:
                dayNightButton.interactable = true;
                TweenX.KillTweener(inGameCanvasTweener);
                inGameCanvas.interactable = true;
                inGameCanvasTweener = inGameCanvas.DOFade(1, 0.2f).SetUpdate(true);
                TweenX.KillTweener(blackoutCanvasTweener);
                blackoutCanvasTweener = pauseBlackoutPanel.DOFade(0, 0.2f).SetUpdate(true);
                break;
            case GameState.Finish:
                TweenX.KillTweener(inGameCanvasTweener);
                inGameCanvas.interactable = false;
                inGameCanvasTweener = inGameCanvas.DOFade(0, 0.2f).SetUpdate(true);
                TweenX.KillTweener(blackoutCanvasTweener);
                blackoutCanvasTweener = pauseBlackoutPanel.DOFade(0.5f, 0.2f).SetUpdate(true);
                break;
            case GameState.Glossary:
                TweenX.KillTweener(inGameCanvasTweener);
                inGameCanvas.interactable = false;
                inGameCanvasTweener = inGameCanvas.DOFade(0, 0.2f).SetUpdate(true);
                TweenX.KillTweener(blackoutCanvasTweener);
                blackoutCanvasTweener = pauseBlackoutPanel.DOFade(0.5f, 0.2f).SetUpdate(true);
                break;
            case GameState.Tutorial:
                TweenX.KillTweener(inGameCanvasTweener);
                inGameCanvas.interactable = false;
                inGameCanvasTweener = inGameCanvas.DOFade(0, 0.2f).SetUpdate(true);
                TweenX.KillTweener(blackoutCanvasTweener);
                //blackoutCanvasTweener = pauseBlackoutPanel.DOFade(0.5f, 0.2f).SetUpdate(true);
                break;
        }
    }

    private void CheckUnitPrices()
    {
        satyrPriceText.text = _DATA.GetUnit(CreatureID.Satyr).cost.ToString();
        orcusPriceText.text = _DATA.GetUnit(CreatureID.Orcus).cost.ToString();
        leshyPriceText.text = _DATA.GetUnit(CreatureID.Leshy).cost.ToString();
        skessaPriceText.text = _DATA.GetUnit(CreatureID.Skessa).cost.ToString();
        goblinPriceText.text = _DATA.GetUnit(CreatureID.Goblin).cost.ToString();
        huldraPriceText.text = _DATA.GetUnit(CreatureID.Huldra).cost.ToString();
        mistcalfPriceText.text = _DATA.GetUnit(CreatureID.Mistcalf).cost.ToString();
        fidhainPriceText.text = _DATA.GetUnit(CreatureID.Fidhain).cost.ToString();
}
    public void OnFormationSelected()
    {
        if(!largeFormationSelected)
        {
            formationObject.transform.localScale = Vector3.one * 16;
            largeFormationSelected = true;
            formationButtonImage.sprite = bigFormation;
        }
        else
        {
            formationObject.transform.localScale = Vector3.one * 6;
            largeFormationSelected = false;
            formationButtonImage.sprite = smallFormation;
        }
    }
    public void MouseOverCombatOptions()
    {
        mouseOverCombatOptions = true;
    }
    public void MouseExitCombatOptions()
    {
        mouseOverCombatOptions = false;
    }
    public void ContinueButton()
    {
        GameEvents.ReportOnContinueButton();
        //waveOverPanel.SetActive(false);
    }
    public void TriggerWaveTextAnimation()
    {
        waveTextAnimator.SetTrigger("newWave");
    }
    public void EnableTowerText()
    {
        transformText.SetActive(true);
    }
    public void DisableTowerText()
    {
        transformText.SetActive(false);
    }

    public void OnGameOver()
    {
        TweenInPanel(gameOverPanel);
        audioSource.clip = _SM.gameOverSound;
        audioSource.Play();
        gameOverPanel.SetActive(true);
        Time.timeScale = 4;
        deathCameraRotator.SetActive(true);
        //_GM.gameState = GameState.Pause;
    }    

    public void OpenUpgradeMenu()
    {
        Debug.Log("OpeningUpgradeMenu");
        homeTreeUpgradePanel.SetActive(true);
        homeTreeUnitPanel.SetActive(false);
        audioSource.clip = _SM.buttonClickSound;
        audioSource.Play();
    }
    public void OpenUnitMenu()
    {
        homeTreeUnitPanel.SetActive(true);
        homeTreeUpgradePanel.SetActive(false);
        audioSource.clip = _SM.buttonClickSound;
        audioSource.Play();
    }

    public void CloseHomeTreeMenu()
    {
        homeTreePanel.SetActive(false);
        audioSource.clip = _SM.closeMenuSound;
        audioSource.Play();
        GameEvents.ReportOnHomeTreeDeselected();
    }
    public void CloseHorgrMenu()
    {
        horgrPanel.SetActive(false);
        audioSource.clip = _SM.closeMenuSound;
        audioSource.Play();
        GameEvents.ReportOnHorgrDeselected();
    }
    public void CloseHutMenu()
    {
        hutPanel.SetActive(false);
        audioSource.clip = _SM.closeMenuSound;
        audioSource.Play();
        GameEvents.ReportOnHutDeselected();
    }

    public void TogglePanel(GameObject _panel, bool _on)
    {
        _panel.SetActive(_on);
    }

    //Used for when we use the escape key to go back
    public void TurnOffWarningPanel()
    {
        if (warningPanel != null)
        {
            warningPanel.SetActive(false);
            warningPanel = null;
        }
    }
    public void SetWarningPanel(GameObject _warningPanel)
    {
        warningPanel = _warningPanel;
    }

    public void CheckWave()
    {
        waveText.text = dayMessage + ": " + _GM.currentDay.ToString() + "/" + _DATA.levelMaxDays;
        TriggerWaveTextAnimation();
    }

    public void CheckEldyr()
    {
        if(_GM.maegen >= 5000 && _GM.wildlife >= 50)
        {
            eldyrButton.SetActive(true);
            ObjectiveText.SetActive(false);
        }
    }
    public void CheckPopulousUI()
    {
        StartCoroutine(WaitForPopulousCheck());
    }
    IEnumerator WaitForPopulousCheck()
    {
        yield return new WaitForEndOfFrame();
        populousText.text = _GM.CheckPopulous().ToString() + "/" + _GM.maxPopulous;
    }
    public void CheckTreeUI()
    {
        treesText.text = _GM.trees.Count.ToString() + "/" + _GM.maxTrees;
    }
    public void CheckWildlifeUI()
    {
        wildlifeText.text = _GM.wildlife.ToString();
    }


    #region error messages
    public void SetError(ErrorID _errorID)
    {
        string errorMessage = "";
        AudioClip errorClip = _SM.warningSound;
        switch (_errorID)
        {
            case ErrorID.TooClose:
                errorMessage = "";
                break;
            case ErrorID.TooFar:
                errorMessage = "Too far away from the forest";
                break;
            case ErrorID.InsufficientMaegen:
                errorMessage = "Not enough Maegen";
                break;
            case ErrorID.InsufficientResources:
                errorMessage = "Not enough resources";
                break;
            case ErrorID.MaxPopulation:
                errorMessage = "You are at maximum population";
                break;
            case ErrorID.ToolCooldown:
                errorMessage = "Can't place until cooldown has ended";
                break;
            case ErrorID.TooManyTrees:
                errorMessage = "There are too many trees in the forest";
                break;
            case ErrorID.CantPlaceTrees:
                errorMessage = "Can't place trees while the enemy is attacking";
                break;
            case ErrorID.ForestUnderAttack:
                errorMessage = "Your forest is under attack!";
                break;
            case ErrorID.WildlifeUnderAttack:
                errorMessage = "Your wildlife is under attack!";
                break;
            case ErrorID.ClaimSite:
                errorMessage = "You need to claim this site";
                break;
            case ErrorID.TooCloseToTower:
                errorMessage = "Too close to another tower";
                break;
            case ErrorID.OutOfBounds:
                errorMessage = "Too far away from the Home Tree";
                break;
            case ErrorID.SpyClose:
                errorMessage = "A Spy is close by!";
                break;
            default:
                errorMessage = "";
                break;

        }

        errorText.text = errorMessage;
        _SM.PlaySound(errorClip);
        errorAnim.SetTrigger("Error");

        //errorTweener.Restart();
        //errorTweener = errorText.DOFade(1, _TWEENING.errorTweenTime).SetEase(_TWEENING.errorTweenEase).OnComplete(() => errorText.DOFade(0, _TWEENING.errorTweenTime).SetEase(_TWEENING.errorTweenEase).SetDelay(_TWEENING.errorTweenDuration));
        //TweenX.KillTweener(errorTweener);
    }

    //public void SetErrorMessageTooClose()
    //{
    //    errorText.text = "";
    //}
    //public void SetErrorMessageTooFar()
    //{
    //    errorText.text = "Too far away from the forest";
    //}
    //public void SetErrorMessageInsufficientMaegen()
    //{
    //    errorText.text = "Not enough Maegen";
    //}
    //public void SetErrorMessageMaxPop()
    //{
    //    errorText.text = "You are at maximum population";
    //}

    //public void SetErrorMessageInsufficientResources()
    //{
    //    errorText.text = "Not enough resources";
    //}
    //public void SetErrorMessageBeaconCooldown()
    //{
    //    errorText.text = "Can't place until cooldown has ended";
    //}
    //public void SetErrorMessageTooManyTrees()
    //{
    //    errorText.text = "There are too many trees in the forest";
    //}
    //public void SetErrorMessageYouAreUnderAttack()
    //{
    //    errorText.text = "Your forest is under attack!";
    //}
    //public void SetErrorMessageYourWildlifeIsUnderAttack()
    //{
    //    errorText.text = "Your wildlife is under attack!";
    //}
    //public void SetErrorMessageNeedToClaimHorgr()
    //{
    //    errorText.text = "You need to claim this site";
    //}
    //public void SetErrorMessageCantPlaceTrees()
    //{
    //    errorText.text = "Can't place trees while the enemy is attacking";
    //}
    //public void SetErrorMessageTooCloseToTower()
    //{
    //    errorText.text = "Too close to another tower";
    //}
    //public void SetErrorMessageOutOfBounds()
    //{
    //    errorText.text = "Too far away from the Home Tree";
    //}
    //public void SetErrorMessageSpy()
    //{
    //    errorText.text = "A Spy is close by!";
    //}
    //public void SetErrorMessageCannotPlace()
    //{
    //    errorText.text = "";
    //}
    #endregion

    #region Wave Over


    public void OnWaveOver()
    {
        ExecuteNextFrame(() =>
        {
            if (gameFinished)
            {
                return;
            }

            ResetPanel(wavePanel);
            upgradePanel.transform.localScale = Vector3.one * 2;
            upgradePanel.canvasGroup.alpha = 0;
            upgradeButton1.SetInteractable(false);
            upgradeButton2.SetInteractable(false);
            continueButton.interactable = false;
            treeResultCanvas.alpha = 0;
            maegenBonusCanvas.alpha = 0;
            maegenTotalCanvas.alpha = 0;
            wildlifeResultCanvas.alpha = 0;
            waveResultsPanel.alpha = 0;
            waveResultsPanel.transform.localScale = Vector3.one * 2;

            SetWaveEndStats();
        });

    }

    private void SetWaveEndStats()
    {
        TweenInPanel(wavePanel);
        _SM.PlaySound(_SM.waveOverSound);
        _SM.PlaySound(_SM.menuDragSound);
        waveTitleText.text = dayMessage + " " + _GM.currentDay.ToString() + " is complete!";

        totalTrees = _GM.trees.Count;
        totalMaegenDrops = GameObject.FindGameObjectsWithTag("MaegenDrop").Length;
        totalMaegen = totalTrees + totalMaegenDrops + GetTreeBonusTotal();

        penaltyText.text = "+" + _FM.numberOfWildlifeToSpawn.ToString();
        totalTreesText.text = totalTrees.ToString();
        treeBonusText.text = "(+" + GetTreeBonusTotal().ToString() + ")";
        totalMaegenDropsText.text = totalMaegenDrops.ToString();
        totalMaegenText.text = "+" + totalMaegen.ToString();

        StartCoroutine(ActivateTextGroups());
    }

    IEnumerator ActivateTextGroups()
    {
        yield return new WaitForSecondsRealtime(1f);
        waveResultsPanel.DOFade(1, 0.4f).SetUpdate(true);
        waveResultsPanel.transform.DOScale(Vector3.one, 0.6f).SetUpdate(true).SetEase(boxEase);

        Debug.Log("Bringing in text groups");
        yield return new WaitForSecondsRealtime(1.6f);
        _SM.PlaySound(_SM.textGroupSound);
        treeResultCanvas.alpha = 1;//.DOFade(1, panelTweenTime).SetUpdate(true); 
        yield return new WaitForSecondsRealtime(statsinTweenTime);
        maegenBonusCanvas.alpha = 1;//DOFade(1, panelTweenTime).SetUpdate(true);
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime);
        _SM.PlaySound(_SM.textGroupSound);
        maegenTotalCanvas.alpha = 1;//DOFade(1, panelTweenTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(statsinTweenTime);
        _SM.PlaySound(_SM.textGroupSound);
        wildlifeResultCanvas.alpha = 1;//DOFade(1, panelTweenTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(statsinTweenTime);
        ShowUpgradeButtons();
    }

    void ShowUpgradeButtons()
    {
        if (_PERK.CanObtainPerk)
        {
            PerkID perk1 = _PERK.GetRandomPerk();
            _PERK.RemovePerk(perk1);
            PerkID perk2 = _PERK.GetRandomPerk();
            _PERK.RemovePerk(perk2);

            print(upgradeButton1.name);
            print(upgradeButton2.name);
            upgradeButton1.SetUpgrade(perk1);
            upgradeButton2.SetUpgrade(perk2);


        }
        upgradePanel.canvasGroup.DOFade(1, 0.8f).SetUpdate(true);
        upgradePanel.transform.DOScale(Vector3.one, 0.6f).SetUpdate(true).SetEase(boxEase).OnComplete(() =>
        { 
            upgradeButton1.SetInteractable(true);
            upgradeButton2.SetInteractable(true);
        });
    }

    private void TweenUpgradeIcon(GameObject _icon)
    {
        upgradeButton1.SetInteractable(false);
        upgradeButton2.SetInteractable(false);
        _icon.GetComponentInChildren<Image>().color = _SETTINGS.colours.highlightedColor;
        _icon.transform.localScale = Vector3.one * 3;
        _icon.SetActive(true);
        _icon.transform.DOScale(Vector3.one, 1).SetLoops(3).SetUpdate(true).OnComplete(() =>
        _icon.GetComponentInChildren<Image>().DOColor(_SETTINGS.colours.upgradeIconsColor, 0.5f).SetUpdate(true));
        continueButton.interactable = true;
        TweenX.KillTweener(continueTweener);
        continueTweener = continueButton.transform.DOScale(Vector3.one * buttonPulseScale, buttonPulseSpeed).SetUpdate(true).SetLoops(-1).SetDelay(3);
    }

    private int GetTreeBonusTotal()
    {
        int treeBonus = 0;
        foreach (GameObject i in _GM.trees)
        {
            treeBonus = treeBonus + i.GetComponent<Tree>().energyMultiplier;
        }
        int totalTreeBonus = treeBonus - totalTrees;
        return totalTreeBonus;
    }

    public void OnContinueButton()
    {
        TweenOutPanel(wavePanel);
        treeTool.SetInteractable(true);
        TweenX.KillTweener(continueTweener); 
        continueButton.transform.localScale = Vector3.one;
        waveText.text = nightMessage;
    }

    void ResetPanel(GameObject _panel)
    {
        _panel.SetActive(false);
        _panel.transform.DOLocalMoveX(panelStartPositionX, 0).SetUpdate(true);
    }

    void TweenInPanel(GameObject _panel)
    {
        _panel.SetActive(true);
        TweenX.KillTweener(winPhasePanelTweener);
        winPhasePanelTweener = _panel.transform.DOLocalMoveX(panelShowPositionX, statsinTweenTime).SetEase(panelEase).SetUpdate(true);
    }

    void TweenOutPanel(GameObject _panel)
    {
        TweenX.KillTweener(winPhasePanelTweener);
        winPhasePanelTweener = _panel.transform.DOLocalMoveX(panelEndPositionX, statsinTweenTime).SetEase(panelEase).OnComplete(() => ResetPanel(_panel)).SetUpdate(true);
    }

    void FadeInPanel(CanvasGroup _canvas)
    {
        if (_canvas == null) return;

        _canvas.gameObject.SetActive(true);
        TweenX.KillTweener(fadeTweener);
            fadeTweener = _canvas.DOFade(1, canvasFadeTime);
    }

    void FadeOutPanel(CanvasGroup _canvas)
    {
        if (_canvas == null) return;

        TweenX.KillTweener(fadeTweener);
        fadeTweener = _canvas.DOFade(0, canvasFadeTime).OnComplete(()=> _canvas.gameObject.SetActive(false));
    }



    public void ShowTreeModifier(bool _show)
    {
        if (_show) FadeInPanel(treePercentageModifier);
        else FadeOutPanel(treePercentageModifier);
    }


    #endregion

    public void BeginNewDay()
    {
        treeTool.SetInteractable(false);
        int enemyProgressTotal = _EM.GetDayTotalEnemyCount();
        enemyProgressText.text = dayProgressMessage + "0/" + enemyProgressTotal;
    }

    public void OnPerkSelected(PerkID perkID)
    {
        switch (perkID)
        {
            case PerkID.BarkSkin: 
                barkSkin.SetActive(true);
                TweenUpgradeIcon(barkSkin);
                break;
            case PerkID.FlyFoot: 
                flyFoot.SetActive(true);
                TweenUpgradeIcon(flyFoot);
                break;
            case PerkID.Tower: 
                tower.SetActive(true);
                TweenUpgradeIcon(tower);
                break;
            case PerkID.Power: 
                power.SetActive(true);
                TweenUpgradeIcon(power);
                break;
            case PerkID.Rune:
                rune.SetActive(true);
                TweenUpgradeIcon(rune);
                break;
            case PerkID.Fyre:
                fyre.SetActive(true);
                TweenUpgradeIcon(fyre);
                break;
            case PerkID.Stormer:
                stormer.SetActive(true);
                TweenUpgradeIcon(stormer);
                break;
            case PerkID.Tree:
                tree.SetActive(true);
                TweenUpgradeIcon(tree);
                break;
            case PerkID.Fertile:
                fertile.SetActive(true);
                TweenUpgradeIcon(fertile);
                break;
            case PerkID.Populous:
                populous.SetActive(true);
                TweenUpgradeIcon(populous);
                break;
            case PerkID.Winfall:
                windfall.SetActive(true);
                TweenUpgradeIcon(windfall);
                break;
            case PerkID.HomeTree:
                homeTree.SetActive(true);
                TweenUpgradeIcon(homeTree);
                break;
        }

    }
    

    
    private void OnJustStragglers()
    {
        //collectMaegenButton.SetActive(true);
    }
    public void CollectMaegen()
    {
        //GameEvents.ReportOnCollectMaegenButton();
       // collectMaegenButton.SetActive(false);
    }

    //Button Presses from Inspector
    public void ShowResultsPanel()
    {
        winPanel.GetComponentInChildren<Button>().interactable = false;
        TweenInPanel(finalScorePanel);
    }

    public void MouseCancel()
    {
    }

    #region Tools
    public void OnFyrePlaced()
    {
        fyreTimeLeft = _DATA.GetTool(ToolID.Fyre).cooldownTime;
        fyreTool.SetInteractable(false);
        fyreAvailable = false;
    }

    private void FyreCheck()
    {
        if (!fyreAvailable)
        {
            if (fyreTimeLeft >= 0)
            {
                fyreTimeLeft -= Time.deltaTime;
                fyreTool.CooldownFill(MathX.MapTo01(fyreTimeLeft, 0, _DATA.GetTool(ToolID.Fyre).cooldownTime));
            }
            else
            {
                fyreAvailable = _DATA.CanUseTool(ToolID.Fyre);
                fyreTool.SetInteractable(_DATA.CanUseTool(ToolID.Fyre));
            }
        }
    }

    public void OnStormerPlaced()
    {
        stormerTimeLeft = _DATA.GetTool(ToolID.Stormer).cooldownTime;
        stormerTool.SetInteractable(false);
        stormerAvailable = false;
    }

    private void StormerCheck()
    {
        if (!stormerAvailable)
        {
            if (stormerTimeLeft >= 0)
            {
                stormerTimeLeft -= Time.deltaTime;
                stormerTool.CooldownFill(MathX.MapTo01(stormerTimeLeft, 0, _DATA.GetTool(ToolID.Stormer).cooldownTime));
            }
            else
            {
                stormerAvailable = _DATA.CanUseTool(ToolID.Stormer);
                stormerTool.SetInteractable(_DATA.CanUseTool(ToolID.Stormer));
            }
        }
    }

    public void OnRunePlaced()
    {
        runeTimeLeft = _DATA.GetTool(ToolID.Rune).cooldownTime;
        runeTool.SetInteractable(false);
        runeAvailable = false;
        //print(_GM.runesCount + " = " + _GM.runesMaegenCost.Length);
    }

    private void RuneCheck()
    {
        if (!runeAvailable)
        {
            if (runeTimeLeft >= 0)
            {
                runeTimeLeft -= Time.deltaTime;
                runeTool.CooldownFill(MathX.MapTo01(runeTimeLeft, 0, _DATA.GetTool(ToolID.Rune).cooldownTime));
            }
            else
            {
                if (_GM.runesAvailable)
                {
                    runeAvailable = true;
                    runeTool.SetInteractable(true);
                }
            }
        }
    }

    public void ChangeTreePercentagePanel(float _percentage, int _perDay, int _price)
    {
        treePercentageText.text = _percentage.ToString("0.0" + "%");
        treePercentageText.color = _SETTINGS.colours.treePercentageGradient.Evaluate(_percentage);
        treeResultText.text = "Every night, get " + _perDay.ToString() + "  <color=#D3C965><sprite name=\"MaegenIcon\">";
        treeCostText.text = _price.ToString();
    }
    #endregion

    private void OnWildlifeValueChange(int _value)
    {
        wildlifeText.text = _value.ToString();
    }

    private void OnLevelWin(LevelID _levelID, int _score, int _maegen)
    {
        TweenInPanel(winPanel);
    }
    public void UpdateWinUI(int _maegen, int _trees, int _wildlife, int _score)
    {
        winMaegenText.text = "+ " + _maegen.ToString();
        winTreesText.text = _GM.trees.Count.ToString() + " (x " + _trees.ToString() + " to total score)";
        winWildlifeText.text = "+ " + _wildlife.ToString() + " x2 = " + _wildlife * 2;
        winScoreText.text = _score.ToString();
        winHighScoreText.text = _GAMESAVE.GetLevelHighScore(_DATA.currentLevelID).ToString();
    }

    private void OnEnemyUnitKilled(Enemy _unitID, string _killedBy)
    {
        ExecuteNextFrame(() =>
        {
            enemyProgressText.text = dayProgressMessage + _EM.currentKillCount + "/" + _EM.GetDayTotalEnemyCount();
        });
    }

    private void OnEnable()
    {
        GameEvents.OnGameStateChanged += OnGameStateChanged;
        GameEvents.OnGameOver += OnGameOver;
        GameEvents.OnLevelWin += OnLevelWin;
        GameEvents.OnFyrePlaced += OnFyrePlaced;
        GameEvents.OnStormerPlaced += OnStormerPlaced;
        GameEvents.OnRunePlaced += OnRunePlaced;
        GameEvents.OnDayOver += OnWaveOver;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnJustStragglers += OnJustStragglers;

        GameEvents.OnUpgradeSelected += OnPerkSelected;

        GameEvents.OnWildlifeValueChange += OnWildlifeValueChange;
        GameEvents.OnFormationSelected += OnFormationSelected;
        GameEvents.OnEnemyUnitKilled += OnEnemyUnitKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
        GameEvents.OnGameOver -= OnGameOver;
        GameEvents.OnLevelWin -= OnLevelWin;
        GameEvents.OnFyrePlaced -= OnFyrePlaced;
        GameEvents.OnStormerPlaced -= OnStormerPlaced;
        GameEvents.OnRunePlaced -= OnRunePlaced;
        GameEvents.OnDayOver -= OnWaveOver;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnJustStragglers -= OnJustStragglers;

        GameEvents.OnUpgradeSelected -= OnPerkSelected;

        GameEvents.OnWildlifeValueChange -= OnWildlifeValueChange;
        GameEvents.OnFormationSelected -= OnFormationSelected;
        GameEvents.OnEnemyUnitKilled -= OnEnemyUnitKilled;
    }
}
