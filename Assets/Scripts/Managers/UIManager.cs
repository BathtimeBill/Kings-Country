using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Serialization;

public class UIManager : Singleton<UIManager>
{
    [Header("Managers")]
    public GlossaryManager glossaryManager;
    public TutorialManager tutorialManager;
    public ExperienceMeter experienceMeter;

    [Header("In Game Panels")]
    public InGamePanels inGamePanels;
    public ToggleButton[] buildingToggles;

    [Header("Interactable Buttons")]
    public InteractableButtons interactableButtons;
    private int currentButton = 0;

    [Header("Resources Top")]
    public TMP_Text maegenText;
    public TMP_Text treesText;
    public TMP_Text wildlifeText;
    public TMP_Text populousText;
    public TMP_Text waveText;
    public TMP_Text enemyProgressText;
    public Image agroBar;
    public Image agroFill;

    [Header("Error")]
    public TMP_Text errorText;
    Animator errorAnim;

    [Header("Bottom")]
    public TMP_Text unitPanelMaegenText;
    public GameObject combatOptionsPanel;
    public bool mouseOverCombatOptions;
    

    [Header("Pause")]
    public CanvasGroup inGameCanvas;
    public CanvasGroup glossaryButtonCanvas;
    public CanvasGroup pauseBlackoutPanel;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    [HideInInspector]
    public GameObject warningPanel; //Used for escape pausing. Auto set.
    public GameObject unitsPanel;
    
    public GameObject transformText;

    public AudioSource audioSource;
    public AudioSource warningAudioSource;
    public GameObject deathCameraRotator;

    [Header("Tools")] 
    public List<ToolButton> toolButtons;
    public ToolButton tool1Button;
    public float tool1TimeLeft;
    public bool tool1Available;
    public ToolButton tool2Button;
    public float tool2TimeLeft;
    public bool tool2Available;
    public ToolButton tool3Button;
    public float tool3TimeLeft;
    public bool tool3Available;

    [Header("Trees")]
    public TreeButton tree1Button;
    public TreeButton tree2Button;
    public TreeButton tree3Button;
    public CanvasGroup treePercentageModifier;
    public TMP_Text treePercentageText;
    public TMP_Text treeResultText;
    public TMP_Text treeCostText;

    [Header("Days")]
    public TMP_Text dayWinTitleText;
    public Animator dayTextAnimator;
    public DayButton dayNightButton;

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
    public WinTexts winTexts;

    [Header("Upgrade")]
    public PerkPanel upgradePanel;
    public PerkButton upgradeButton1;
    public PerkButton upgradeButton2;

    [Header("Current Perk Panel")] 
    public List<GameObject> inGamePerks;

    [Header("Mouse Over UI")]
    public bool mouseOverUI;
    public List<GameObject> elements;
    [Header("Formations")]
    public GameObject formationObject;
    private bool largeFormationSelected;
    public Image formationButtonImage;
    public Sprite smallFormation;
    public Sprite bigFormation;

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

    public void SetInteractable(InteractableButton _b, bool _i) => _b.SetInteractable(_i);

    private new void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        FadeX.InstantTransparent(inGameCanvas);
        FadeX.InstantTransparent(glossaryButtonCanvas);

        CheckTreeUI();
        CheckWildlifeUI();
        CheckPopulousUI();
        ResetAgroBar();
        tool1TimeLeft = 0;
        tool2TimeLeft = 0;

        tool1Button.SetInteractable(_GAME.fyreAvailable);
        tool2Button.SetInteractable(_GAME.stormerAvailable);
        tool3Button.SetInteractable(_GAME.runesAvailable);

        formationObject = GameObject.FindGameObjectWithTag("Destination");

        ResetPanel(wavePanel);
        ResetPanel(winPanel);
        ResetPanel(finalScorePanel);
        ResetPanel(gameOverPanel);
        ObjectX.ToggleObjects(inGamePerks, false);
        waveText.text = nightMessage;

        treePercentageModifier.alpha = 0f;
        treePercentageModifier.gameObject.SetActive(false);

        errorAnim = errorText.GetComponent<Animator>();
        errorText.DOFade(0, 0);

