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
    public StartCamera startCamera;

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
            ChangeGameState(GameState.Intro);
        else
            CheckTutorial();
    }

    public void CheckTutorial()
    {
        if (thisLevel == LevelID.Ironwood)
        {
            if (_TESTING.overrideTutorial && !_TESTING.showTutorial)
                ChangeGameState(GameState.Build);
            else
                ChangeGameState(GameState.Tutorial);
        }
        else
            ChangeGameState(GameState.Build);
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
                startCamera.gameObject.SetActive(true);
                startCamera.StartCamAnimation(thisLevel);
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
                _TUTORIAL.StartTutorial();
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
        if (maegen >= maxMaegen)
            return;
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
        dayAgroTimeLimit = initialAgroLength += 7;  //TODO adds 10 seconds per day 
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
        populous = _UM.unitList.Count;
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

        if(_UM.unitList.Count == 0)
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
        int maegenAmount = maegen;
        int wildlifeAmount = wildlife;
        int treeAmount = trees.Count;
        int populousAmount = populous;

        int maegenBonus;
        if (MathX.InRange(maegenAmount, 0, 9))
            maegenBonus = 1;
        else if (MathX.InRange(maegenAmount, 20, 29))
            maegenBonus = 2;
        else if (MathX.InRange(maegenAmount, 30, 39))
            maegenBonus = 3;
        else
            maegenBonus = 4;

        int treeBonus;
        if (MathX.InRange(treeAmount, 0, 19))
            treeBonus = 1;
        else if (MathX.InRange(treeAmount, 20, 29))
            treeBonus = 2;
        else if (MathX.InRange(treeAmount, 30, 39))
            treeBonus = 3;
        else
            treeBonus = 4;

        int wildlifeBonus;
        if (MathX.InRange(wildlifeAmount, 0, 5))
            wildlifeBonus = 1;
        else if (MathX.InRange(wildlifeAmount, 6, 10))
            wildlifeBonus = 2;
        else if (MathX.InRange(wildlifeAmount, 11, 20))
            wildlifeBonus = 3;
        else
            wildlifeBonus = 4;

        int populousBonus;
        if (MathX.InRange(populousAmount, 0, 5))
            populousBonus = 1;
        else if (MathX.InRange(populousAmount, 6, 7))
            populousBonus = 2;
        else if (MathX.InRange(populousAmount, 8, 9))
            populousBonus = 3;
        else
            populousBonus = 4;

        int finalScore = (maegenAmount * maegenBonus) + (treeAmount * treeBonus) + (wildlifeAmount * wildlifeBonus) + (populousAmount * populousBonus);
        //Log("Final Score: " + finalScore);
        score = finalScore;
        StartCoroutine(_UI.UpdateWinUI(maegenAmount, maegenBonus, treeAmount, treeBonus, wildlifeAmount, wildlifeBonus, populousAmount, populousBonus, finalScore));
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
