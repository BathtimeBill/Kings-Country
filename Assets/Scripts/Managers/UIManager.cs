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

    public GameObject maegenCost;
    public TMP_Text maegenCostText;
    public GameObject wildlifeCost;
    public TMP_Text wildlifeCostText;

    public GameObject transformText;

    public AudioSource audioSource;
    public AudioSource warningAudioSource;

    

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


    void Start()
    {
        StartCoroutine(WaitToCheckForToolButtons());
        CheckTreeUI();
        CheckWildlifeUI();
        CheckPopulousUI();
        CheckWave();
        beaconTimeLeft = 0;
        stormerTimeLeft = 0;
    }


    void Update()
    {
        maegenText.text = _GM.maegen.ToString();

        if (beaconPlaced)
        {
            beaconTimeLeft += 1 * Time.deltaTime;
            beaconCooldownSlider.value = CalculateTimeLeft();
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
            stormerCooldownSlider.value = CalculateStormerTimeLeft();
            stormerToolImage.sprite = unusableStormerTool;
            if (stormerTimeLeft >= stormerMaxTimeLeft)
            {
                stormerToolImage.sprite = usableStormerTool;
                stormerCooldownSliderObject.SetActive(false);
                stormerTimeLeft = 0;
                stormerPlaced = false;
            }
        }

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
    float CalculateTimeLeft()
    {
        return beaconTimeLeft / beaconMaxTimeLeft;
    }
    float CalculateStormerTimeLeft()
    {
        return stormerTimeLeft / stormerMaxTimeLeft;
    }
    public void OnGameOver()
    {
        audioSource.clip = _SM.gameOverSound;
        audioSource.Play();
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
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
    }
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
    public void CloseHomeTreeMenu()
    {
        homeTreePanel.SetActive(false);
        audioSource.clip = _SM.closeMenuSound;
        audioSource.Play();
    }
    public void CloseHorgrMenu()
    {
        horgrPanel.SetActive(false);
        audioSource.clip = _SM.closeMenuSound;
        audioSource.Play();
    }
    public void CloseHutMenu()
    {
        hutPanel.SetActive(false);
        audioSource.clip = _SM.closeMenuSound;
        audioSource.Play();
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
        treesText.text = _GM.trees.Length.ToString() + "/" + _GM.maxTrees;
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
        if(_GM.currentWave!= 10)
        waveOverPanel.SetActive(true);
        
    }
    public void OnContinueButton()
    {
        waveOverPanel.SetActive(false);
        treeToolImage.sprite = usableTreeTool;
        collectMaegenButton.SetActive(false);
        StartCoroutine(WaitToCheckForToolButtons());
    }
    IEnumerator WaitToCheckForToolButtons()
    {
        yield return new WaitForSeconds(1);
        if (_GM.runes.Length == 0)
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
        if(_GM.runes.Length == 1)
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
        if (_GM.runes.Length == 2)
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
        if (_GM.runes.Length == 3)
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
        if (_GM.runes.Length > 3)
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
    private void OnEnable()
    {
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
