using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;

public class GameManager : Singleton<GameManager>
{
    #region Varialbles
    [Header("Level Related")]
    public LevelID thisLevel;

    public HomeTree homeTree;
    public Hut hut;
    public Horgr horgr;

    public bool tutorial;
    public float gameTime = 0;
    public LevelNumber level;
    public PlayMode playmode;
    public GameState gameState;
    public GameState previousState;
    public List<GameObject> trees;
    public StartCamera startCamera;
    public GameSpeed gameSpeed = GameSpeed.Normal;

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
    public int populous;
    public int maxPopulous = 10;
    public int maxMaegen;
    public int treeCount => trees.Count;

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
    
    [Header("Wildlife")]
    public float wildlifeSpawnRadius = 60f;

    public Transform tutorialWildlifeSpawnLocation;
    public List<GameObject> currentWildlife = new List<GameObject>();
    public int wildlifeCount => currentWildlife.Count;
    public List<WildlifeID> availableWildlife = new List<WildlifeID>();
    [HideInInspector] public float numberOfWildlifeToSpawn;

    public bool fyreAvailable => _DATA.CanUseTool(ToolID.Fyre);
    public bool stormerAvailable => _DATA.CanUseTool(ToolID.Stormer);
    public bool runesAvailable => wildlifeCount >= runesWildlifeCost[runesCount] && maegen >= runesMaegenCost[runesCount] && !atMaxRuins;
    public bool atMaxRuins => runesCount == runesMaegenCost.Length;
    public int runesCount => runes.Count;
    public GameObject GetRandomTree => ListX.GetRandomItemFromList(trees);
    
    #endregion

    public void SetPlayMode(PlayMode _mode) => playmode = _mode;
    public void SetPreviousState(GameState _gs) => previousState = _gs;

    private new void Awake()
    {
        base.Awake();
        homeTree = FindFirstObjectByType<HomeTree>();
        CheckSites();
    }
    void Start()
    {
        SetPreviousState(GameState.Build);
        IncreaseMaegen(_DATA.GetLevel(thisLevel).startingMaegen);
        currentDay = 0;
        SetPlayMode(PlayMode.DefaultMode);
        SetGame();
        trees.AddRange(GameObject.FindGameObjectsWithTag("Tree"));
        

        ExecuteAfterFrames(1, () =>
        {
            if (!_inTutorial)
                WildlifeInstantiate();
        });

        if (!_TESTING.skipIntro)
            ChangeGameState(GameState.Intro);
        else
            CheckTutorial();
    }

