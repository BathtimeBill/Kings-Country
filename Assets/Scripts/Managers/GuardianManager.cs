using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianManager : Singleton<GuardianManager>
{
    public List<Guardian> guardianList = new List<Guardian>();
    private List<Ragdoll> ragdollList = new List<Ragdoll>();
    public List<Guardian> guardianSelected = new List<Guardian>();
    public bool canDoubleClick = false;
    public float doubleClickTime = 0.3f;
    public GameObject[] destinations;
    public AudioSource audioSource;
    [HideInInspector] public List<Guardian> controlGroup01;
    [HideInInspector] public List<Guardian> controlGroup02;
    [HideInInspector] public List<Guardian> controlGroup03;
    [HideInInspector] public List<Guardian> controlGroup04;
    [HideInInspector] public List<Guardian> controlGroup05;
    [HideInInspector] public List<Guardian> controlGroup06;
    [HideInInspector] public List<Guardian> controlGroup07;
    [HideInInspector] public List<Guardian> controlGroup08;
    [HideInInspector] public List<Guardian> controlGroup09;
    [HideInInspector] public List<Guardian> controlGroup10;
    [HideInInspector] public GameObject selectedUnit;
    
    private List<GameObject> guardianPool = new List<GameObject>();
    private List<GameObject> spawnParticlePool = new List<GameObject>();
    private List<GameObject> ragdollPool = new List<GameObject>();

    public void SpawnGuardian(GuardianData _guardianData, Transform _location)
    {
        if (_GAME.maegen < _guardianData.cost)
        {
            _UI.SetError(ErrorID.InsufficientMaegen);
            return;
        }

        if (_GAME.populous >= _GAME.maxPopulous)
        {
            _UI.SetError(ErrorID.MaxPopulation);
            return;
        }

        GuardianData guardian = _guardianData;
        //GameObject go = PoolX.GetFromPool(_DATA.GetUnit(guardian.id).playModel, guardianPool);
        GameObject go = Instantiate(_DATA.GetUnit(guardian.id).playModel, _location.position, _location.rotation);
        GameObject particles = PoolX.GetFromPool(_DATA.GetUnit(guardian.id).spawnParticles, spawnParticlePool);
        go.transform.position = particles.transform.position = _location.transform.position;
        go.transform.rotation = particles.transform.rotation = transform.rotation;
        
        _GAME.DecreaseMaegen(_guardianData.cost);
        _UI.CheckPopulousUI();
    }

    public void RemoveGuardian(Guardian guardian, Vector3 _position, Quaternion _rotation)
    {
        Deselect(guardian);
        guardianList.Remove(guardian);
        RemoveSelectedUnit(guardian);
        
        //CHECK - May need to reset ragdoll physics when getting object
        //GameObject go = PoolX.GetFromPool(_unit.unitData.ragdollModel, ragdollPool);
        if (NotNull(guardian.guardianData.ragdollModel))
        {
            GameObject go = Instantiate(guardian.guardianData.ragdollModel, _position, _rotation);
            Destroy(go, 15);
            //ragdollList.Add(go.GetComponent<Ragdoll>());
        }
        //go.transform.position = _position;
        //go.transform.rotation = _rotation;
        
        Destroy(guardian.gameObject);
        //_unit.gameObject.SetActive(false);
    }
    
    public void GroupUnits(int _group)
    {
        GetGroup(_group).Clear();
        //if (_units != null)
        //{
        //    //ListX.RemoveGameObjectsWithScript(_units, typeof(Unit));
        //}
        foreach (Guardian unit in guardianSelected)
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
        foreach (Guardian go in GetGroup(_group))
            DragSelect(go);
        selectedUnit = GetGroup(_group)[0].gameObject;
    }

    public void RemoveSelectedUnit(Guardian guardian)
    {
        if (controlGroup01.Contains(guardian)) controlGroup01.Remove(guardian);
        if (controlGroup02.Contains(guardian)) controlGroup02.Remove(guardian);
        if (controlGroup03.Contains(guardian)) controlGroup03.Remove(guardian);
        if (controlGroup04.Contains(guardian)) controlGroup04.Remove(guardian);
        if (controlGroup05.Contains(guardian)) controlGroup05.Remove(guardian);
        if (controlGroup06.Contains(guardian)) controlGroup06.Remove(guardian);
        if (controlGroup07.Contains(guardian)) controlGroup07.Remove(guardian);
        if (controlGroup08.Contains(guardian)) controlGroup08.Remove(guardian);
        if (controlGroup09.Contains(guardian)) controlGroup09.Remove(guardian);
        if (controlGroup10.Contains(guardian)) controlGroup10.Remove(guardian);
    }
    private List<Guardian> GetGroup(int _group)
    {
        List<Guardian> _units = controlGroup01;
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
        foreach (Guardian unit in guardianSelected)
        {
            if (_DATA.IsTowerUnit(unit.guardianID))
                _UI.EnableTowerText();
        }
    }

    public void ClickSelect(Guardian guardianToAdd)
    {
        DeselectAll();
        guardianSelected.Add(guardianToAdd);
        guardianToAdd.isSelected = true;
        guardianToAdd.selectionRing.Select(true);
        GameEvents.ReportOnObjectSelected(guardianToAdd.gameObject);
        StartCoroutine(WaitToCheckIsTower());
    }

    public void ShiftClickSelect(Guardian guardianToAdd)
    {
        if (!guardianSelected.Contains(guardianToAdd))
        {
            guardianSelected.Add(guardianToAdd);
            guardianToAdd.isSelected = true;
            guardianToAdd.selectionRing.Select(true);
            StartCoroutine(WaitToCheckIsTower());
        }
        else
        {
            guardianToAdd.isSelected = false;
            guardianSelected.Remove(guardianToAdd);
            guardianToAdd.selectionRing.Select(true);
            StartCoroutine(WaitToCheckIsTower());
        }
    }

    public void DoubleClickSelect(Guardian guardianToAdd)
    {
        guardianSelected.Add(guardianToAdd);
        guardianToAdd.isSelected = true;
        guardianToAdd.selectionRing.Select(true);
        StartCoroutine(WaitToCheckIsTower());
    }

    public void DragSelect(Guardian guardianToAdd)
    {
        if (!guardianSelected.Contains(guardianToAdd))
        {
            guardianSelected.Add(guardianToAdd);
            guardianToAdd.isSelected = true;
            guardianToAdd.selectionRing.Select(true);
            StartCoroutine(WaitToCheckIsTower());
        }
    }

    public void DeselectAll()
    {
        if(_UI.mouseOverCombatOptions == false)
        {
            foreach (var unit in guardianSelected)
            {
                unit.isSelected = false;
                unit.selectionRing.Select(false);
                _UI.DisableTowerText();
            }
            guardianSelected.Clear();
        }
        GameEvents.ReportOnObjectSelected(null);
    }

    public void Deselect(Guardian guardianToDeselect)
    {
        guardianToDeselect.isSelected = false;
        guardianToDeselect.selectionRing.Select(false);
        _UI.DisableTowerText();
        guardianSelected.Remove(guardianToDeselect);
        GameEvents.ReportOnObjectSelected(null);
    }

    public void AssignDestination()
    {
        for (int i = 0; i < guardianSelected.Count; i++)
        {
            guardianSelected[i].SetDestination(destinations[i].transform);
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
        if(guardianSelected.Count != 0)
        {
            AssignDestination();
            if (guardianSelected[0].guardianID == GuardianID.Goblin)
            {
                audioSource.clip = _SM.GetGoblinVocal();
                audioSource.Play();
            }
            if (guardianSelected[0].guardianID == GuardianID.Leshy)
            {
                audioSource.clip = _SM.GetLeshyVocal();
                audioSource.Play();
            }
            if (guardianSelected[0].guardianID == GuardianID.Orcus)
            {
                audioSource.clip = _SM.GetOrcusVocal();
                audioSource.Play();
            }
            if (guardianSelected[0].guardianID == GuardianID.Satyr)
            {
                audioSource.clip = _SM.GetSatyrVocal();
                audioSource.Play();
            }
            if (guardianSelected[0].guardianID == GuardianID.Skessa)
            {
                audioSource.clip = _SM.GetSkessaVocal();
                audioSource.Play();
            }
            if (guardianSelected[0].guardianID == GuardianID.Mistcalf)
            {
                audioSource.clip = _SM.GetGolemVocal();
                audioSource.Play();
            }
            if (guardianSelected[0].guardianID == GuardianID.Fidhain)
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
