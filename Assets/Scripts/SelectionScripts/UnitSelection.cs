using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : GameBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitSelected = new List<GameObject>();

    public static UnitSelection _instance;
    public static UnitSelection Instance { get { return _instance; } }

    public GameObject huldraToFind;
    public bool containsHuldra;
    public bool canDoubleClick = false;
    public float doubleClickTime = 0.3f;
    public GameObject[] destinations;

    public AudioSource audioSource;

    public List<GameObject> controlGroup1;
    public List<GameObject> controlGroup2;
    public List<GameObject> controlGroup3;
    public List<GameObject> controlGroup4;
    public List<GameObject> controlGroup5;
    public List<GameObject> controlGroup6;
    public List<GameObject> controlGroup7;
    public List<GameObject> controlGroup8;
    public List<GameObject> controlGroup9;
    public List<GameObject> controlGroup10;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Group1()
    {
        if (controlGroup1 != null)
        {
            RemoveGameObjectsWithScript(controlGroup1, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {

            controlGroup1.Add(go);
        }
    }
    public void Group2()
    {
        if (controlGroup2 != null)
        {
            RemoveGameObjectsWithScript(controlGroup2, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {
            controlGroup2.Add(go);
        }
    }
    public void Group3()
    {
        if (controlGroup3 != null)
        {
            RemoveGameObjectsWithScript(controlGroup3, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {
            controlGroup3.Add(go);
        }
    }
    public void Group4()
    {
        if (controlGroup4 != null)
        {
            RemoveGameObjectsWithScript(controlGroup4, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {
            controlGroup4.Add(go);
        }
    }
    public void Group5()
    {
        if (controlGroup5 != null)
        {
            RemoveGameObjectsWithScript(controlGroup5, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {
            controlGroup5.Add(go);
        }
    }
    public void Group6()
    {
        if (controlGroup6 != null)
        {
            RemoveGameObjectsWithScript(controlGroup6, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {
            controlGroup6.Add(go);
        }
    }
    public void Group7()
    {
        if (controlGroup7 != null)
        {
            RemoveGameObjectsWithScript(controlGroup7, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {
            controlGroup7.Add(go);
        }
    }
    public void Group8()
    {
        if (controlGroup8 != null)
        {
            RemoveGameObjectsWithScript(controlGroup8, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {
            controlGroup8.Add(go);
        }
    }
    public void Group9()
    {
        if (controlGroup9 != null)
        {
            RemoveGameObjectsWithScript(controlGroup9, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {
            controlGroup9.Add(go);
        }
    }
    public void Group10()
    {
        if (controlGroup10 != null)
        {
            RemoveGameObjectsWithScript(controlGroup10, typeof(Unit));
        }
        foreach (GameObject go in unitSelected)
        {
            controlGroup10.Add(go);
        }
    }
    public void GroupSelect1()
    {
        if(controlGroup1 != null)
        {
            DeselectAll();
            foreach (GameObject go in controlGroup1)
            {
                DragSelect(go);
            }
        }
    }
    public void GroupSelect2()
    {
        DeselectAll();
        if (controlGroup1 != null)
        {
            foreach (GameObject go in controlGroup2)
            {
                ShiftClickSelect(go);
            }
        }
    }
    public void GroupSelect3()
    {
        DeselectAll();
        if (controlGroup1 != null)
        {
            foreach (GameObject go in controlGroup3)
            {
                ShiftClickSelect(go);
            }
        }
    }
    public void GroupSelect4()
    {
        DeselectAll();
        if (controlGroup1 != null)
        {
            foreach (GameObject go in controlGroup4)
            {
                ShiftClickSelect(go);
            }
        }
    }
    public void GroupSelect5()
    {
        DeselectAll();
        if (controlGroup1 != null)
        {
            foreach (GameObject go in controlGroup5)
            {
                ShiftClickSelect(go);
            }
        }
    }
    public void GroupSelect6()
    {
        DeselectAll();
        if (controlGroup1 != null)
        {
            foreach (GameObject go in controlGroup6)
            {
                ShiftClickSelect(go);
            }
        }
    }
    public void GroupSelect7()
    {
        DeselectAll();
        if (controlGroup1 != null)
        {
            foreach (GameObject go in controlGroup7)
            {
                ShiftClickSelect(go);
            }
        }
    }
    public void GroupSelect8()
    {
        DeselectAll();
        if (controlGroup1 != null)
        {
            foreach (GameObject go in controlGroup8)
            {
                ShiftClickSelect(go);
            }
        }
    }
    public void GroupSelect9()
    {
        DeselectAll();
        if (controlGroup1 != null)
        {
            foreach (GameObject go in controlGroup9)
            {
                ShiftClickSelect(go);
            }
        }
    }
    public void GroupSelect10()
    {
        DeselectAll();
        if (controlGroup1 != null)
        {
            foreach (GameObject go in controlGroup10)
            {
                ShiftClickSelect(go);
            }
        }
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
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitID.Goblin)
                {
                    audioSource.clip = _SM.GetGoblinVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitID.Leshy)
                {
                    audioSource.clip = _SM.GetLeshyVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitID.Orcus)
                {
                    audioSource.clip = _SM.GetOrcusVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitID.Satyr)
                {
                    audioSource.clip = _SM.GetSatyrVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitID.Skessa)
                {
                    audioSource.clip = _SM.GetSkessaVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitID.Golem)
                {
                    audioSource.clip = _SM.GetGolemVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitID.Fidhain)
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
        CheckForHuldra();
    }
    private void CheckForHuldra()
    {
        foreach (GameObject unit in unitSelected)
        {
            if(unit.gameObject.GetComponent<IAmAHuldra>())
            {
                containsHuldra = true;
                break;
            }
            else
            {
                containsHuldra = false;
                break;
            }
        }
        if(containsHuldra)
        {
            Debug.Log("A Huldra is selected");
            _UI.EnableTowerText();
            //_GM.boundry.SetActive(true);
        }
        else
        {
            Debug.Log("No Huldra is selected");
        }

    }

    IEnumerator WaitToCheckForUnits()
    {
        yield return new WaitForEndOfFrame();
        //CheckForUnits();
    }
    private void CheckForUnits()
    {
        if(unitSelected.Count > 0)
        {
            _UI.combatOptionsPanel.SetActive(true);
        }
        else
        {
            _UI.combatOptionsPanel.SetActive(false);
        }
             
    }
    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        unitSelected.Add(unitToAdd);
        //unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        unitToAdd.GetComponent<Unit>().isSelected = true;
        unitToAdd.gameObject.GetComponent<Unit>().selectionCircle.SetActive(true);
        StartCoroutine(WaitToCheckHuldra());
        StartCoroutine(WaitToCheckForUnits());
    }
    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            //unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<Unit>().isSelected = true;
            unitToAdd.gameObject.GetComponent<Unit>().selectionCircle.SetActive(true);
            StartCoroutine(WaitToCheckHuldra());
            StartCoroutine(WaitToCheckForUnits());
        }
        else
        {
            //unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
            unitToAdd.GetComponent<Unit>().isSelected = false;
            unitSelected.Remove(unitToAdd);
            unitToAdd.gameObject.GetComponent<Unit>().selectionCircle.SetActive(true);
            StartCoroutine(WaitToCheckHuldra());
            StartCoroutine(WaitToCheckForUnits());
        }
    }
    public void DoubleClickSelect(GameObject unitToAdd)
    {
        unitSelected.Add(unitToAdd);
        unitToAdd.GetComponent<Unit>().isSelected = true;
        unitToAdd.gameObject.GetComponent<Unit>().selectionCircle.SetActive(true);
        StartCoroutine(WaitToCheckHuldra());
    }
    public void DragSelect(GameObject unitToAdd)
    {
        if (!unitSelected.Contains(unitToAdd))
        {
            unitSelected.Add(unitToAdd);
            //unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<Unit>().isSelected = true;
            unitToAdd.gameObject.GetComponent<Unit>().selectionCircle.SetActive(true);
            StartCoroutine(WaitToCheckHuldra());
            StartCoroutine(WaitToCheckForUnits());
        }
    }
    public void DeselectAll()
    {
        if(_UI.mouseOverCombatOptions == false)
        {
            foreach (var unit in unitSelected)
            {
                unit.GetComponent<Unit>().isSelected = false;
                unit.gameObject.GetComponent<Unit>().selectionCircle.SetActive(false);
                //unit.transform.GetChild(0).gameObject.SetActive(false);
                containsHuldra = false;
                _UI.DisableTowerText();
            }
            StartCoroutine(WaitToCheckForUnits());
            unitSelected.Clear();
        }

    }
    public void Deselect(GameObject unitToDeselect)
    {
        unitToDeselect.GetComponent<Unit>().isSelected = false;
        unitToDeselect.gameObject.GetComponent<Unit>().selectionCircle.SetActive(false);
        //unit.transform.GetChild(0).gameObject.SetActive(false);
        containsHuldra = false;
        _UI.DisableTowerText();
        unitSelected.Remove(unitToDeselect);
    }

    public void AssignDestination()
    {
        for (int i = 0; i < unitSelected.Count; i++)
        {
            unitSelected[i].GetComponent<Unit>().targetDest = destinations[i];
        }
    }



    // Function to remove GameObjects with a specific script attached from a given list
    void RemoveGameObjectsWithScript(List<GameObject> objectsList, System.Type scriptType)
    {
        // Use RemoveAll with a predicate to check for the script's presence
        objectsList.RemoveAll(obj => obj != null && obj.GetComponent(scriptType) != null);
    }
}
