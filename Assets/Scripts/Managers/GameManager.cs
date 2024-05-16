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
    public GameState previousState;
    public List<GameObject> trees;
    public int startingMaegen;
    public GameObject boundry;
    public GameObject introCam;

    [Header("Waves")]
    public int currentWave;
    public int agroWaveLength;
    public int breakWaveLenth;
    public bool agroWave;
    public bool canFinishWave;
    public bool peaceTime;

    [Header("Buildings Cooldown")]
    public float treeCooldown;
    public float hutCooldown;
    public float horgrCooldown;

    [Header("Player Resources")]
    public int maegen;
    public int maxTrees;
    public int wildlife;
    public int populous;
    public int maxPopulous = 10;
    public int maxMaegen;

    [Header("Runes")]
    public Tool runesTool;
    public int[] runesMaegenCost;
    public int[] runesWildlifeCost;
    public List<GameObject> runes;
    public float runeHealRate = 12;

    [Header("Fyre")]
    public Tool fyreTool;
    public GameObject[] beacons;

    [Header("Stormer")]
    public Tool stormerTool;

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

    public bool fyreAvailable => wildlife >= fyreTool.cost;
    public bool stormerAvailable => wildlife >= stormerTool.cost;
    public bool runesAvailable => wildlife >= runesWildlifeCost[runesCount] && maegen >= runesMaegenCost[runesCount] && !atMaxRuins;
    public bool atMaxRuins => runesCount == runesMaegenCost.Length;
    public int runesCount => runes.Count;

    public bool skipIntro;

    void Start()
    {
        Time.timeScale = 1;
        IncreaseMaegen(startingMaegen);
        currentWave = 0;
        playmode = PlayMode.DefaultMode;
        SetGame();
        trees.AddRange(GameObject.FindGameObjectsWithTag("Tree"));
        if(!skipIntro) 
            StartCoroutine(EndOfIntroCamera());
        _UI.CheckTreeUI();
        peaceTime = true;
    }
    IEnumerator EndOfIntroCamera()
    {
        yield return new WaitForSeconds(10);
        introCam.SetActive(false);
        gameState = GameState.Play;
        GameEvents.ReportOnGameStateChanged(gameState);
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

    public void ChangeGameState(GameState _gameState)
    {
        gameState = _gameState;
        switch(gameState)
        {
            case GameState.Intro:
                Time.timeScale = 1;
                break;
            case GameState.Build:
                Time.timeScale = 1;
                _UI.TogglePanel(_UI.pausePanel, false);
                break;
            case GameState.Play:
                Time.timeScale = 1;
                _UI.TogglePanel(_UI.pausePanel, false);
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                _UI.TogglePanel(_UI.pausePanel, true);
                break;
            case GameState.Win:
                Time.timeScale = 1;
                break;

        }
        GameEvents.ReportOnGameStateChanged(_gameState);
    }

    public void SetGame()
    {
        print("SET GAME!");
        //_GM.boundry.SetActive(true);
        maxMaegen = _UI.totalMaegen + maegen;
        gameState = GameState.Build;
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

    public void IncreaseMaegen(int _value)
    {
        maegen += _value;
        _UI.UpdateMaegenText(maegen);
    }

    public void DecreaseMaegen(int _value)
    {
        maegen -= _value;
        _UI.UpdateMaegenText(maegen);
    }

    //The wave system is managed by two coroutines. The 'agro' wave lasts 1 minute, during which enemies are spawned in (EnemyManager) and then a break period of 4 mins. 
    IEnumerator ManageWaveAgro()
    {
        _UI.nextRoundButton.interactable = false;
        _GM.peaceTime = false;
        agroWave = true;
        currentWave++;
        _UI.TriggerWaveTextAnimation();
        _UI.CheckWave();
        yield return new WaitForSeconds(agroWaveLength);
        agroWave = false;
    }
    //IEnumerator CheckForCollectMaegen()
    //{
    //    yield return new WaitForSeconds(5);
       
    //    int totalEnemies = _HM.enemies.Count + _HUTM.enemies.Count;
    //    if (totalEnemies == _EM.enemies.Count)
    //    {
    //        if(canFinishWave)
    //        {
    //            GameEvents.ReportOnJustStragglers();
    //        }
    //    }
    //    StartCoroutine(CheckForCollectMaegen());
    //}
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
        GameEvents.ReportOnWaveBegin();
        //_SM.PlaySound(_SM.waveBeginSound);
    }

    public void AddRune(GameObject _rune)
    {
        runes.Add(_rune);
    }

    //Checks the scene for how many Runes are present.
    public void CheckRunes()
    {
        runes = GameObject.FindGameObjectsWithTag("RuneObject").ToList<GameObject>();
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
        ////trees = GameObject.FindGameObjectsWithTag("Tree");
        //if (_TPlace.maegenPerWave <= 1)
        //{
        //    maegen -= 2;
        //}
        //if (_TPlace.maegenPerWave == 2)
        //{
        //    maegen -= 3;
        //}
        //if (_TPlace.maegenPerWave == 3)
        //{
        //    maegen -= 4;
        //}
        //_UI.CheckTreeUI();
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
        ChangeGameState(GameState.Win);
        GameEvents.ReportOnRuneDestroyed();

    }
    public void OnContinueButton()
    {
        ChangeGameState(GameState.Build);
        SetGame();
    }
    //public void OnCollectMaegenButton()
    //{
    //    StopCoroutine(CheckForCollectMaegen());
    //}
    private void OnWaveBegin()
    {
        gameState = GameState.Play;
        //boundry.SetActive(false);
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

    void OnGameWin()
    {
        
    }
    private void OnEnable()
    {
        GameEvents.OnUnitKilled += OnUnitKilled;
        GameEvents.OnWispDestroy += OnWispDestroy;
        GameEvents.OnTreePlaced += OnTreePlaced;
        GameEvents.OnTreeDestroyed += OnTreeDestroy;
        GameEvents.OnJarnnefiUpgrade += OnJarnnefiUpgrade;
        GameEvents.OnRuneDestroyed += OnRuneDestroyed;
        GameEvents.OnPopulousUpgrade += OnPopulousUpgrade;
        GameEvents.OnTreeHit += OnTreeHit;
        GameEvents.OnWildlifeKilled += OnWildlifeKilled;
        GameEvents.OnWaveOver += OnWaveOver;
        GameEvents.OnWaveBegin += OnWaveBegin;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnGameWin += OnGameWin;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitKilled -= OnUnitKilled;
        GameEvents.OnWispDestroy -= OnWispDestroy;
        GameEvents.OnTreePlaced -= OnTreePlaced;
        GameEvents.OnTreeDestroyed -= OnTreeDestroy;
        GameEvents.OnJarnnefiUpgrade -= OnJarnnefiUpgrade;
        GameEvents.OnRuneDestroyed -= OnRuneDestroyed;
        GameEvents.OnPopulousUpgrade -= OnPopulousUpgrade;
        GameEvents.OnTreeHit -= OnTreeHit;
        GameEvents.OnWildlifeKilled -= OnWildlifeKilled;
        GameEvents.OnWaveOver -= OnWaveOver;
        GameEvents.OnWaveBegin -= OnWaveBegin;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnGameWin -= OnGameWin;
    }
}
