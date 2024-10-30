using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    public List<Unit> unitList = new List<Unit>();
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

    public void GroupUnits(int _group)
    {
        GetGroup(_group).Clear();
        //if (_units != null)
        //{
        //    //ListX.RemoveGameObjectsWithScript(_units, typeof(Unit));
        //}
        foreach (Unit unit in unitSelected)
        {
            unit.ChangeGroupNumber(_group.ToString());
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

    public List<Unit> GetGroup(int _group)
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
        unitToAdd.selectionCircle.SetActive(true);
        GameEvents.ReportOnObjectSelected(unitToAdd.gameObject);
        StartCoroutine(WaitToCheckIsTower());
    }

    public void ShiftClickSelect(Unit unitToAdd)
    {
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            unitToAdd.isSelected = true;
            unitToAdd.selectionCircle.SetActive(true);
            StartCoroutine(WaitToCheckIsTower());
        }
        else
        {
            unitToAdd.isSelected = false;
            unitSelected.Remove(unitToAdd);
            unitToAdd.selectionCircle.SetActive(true);
            StartCoroutine(WaitToCheckIsTower());
        }
    }

    public void DoubleClickSelect(Unit unitToAdd)
    {
        unitSelected.Add(unitToAdd);
        unitToAdd.isSelected = true;
        unitToAdd.selectionCircle.SetActive(true);
        StartCoroutine(WaitToCheckIsTower());
    }

    public void DragSelect(Unit unitToAdd)
    {
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            unitToAdd.isSelected = true;
            unitToAdd.selectionCircle.SetActive(true);
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
                unit.selectionCircle.SetActive(false);
                _UI.DisableTowerText();
            }
            unitSelected.Clear();
        }
        GameEvents.ReportOnObjectSelected(null);
    }

    public void Deselect(Unit unitToDeselect)
    {
        unitToDeselect.isSelected = false;
        unitToDeselect.selectionCircle.SetActive(false);
        _UI.DisableTowerText();
        unitSelected.Remove(unitToDeselect);
        GameEvents.ReportOnObjectSelected(null);
    }

    public void AssignDestination()
    {
        for (int i = 0; i < unitSelected.Count; i++)
        {
            unitSelected[i].targetDest = destinations[i];
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
        
        _CAMERA.TweenCameraPosition(selectedUnit.transform, 0.5f);
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
    
    private void OnEnable()
    {
        GameEvents.OnObjectSelected += OnObjectSelected;
        InputManager.OnUnitFocus += OnUnitFocus;
        InputManager.OnDeselectButtonPressed += OnDeselectButtonPressed;
        
    }
    private void OnDisable()
    {
        GameEvents.OnObjectSelected -= OnObjectSelected;
        InputManager.OnUnitFocus -= OnUnitFocus;
        InputManager.OnDeselectButtonPressed -= OnDeselectButtonPressed;
    }

    
}