    private void CheckSites()
    {
        if (_DATA.currentLevel.availableBuildings.Contains(SiteID.Hut))
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("HutSpawnPoint");
            Transform t = ArrayX.GetRandomItemFromArray(spawnPoints).transform;
            GameObject go = Instantiate(_DATA.GetSitePrefab(SiteID.Hut), t.position, t.rotation);
            hut = go.GetComponent<Hut>();
        }
        if (_DATA.currentLevel.availableBuildings.Contains(SiteID.Horgr))
        {
            GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("HorgrSpawnPoint");
            Transform t = ArrayX.GetRandomItemFromArray(spawnPoints).transform;
            GameObject go = Instantiate(_DATA.GetSitePrefab(SiteID.Horgr), t.position, t.rotation);
            horgr = go.GetComponent<Horgr>();
        }  
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
        SetGameSpeed(GameSpeed.Normal);
    }
    
    public void SetGameSpeed(GameSpeed _gameSpeed)
    {
        gameSpeed = _gameSpeed;
        switch (_gameSpeed)
        {
            case GameSpeed.Normal:
                timeAudioSource.clip = _SM.timeStopSound;
                _EFFECTS.SetChromatic(0);
                _EFFECTS.SetGrain(0);
                Time.timeScale = 1;
                break;
            case GameSpeed.Fast:
                timeAudioSource.clip = _SM.timeSpeedUpSound;
                _EFFECTS.SetChromatic(1);
                _EFFECTS.SetGrain(1);
                Time.timeScale = 3;
                break;
        }
        timeAudioSource.Play();
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
        GameEvents.ReportOnDayBegin(currentDay);
        ExecuteAfterSeconds(1, () => currentAgroTime = 0f);
        StartCoroutine(SpawnPickups());
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

    //Checks the scene for how many player units are present.
    public int CheckPopulous()
    {
        populous = _UM.unitList.Count;
        return populous;
    }

    private void OnTreePlaced(TreeID _treeID)
    {
    }

    //When a tree is destroyed, we wait one frame and update the tree count and UI.
    private void OnTreeDestroy(TreeID _id)
    {
        StartCoroutine(WaitForTreeDestroy());
    }
    IEnumerator WaitForTreeDestroy()
    {
        yield return new WaitForSeconds(0.1f);
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

    public void OnDayOver(int _day)
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
        
        //Wildlife Stuff
        numberOfWildlifeToSpawn = Mathf.Round(_DATA.HasPerk(PerkID.Fertile) ? treeCount / 3f : treeCount / 5f);
    }
    public void OnContinueButton()
    {
        ChangeGameState(GameState.Build);
        SetGame();
        
        for (int i = 0; i < numberOfWildlifeToSpawn; i++)
        {
            WildlifeInstantiate();
        }
    }
    
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



    //Score Stuff
    public void CalculateScore()
    {
        int maegenAmount = maegen;
        int wildlifeAmount = wildlifeCount;
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

    #region Wildlife
    //This finds a random location within a sphere from the home tree and spawns an animal there.
    public void WildlifeInstantiate(bool _tutorial = false)
    {
        CheckForAvailableWildlife();
        WildlifeData wildlifeData = _DATA.GetWildlife(ListX.GetRandomItemFromList(availableWildlife));
        Vector3 randomLocation = transform.position + Random.insideUnitSphere * wildlifeSpawnRadius;
        NavMesh.SamplePosition(randomLocation, out NavMeshHit hit, wildlifeSpawnRadius, 1);
        Vector3 spawnLocation = _tutorial ? tutorialWildlifeSpawnLocation.position : hit.position;
        GameObject spawnAnimal = Instantiate(wildlifeData.playModel, spawnLocation, transform.rotation);
        currentWildlife.Add(spawnAnimal);
        Instantiate(wildlifeData.spawnParticle, spawnLocation, transform.rotation);
        GameEvents.ReportOnWildlifeValueChanged(wildlifeCount);
    }
    
    
    //This checks how many trees are in the scene and adjusts the available wildlife accordingly.
    private void CheckForAvailableWildlife()
    {
        for (int i = 0; i < _DATA.wildlifeData.Count; i++)
        {
            if(!availableWildlife.Contains(_DATA.wildlifeData[i].id) && wildlifeCount >= _DATA.wildlifeData[i].avalaibleAt)
               availableWildlife.Add(_DATA.wildlifeData[i].id);
        }
    }
    
    private void OnWildlifeKilled(GameObject _wildlife)
    {
        currentWildlife.Remove(_wildlife);
        if (timeSinceWildlifeKilled > 30)
        {
            _UI.SetError(ErrorID.WildlifeUnderAttack);
            _UI.warningAudioSource.clip = _SM.warningSound;
            _UI.warningAudioSource.Play();
        }
        timeSinceWildlifeKilled = 0;
        GameEvents.ReportOnWildlifeValueChanged(wildlifeCount);
    }
    
    #endregion
    
    #region Pickups
    
    [Header("Pickups")]
    public float pickupPlacementRadius;
    public GameObject[] pickups;
    
    IEnumerator SpawnPickups()
    {
        if(_currentGameState == GameState.Play)
            SpawnPickup();

        yield return new WaitForSeconds(Random.Range(20, 50));
        StartCoroutine(SpawnPickups());
    }

    private void SpawnPickup()
    {
        _GLOSSARY.NewGlossaryAvailable(GlossaryID.Portal, "Portals");
        Vector3 randomLocation = transform.position + Random.insideUnitSphere * pickupPlacementRadius;
        NavMesh.SamplePosition(randomLocation, out NavMeshHit hit, pickupPlacementRadius, 1);
        GameObject pickup = Instantiate(ArrayX.GetRandomItemFromArray(pickups), hit.position, transform.rotation);
        Destroy(pickup, 45);
    }
    
    
    #endregion
    
    private void OnGameStateChanged(GameState _gameState)
    {
        if(_gameState != gameState)
            ChangeGameState(_gameState);
    }

    private void OnGameOver()
    {
        ChangeGameState(GameState.Lose);
    }

    private void OnGameSpeedButton(GameSpeed _gameSpeed)
    {
        if(_hasInput)
            SetGameSpeed(_gameSpeed);
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
        GameEvents.OnGameOver += OnGameOver;
        
        InputManager.OnGameSpeedButton += OnGameSpeedButton;
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
        GameEvents.OnGameOver -= OnGameOver;
        
        InputManager.OnGameSpeedButton -= OnGameSpeedButton;
    }
}
