using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : Singleton<GameManager>
{
    [Header("Level Related")]
    public LevelID thisLevel;

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

    [Header("Score")]
    public int score;
    public int highScore;

    [Header("Days")]
    public int currentDay;
    private float currentAgroTime;
    private float dayAgroTimeLimit;
    public float initialAgroLength;
    public bool agroPhase => currentAgroTime < dayAgroTimeLimit;

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
    public ToolData runesTool;
    public int[] runesMaegenCost;
    public int[] runesWildlifeCost;
    public List<GameObject> runes;
    public float runeHealRate = 12;

    [Header("Fyre")]
    public ToolData fyreTool;

    [Header("Stormer")]
    public ToolData stormerTool;

    [Header("Unit Health")]
    public float towerHealth;
    public float spitTowerHealth;

    [Header("Unit Damage")]
    public float spitDamage;
    public float spitExplosionDamage;

    [Header("Enemy Damage")]
    //public float axe1Damage; - Logger
    //public float axe2Damage; - Lumberjack
    //public float sword2Damage; - Dreng
    //public float sword3Damage; - Bezerker
    //public float arrow1Damage; - Wathe
    //public float arrow2Damage; - Hunter
    //public float lordDamage; - Lord

    [Header("Horgr")]
    public bool horgrClaimedByPlayer;

    [Header("Alert")]
    public float timeSinceAttack = 0f;
    public float timeSinceWildlifeKilled = 0f;
    public GameObject warningSprite;

    [Header("Time")]
    public AudioSource timeAudioSource;

    public bool fyreAvailable => _DATA.CanUseTool(ToolID.Fyre);
    public bool stormerAvailable => _DATA.CanUseTool(ToolID.Stormer);
    public bool runesAvailable => wildlife >= runesWildlifeCost[runesCount] && maegen >= runesMaegenCost[runesCount] && !atMaxRuins;
    public bool atMaxRuins => runesCount == runesMaegenCost.Length;
    public int runesCount => runes.Count;

    public void SetPlayMode(PlayMode _mode) => playmode = _mode;
    public void SetPreviousState(GameState _gs) => previousState = _gs;

    void Start()
    {
        SetPreviousState(GameState.Build);
        IncreaseMaegen(startingMaegen);
        currentDay = 0;
        SetPlayMode(PlayMode.DefaultMode);
        SetGame();
        trees.AddRange(GameObject.FindGameObjectsWithTag("Tree"));
        if (!_TESTING.skipIntro)
            StartCoroutine(EndOfIntroCamera());
        _UI.CheckTreeUI();
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

        if (_inDay)
        {
            currentAgroTime += Time.deltaTime;
            _UI.UpdateAgroBar(currentAgroTime, dayAgroTimeLimit);
        }

    }

    public float GetPercentageIncrease(float originalValue, float percentage)
    {
        float f = originalValue + (originalValue * percentage);
        return f;
    }

    public void ChangeGameState(GameState _gameState)
    {
        gameState = _gameState;
        switch (gameState)
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
            case GameState.Transitioning:
                Time.timeScale = 1;
                break;
            case GameState.Glossary:
                Time.timeScale = 0;
                break;
            case GameState.Tutorial:
                Time.timeScale = 1;
                _UI.TogglePanel(_UI.pausePanel, false);
                break;
            case GameState.Lose:
                Time.timeScale = 1;
                break;
        }
        GameEvents.ReportOnGameStateChanged(gameState);
    }

    public void SetGame()
    {
        maxMaegen = _UI.totalMaegen + maegen;
        timeAudioSource.clip = _SM.timeStopSound;
        timeAudioSource.Play();
        _EFFECTS.SetChromatic(0);
        _EFFECTS.SetGrain(0);
        Time.timeScale = 1;
    }
    public void SpeedGame()
    {
        timeAudioSource.clip = _SM.timeSpeedUpSound;
        timeAudioSource.Play();
        _EFFECTS.SetChromatic(1);
        _EFFECTS.SetGrain(1);
        Time.timeScale = 3;
    }

    public void IncreaseMaegen(int _value)
    {
        _UI.UpdateMaegenText(maegen, maegen + _value);
        maegen += _value;
        GameEvents.ReportOnMaegenChange(maegen);
    }

    public void DecreaseMaegen(int _value)
    {
        _UI.UpdateMaegenText(maegen, maegen - _value);
        maegen -= _value;
        GameEvents.ReportOnMaegenChange(maegen);
    }

    public void BeginNewDay()
    {
        currentDay++;
        dayAgroTimeLimit = initialAgroLength + 10;  //TODO adds 10 seconds per day 
        ChangeGameState(GameState.Play);
        _EM.BeginNewDay();
        _UI.BeginNewDay();
        GameEvents.ReportOnDayBegin();
        ExecuteAfterSeconds(1, () => currentAgroTime = 0f);
        //boundry.SetActive(false);
    }

    public void ContinueToNextRound()
    {
        //StartCoroutine(ManageWaveAgro());
        //StartCoroutine(WaitForCanFinishWave());
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
    private void OnTreePlaced(ToolID _treeID)
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
    private void OnTreeDestroy(ToolID _id)
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
        //TODO - How does this work?
        //satyrDamage += satyrDamage * 0.3f;
        //orcusDamage += orcusDamage * 0.3f;
        //leshyDamage += leshyDamage * 0.3f;
        //skessaDamage += skessaDamage * 0.3f;
        //goblinDamage += goblinDamage * 0.3f;
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
            _UI.SetError(ErrorID.ForestUnderAttack);
            _UI.warningAudioSource.clip = _SM.warningSound;
            _UI.warningAudioSource.Play();
        }
        timeSinceAttack = 0;
    }

    public void OnDayOver()
    {
        //print(currentDay + " / " + _DATA.levelMaxDays);
        if(currentDay == _DATA.levelMaxDays)
        {
            SetGame();
            GameEvents.ReportOnGameWin(_DATA.currentLevelID, score, maegen);
            CalculateScore();
            ChangeGameState(GameState.Finish);
        }
        else
        {
            ChangeGameState(GameState.Win);
            GameEvents.ReportOnRuneDestroyed();
        }
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
    
    private void OnWispDestroy()
    {
        if(maegen > maxMaegen)
            maegen = maxMaegen;
    }
    private void OnUnitKilled(string _unitID, string _killedBy, int _daysSurvived)
    {
        if (!_DATA.IsCreatureUnit(_unitID))
            return;

        if(UnitSelection.Instance.unitList.Count == 0)
        {
            if(maegen == 0)
            {
                GameEvents.ReportOnGameOver();
            }
        }
    }

    public void OnWildlifeKilled()
    {
        if (timeSinceWildlifeKilled > 30)
        {
            _UI.SetError(ErrorID.WildlifeUnderAttack);
            _UI.warningAudioSource.clip = _SM.warningSound;
            _UI.warningAudioSource.Play();
        }
        timeSinceWildlifeKilled = 0;
    }

    private void OnWildlifeValueChange(int _value)
    {
        wildlife = _value;
    }

    //Score Stuff
    public void CalculateScore()
    {
        int maegenBonus = maegen;
        int wildlifeBonus = wildlife * 2;
        int treeBonus = 1;

        if (trees.Count > 0 && trees.Count < 10)
        {
            treeBonus = 1;
        }
        if (trees.Count > 9 && trees.Count < 20)
        {
            treeBonus = 1;
        }
        if (trees.Count > 19 && trees.Count < 30)
        {
            treeBonus = 2;
        }
        if (trees.Count > 29 && trees.Count < 40)
        {
            treeBonus = 3;
        }
        if (trees.Count == 40)
        {
            treeBonus = 4;
        }

        int finalScore = (maegenBonus + wildlifeBonus) * treeBonus;
        print("Final Score: " + finalScore);
        score = finalScore;
        _UI.UpdateWinUI(maegenBonus, treeBonus, wildlifeBonus, finalScore);
    }

    private void OnGameStateChanged(GameState _gameState)
    {
        if(_gameState != gameState)
            ChangeGameState(_gameState);
    }

    private void OnGameOver()
    {
        ChangeGameState(GameState.Lose);
    }

    private void OnEnable()
    {
        GameEvents.OnGameStateChanged += OnGameStateChanged;
        GameEvents.OnCreatureKilled += OnUnitKilled;
        GameEvents.OnWispDestroy += OnWispDestroy;
        GameEvents.OnTreePlaced += OnTreePlaced;
        GameEvents.OnTreeDestroyed += OnTreeDestroy;
        GameEvents.OnJarnnefiUpgrade += OnJarnnefiUpgrade;
        GameEvents.OnRuneDestroyed += OnRuneDestroyed;
        GameEvents.OnPopulousUpgrade += OnPopulousUpgrade;
        GameEvents.OnTreeHit += OnTreeHit;
        GameEvents.OnDayOver += OnDayOver;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnWildlifeKilled += OnWildlifeKilled;
        GameEvents.OnWildlifeValueChange += OnWildlifeValueChange;
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
        GameEvents.OnCreatureKilled -= OnUnitKilled;
        GameEvents.OnWispDestroy -= OnWispDestroy;
        GameEvents.OnTreePlaced -= OnTreePlaced;
        GameEvents.OnTreeDestroyed -= OnTreeDestroy;
        GameEvents.OnJarnnefiUpgrade -= OnJarnnefiUpgrade;
        GameEvents.OnRuneDestroyed -= OnRuneDestroyed;
        GameEvents.OnPopulousUpgrade -= OnPopulousUpgrade;
        GameEvents.OnTreeHit -= OnTreeHit;
        GameEvents.OnDayOver -= OnDayOver;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnWildlifeKilled -= OnWildlifeKilled;
        GameEvents.OnWildlifeValueChange -= OnWildlifeValueChange;
        GameEvents.OnGameOver -= OnGameOver;
    }
}
