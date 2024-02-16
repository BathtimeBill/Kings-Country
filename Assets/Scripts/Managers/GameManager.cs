using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameManager : Singleton<GameManager>
{
    public bool tutorial;
    public float gameTime = 0;
    public LevelNumber level;
    public PlayMode playmode;
    public GameState gameState;
    public List<GameObject> trees;
    public bool isPaused;
    public int startingMaegen;
    public GameObject boundry;
    public GameObject introCam;

    [Header("Waves")]
    public int currentWave;
    public int agroWaveLength;
    public int breakWaveLenth;
    public bool agroWave;
    public bool canFinishWave;
    public bool downTime;



    [Header("Player Resources")]
    public int maegen;
    public int maxTrees;
    public int wildlife;
    public int populous;
    public int maxPopulous = 10;
    public int maxMaegen;

    [Header("Runes")]
    public List<GameObject> runes;

    [Header("Beacons")]
    public GameObject[] beacons;

    [Header("Unit Health")]
    public float satyrHealth;
    public float orcusHealth;
    public float leshyHealth;
    public float huldraHealth;
    public float skessaHealth;
    public float goblinHealth;
    public float golemHealth;
    public float dryadHealth;
    public float towerHealth;
    public float spitTowerHealth;

    [Header("Unit Damage")]
    public float satyrDamage;
    public float orcusDamage;
    public float leshyDamage;
    public float skessaDamage;
    public float goblinDamage;
    public float golemDamage;
    public float dryadDamage;
    public float spitDamage;
    public float spitExplosionDamage;

    [Header("Unit Speed")]
    public float satyrSpeed;
    public float orcusSpeed;
    public float leshySpeed;
    public float skessaSpeed;
    public float goblinSpeed;
    public float golemSpeed;
    public float dryadSpeed;
    public float huldraSpeed;

    [Header("Unit Price")]
    public int satyrPrice;
    public int orcusPrice;
    public int leshyPrice;
    public int skessaPrice;
    public int goblinPrice;
    public int huldraPrice;
    public int golemPrice;
    public int dryadPrice;

    [Header("Enemy Health")]
    public float watheHealth;
    public float hunterHealth;
    public float bjornjeggerHealth;
    public float drengHealth;
    public float beserkrHealth;
    public float knightHealth;
    public float loggerHealth;
    public float lumberjackHealth;
    public float lordHealth;

    [Header("Enemy Damage")]
    public float axe1Damage;
    public float axe2Damage;
    public float sword2Damage;
    public float sword3Damage;
    public float arrow1Damage;
    public float arrow2Damage;
    public float lordDamage;

    [Header("Horgr")]
    public bool horgrClaimedByPlayer;

    [Header("Alert")]
    public float timeSinceAttack = 0f;
    public float timeSinceWildlifeKilled = 0f;
    public GameObject warningSprite;

    [Header("Time")]
    public AudioSource timeAudioSource;
    public Volume globalVolume;
    private FilmGrain grain;
    private ChromaticAberration chromaticAberration;

    void Start()
    {
        Time.timeScale = 1;
        maegen = startingMaegen;
        currentWave = 0;
        playmode = PlayMode.DefaultMode;
        gameState = GameState.Pause;
        trees.AddRange(GameObject.FindGameObjectsWithTag("Tree"));
        StartCoroutine(EndOfIntroCamera());
        _UI.CheckTreeUI();
        downTime = true;
    }
    IEnumerator EndOfIntroCamera()
    {
        yield return new WaitForSeconds(10);
        introCam.SetActive(false);
        gameState = GameState.Play;
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        timeSinceWildlifeKilled += Time.deltaTime;

    }
    public float GetPercentageIncrease(float originalValue, float percentage)
    {
        float f = originalValue + (originalValue * percentage);
        return f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        timeAudioSource.clip = _SM.timeStopSound;
        timeAudioSource.Play();
        globalVolume.profile.TryGet(out grain);
        globalVolume.profile.TryGet(out chromaticAberration);
        chromaticAberration.intensity.value = 0;
        grain.intensity.value = 0;
        Time.timeScale = 1;

    }
    public void SpeedGame()
    {
        timeAudioSource.clip = _SM.timeSpeedUpSound;
        timeAudioSource.Play();
        globalVolume.profile.TryGet(out grain);
        globalVolume.profile.TryGet(out chromaticAberration);
        chromaticAberration.intensity.value = 1;
        grain.intensity.value = 1;
        Time.timeScale = 3;
    }

    //The wave system is managed by two coroutines. The 'agro' wave lasts 1 minute, during which enemies are spawned in (EnemyManager) and then a break period of 4 mins. 
    IEnumerator ManageWaveAgro()
    {
        _UI.nextRoundButton.interactable = false;
        _UI.treeToolImage.sprite = _UI.unusableTreeTool;
        _GM.downTime = false;
        agroWave = true;
        currentWave++;
        _UI.TriggerWaveTextAnimation();
        _UI.CheckWave();
        yield return new WaitForSeconds(agroWaveLength);
        agroWave = false;
        StartCoroutine(CheckForCollectMaegen());
    }
    IEnumerator CheckForCollectMaegen()
    {
        yield return new WaitForSeconds(5);
       
        int totalEnemies = _HM.enemies.Count + _HUTM.enemies.Count;
        if (totalEnemies == _EM.enemies.Count)
        {
            if(canFinishWave)
            {
                GameEvents.ReportOnJustStragglers();
            }
        }
        StartCoroutine(CheckForCollectMaegen());
    }
    IEnumerator WaitForCanFinishWave()
    {
        yield return new WaitForSeconds(60);
        canFinishWave = true;
    }
    IEnumerator ManageWaveBreak()
    {
        agroWave = false;
        yield return new WaitForSeconds(breakWaveLenth);
        StartCoroutine(ManageWaveAgro());
    }

    public void ContinueToNextRound()
    {
        StartCoroutine(ManageWaveAgro());
        StartCoroutine(WaitForCanFinishWave());
        GameEvents.ReportOnStartNextRound();
        _SM.PlaySound(_SM.waveBeginSound);
    }

    //Checks the scene for how many Runes are present.
    public void CheckRunes()
    {
        runes = GameObject.FindGameObjectsWithTag("RuneObject").ToList<GameObject>();
    }
    public void CheckBeacons()
    {
        beacons = GameObject.FindGameObjectsWithTag("Beacon");
    }
    //Checks the scene for how many player units are present.
    public int CheckPopulous()
    {
        populous = UnitSelection.Instance.unitList.Count;
        return populous;
    }
    //Updates the the count for how many trees exist and updates the UI.
    private void OnTreePlaced()
    {
        //trees = GameObject.FindGameObjectsWithTag("Tree");
        if (_TPlace.maegenPerWave <= 1)
        {
            maegen -= 2;
        }
        if (_TPlace.maegenPerWave == 2)
        {
            maegen -= 3;
        }
        if (_TPlace.maegenPerWave == 3)
        {
            maegen -= 4;
        }
        _UI.CheckTreeUI();
    }

    //When a tree is destroyed, we wait one frame and update the tree count and UI.
    private void OnTreeDestroy()
    {
        StartCoroutine(WaitForTreeDestroy());
    }
    IEnumerator WaitForTreeDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        //trees = GameObject.FindGameObjectsWithTag("Tree");
        _UI.CheckTreeUI();
    }

    //Once the 'Jarnnefi' upgrade has been purchased, the damage values for the player units are increased by 30%.
    public void OnJarnnefiUpgrade()
    {
        satyrDamage += satyrDamage * 0.3f;
        orcusDamage += orcusDamage * 0.3f;
        leshyDamage += leshyDamage * 0.3f;
        skessaDamage += skessaDamage * 0.3f;
        goblinDamage += goblinDamage * 0.3f;
    }

    //When a rune has been depleted, we wait until the end of the frame and update the rune count and UI.
    public void OnRuneDestroyed()
    {
        StartCoroutine(WaitForRuneCheck());
    }
    IEnumerator WaitForRuneCheck()
    {
        yield return new WaitForEndOfFrame();
        CheckRunes();
    }
    public void OnBeaconDestroyed()
    {
        StartCoroutine(WaitForBeaconCheck());
    }
    IEnumerator WaitForBeaconCheck()
    {
        yield return new WaitForEndOfFrame();
        CheckBeacons();
    }
    private void OnPopulousUpgrade()
    {
        maxPopulous += 5;
        _UI.CheckPopulousUI();
    }

    public void OnTreeHit()
    {
        if(timeSinceAttack > 30)
        {
            _UI.SetErrorMessageYouAreUnderAttack();
            _PC.Error();
            _UI.warningAudioSource.clip = _SM.warningSound;
            _UI.warningAudioSource.Play();
        }
        timeSinceAttack = 0;
    }

    public void OnWildlifeKilled()
    {
        if (timeSinceWildlifeKilled > 30)
        {
            _UI.SetErrorMessageYourWildlifeIsUnderAttack();
            _PC.Error();
            _UI.warningAudioSource.clip = _SM.warningSound;
            _UI.warningAudioSource.Play();
        }
        timeSinceWildlifeKilled = 0;
    }

    public void OnWaveOver()
    {
        canFinishWave = false;
        agroWave = false;
        gameState = GameState.Pause;
        Time.timeScale = 0;
        StopCoroutine(CheckForCollectMaegen());
        GameEvents.ReportOnRuneDestroyed();

    }
    public void OnContinueButton()
    {
        gameState = GameState.Play;
        Time.timeScale = 1;
        StopCoroutine(CheckForCollectMaegen());
        _GM.boundry.SetActive(true);
        maxMaegen = _WM.totalMaegen + maegen;
        PlayGame();
    }
    public void OnCollectMaegenButton()
    {
        StopCoroutine(CheckForCollectMaegen());
    }
    private void OnStartNextRound()
    {
        boundry.SetActive(false);
    }
    private void OnWispDestroy()
    {
        if(maegen > maxMaegen)
            maegen = maxMaegen;
    }
    private void OnUnitKilled()
    {
        if(UnitSelection.Instance.unitList.Count == 0)
        {
            if(maegen == 0)
            {
                GameEvents.ReportOnGameOver();
            }
        }
    }
    private void OnEnable()
    {
        GameEvents.OnUnitKilled += OnUnitKilled;
        GameEvents.OnWispDestroy += OnWispDestroy;
        GameEvents.OnTreePlaced += OnTreePlaced;
        GameEvents.OnTreeDestroyed += OnTreeDestroy;
        GameEvents.OnJarnnefiUpgrade += OnJarnnefiUpgrade;
        GameEvents.OnRuneDestroyed += OnRuneDestroyed;
        GameEvents.OnBeaconDestroyed += OnBeaconDestroyed;
        GameEvents.OnPopulousUpgrade += OnPopulousUpgrade;
        GameEvents.OnTreeHit += OnTreeHit;
        GameEvents.OnWildlifeKilled += OnWildlifeKilled;
        GameEvents.OnWaveOver += OnWaveOver;
        GameEvents.OnStartNextRound += OnStartNextRound;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnCollectMaegenButton += OnCollectMaegenButton;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitKilled -= OnUnitKilled;
        GameEvents.OnWispDestroy -= OnWispDestroy;
        GameEvents.OnTreePlaced -= OnTreePlaced;
        GameEvents.OnTreeDestroyed -= OnTreeDestroy;
        GameEvents.OnJarnnefiUpgrade -= OnJarnnefiUpgrade;
        GameEvents.OnRuneDestroyed -= OnRuneDestroyed;
        GameEvents.OnBeaconDestroyed -= OnBeaconDestroyed;
        GameEvents.OnPopulousUpgrade -= OnPopulousUpgrade;
        GameEvents.OnTreeHit -= OnTreeHit;
        GameEvents.OnWildlifeKilled -= OnWildlifeKilled;
        GameEvents.OnWaveOver -= OnWaveOver;
        GameEvents.OnStartNextRound -= OnStartNextRound;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnCollectMaegenButton -= OnCollectMaegenButton;
    }
}
