using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public Sprite usableBeaconTool;
    public Sprite unusableBeaconTool;

    public Image beaconToolImage;

    public Slider beaconCooldownSlider;
    public GameObject beaconCooldownSliderObject;
    public float beaconTimeLeft;
    public float beaconMaxTimeLeft;


    public bool beaconPlaced;

    public Slider stormerCooldownSlider;
    public GameObject stormerCooldownSliderObject;
    public float stormerTimeLeft;
    public float stormerMaxTimeLeft;
    public Image stormerToolImage;
    public Sprite usableStormerTool;
    public Sprite unusableStormerTool;

    public bool stormerPlaced;

    public GameObject maegenCost;
    public TMP_Text maegenCostText;
    public GameObject wildlifeCost;
    public TMP_Text wildlifeCostText;

    public AudioSource audioSource;
    public AudioSource warningAudioSource;

    void Start()
    {
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

        if(beaconPlaced)
        {
            beaconTimeLeft += 1 * Time.deltaTime;
            beaconCooldownSlider.value = CalculateTimeLeft();
            beaconToolImage.sprite = unusableBeaconTool;
            if(beaconTimeLeft >= beaconMaxTimeLeft)
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

    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
        GameEvents.OnBeaconPlaced += OnBeaconPlaced;
        GameEvents.OnStormerPlaced += OnStormerPlaced;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
        GameEvents.OnBeaconPlaced -= OnBeaconPlaced;
        GameEvents.OnStormerPlaced -= OnStormerPlaced;
    }
}
