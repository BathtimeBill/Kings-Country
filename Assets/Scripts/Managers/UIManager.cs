using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;

public class UIManager : Singleton<UIManager>
{
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
    public GameObject GameOverPanel;
    public GameObject pausePanel;
    public GameObject settingsPanel;
    public GameObject winPanel;
    public GameObject eldyrButton;
    public GameObject ObjectiveText;

    public bool settingsOpen;

    public GameObject transformText;

    public AudioSource audioSource;
    public AudioSource warningAudioSource;

    public GameObject areYouSurePanel;

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
    public Animator waveTextAnimator;
    public Button nextRoundButton;
    public GameObject waveOverPanel;
    public Button treetoolButton;
    public GameObject collectMaegenButton;

    [Header("Current Upgrade Panel")]
    public GameObject barkSkin;
    public GameObject flyFoot;
    public GameObject power;
    public GameObject tower;
    public GameObject rune;
    public GameObject beacon;
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
        waveOverPanel.SetActive(false);
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
        audioSource.clip = _SM.gameOverSound;
        audioSource.Play();
        GameOverPanel.SetActive(true);
        Time.timeScale = 4;
        deathCameraRotator.SetActive(true);
        _GM.gameState = GameState.Pause;
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
    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
        settingsOpen = true;
    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
        settingsOpen = false;
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
    public void OpenWinPanel()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0;
        _GM.gameState = GameState.Pause;
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

    


    public void OnWaveOver()
    {
        if(_GM.currentWave!= _WM.winLevel)
        {
            waveOverPanel.SetActive(true);
            settingsOpen = true;
        }
    }
    public void OnContinueButton()
    {
        waveOverPanel.SetActive(false);
        collectMaegenButton.SetActive(false);
        settingsOpen = false;
        treeTool.SetInteractable(true);
    }

    private void OnWaveBegin()
    {
        treeTool.SetInteractable(false);
    }



    private void OnBorkrskinnUpgrade()
    {
        barkSkin.SetActive(true);
    }
    private void OnFlugafotrUpgrade()
    {
        flyFoot.SetActive(true);
    }
    private void OnJarnnefiUpgrade()
    {
        power.SetActive(true);
    }
    private void OnTowerUpgrade()
    {
        tower.SetActive(true);
    }
    private void OnRuneUpgrade()
    {
        rune.SetActive(true);
    }
    private void OnBeaconUpgrade()
    {
        beacon.SetActive(true);
    }
    private void OnStormerUpgrade()
    {
        stormer.SetActive(true);
    }
    private void OnTreeUpgrade()
    {
        tree.SetActive(true);
    }
    private void OnFertileSoilUpgrade()
    {
        fertile.SetActive(true);
    }
    private void OnPopulousUpgrade()
    {
        populous.SetActive(true);
    }
    private void OnWinfallUpgrade()
    {
        windfall.SetActive(true);
    }
    private void OnHomeTreeUpgrade()
    {
        homeTree.SetActive(true);
    }
    private void OnGameWin()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    private void OnJustStragglers()
    {
        collectMaegenButton.SetActive(true);
    }
    public void CollectMaegen()
    {
        GameEvents.ReportOnCollectMaegenButton();
        collectMaegenButton.SetActive(false);
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
        GameEvents.OnGameOver += OnGameOver;
        GameEvents.OnGameWin += OnGameWin;
        GameEvents.OnFyrePlaced += OnFyrePlaced;
        GameEvents.OnStormerPlaced += OnStormerPlaced;
        GameEvents.OnRunePlaced += OnRunePlaced;
        GameEvents.OnWaveOver += OnWaveOver;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnWaveBegin += OnWaveBegin;
        GameEvents.OnJustStragglers += OnJustStragglers;

        GameEvents.OnBorkrskinnUpgrade += OnBorkrskinnUpgrade;
        GameEvents.OnFlugafotrUpgrade += OnFlugafotrUpgrade;
        GameEvents.OnJarnnefiUpgrade += OnJarnnefiUpgrade;
        GameEvents.OnTowerUpgrade += OnTowerUpgrade;
        GameEvents.OnRuneUpgrade += OnRuneUpgrade;
        GameEvents.OnBeaconUpgrade += OnBeaconUpgrade;
        GameEvents.OnStormerUpgrade += OnStormerUpgrade;
        GameEvents.OnTreeUpgrade += OnTreeUpgrade;
        GameEvents.OnFertileSoilUpgrade += OnFertileSoilUpgrade;
        GameEvents.OnPopulousUpgrade += OnPopulousUpgrade;
        GameEvents.OnWinfallUpgrade += OnWinfallUpgrade;
        GameEvents.OnHomeTreeUpgrade += OnHomeTreeUpgrade;

        GameEvents.OnWildlifeValueChange += OnWildlifeValueChange;
        GameEvents.OnFormationSelected += OnFormationSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
        GameEvents.OnGameWin -= OnGameWin;
        GameEvents.OnFyrePlaced -= OnFyrePlaced;
        GameEvents.OnStormerPlaced -= OnStormerPlaced;
        GameEvents.OnRunePlaced -= OnRunePlaced;
        GameEvents.OnWaveOver -= OnWaveOver;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnWaveBegin -= OnWaveBegin;
        GameEvents.OnJustStragglers -= OnJustStragglers;

        GameEvents.OnBorkrskinnUpgrade -= OnBorkrskinnUpgrade;
        GameEvents.OnFlugafotrUpgrade -= OnFlugafotrUpgrade;
        GameEvents.OnJarnnefiUpgrade -= OnJarnnefiUpgrade;
        GameEvents.OnTowerUpgrade -= OnTowerUpgrade;
        GameEvents.OnRuneUpgrade -= OnRuneUpgrade;
        GameEvents.OnBeaconUpgrade -= OnBeaconUpgrade;
        GameEvents.OnStormerUpgrade -= OnStormerUpgrade;
        GameEvents.OnTreeUpgrade -= OnTreeUpgrade;
        GameEvents.OnFertileSoilUpgrade -= OnFertileSoilUpgrade;
        GameEvents.OnPopulousUpgrade -= OnPopulousUpgrade;
        GameEvents.OnWinfallUpgrade -= OnWinfallUpgrade;
        GameEvents.OnHomeTreeUpgrade -= OnHomeTreeUpgrade;

        GameEvents.OnWildlifeValueChange -= OnWildlifeValueChange;
        GameEvents.OnFormationSelected -= OnFormationSelected;
    }
}
