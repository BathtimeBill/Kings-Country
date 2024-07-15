using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public UpgradePanel upgradePanel;
    public Transform cameras;

    public GameObject[] upgradeCategories;
    public ParticleSystem[] spawnParticles;
    public GameObject[] upgradeParticles;
    public GameObject[] upgradeCameras;
    public GameObject[] upgradeBases;
    public List<UpgradeSlot> upgradeSlots;

    private UpgradeObject currentUpgradeObject;
    private UpgradeObject previousUpgradeObject;
    private int currentObjectIndex;
    private GameObject currentUpgradeBase;
    private GameObject previousUpgradeBase;

    private Animator anim;

    private new void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        ChangeCategory(UpgradeCategoryID.HomeTree);
    }

    public void ChangeCategory(UpgradeCategoryID _category)
    {
        TurnOffCameras();
        TurnOffCategories();
        ShowParticles();
        upgradeCategories[(int)_category].SetActive(true);

        UpgradeSlot ugs = upgradeSlots.Find(x => x.upgradeCategory == _category);
        if (ugs == null) 
            return;

        ChangeUpgrade(ugs.upgradeSlots[0].GetComponent<UpgradeObject>());
    }

    public void ChangeUpgrade(UpgradeObject _upgradeObject)
    {
        previousUpgradeObject = currentUpgradeObject;
        currentUpgradeObject = _upgradeObject;

        if(_upgradeObject.upgradeType == UpgradeType.Unit)
        {
            upgradePanel.ChangeUpgrade(_upgradeObject.GetComponent<UpgradeUnit>().unitID);
        }
        if (_upgradeObject.upgradeType == UpgradeType.Tool)
        {
            upgradePanel.ChangeUpgrade(_upgradeObject.GetComponent<UpgradeTool>().toolID);
        }
        if (_upgradeObject.upgradeType == UpgradeType.Wildlife)
        {
            upgradePanel.ChangeUpgrade(_upgradeObject.GetComponent<UpgradeWildlife>().wildlifeID);
        }

        //gets the int value of the upgrade to use in list checks
        UpgradeSlot ugs = upgradeSlots.Find(x => x.upgradeCategory == currentUpgradeObject.upgradeCategoryID);
        if (ugs == null) return;
        currentObjectIndex = ugs.upgradeSlots.IndexOf(currentUpgradeObject.gameObject);

        previousUpgradeBase = currentUpgradeBase;
        currentUpgradeBase = upgradeBases[currentObjectIndex];

        ChangeUpgradeLayer();
    }

    private void ChangeUpgradeLayer()
    {
        if(previousUpgradeObject != null)
            ObjectX.SetLayerRecursively(previousUpgradeObject.transform, LayerMask.NameToLayer("Default"));

        ObjectX.SetLayerRecursively(currentUpgradeObject.transform, LayerMask.NameToLayer("3DModel"));

        if (previousUpgradeBase != null)
            ObjectX.SetLayerRecursively(previousUpgradeBase.transform, LayerMask.NameToLayer("Default"));

        ObjectX.SetLayerRecursively(currentUpgradeBase.transform, LayerMask.NameToLayer("3DModel"));
    }

    private void ChangeUpgradeCamera()
    {
        TurnOffCameras();
        upgradeCameras[currentObjectIndex].SetActive(true);
    }

    public void UpgradeObject()
    {
        anim.SetTrigger("UpgradeLevel2");
    }

    private void TurnOffCategories()
    {
        for(int i=0; i < upgradeCategories.Length; i++)
        {
            upgradeCategories[i].SetActive(false);
        }
    }

    private void ShowParticles()
    {
        for (int i = 0; i < spawnParticles.Length; i++)
        {
            spawnParticles[i].gameObject.SetActive(true);
            spawnParticles[i].Play();
        }
    }

    private void TurnOffCameras()
    {
        for (int i = 0; i < upgradeCameras.Length; i++)
            upgradeCameras[i].SetActive(false);
    }

    #region Animation Events
    public void ZoomToUpgrade()
    {
        ChangeUpgradeCamera();
    }

    public void ReturnToNormalView()
    {
        TurnOffCameras();
    }
    public void PlayUpgradeParticles()
    {
        upgradeParticles[currentObjectIndex].SetActive(true);
        upgradeCameras[currentObjectIndex].transform.DOShakePosition(1.5f);
    }
    public void ChangeUnitLevel()
    {

    }
    #endregion

    #region Editor
    public void Setup()
    {
        //foreach (UnitUpgrade ul in transform.GetComponentsInChildren<UnitUpgrade>())
        //{
        //    ul.unitUpgrades = this;
        //}
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(UpgradeManager))]
    [CanEditMultipleObjects]

    public class UpgradeManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(20);
            GUI.backgroundColor = Color.green;
            UpgradeManager upgrades = (UpgradeManager)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Home Tree"))
            {
                upgrades.ChangeCategory(UpgradeCategoryID.HomeTree);
                EditorUtility.SetDirty(upgrades);
            }
            if (GUILayout.Button("Hut"))
            {
                upgrades.ChangeCategory(UpgradeCategoryID.Hut);
                EditorUtility.SetDirty(upgrades);
            }
            if (GUILayout.Button("Hogyr"))
            {
                upgrades.ChangeCategory(UpgradeCategoryID.Hogyr);
                EditorUtility.SetDirty(upgrades);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Tree"))
            {
                upgrades.ChangeCategory(UpgradeCategoryID.Tree);
                EditorUtility.SetDirty(upgrades);
            }
            if (GUILayout.Button("Tool"))
            {
                upgrades.ChangeCategory(UpgradeCategoryID.Tool);
                EditorUtility.SetDirty(upgrades);
            }
            if (GUILayout.Button("Wildlife"))
            {
                upgrades.ChangeCategory(UpgradeCategoryID.Wildlife);
                EditorUtility.SetDirty(upgrades);
            }
            GUILayout.EndHorizontal();


        }
    }
#endif
    #endregion
}

[System.Serializable]
public class UpgradeSlot
{
    public UpgradeCategoryID upgradeCategory;
    public List<GameObject> upgradeSlots;
}

public class UpgradeObject : GameBehaviour
{
    public UpgradeCategoryID upgradeCategoryID;
    public UpgradeType upgradeType;
}