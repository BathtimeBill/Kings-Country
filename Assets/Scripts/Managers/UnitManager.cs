using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    public List<Unit> unitList = new List<Unit>();
    private List<Ragdoll> ragdollList = new List<Ragdoll>();
    public List<Unit> unitSelected = new List<Unit>();
    public bool canDoubleClick = false;
    public float doubleClickTime = 0.3f;
    public GameObject[] destinations;
    public AudioSource audioSource;
    [HideInInspector] public List<Unit> controlGroup01;
    [HideInInspector] public List<Unit> controlGroup02;
    [HideInInspector] public List<Unit> controlGroup03;
    [HideInInspector] public List<Unit> controlGroup04;
    [HideInInspector] public List<Unit> controlGroup05;
    [HideInInspector] public List<Unit> controlGroup06;
    [HideInInspector] public List<Unit> controlGroup07;
    [HideInInspector] public List<Unit> controlGroup08;
    [HideInInspector] public List<Unit> controlGroup09;
    [HideInInspector] public List<Unit> controlGroup10;
    [HideInInspector] public GameObject selectedUnit;
    
    private List<GameObject> guardianPool = new List<GameObject>();
    private List<GameObject> spawnParticlePool = new List<GameObject>();
    private List<GameObject> ragdollPool = new List<GameObject>();

    public void SpawnGuardian(UnitData _unitData, Transform _location)
    {
        if (_GM.maegen < _unitData.cost)
        {
            _UI.SetError(ErrorID.InsufficientMaegen);
            return;
        }

        if (_GM.populous >= _GM.maxPopulous)
        {
            _UI.SetError(ErrorID.MaxPopulation);
            return;
        }

        UnitData guardian = _unitData;
        //GameObject go = PoolX.GetFromPool(_DATA.GetUnit(guardian.id).playModel, guardianPool);
        GameObject go = Instantiate(_DATA.GetUnit(guardian.id).playModel, _location.position, _location.rotation);
        GameObject particles = PoolX.GetFromPool(_DATA.GetUnit(guardian.id).spawnParticles, spawnParticlePool);
        go.transform.position = particles.transform.position = _location.transform.position;
        go.transform.rotation = particles.transform.rotation = transform.rotation;
        
        _GM.DecreaseMaegen(_unitData.cost);
        _UI.CheckPopulousUI();
    }

    public void RemoveGuardian(Unit _unit, Vector3 _position, Quaternion _rotation)
    {
        Deselect(_unit);
        unitList.Remove(_unit);
        RemoveSelectedUnit(_unit);
        
        //CHECK - May need to reset ragdoll physics when getting object
        //GameObject go = PoolX.GetFromPool(_unit.unitData.ragdollModel, ragdollPool);
        GameObject go = Instantiate(_unit.unitData.ragdollModel, _position, _rotation);
        ragdollList.Add(go.GetComponent<Ragdoll>());
        //go.transform.position = _position;
        //go.transform.rotation = _rotation;
        
        Destroy(_unit.gameObject);
        //_unit.gameObject.SetActive(false);
    }
    
    public void GroupUnits(int _group)
    {
        GetGroup(_group).Clear();
        //if (_units != null)
        //{
        //    //ListX.RemoveGameObjectsWithScript(_units, typeof(Unit));
        //}
        foreach (Unit unit in unitSelected)
        {
            unit.healthBar.ChangeGroupNumber(_group.ToString());
            GetGroup(_group).Add(unit);
        }
    }

    public void SelectGroup(int _group)
    {
        if (GetGroup(_group) == null)
            return;
        
        DeselectAll();
        foreach (Unit go in GetGroup(_group))
            DragSelect(go);
        selectedUnit = GetGroup(_group)[0].gameObject;
    }

    public void RemoveSelectedUnit(Unit _unit)
    {
        if (controlGroup01.Contains(_unit)) controlGroup01.Remove(_unit);
        if (controlGroup02.Contains(_unit)) controlGroup02.Remove(_unit);
        if (controlGroup03.Contains(_unit)) controlGroup03.Remove(_unit);
        if (controlGroup04.Contains(_unit)) controlGroup04.Remove(_unit);
        if (controlGroup05.Contains(_unit)) controlGroup05.Remove(_unit);
        if (controlGroup06.Contains(_unit)) controlGroup06.Remove(_unit);
        if (controlGroup07.Contains(_unit)) controlGroup07.Remove(_unit);
        if (controlGroup08.Contains(_unit)) controlGroup08.Remove(_unit);
        if (controlGroup09.Contains(_unit)) controlGroup09.Remove(_unit);
        if (controlGroup10.Contains(_unit)) controlGroup10.Remove(_unit);
    }
    private List<Unit> GetGroup(int _group)
    {
        List<Unit> _units = controlGroup01;
        if (_group == 1) _units = controlGroup01;
        if (_group == 2) _units = controlGroup02;
        if (_group == 3) _units = controlGroup03;
        if (_group == 4) _units = controlGroup04;
        if (_group == 5) _units = controlGroup05;
        if (_group == 6) _units = controlGroup06;
        if (_group == 7) _units = controlGroup07;
        if (_group == 8) _units = controlGroup08;
        if (_group == 9) _units = controlGroup09;
        if (_group == 10) _units = controlGroup10;
        return _units;
    }

    public IEnumerator DoubleClick()
    {
        canDoubleClick = true;
        yield return new WaitForSeconds(doubleClickTime);
        canDoubleClick = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {
            DeselectAll();
        }
    }

    IEnumerator WaitToCheckIsTower()
    {
        yield return new WaitForEndOfFrame();
        foreach (Unit unit in unitSelected)
        {
            if (_DATA.IsTowerUnit(unit.unitID))
                _UI.EnableTowerText();
        }
    }

    public void ClickSelect(Unit unitToAdd)
    {
        DeselectAll();
        unitSelected.Add(unitToAdd);
        unitToAdd.isSelected = true;
        unitToAdd.selectionRing.Select(true);
        GameEvents.ReportOnObjectSelected(unitToAdd.gameObject);
        StartCoroutine(WaitToCheckIsTower());
    }

    public void ShiftClickSelect(Unit unitToAdd)
    {
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            unitToAdd.isSelected = true;
            unitToAdd.selectionRing.Select(true);
            StartCoroutine(WaitToCheckIsTower());
        }
        else
        {
            unitToAdd.isSelected = false;
            unitSelected.Remove(unitToAdd);
            unitToAdd.selectionRing.Select(true);
            StartCoroutine(WaitToCheckIsTower());
        }
    }

    public void DoubleClickSelect(Unit unitToAdd)
    {
        unitSelected.Add(unitToAdd);
        unitToAdd.isSelected = true;
        unitToAdd.selectionRing.Select(true);
        StartCoroutine(WaitToCheckIsTower());
    }

    public void DragSelect(Unit unitToAdd)
    {
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            unitToAdd.isSelected = true;
            unitToAdd.selectionRing.Select(true);
            StartCoroutine(WaitToCheckIsTower());
        }
    }

    public void DeselectAll()
    {
        if(_UI.mouseOverCombatOptions == false)
        {
            foreach (var unit in unitSelected)
            {
                unit.isSelected = false;
                unit.selectionRing.Select(false);
                _UI.DisableTowerText();
            }
            unitSelected.Clear();
        }
        GameEvents.ReportOnObjectSelected(null);
    }

    public void Deselect(Unit unitToDeselect)
    {
        unitToDeselect.isSelected = false;
        unitToDeselect.selectionRing.Select(false);
        _UI.DisableTowerText();
        unitSelected.Remove(unitToDeselect);
        GameEvents.ReportOnObjectSelected(null);
    }

    public void AssignDestination()
    {
        for (int i = 0; i < unitSelected.Count; i++)
        {
            unitSelected[i].SetDestination(destinations[i].transform);
        }
    }

    private void OnObjectSelected(GameObject _gameObject)
    {
        selectedUnit = _gameObject;
    }
    
    private void OnUnitFocus()
    {
        if (selectedUnit == null)
            return;
        
        _CAMERA.TweenCameraPosition(selectedUnit.transform.position, _TWEENING.focusTweenTime);
    }
    
    private void OnDeselectButtonPressed()
    {
        if(unitSelected.Count != 0)
        {
            AssignDestination();
            if (unitSelected[0].unitID == CreatureID.Goblin)
            {
                audioSource.clip = _SM.GetGoblinVocal();
                audioSource.Play();
            }
            if (unitSelected[0].unitID == CreatureID.Leshy)
            {
                audioSource.clip = _SM.GetLeshyVocal();
                audioSource.Play();
            }
            if (unitSelected[0].unitID == CreatureID.Orcus)
            {
                audioSource.clip = _SM.GetOrcusVocal();
                audioSource.Play();
            }
            if (unitSelected[0].unitID == CreatureID.Satyr)
            {
                audioSource.clip = _SM.GetSatyrVocal();
                audioSource.Play();
            }
            if (unitSelected[0].unitID == CreatureID.Skessa)
            {
                audioSource.clip = _SM.GetSkessaVocal();
                audioSource.Play();
            }
            if (unitSelected[0].unitID == CreatureID.Mistcalf)
            {
                audioSource.clip = _SM.GetGolemVocal();
                audioSource.Play();
            }
            if (unitSelected[0].unitID == CreatureID.Fidhain)
            {
                audioSource.clip = _SM.GetFidhainVocal();
                audioSource.Play();
            }
        }
    }
    
    private void OnDayOver(int obj)
    {
        for(int i=0; i<ragdollList.Count; i++)
            Destroy(ragdollList[i].gameObject);
        ragdollList.Clear();
    }
    
    private void OnEnable()
    {
        GameEvents.OnObjectSelected += OnObjectSelected;
        GameEvents.OnDayOver += OnDayOver;
        InputManager.OnUnitFocus += OnUnitFocus;
        InputManager.OnDeselectButtonPressed += OnDeselectButtonPressed;
    }
    
    private void OnDisable()
    {
        GameEvents.OnObjectSelected -= OnObjectSelected;
        GameEvents.OnDayOver -= OnDayOver;
        InputManager.OnUnitFocus -= OnUnitFocus;
        InputManager.OnDeselectButtonPressed -= OnDeselectButtonPressed;
    }
}
