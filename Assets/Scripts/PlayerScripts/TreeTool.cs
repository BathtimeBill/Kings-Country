using System.Collections.Generic;
using UnityEngine;

public class TreeTool : GameBehaviour
{
    public TreeID treeID;
    [BV.EnumList(typeof(TreeID))]
    public List<GameObject> treeObjects;
    private TreeData treeData;
    private bool CanPlace = true;
    public bool canPlace
    {
        get { return CanPlace; }
        set {CanPlace = value; SetMaterials(value);}
    }
    public Material canPlaceMat;
    public Material cannotPlaceMat;
    private Renderer[] myRenderers;
    private Collider collider;
    
    [Header("Tree Stats")]
    [ReadOnly] public bool tooFarAway = false;
    [ReadOnly] public bool insufficientMaegen = false;
    [ReadOnly] public float distanceFromTree;
    [ReadOnly] public float energyMultiplier;
    [ReadOnly] public int maegenPerDay;
    [ReadOnly] public int maegenCost;
    public float distanceBreak1 = 8;
    public float distanceBreak2 = 16;
    public float distanceBreak3 = 32;
    public Vector3 uiPanelOffset;
    
    public void Start()
    {
        ObjectX.ToggleObjects(treeObjects, false);
        treeData = _DATA.GetTree(treeID);
        myRenderers = GetComponentsInChildren<Renderer>(true);
        collider = GetComponent<Collider>();
    }
    public void Select(TreeID _treeID)
    {
        ExecuteNextFrame(() =>  //Used to stop the pop from the object not resetting in time
        {
            treeObjects[(int)_treeID].SetActive(true);
            _UI.ShowTreeModifier(true);
            collider.enabled = true;
        });
        
    }

    public void Deselect()
    {
        ObjectX.ToggleObjects(treeObjects, false);
        collider.enabled = false;
    }

    public void Use()
    {
        if (_GM.trees.Count < _GM.maxTrees)
        {
            if (canPlace)
            {
                Vector3 randomSize = new Vector3(1, Random.Range(treeData.sizeRange.min, treeData.sizeRange.max), 1);
                GameObject treeInstance = Instantiate(treeData.playModel, transform.position, transform.rotation);
                treeInstance.transform.localScale = randomSize;
                treeInstance.GetComponent<Tree>().energyMultiplier = maegenPerDay;
                GameEvents.ReportOnTreePlaced(TreeID.Tree);
                _GM.DecreaseMaegen(maegenCost);
            }
            else if (tooFarAway)
            {
                _UI.SetError(ErrorID.TooFar);
            }
            else if (insufficientMaegen)
            {
                _UI.SetError(ErrorID.InsufficientMaegen);
            }
        }
        else
        {
            _UI.SetError(ErrorID.TooManyTrees);
        }
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
    
    private void SetMaterials(bool canUse)
    {
        foreach (var r in myRenderers)
        {
            r.material = canUse ? canPlaceMat : cannotPlaceMat;
        }
    }

    private void Update()
    {
        if (_GM.playmode != PlayMode.TreeMode)
            return;

        if(_GM.trees.Count != 0)
            distanceFromTree = Vector3.Distance(ObjectX.GetClosest(gameObject, _GM.trees).transform.position, transform.position);
        else
            distanceFromTree = Vector3.Distance(_HOME.transform.position, transform.position);
        
        energyMultiplier = 1f / distanceBreak3 * distanceFromTree;
        if (distanceFromTree <= distanceBreak1)
            maegenPerDay = 1;
        else if (distanceFromTree <= distanceBreak2)
            maegenPerDay = 2;
        else if (distanceFromTree <= distanceBreak3)
            maegenPerDay = 3;
        maegenCost = maegenPerDay + 1;
        
        if (distanceFromTree >= distanceBreak3 && !tooFarAway)
        {
            tooFarAway = true;
            canPlace = false;
        }
        if (distanceFromTree < distanceBreak3 && tooFarAway)
        {
            if(!insufficientMaegen)
            {
                tooFarAway = false;
                canPlace = true;
            }
        }
        
        int requiredMaegen = maegenPerDay + 1;

        if (maegenPerDay <= 3)
        {
            if (_GM.maegen < requiredMaegen && insufficientMaegen == false)
            {
                insufficientMaegen = true;
                canPlace = false;
            }
            else if (_GM.maegen >= requiredMaegen && insufficientMaegen == true)
            {
                insufficientMaegen = false;
                canPlace = true;
            }
        }

        _UI.ChangeTreePercentagePanel(energyMultiplier, maegenPerDay, maegenCost, canPlace);
        _UI.treePercentageModifier.transform.position = Input.mousePosition + uiPanelOffset;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree") || 
            other.CompareTag("CantPlace") || 
            other.CompareTag("River") || 
            other.CompareTag("Home Tree") || 
            other.CompareTag("Hut") || 
            other.CompareTag("Horgr") || 
            other.CompareTag("Wildlife"))
        {
            canPlace = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tree") || 
            other.CompareTag("CantPlace") || 
            other.CompareTag("River") || 
            other.CompareTag("Home Tree") || 
            other.CompareTag("Hut") || 
            other.CompareTag("Horgr") || 
            other.CompareTag("Wildlife"))
        {
            if(!tooFarAway && !insufficientMaegen)
            {
                canPlace = true;
            }
        }
    }
}
