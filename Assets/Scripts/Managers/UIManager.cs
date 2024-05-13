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




    public Slider beaconCooldownSlider;
    public GameObject beaconCooldownSliderObject;
    public float beaconTimeLeft;
    public float beaconMaxTimeLeft;


    public bool beaconPlaced;

    public Slider stormerCooldownSlider;
    public GameObject stormerCooldownSliderObject;
    public float stormerTimeLeft;
    public float stormerMaxTimeLeft;




    public bool stormerPlaced;



    public GameObject transformText;

    public AudioSource audioSource;
    public AudioSource warningAudioSource;

    public GameObject areYouSurePanel;

    public GameObject deathCameraRotator;

    [Header("Tools")]
    public int fyreCost;
    public int stormerCost;

    public GameObject treeToolSelectionBox;
    public GameObject runeToolSelectionBox;
    public GameObject beaconToolSelectionBox;
    public GameObject stormerToolSelectionBox;

    public Sprite usableTreeTool;
    public Sprite unusableTreeTool;
    public Sprite usableRuneTool;
    public Sprite unusableRuneTool;
    public Sprite usableBeaconTool;
    public Sprite unusableBeaconTool;
    public Sprite usableStormerTool;
    public Sprite unusableStormerTool;

    public Image treeToolImage;
    public Image runeToolImage;
    public Image beaconToolImage;
    public Image stormerToolImage;

    public Button runeToolButton;
    public Button beaconToolButton;
    public Button stormerToolButton;

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
        //StartCoroutine(WaitToCheckForToolButtons());
        CheckTreeUI();
        CheckWildlifeUI();
        CheckPopulousUI();
        CheckWave();
        CheckUnitPrices();
        beaconTimeLeft = 0;
        stormerTimeLeft = 0;
        StartCoroutine(CheckToolAvailability());
    }


    void Update()
    {
        maegenText.text = _GM.maegen.ToString();

        if (beaconPlaced)
        {
            beaconTimeLeft += 1 * Time.deltaTime;
            beaconCooldownSlider.value = CalculateCooldownTimeLeft(beaconTimeLeft, beaconMaxTimeLeft);
            beaconToolImage.sprite = unusableBeaconTool;
            if (beaconTimeLeft >= beaconMaxTimeLeft)
            {
                beaconToolImage.sprite = usableBeaconTool;
                beaconCooldownSliderObject.SetActive(false);
                beaconPlaced = false;
                beaconTimeLeft = 0;
            }
        }
        if (stormerPlaced)
        {
            stormerTimeLeft += 1 * Time.deltaTime;
            stormerCooldownSlider.value = CalculateCooldownTimeLeft(stormerTimeLeft, stormerMaxTimeLeft);
            stormerToolImage.sprite = unusableStormerTool;
            if (stormerTimeLeft >= stormerMaxTimeLeft)
            {
                stormerToolImage.sprite = usableStormerTool;
                stormerCooldownSliderObject.SetActive(false);
                stormerTimeLeft = 0;
                stormerPlaced = false;
            }
        }
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    GameEvents.ReportOnGameOver();
        //}
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
    public void FormationButtonPressed()
    {
        _SM.PlaySound(_SM.formationSound);
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
    float CalculateCooldownTimeLeft(float _timeLeft, float _maxTime)
    {
        return _timeLeft / _maxTime;
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

    public void OnBeaconPlaced()
    {
        beaconCooldownSliderObject.SetActive(true);
        beaconPlaced = true;
    }

    public void OnStormerPlaced()
    {
        stormerCooldownSliderObject.SetActive(true);
        stormerPlaced = true;
    }
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
        treeToolImage.sprite = usableTreeTool;
        collectMaegenButton.SetActive(false);
        settingsOpen = false;
        //StartCoroutine(WaitToCheckForToolButtons());
    }
    IEnumerator WaitToCheckForToolButtons()
    {
        yield return new WaitForSeconds(1);
        if (_GM.runes.Count == 0)
        {
            if (_GM.maegen < 2 || _GM.wildlife < 5)
            {
                runeToolButton.interactable = false;
                runeToolImage.sprite = unusableRuneTool;
            }
            if (_GM.maegen >= 2 && _GM.wildlife >= 5)
            {
                runeToolButton.interactable = true;
                runeToolImage.sprite = usableRuneTool;
            }
        }
        if(_GM.runes.Count == 1)
        {
            if (_GM.maegen < 4 || _GM.wildlife < 7)
            {
                runeToolButton.interactable = false;
                runeToolImage.sprite = unusableRuneTool;
            }
            if (_GM.maegen >= 4 && _GM.wildlife >= 7)
            {
                runeToolButton.interactable = true;
                runeToolImage.sprite = usableRuneTool;
            }
        }
        if (_GM.runes.Count == 2)
        {
            if (_GM.maegen < 8 || _GM.wildlife < 10)
            {
                runeToolButton.interactable = false;
                runeToolImage.sprite = unusableRuneTool;
            }
            if (_GM.maegen >= 8 && _GM.wildlife >= 10)
            {
                runeToolButton.interactable = true;
                runeToolImage.sprite = usableRuneTool;
            }
        }
        if (_GM.runes.Count == 3)
        {
            if (_GM.maegen < 16 || _GM.wildlife < 15)
            {
                runeToolButton.interactable = false;
                runeToolImage.sprite = unusableRuneTool;
            }
            if (_GM.maegen >= 16 && _GM.wildlife >= 15)
            {
                runeToolButton.interactable = true;
                runeToolImage.sprite = usableRuneTool;
            }
        }
        if (_GM.runes.Count > 3)
        {
            runeToolButton.interactable = false;
            runeToolImage.sprite = unusableRuneTool;
        }

        if(_GM.wildlife >= 10)
        {
            beaconToolButton.interactable = true;
            beaconToolImage.sprite = usableBeaconTool;
        }
        else
        {
            beaconToolButton.interactable = false;
            beaconToolImage.sprite = unusableBeaconTool;
        }
        if(_GM.wildlife >= 20)
        {
            stormerToolButton.interactable = true;
            stormerToolImage.sprite= usableStormerTool;
        }
        else
        {
            stormerToolButton.interactable = false;
            stormerToolImage.sprite = unusableStormerTool;
        }
    }

    private void OnStartNextRound()
    {
        if (treeToolImage != null) 
        {
            treeToolImage.sprite = unusableTreeTool; 
        }
        else
        {
            Debug.LogWarning("Image component is null. Make sure it is assigned.");
        }

        //if(horgrPanel.activeInHierarchy)
        //{
        //    horgrPanel.gameObject.SetActive(false);
        //}
        //if (hutPanel.activeInHierarchy)
        //{
        //    hutPanel.gameObject.SetActive(false);
        //}
        //if (homeTreePanel.activeInHierarchy)
        //{
        //    homeTreePanel.gameObject.SetActive(false);
        //}
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

    public void SelectAttack()
    {
        GameEvents.ReportOnAttackSelected();
        _SM.PlaySound(_SM.attackSound);

    }
    public void SelectDefend()
    {
        GameEvents.ReportOnDefendSelected();
        _SM.PlaySound(_SM.defendSound);
    }
    public void OnAttackSelected()
    {
        
    }
    public void OnDefendSelected()
    {

    }

    IEnumerator CheckToolAvailability()
    {
        yield return new WaitForSeconds(0.5f);
        HandleRuneButton();
        HandleFyreButton();
        HandleStormerButton();
        StartCoroutine(CheckToolAvailability());

    }



    private void HandleRuneButton()
    {
        if (_GM.runes.Count == 0)
        {
            if (_GM.maegen < _RPlace.maegenCost1 || _GM.wildlife < _RPlace.wildlifeCost1)
            {
                runeToolButton.interactable = false;
                runeToolImage.sprite = unusableRuneTool;
            }
            if (_GM.maegen >= _RPlace.maegenCost1 && _GM.wildlife >= _RPlace.wildlifeCost1)
            {
                runeToolButton.interactable = true;
                runeToolImage.sprite = usableRuneTool;
            }
        }
        if (_GM.runes.Count == 1)
        {
            if (_GM.maegen < _RPlace.maegenCost2 || _GM.wildlife < _RPlace.wildlifeCost2)
            {
                runeToolButton.interactable = false;
                runeToolImage.sprite = unusableRuneTool;
            }
            if (_GM.maegen >= _RPlace.maegenCost2 && _GM.wildlife >= _RPlace.wildlifeCost2)
            {
                runeToolButton.interactable = true;
                runeToolImage.sprite = usableRuneTool;
            }
        }
        if (_GM.runes.Count == 2)
        {
            if (_GM.maegen < _RPlace.maegenCost3 || _GM.wildlife < _RPlace.wildlifeCost3)
            {
                runeToolButton.interactable = false;
                runeToolImage.sprite = unusableRuneTool;
            }
            if (_GM.maegen >= _RPlace.maegenCost3 && _GM.wildlife >= _RPlace.wildlifeCost3)
            {
                runeToolButton.interactable = true;
                runeToolImage.sprite = usableRuneTool;
            }
        }
        if (_GM.runes.Count == 3)
        {
            if (_GM.maegen < _RPlace.maegenCost4 || _GM.wildlife < _RPlace.wildlifeCost4)
            {
                runeToolButton.interactable = false;
                runeToolImage.sprite = unusableRuneTool;
            }
            if (_GM.maegen >= _RPlace.maegenCost4 && _GM.wildlife >= _RPlace.wildlifeCost4)
            {
                runeToolButton.interactable = true;
                runeToolImage.sprite = usableRuneTool;
            }
        }
        if (_GM.runes.Count > 3)
        {
            runeToolButton.interactable = true;
            runeToolImage.sprite = usableRuneTool;
        }
    }
    private void HandleFyreButton()
    {
        if(_GM.wildlife < _UI.fyreCost)
        {
            beaconToolButton.interactable = false;
            beaconToolImage.sprite = unusableBeaconTool;
        }
        else
        {
            if(beaconCooldownSlider.gameObject.activeInHierarchy == false)
            {
                beaconToolButton.interactable = true;
                beaconToolImage.sprite = usableBeaconTool;
            }
        }
    }

    private void HandleStormerButton()
    {
        if(_GM.wildlife < _UI.stormerCost)
        {
            stormerToolButton.interactable = false;
            stormerToolImage.sprite = unusableStormerTool;
        }
        else
        {
            stormerToolButton.interactable = true;
            stormerToolImage.sprite = usableStormerTool;
        }
    }


    private void OnEnable()
    {
        GameEvents.OnAttackSelected += OnAttackSelected;
        GameEvents.OnDefendSelected += OnDefendSelected;
        GameEvents.OnGameOver += OnGameOver;
        GameEvents.OnGameWin += OnGameWin;
        GameEvents.OnBeaconPlaced += OnBeaconPlaced;
        GameEvents.OnStormerPlaced += OnStormerPlaced;
        GameEvents.OnWaveOver += OnWaveOver;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnStartNextRound += OnStartNextRound;
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
    }

    private void OnDisable()
    {
        GameEvents.OnDefendSelected -= OnDefendSelected;
        GameEvents.OnAttackSelected -= OnAttackSelected;
        GameEvents.OnGameOver -= OnGameOver;
        GameEvents.OnGameWin -= OnGameWin;
        GameEvents.OnBeaconPlaced -= OnBeaconPlaced;
        GameEvents.OnStormerPlaced -= OnStormerPlaced;
        GameEvents.OnWaveOver -= OnWaveOver;
        GameEvents.OnContinueButton -= OnContinueButton;
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
    }
}
