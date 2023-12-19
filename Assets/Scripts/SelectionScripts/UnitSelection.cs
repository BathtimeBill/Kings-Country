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

    public void SelectAllOfSameType()
    {
        foreach (GameObject go in unitList) 
        {
            
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
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitType.GoblinUnit)
                {
                    audioSource.clip = _SM.GetGoblinVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitType.LeshyUnit)
                {
                    audioSource.clip = _SM.GetLeshyVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitType.OrcusUnit)
                {
                    audioSource.clip = _SM.GetOrcusVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitType.SatyrUnit)
                {
                    audioSource.clip = _SM.GetSatyrVocal();
                    audioSource.Play();
                }
                if (unitSelected[0].GetComponent<Unit>().unitType == UnitType.VolvaUnit)
                {
                    audioSource.clip = _SM.GetSkessaVocal();
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
            _GM.boundry.SetActive(true);
        }
        else
        {
            Debug.Log("No Huldra is selected");
        }

    }

    IEnumerator WaitToCheckForUnits()
    {
        yield return new WaitForEndOfFrame();
        CheckForUnits();
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
                if (_GM.downTime == false)
                {
                    _GM.boundry.SetActive(false);
                }
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
        if(_GM.downTime == false)
        {
            _GM.boundry.SetActive(false);
        }
        unitSelected.Remove(unitToDeselect);
    }

    public void AssignDestination()
    {
        for (int i = 0; i < unitSelected.Count; i++)
        {
            unitSelected[i].GetComponent<Unit>().targetDest = destinations[i];
        }
    }
}
