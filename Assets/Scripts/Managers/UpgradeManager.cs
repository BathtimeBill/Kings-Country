using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

public class UpgradeManager : GameBehaviour
{
    public GameObject[] upgradeCategories;
    public ParticleSystem[] spawnParticles;

    private void Start()
    {
        //ChangeCategory(UpgradeCategoryID.Tree);
    }

    public void ChangeCategory(UpgradeCategoryID _category)
    {
        TurnOffCategories();
        ShowParticles();
        upgradeCategories[(int)_category].SetActive(true);
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