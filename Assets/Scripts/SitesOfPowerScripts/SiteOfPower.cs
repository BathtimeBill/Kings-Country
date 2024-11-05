using UnityEngine;
using System.Collections.Generic;

public class SiteOfPower : GameBehaviour
{
    public SiteData siteData;
    public GameObject spawnParticle;
    public GameObject spawnLocation;
    public HealthBar healthBar;
    public SelectionRing selectionRing;
    
    public List<Enemy> enemies;
    public List<Unit> units;
    public bool playerOwns;

    public BV.Range spawnRates;
    
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
    
    private void SpawnUnit(UnitData _unitData)
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
            SpawnUnit(_unitData);
    }
    private void OnSiteSelected(SiteID _ID, bool _selected)
    {
        selectionRing.Select(_ID == siteData.id && _selected);
    }
    
    private void OnHumanKilled(Enemy _enemy, string _killer)
    {
        RemoveEnemy(_enemy);
    }

    private void OnGameStateChanged(GameState _gameState) => healthBar.gameObject.SetActive(_inGame);

    protected virtual void OnEnable()
    {
        GameEvents.OnUnitButtonPressed += OnUnitButtonPressed;
        GameEvents.OnGameStateChanged += OnGameStateChanged;
        GameEvents.OnSiteSelected += OnSiteSelected;
        GameEvents.OnHumanKilled += OnHumanKilled;
    }
    
    protected virtual void OnDisable()
    {
        GameEvents.OnUnitButtonPressed -= OnUnitButtonPressed;
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
        GameEvents.OnSiteSelected -= OnSiteSelected;
        GameEvents.OnHumanKilled -= OnHumanKilled;
    }
}
