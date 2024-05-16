using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    public UISettings UISettings;
    public TMP_Text maegenText;
    public TMP_Text treesText;
    public TMP_Text wildlifeText;
    public TMP_Text populousText;
    public TMP_Text waveText;
    public TMP_Text errorText;


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
    public float fyreMaxTimeLeft;
    public bool fyreAvailable;

    public ToolButton stormerTool;
    public int stormerCost;
    public float stormerTimeLeft;
    public float stormerMaxTimeLeft;
    public bool stormerAvailable;

    public ToolButton runeTool;
    public float runeTimeLeft;
    public float runeMaxTimeLeft;
    public bool runeAvailable;

    public ToolButton treeTool;

    [Header("Misc")]
    public GameObject maegenCost;
    public TMP_Text maegenCostText;
    public GameObject wildlifeCost;
    public TMP_Text wildlifeCostText;

    [Header("Waves")]
    public TMP_Text waveTitleText;
    
    public Animator waveTextAnimator;
    public Button nextRoundButton;
    public Button treetoolButton;

    [Header("Wave End Stats")]
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

    [Header("Upgrade")]
    public UpgradePanel upgradePanel;
    public UpgradeButton upgradeButton1;
    public UpgradeButton upgradeButton2;

    [Header("Current Upgrade Panel")]
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
    public TMP_Text golemPriceText;
    public TMP_Text dryadPriceText;

    [Header("Panel Tweening")]
    public Ease panelEase = Ease.InOutSine;
    public Ease boxEase = Ease.InExpo;
    public float panelTweenTime = 0.5f;
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

    public void UpdateMaegenText(int _value)
    {
        maegenText.text = _value.ToString();
    }

    private void OnGameStateChanged(GameState gameState)
    {
        throw new System.NotImplementedException();
    }

    private void CheckUnitPrices()
    {
        satyrPriceText.text = _GM.satyrPrice.ToString();
        orcusPriceText.text = _GM.orcusPrice.ToString();
        leshyPriceText.text = _GM.leshyPrice.ToString();
        skessaPriceText.text = _GM.skessaPrice.ToString();
        goblinPriceText.text = _GM.goblinPrice.ToString();
        huldraPriceText.text = _GM.huldraPrice.ToString();
        golemPriceText.text = _GM.golemPrice.ToString();
        dryadPriceText.text = _GM.dryadPrice.ToString();
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
        waveText.text = "Wave: " + _GM.currentWave.ToString();
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
    public void SetErrorMessageTooClose()
    {
        errorText.text = "";
    }
    public void SetErrorMessageTooFar()
    {
        errorText.text = "Too far away from the forest";
    }
    public void SetErrorMessageInsufficientMaegen()
    {
        errorText.text = "Not enough Maegen";
    }
    public void SetErrorMessageMaxPop()
    {
        errorText.text = "You are at maximum population";
    }

    public void SetErrorMessageInsufficientResources()
    {
        errorText.text = "Not enough resources";
    }
    public void SetErrorMessageBeaconCooldown()
    {
        errorText.text = "Can't place until cooldown has ended";
    }
    public void SetErrorMessageTooManyTrees()
    {
        errorText.text = "There are too many trees in the forest";
    }
    public void SetErrorMessageYouAreUnderAttack()
    {
        errorText.text = "Your forest is under attack!";
    }
    public void SetErrorMessageYourWildlifeIsUnderAttack()
    {
        errorText.text = "Your wildlife is under attack!";
    }
    public void SetErrorMessageNeedToClaimHorgr()
    {
        errorText.text = "You need to claim this site";
    }
    public void SetErrorMessageCantPlaceTrees()
    {
        errorText.text = "Can't place trees while the enemy is attacking";
    }
    public void SetErrorMessageTooCloseToTower()
    {
        errorText.text = "Too close to another tower";
    }
    public void SetErrorMessageOutOfBounds()
    {
        errorText.text = "Too far away from the Home Tree";
    }
    public void SetErrorMessageSpy()
    {
        errorText.text = "A Spy is close by!";
    }
    public void SetErrorMessageCannotPlace()
    {
        errorText.text = "";
    }
    #endregion

    #region Wave Over


    public void OnWaveOver()
    {
        //if (_GM.currentWave == _GM.winLevel)
        //{
        //    GameEvents.ReportOnGameWin();
        //    return;
        //}

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
    }

    private void SetWaveEndStats()
    {
        TweenInPanel(wavePanel);
        _SM.PlaySound(_SM.waveOverSound);
        _SM.PlaySound(_SM.menuDragSound);
        waveTitleText.text = "Wave " + _GM.currentWave.ToString() + " is complete!";

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
        yield return new WaitForSecondsRealtime(panelTweenTime + 0.1f);
        maegenBonusCanvas.alpha = 1;//DOFade(1, panelTweenTime).SetUpdate(true);
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(panelTweenTime + 0.1f);
        _SM.PlaySound(_SM.textGroupSound);
        maegenTotalCanvas.alpha = 1;//DOFade(1, panelTweenTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(panelTweenTime + 0.1f);
        _SM.PlaySound(_SM.textGroupSound);
        wildlifeResultCanvas.alpha = 1;//DOFade(1, panelTweenTime).SetUpdate(true);
        yield return new WaitForSecondsRealtime(panelTweenTime + 1f);
        ShowUpgradeButtons();
    }

    void ShowUpgradeButtons()
    {
        if (_UM.availableUpgrades.Count > 1)
        {
            UpgradeID upgrade1 = _UM.GetRandomUpgrade();
            _UM.RemoveUpgrade(upgrade1);
            UpgradeID upgrade2 = _UM.GetRandomUpgrade();
            _UM.RemoveUpgrade(upgrade2);

            print(upgradeButton1.name);
            print(upgradeButton2.name);
            upgradeButton1.SetUpgrade(upgrade1);
            upgradeButton2.SetUpgrade(upgrade2);


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
        _icon.GetComponentInChildren<Image>().color = UISettings.highlightedColor;
        _icon.transform.localScale = Vector3.one * 3;
        _icon.SetActive(true);
        _icon.transform.DOScale(Vector3.one, 1).SetLoops(3).SetUpdate(true).OnComplete(() =>
        _icon.GetComponentInChildren<Image>().DOColor(UISettings.upgradeIconsColor, 0.5f).SetUpdate(true));
        continueButton.interactable = true;
        KillTweener(continueTweener);
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
        KillTweener(continueTweener); 
        continueButton.transform.localScale = Vector3.one;
    }

    void ResetPanel(GameObject _panel)
    {
        _panel.SetActive(false);
        _panel.transform.DOLocalMoveX(panelStartPositionX, 0).SetUpdate(true);
    }

    void TweenInPanel(GameObject _panel)
    {
        _panel.SetActive(true);
        KillTweener(winPhasePanelTweener);
        winPhasePanelTweener = _panel.transform.DOLocalMoveX(panelShowPositionX, panelTweenTime).SetEase(panelEase).SetUpdate(true);
    }

    void TweenOutPanel(GameObject _panel)
    {
        KillTweener(winPhasePanelTweener);
        winPhasePanelTweener = _panel.transform.DOLocalMoveX(panelEndPositionX, panelTweenTime).SetEase(panelEase).OnComplete(() => ResetPanel(_panel)).SetUpdate(true);
    }


    #endregion

    private void OnWaveBegin()
    {
        treeTool.SetInteractable(false);
    }

    public void OnUpgradeSelected(UpgradeID upgradeID)
    {
        switch (upgradeID)
        {
            case UpgradeID.BarkSkin: 
                barkSkin.SetActive(true);
                TweenUpgradeIcon(barkSkin);
                break;
            case UpgradeID.FlyFoot: 
                flyFoot.SetActive(true);
                TweenUpgradeIcon(flyFoot);
                break;
            case UpgradeID.Tower: 
                tower.SetActive(true);
                TweenUpgradeIcon(tower);
                break;
            case UpgradeID.Power: 
                power.SetActive(true);
                TweenUpgradeIcon(power);
                break;
            case UpgradeID.Rune:
                rune.SetActive(true);
                TweenUpgradeIcon(rune);
                break;
            case UpgradeID.Fyre:
                fyre.SetActive(true);
                TweenUpgradeIcon(fyre);
                break;
            case UpgradeID.Stormer:
                stormer.SetActive(true);
                TweenUpgradeIcon(stormer);
                break;
            case UpgradeID.Tree:
                tree.SetActive(true);
                TweenUpgradeIcon(tree);
                break;
            case UpgradeID.Fertile:
                fertile.SetActive(true);
                TweenUpgradeIcon(fertile);
                break;
            case UpgradeID.Populous:
                populous.SetActive(true);
                TweenUpgradeIcon(populous);
                break;
            case UpgradeID.Winfall:
                windfall.SetActive(true);
                TweenUpgradeIcon(windfall);
                break;
            case UpgradeID.HomeTree:
                homeTree.SetActive(true);
                TweenUpgradeIcon(homeTree);
                break;
        }

    }
    

    private void OnGameWin()
    {
        winPanel.SetActive(true);
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

    public void MouseCancel()
    {
        maegenCost.SetActive(false);
        wildlifeCost.SetActive(false);
    }

    #region Tools
    public void OnFyrePlaced()
    {
        fyreTimeLeft = fyreMaxTimeLeft;
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
                fyreTool.CooldownFill(MathX.MapTo01(fyreTimeLeft, 0, fyreMaxTimeLeft));
            }
            else
            {
                fyreAvailable = true;
                fyreTool.SetInteractable(true);
            }
        }
    }

    public void OnStormerPlaced()
    {
        stormerTimeLeft = stormerMaxTimeLeft;
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
                stormerTool.CooldownFill(MathX.MapTo01(stormerTimeLeft, 0, stormerMaxTimeLeft));
            }
            else
            {
                stormerAvailable = true;
                stormerTool.SetInteractable(true);
            }
        }
    }

    public void OnRunePlaced()
    {
        runeTimeLeft = runeMaxTimeLeft;
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
                runeTool.CooldownFill(MathX.MapTo01(runeTimeLeft, 0, runeMaxTimeLeft));
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
    #endregion

    private void OnWildlifeValueChange(int _value)
    {
        wildlifeText.text = _value.ToString();
    }

    private void OnEnable()
    {
        GameEvents.OnGameStateChanged += OnGameStateChanged;
        GameEvents.OnGameOver += OnGameOver;
        GameEvents.OnGameWin += OnGameWin;
        GameEvents.OnFyrePlaced += OnFyrePlaced;
        GameEvents.OnStormerPlaced += OnStormerPlaced;
        GameEvents.OnRunePlaced += OnRunePlaced;
        GameEvents.OnWaveOver += OnWaveOver;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnWaveBegin += OnWaveBegin;
        GameEvents.OnJustStragglers += OnJustStragglers;

        GameEvents.OnUpgradeSelected += OnUpgradeSelected;

        GameEvents.OnWildlifeValueChange += OnWildlifeValueChange;
        GameEvents.OnFormationSelected += OnFormationSelected;
    }

    

    private void OnDisable()
    {
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
        GameEvents.OnGameOver -= OnGameOver;
        GameEvents.OnGameWin -= OnGameWin;
        GameEvents.OnFyrePlaced -= OnFyrePlaced;
        GameEvents.OnStormerPlaced -= OnStormerPlaced;
        GameEvents.OnRunePlaced -= OnRunePlaced;
        GameEvents.OnWaveOver -= OnWaveOver;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnWaveBegin -= OnWaveBegin;
        GameEvents.OnJustStragglers -= OnJustStragglers;

        GameEvents.OnUpgradeSelected -= OnUpgradeSelected;

        GameEvents.OnWildlifeValueChange -= OnWildlifeValueChange;
        GameEvents.OnFormationSelected -= OnFormationSelected;
    }
}
