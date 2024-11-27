using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SiteOfPower : GameBehaviour
{
    public SiteData siteData;
    public GameObject spawnLocation;
    public SiteHealthBar healthBar;
    public SelectionRing selectionRing;
    public MapIcon mapIcon;
    
    public List<Enemy> enemies;
    public List<Unit> units;
    public GameObject playerControlFX;
    public GameObject enemyControlFX;

    public BV.Range spawnRates;
    [FormerlySerializedAs("enemyTimeLeft")] public float currentClaimTime;
    public SiteState siteState;
    [FormerlySerializedAs("claimRate")] public float currentClaimRate;
    private float spawnDelay = 1f;

    public void Start()
    {
        currentClaimRate = siteData.claimRate;
        ChangeSiteState(SiteState.Claimed);
        healthBar.AdjustHealthBar(currentClaimTime, siteData.claimTime, GetCaptureColor(siteState));
    }

    public void OnTriggerStay(Collider other)
    {
        if (siteData.id == SiteID.HomeTree)
            return;
        
        bool isSpecialEnemy = other.GetComponent<Enemy>() && siteData.siteEnemies.Contains(other.GetComponent<Enemy>().unitID);
        bool isUnit = other.GetComponent<Unit>() && _DATA.IsCreatureUnit(other.GetComponent<Unit>().unitID.ToString());

        if (isSpecialEnemy && siteState != SiteState.Captured)
        {
            if(enemies.Count > units.Count)
                currentClaimTime -= currentClaimRate * Time.deltaTime;
        }

        if (isUnit && siteState != SiteState.Claimed)
        {
            if(units.Count > enemies.Count)
                currentClaimTime += currentClaimRate * Time.deltaTime;
        }

        if (currentClaimTime <= 0)
            ChangeSiteState(SiteState.Captured);
        else if (currentClaimTime >= siteData.claimTime)
            ChangeSiteState(SiteState.Claimed);
        else
            ChangeSiteState(SiteState.Neutral);
        
        healthBar.AdjustHealthBar(currentClaimTime, siteData.claimTime, GetCaptureColor(siteState));
    }

    private void ChangeSiteState(SiteState _state)
    {
        siteState = _state;
        if (siteData.id == SiteID.HomeTree)
            return;
        
        switch (_state)
        {
            case SiteState.Claimed:
                currentClaimTime = siteData.claimTime;
                playerControlFX.SetActive(true);
                enemyControlFX.SetActive(false);
                break;
            case SiteState.Neutral:
                playerControlFX.SetActive(false);
                enemyControlFX.SetActive(false);
                break;
            case SiteState.Captured:
                currentClaimTime = 0;
                playerControlFX.SetActive(false);
                enemyControlFX.SetActive(true);
                spawnDelay -= Time.deltaTime;
                if (spawnDelay <= 0)
                {
                    spawnDelay = Random.Range(spawnRates.min, spawnRates.max);
                    _EM.SpawnHutEnemy(spawnLocation.transform.position);
                }
                break;
        }
        mapIcon.ChangeMapIconColor(GetCaptureColor(_state));
    }

    public Color GetCaptureColor(SiteState _state)
    {
        if (_state == SiteState.Claimed) return Color.green;
        else if (_state == SiteState.Captured) return Color.red;
        else return Color.white;
    }
    
    public void AddUnit(Unit _unit) { if (!units.Contains(_unit)) units.Add(_unit); }
    public void RemoveUnit(Unit _unit) { if (units.Contains(_unit)) units.Remove(_unit); }
    public bool ContainsUnit(Unit _unit) => units.Contains(_unit);
    public bool HasUnits() => units.Count > 0;
    public void AddEnemy(Enemy _enemy) { if (!enemies.Contains(_enemy)) enemies.Add(_enemy); }
    public void RemoveEnemy(Enemy _enemy) { if (enemies.Contains(_enemy)) enemies.Remove(_enemy); }
    public bool ContainsEnemy(Enemy _enemy) => enemies.Contains(_enemy);
    public bool HasEnemies() => enemies.Count > 0;


    private void OnUnitButtonPressed(UnitData _unitData)
    {
        if(siteData.siteGuardians.Contains(_unitData.id)) 
            _UM.SpawnGuardian(_unitData, spawnLocation.transform);
    }
    private void OnSiteSelected(SiteID _ID, bool _selected) => selectionRing.Select(_ID == siteData.id && _selected);
    private void OnHumanKilled(Enemy _enemy, string _killer) => RemoveEnemy(_enemy);
    private void OnGameStateChanged(GameState _gameState) => healthBar.gameObject.SetActive(_inGame);
    private void OnContinueButton() => ChangeSiteState(SiteState.Claimed);

    protected virtual void OnEnable()
    {
        GameEvents.OnUnitButtonPressed += OnUnitButtonPressed;
        GameEvents.OnGameStateChanged += OnGameStateChanged;
        GameEvents.OnSiteSelected += OnSiteSelected;
        GameEvents.OnHumanKilled += OnHumanKilled;
        GameEvents.OnContinueButton += OnContinueButton;
    }
    
    protected virtual void OnDisable()
    {
        GameEvents.OnUnitButtonPressed -= OnUnitButtonPressed;
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
        GameEvents.OnSiteSelected -= OnSiteSelected;
        GameEvents.OnHumanKilled -= OnHumanKilled;
        GameEvents.OnContinueButton -= OnContinueButton;
    }
}
