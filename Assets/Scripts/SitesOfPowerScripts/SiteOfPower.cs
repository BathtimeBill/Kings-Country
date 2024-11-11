using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SiteOfPower : GameBehaviour
{
    public SiteData siteData;
    public GameObject spawnParticle;
    public GameObject spawnLocation;
    public SiteHealthBar healthBar;
    public SelectionRing selectionRing;
    public MapIcon mapIcon;
    
    public List<Enemy> enemies;
    public List<Unit> units;
    public bool playerOwns;
    public GameObject playerControlFX;
    public GameObject enemyControlFX;

    public BV.Range spawnRates;
    [FormerlySerializedAs("enemyTimeLeft")] public float currentClaimTime;
    public float enemyMaxTimeLeft;
    public SiteState siteState;
    public float claimRate;

    public void Start()
    {
        ChangeSiteState(SiteState.Claimed);
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
                currentClaimTime -= claimRate * Time.deltaTime;
        }

        if (isUnit && siteState != SiteState.Claimed)
        {
            if(units.Count > enemies.Count)
                currentClaimTime += claimRate * Time.deltaTime;
        }

        if (currentClaimTime <= 0)
        {
            currentClaimTime = 0;
            ChangeSiteState(SiteState.Captured);
        }
        else if (currentClaimTime >= siteData.claimTime)
        {
            currentClaimTime = siteData.claimTime;
            ChangeSiteState(SiteState.Claimed);
        }
        else
        {
            ChangeSiteState(SiteState.Neutral);
        }
    }

    private void ChangeSiteState(SiteState _state)
    {
        siteState = _state;
        if (siteData.id == SiteID.HomeTree)
            return;
        
        switch (_state)
        {
            case SiteState.Claimed:
                currentClaimTime = enemyMaxTimeLeft;
                mapIcon.ChangeMapIconColor(Color.green);
                playerControlFX.SetActive(true);
                enemyControlFX.SetActive(false);
                StopCoroutine(SpawnHumanUnits());
                break;
            case SiteState.Neutral:
                currentClaimTime = enemyMaxTimeLeft;
                mapIcon.ChangeMapIconColor(Color.white);
                playerControlFX.SetActive(false);
                enemyControlFX.SetActive(false);
                StopCoroutine(SpawnHumanUnits());
                break;
            case SiteState.Captured:
                currentClaimTime = enemyMaxTimeLeft;
                mapIcon.ChangeMapIconColor(Color.red);
                playerControlFX.SetActive(false);
                enemyControlFX.SetActive(true);
                StartCoroutine(SpawnHumanUnits());
                break;
        }
    }
    
    private IEnumerator SpawnHumanUnits()
    {
        yield return new WaitForSeconds(Random.Range(spawnRates.min, spawnRates.max));
        _EM.SpawnHutEnemy(spawnLocation.transform.position);
        StartCoroutine(SpawnHumanUnits());
    }
    
    public void AddUnit(Unit _unit)
    {
        if (!units.Contains(_unit)) units.Add(_unit);
    }
    public void RemoveUnit(Unit _unit)
    {
        if (units.Contains(_unit)) units.Remove(_unit);
    }
    
    public bool ContainsUnit(Unit _unit) => units.Contains(_unit);
    public bool HasUnits() => units.Count > 0;
    
    public void AddEnemy(Enemy _enemy)
    {
        if (!enemies.Contains(_enemy)) enemies.Add(_enemy);
    }
    
    public void RemoveEnemy(Enemy _enemy)
    {
        if (enemies.Contains(_enemy)) enemies.Remove(_enemy);
    }
    public bool ContainsEnemy(Enemy _enemy) => enemies.Contains(_enemy);
    public bool HasEnemies() => enemies.Count > 0;
    
    private void SpawnGuardian(UnitData _unitData)
    {
        int cost = _unitData.cost;
        if (_GM.maegen < cost)
        {
            _UI.SetError(ErrorID.InsufficientMaegen);
            return;
        }

        if(_GM.populous < _GM.maxPopulous)
        {
            _GM.DecreaseMaegen(cost);
            Instantiate(_unitData.playModel, spawnLocation.transform.position, spawnLocation.transform.rotation);
            Instantiate(spawnParticle, spawnLocation.transform.position, Quaternion.Euler(-90, 0, 0));
            _UI.CheckPopulousUI();
        }
        else
        {
            _UI.SetError(ErrorID.MaxPopulation);
        }
    }
    
    private void OnUnitButtonPressed(UnitData _unitData)
    {
        if(siteData.siteGuardians.Contains(_unitData.id))
            SpawnGuardian(_unitData);
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
