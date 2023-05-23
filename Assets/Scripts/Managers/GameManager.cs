using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : Singleton<GameManager>
{
    public float gameTime = 0;
    
    public PlayMode playmode;
    public GameState gameState;
    public GameObject[] trees;
    public bool isPaused;
    public int startingMaegen;

    [Header("Waves")]
    public int currentWave;
    public int agroWaveLength;
    public int breakWaveLenth;
    public bool agroWave;


    [Header("Player Resources")]
    public int maegen;
    public int maxTrees;
    public int wildlife;
    public int populous;
    public int maxPopulous = 10;

    [Header("Runes")]
    public GameObject[] runes;

    [Header("Beacons")]
    public GameObject[] beacons;

    [Header("Unit Damage")]
    public float satyrDamage;
    public float orcusDamage;
    public float leshyDamage;
    public float skessaDamage;

    [Header("Horgr")]
    public bool horgrClaimedByPlayer;

    [Header("Alert")]
    public float timeSinceAttack = 0f;
    public float timeSinceWildlifeKilled = 0f;
    public GameObject warningSprite;

    void Start()
    {
        Time.timeScale = 1;
        maegen = startingMaegen;
        currentWave = 0;
        playmode = PlayMode.DefaultMode;
        gameState = GameState.Play;
        trees = GameObject.FindGameObjectsWithTag("Tree");
        StartCoroutine(ManageWaveAgro());
        _UI.CheckTreeUI();
        satyrDamage = 15;
        orcusDamage = 25;
        leshyDamage = 60;
        skessaDamage = 40;
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        timeSinceWildlifeKilled += Time.deltaTime;
    }

    //The wave system is managed by two coroutines. The 'agro' wave lasts 1 minute, during which enemies are spawned in (EnemyManager) and then a break period of 4 mins. 
    IEnumerator ManageWaveAgro()
    {
        agroWave = true;
        currentWave++;
        _UI.CheckWave();
        yield return new WaitForSeconds(agroWaveLength);
        StartCoroutine(ManageWaveBreak());
    }
    IEnumerator ManageWaveBreak()
    {
        agroWave = false;
        yield return new WaitForSeconds(breakWaveLenth);
        StartCoroutine(ManageWaveAgro());
    }

    //Checks the scene for how many Runes are present.
    public void CheckRunes()
    {
        runes = GameObject.FindGameObjectsWithTag("RuneObject");
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
        trees = GameObject.FindGameObjectsWithTag("Tree");
        maegen -= 16;
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
        trees = GameObject.FindGameObjectsWithTag("Tree");
        _UI.CheckTreeUI();
    }

    //Once the 'Jarnnefi' upgrade has been purchased, the damage values for the player units are increased by 30%.
    public void OnJarnnefiUpgrade()
    {
        satyrDamage += satyrDamage * 0.3f;
        orcusDamage += orcusDamage * 0.3f;
        leshyDamage += leshyDamage * 0.3f;
        skessaDamage += skessaDamage * 0.3f;
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
        maxPopulous += 10;
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
    private void OnEnable()
    {
        GameEvents.OnTreePlaced += OnTreePlaced;
        GameEvents.OnTreeDestroyed += OnTreeDestroy;
        GameEvents.OnJarnnefiUpgrade += OnJarnnefiUpgrade;
        GameEvents.OnRuneDestroyed += OnRuneDestroyed;
        GameEvents.OnBeaconDestroyed += OnBeaconDestroyed;
        GameEvents.OnPopulousUpgrade += OnPopulousUpgrade;
        GameEvents.OnTreeHit += OnTreeHit;
        GameEvents.OnWildlifeKilled += OnWildlifeKilled;
    }

    private void OnDisable()
    {
        GameEvents.OnTreePlaced -= OnTreePlaced;
        GameEvents.OnTreeDestroyed -= OnTreeDestroy;
        GameEvents.OnJarnnefiUpgrade -= OnJarnnefiUpgrade;
        GameEvents.OnRuneDestroyed -= OnRuneDestroyed;
        GameEvents.OnBeaconDestroyed -= OnBeaconDestroyed;
        GameEvents.OnPopulousUpgrade -= OnPopulousUpgrade;
        GameEvents.OnTreeHit -= OnTreeHit;
        GameEvents.OnWildlifeKilled -= OnWildlifeKilled;
    }
}