        _SETTINGS.colours.ChangePanelColour(_SAVE.GetPanelColour(), _SAVE);
    }
    
    void Update()
    {
        FyreCheck();
        StormerCheck();
        RuneCheck();
        //ToolCheck(tool1Button);
        //toolButtons[0].ToolCheck();
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
                FadeX.FadeOut(inGameCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeOut(glossaryButtonCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeTo(pauseBlackoutPanel, _TWEENING.blackoutPanelFade, _TWEENING.blackoutPanelTime);
                SetBuildingToggleShiny(false);
                break;
            case GameState.Play:
                SetInteractable(dayNightButton, false);
                FadeX.FadeIn(inGameCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeIn(glossaryButtonCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeOut(pauseBlackoutPanel, _TWEENING.blackoutPanelTime);
                SetBuildingToggleShiny(true);
                break;
            case GameState.Build:
                SetInteractable(dayNightButton, true);
                FadeX.FadeIn(inGameCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeIn(glossaryButtonCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeOut(pauseBlackoutPanel, _TWEENING.blackoutPanelTime);
                SetBuildingToggleShiny(true);
                break;
            case GameState.Finish:
                FadeX.FadeOut(inGameCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeOut(glossaryButtonCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeTo(pauseBlackoutPanel, _TWEENING.blackoutPanelFade, _TWEENING.blackoutPanelTime);
                SetBuildingToggleShiny(false);
                break;
            case GameState.Glossary:
                FadeX.FadeOut(inGameCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeOut(glossaryButtonCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeTo(pauseBlackoutPanel, _TWEENING.blackoutPanelFade, _TWEENING.blackoutPanelTime);
                SetBuildingToggleShiny(false);
                break;
            case GameState.Tutorial:
                SetInteractable(dayNightButton, false);
                FadeX.FadeIn(inGameCanvas, _TWEENING.blackoutPanelTime);
                FadeX.FadeIn(glossaryButtonCanvas, _TWEENING.blackoutPanelTime);
                //inGameCanvas.interactable = true;
                //inGameCanvas.alpha = 1;
                FadeX.FadeOut(pauseBlackoutPanel, _TWEENING.blackoutPanelTime);
                break;
        }
    }

    private void OnCombatSelected(CombatID _combatID)
    {
        if (_combatID != CombatID.Formation)
            return;
        
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
        deathCameraRotator.SetActive(true);
        //_GM.gameState = GameState.Pause;
    }    

    public void SetBuildingToggleShiny(bool _shine)
    {
        for(int i=0;i<buildingToggles.Length;i++)
        {
            buildingToggles[i].SetShiny(_shine);
        }
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
    
    public void CheckPopulousUI()
    {
        StartCoroutine(WaitForPopulousCheck());
    }
    IEnumerator WaitForPopulousCheck()
    {
        yield return new WaitForEndOfFrame();
        populousText.text = _GAME.CheckPopulous().ToString() + "/" + _GAME.maxPopulous;
    }
    public void CheckTreeUI()
    {
        treesText.text = _GAME.trees.Count.ToString() + "/" + _GAME.maxTrees;
    }
    public void CheckWildlifeUI()
    {
        wildlifeText.text = _GAME.wildlifeCount.ToString();
    }

    public void ResetAgroBar() => agroFill.fillAmount = 0;
    public void UpdateAgroBar(float _current, float _max) => agroFill.fillAmount = MathX.MapTo01(_current, 0, _max);

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
    #endregion

    #region Wave Over


    public void OnDayOver(int _day)
    {
        ExecuteNextFrame(() =>
        {
            if (_GameFinished)
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
            ResetAgroBar();
            SetWaveEndStats();
        });

    }

    private void SetWaveEndStats()
    {
        TweenInPanel(wavePanel);
        _SM.PlaySound(_SM.waveOverSound);
        _SM.PlaySound(_SM.menuDragSound);
        dayWinTitleText.text = dayMessage + " " + _GAME.currentDay.ToString() + " is complete!";

        totalTrees = _GAME.trees.Count;
        totalMaegenDrops = GameObject.FindGameObjectsWithTag("MaegenDrop").Length;
        totalMaegen = totalTrees + totalMaegenDrops + GetTreeBonusTotal();

        penaltyText.text = "+" + _GAME.numberOfWildlifeToSpawn.ToString();
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
        if (_DATA.CanObtainPerk)
        {
            PerkID perk1 = _DATA.GetRandomPerk();
            _DATA.RemovePerk(perk1);
            PerkID perk2 = _DATA.GetRandomPerk();
            _DATA.RemovePerk(perk2);

            //print(upgradeButton1.name);
            //print(upgradeButton2.name);
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

    private int GetTreeBonusTotal()
    {
        int treeBonus = 0;
        foreach (GameObject i in _GAME.trees)
        {
            treeBonus = treeBonus + i.GetComponent<Tree>().energyMultiplier;
        }
        int totalTreeBonus = treeBonus - totalTrees;
        return totalTreeBonus;
    }

    public void OnContinueButton()
    {
        TweenOutPanel(wavePanel);
        tree1Button.SetInteractable(true);
        TweenX.KillTweener(continueTweener); 
        continueButton.transform.localScale = Vector3.one;
        waveText.text = nightMessage + ": " + (_GAME.currentDay + 1) + "/" + _DATA.levelMaxDays;
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
        _PC.tools.treeTool.Deselect();
        int enemyProgressTotal = _EM.GetDayTotalEnemyCount();
        enemyProgressText.text = dayProgressMessage + "0/" + enemyProgressTotal;
        waveText.text = dayMessage + ": " + _GAME.currentDay.ToString() + "/" + _DATA.levelMaxDays;
        dayTextAnimator.SetTrigger("NewDay");
    }

    

    //Button Presses from Inspector
    public void ShowResultsPanel()
    {
        _GAME.CalculateScore();
        winPanel.GetComponentInChildren<Button>().interactable = false;
        TweenInPanel(finalScorePanel);
    }

    public void MouseCancel()
    {
    }

    #region Tools
    public void OnToolUsed()
    {
        tool1TimeLeft = _DATA.GetTool(tool1Button.toolID).cooldownTime;
        tool1Button.SetInteractable(false);
        tool1Available = false;
    }

    private void ToolCheck(ToolButton _toolButton)
    {
        if (!_toolButton.toolAvailable)
        {
            if (tool1TimeLeft >= 0)
            {
                tool1TimeLeft -= Time.deltaTime;
                tool1Button.CooldownFill(MathX.MapTo01(tool1TimeLeft, 0, _DATA.GetTool(tool1Button.toolID).cooldownTime));
            }
            else
            {
                tool1Available = _DATA.CanUseTool(tool1Button.toolID);
                tool1Button.SetInteractable(_DATA.CanUseTool(tool1Button.toolID));
            }
        }
    }
    
    public void OnFyrePlaced()
    {
        tool1TimeLeft = _DATA.GetTool(tool1Button.toolID).cooldownTime;
        tool1Button.SetInteractable(false);
        tool1Available = false;
    }

    private void FyreCheck()
    {
        if (!tool1Available)
        {
            if (tool1TimeLeft >= 0)
            {
                tool1TimeLeft -= Time.deltaTime;
                tool1Button.CooldownFill(MathX.MapTo01(tool1TimeLeft, 0, _DATA.GetTool(tool1Button.toolID).cooldownTime));
            }
            else
            {
                tool1Available = _DATA.CanUseTool(tool1Button.toolID);
                tool1Button.SetInteractable(_DATA.CanUseTool(tool1Button.toolID));
            }
        }
    }

    public void OnStormerPlaced()
    {
        tool2TimeLeft = _DATA.GetTool(ToolID.Stormer).cooldownTime;
        tool2Button.SetInteractable(false);
        tool2Available = false;
    }

    private void StormerCheck()
    {
        if (!tool2Available)
        {
            if (tool2TimeLeft >= 0)
            {
                tool2TimeLeft -= Time.deltaTime;
                tool2Button.CooldownFill(MathX.MapTo01(tool2TimeLeft, 0, _DATA.GetTool(ToolID.Stormer).cooldownTime));
            }
            else
            {
                tool2Available = _DATA.CanUseTool(ToolID.Stormer);
                tool2Button.SetInteractable(_DATA.CanUseTool(ToolID.Stormer));
            }
        }
    }

    public void OnRunePlaced()
    {
        tool3TimeLeft = _DATA.GetTool(ToolID.Rune).cooldownTime;
        tool3Button.SetInteractable(false);
        tool3Available = false;
        //print(_GM.runesCount + " = " + _GM.runesMaegenCost.Length);
    }

    private void RuneCheck()
    {
        if (!tool3Available)
        {
            if (tool3TimeLeft >= 0)
            {
                tool3TimeLeft -= Time.deltaTime;
                tool3Button.CooldownFill(MathX.MapTo01(tool3TimeLeft, 0, _DATA.GetTool(ToolID.Rune).cooldownTime));
            }
            else
            {
                if (_GAME.runesAvailable)
                {
                    tool3Available = true;
                    tool3Button.SetInteractable(true);
                }
            }
        }
    }

    public void ChangeTreePercentagePanel(float _percentage, int _perDay, int _price, bool _canPlace)
    {
        treePercentageText.text = _canPlace ? _percentage.ToString("0.0" + "%") : "-";
        treePercentageText.color = _canPlace ? _SETTINGS.colours.treePercentageGradient.Evaluate(_percentage) : _COLOUR.cooldownColor;
        treeResultText.text = _canPlace ? "Every night, get " + _perDay.ToString() + "  <color=#D3C965><sprite name=\"MaegenIcon\">" : "Can't place tree here";
        treeCostText.text = _canPlace ? _price.ToString() : "-";
    }
    #endregion

    private void OnWildlifeValueChange(int _value) => wildlifeText.text = _value.ToString();

    private void OnLevelWin(LevelID _levelID, int _score, int _maegen)
    {
        winTexts.maegen.text = "";
        winTexts.maegenBonus.text = "";
        winTexts.maegenTotal.text = "";

        winTexts.trees.text = "";
        winTexts.treesBonus.text = "";
        winTexts.treesTotal.text = "";

        winTexts.wildlife.text = "";
        winTexts.wildlifeBonus.text = "";
        winTexts.wildlifeTotal.text = "";

        winTexts.populous.text = "";
        winTexts.populousBonus.text = "";
        winTexts.populousTotal.text = "";

        winTexts.finalEXP.text = "";
        winTexts.bestEXP.text = "";

        experienceMeter.gameObject.SetActive(false);
        TweenInPanel(winPanel);

    }
    public IEnumerator UpdateWinUI(int _maegen, int _maegenBonus, int _trees, int _treesBonus, int _wildlife, int _wildlifeBonus, int _populous, int _populousBonus, int _total)
    {
        yield return new WaitForSeconds(2);

        winTexts.maegen.text = "+ " + _maegen.ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime/2);
        winTexts.maegenBonus.text = "x" + _maegenBonus.ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime / 2);
        winTexts.maegenTotal.text = (_maegen * _maegenBonus).ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime);

        winTexts.trees.text = "+ " + _trees.ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime / 2);
        winTexts.treesBonus.text = "x" + _treesBonus.ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime / 2);
        winTexts.treesTotal.text = (_trees * _treesBonus).ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime);

        winTexts.wildlife.text = "+ " + _wildlife.ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime / 2);
        winTexts.wildlifeBonus.text = "x" + _wildlifeBonus.ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime / 2);
        winTexts.wildlifeTotal.text = (_wildlife * _wildlifeBonus).ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime);

        winTexts.populous.text = "+ " + _populous.ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime / 2);
        winTexts.populousBonus.text = "x" + _populousBonus.ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime / 2);
        winTexts.populousTotal.text = (_populous * _populousBonus).ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime * 2);

        experienceMeter.gameObject.SetActive(true);
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime);
        winTexts.finalEXP.text = _total.ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(statsinTweenTime);
        winTexts.bestEXP.text = _GAMESAVE.GetLevelHighScore(_DATA.currentLevelID).ToString();
        _SM.PlaySound(_SM.textGroupSound);
        yield return new WaitForSecondsRealtime(2);
        experienceMeter.IncreaseExperience(_total);
    }

    private void OnEnemyUnitKilled(Enemy _unitID, string _killedBy)
    {
        if (!_InDay)
            return;
        
        ExecuteNextFrame(() =>
        {
            enemyProgressText.text = dayProgressMessage + _EM.currentKillCount + "/" + _EM.GetDayTotalEnemyCount();
        });
    }

    private void OnCycleTool(int _direction)
    {
        List<InteractableButton> interactable = interactableButtons.buttons.FindAll(x => x.interactable == true);

        if(_direction == 1)
            currentButton = ListX.IncrementCounter(currentButton, interactable);
        else
            currentButton = ListX.DecrementCounter(currentButton, interactable);

        for (int i=0;i< interactable.Count;i++) 
        {
            interactable[i].SetActivated(false);
            interactable[currentButton].SetActivated(true);
        }
        //for (int i = 0; i < interactableButtons.buttons.Count; i++)
        //{
        //    interactableButtons.buttons[currentButton].SetInteractable(true);
        //}
    }
    
    private void OnPerkSelected(PerkID perkID)
    {
        upgradeButton1.SetInteractable(false);
        upgradeButton2.SetInteractable(false);
        
        PerkData pd = _DATA.GetPerk(perkID);
        GameObject po = ObjectX.GetNextInactive(inGamePerks);
        po.GetComponent<Tooltip>().title = pd.name;
        po.GetComponent<Tooltip>().message = pd.description;
        po.GetComponentInChildren<Image>(true).sprite = pd.icon;
        po.GetComponentInChildren<Image>(true).color = _SETTINGS.colours.highlightedColor;
        po.transform.localScale = Vector3.one * 3;
        po.transform.SetAsLastSibling();
        po.SetActive(true);
        po.transform.DOScale(Vector3.one, 1).SetLoops(3).SetUpdate(true).OnComplete(() =>
            po.GetComponentInChildren<Image>().DOColor(_SETTINGS.colours.upgradeIconsColor, 0.5f).SetUpdate(true));
        continueButton.interactable = true;
        TweenX.KillTweener(continueTweener);
        continueTweener = continueButton.transform.DOScale(Vector3.one * buttonPulseScale, buttonPulseSpeed).SetUpdate(true).SetLoops(-1).SetDelay(3);
        _SM.PlaySound(_SM.upgradeSound);
    }

    private void OnEnable()
    {
        GameEvents.OnGameStateChanged += OnGameStateChanged;
        GameEvents.OnGameOver += OnGameOver;
        GameEvents.OnLevelWin += OnLevelWin;
        GameEvents.OnFyrePlaced += OnFyrePlaced;
        GameEvents.OnStormerPlaced += OnStormerPlaced;
        GameEvents.OnRunePlaced += OnRunePlaced;
        GameEvents.OnDayOver += OnDayOver;
        GameEvents.OnContinueButton += OnContinueButton;


        GameEvents.OnWildlifeValueChange += OnWildlifeValueChange;
        GameEvents.OnCombatSelected += OnCombatSelected;
        GameEvents.OnHumanKilled += OnEnemyUnitKilled;
        GameEvents.OnPerkSelected += OnPerkSelected;

        InputManager.OnCycleTool += OnCycleTool;
    }

    

    private void OnDisable()
    {
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
        GameEvents.OnGameOver -= OnGameOver;
        GameEvents.OnLevelWin -= OnLevelWin;
        GameEvents.OnFyrePlaced -= OnFyrePlaced;
        GameEvents.OnStormerPlaced -= OnStormerPlaced;
        GameEvents.OnRunePlaced -= OnRunePlaced;
        GameEvents.OnDayOver -= OnDayOver;
        GameEvents.OnContinueButton -= OnContinueButton;


        GameEvents.OnWildlifeValueChange -= OnWildlifeValueChange ;
        GameEvents.OnCombatSelected -= OnCombatSelected;
        GameEvents.OnHumanKilled -= OnEnemyUnitKilled;
        GameEvents.OnPerkSelected -= OnPerkSelected;

        InputManager.OnCycleTool -= OnCycleTool;
    }
}

[System.Serializable]
public class InGamePanels
{
    public CanvasGroup dayNightPanel;
    public CanvasGroup treePanel;
    public CanvasGroup toolPanel;
    public CanvasGroup combatPanel;
    public CanvasGroup speedPanel;
    public CanvasGroup unitPanel;
    public CanvasGroup perksPanel;
    public CanvasGroup resourcesPanel;
    public CanvasGroup mapPanel;
    public CanvasGroup tasksPanel;

    public void Show(CanvasGroup cvg) => FadeX.FadeIn(cvg);
}

[System.Serializable]
public class WinTexts
{
    public TMP_Text maegen;
    public TMP_Text maegenBonus;
    public TMP_Text maegenTotal;
    public TMP_Text trees;
    public TMP_Text treesBonus;
    public TMP_Text treesTotal;
    public TMP_Text wildlife;
    public TMP_Text wildlifeBonus;
    public TMP_Text wildlifeTotal;
    public TMP_Text populous;
    public TMP_Text populousBonus;
    public TMP_Text populousTotal;
    public TMP_Text finalEXP;
    public TMP_Text bestEXP;
}

[System.Serializable]
public class InteractableButtons
{
    public List<InteractableButton> buttons;
}