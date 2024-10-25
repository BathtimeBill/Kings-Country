using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : Singleton<UnitManager>
{
    public List<Unit> unitList = new List<Unit>();
    public List<Unit> unitSelected = new List<Unit>();

    public GameObject huldraToFind;
    public bool containsHuldra;
    public bool canDoubleClick = false;
    public float doubleClickTime = 0.3f;
    public GameObject[] destinations;

    public AudioSource audioSource;

    public List<Unit> controlGroup01;
    public List<Unit> controlGroup02;
    public List<Unit> controlGroup03;
    public List<Unit> controlGroup04;
    public List<Unit> controlGroup05;
    public List<Unit> controlGroup06;
    public List<Unit> controlGroup07;
    public List<Unit> controlGroup08;
    public List<Unit> controlGroup09;
    public List<Unit> controlGroup10;

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
        if (GetGroup(_group) != null)
        {
            DeselectAll();
            foreach (Unit go in GetGroup(_group))
            {
                DragSelect(go);
            }
        }
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
        if(Input.GetMouseButtonDown(1))
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
            else
            {
                return;
            }
        }
    }

    IEnumerator WaitToCheckHuldra()
    {
        yield return new WaitForEndOfFrame();
        foreach (Unit unit in unitSelected)
        {
            if (unit.gameObject.GetComponent<IAmAHuldra>())
            {
                _UI.EnableTowerText();
            }
        }
    }

    public void ClickSelect(Unit unitToAdd)
    {
        DeselectAll();
        unitSelected.Add(unitToAdd);
        unitToAdd.isSelected = true;
        unitToAdd.selectionCircle.SetActive(true);
        StartCoroutine(WaitToCheckHuldra());
    }

    public void ShiftClickSelect(Unit unitToAdd)
    {
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            unitToAdd.isSelected = true;
            unitToAdd.selectionCircle.SetActive(true);
            StartCoroutine(WaitToCheckHuldra());
        }
        else
        {
            unitToAdd.isSelected = false;
            unitSelected.Remove(unitToAdd);
            unitToAdd.selectionCircle.SetActive(true);
            StartCoroutine(WaitToCheckHuldra());
        }
    }

    public void DoubleClickSelect(Unit unitToAdd)
    {
        unitSelected.Add(unitToAdd);
        unitToAdd.isSelected = true;
        unitToAdd.selectionCircle.SetActive(true);
        StartCoroutine(WaitToCheckHuldra());
    }

    public void DragSelect(Unit unitToAdd)
    {
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            unitToAdd.isSelected = true;
            unitToAdd.selectionCircle.SetActive(true);
            StartCoroutine(WaitToCheckHuldra());
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

    }

    public void Deselect(Unit unitToDeselect)
    {
        unitToDeselect.isSelected = false;
        unitToDeselect.selectionCircle.SetActive(false);
        _UI.DisableTowerText();
        unitSelected.Remove(unitToDeselect);
    }

    public void AssignDestination()
    {
        for (int i = 0; i < unitSelected.Count; i++)
        {
            unitSelected[i].targetDest = destinations[i];
        }
    }
}
