using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class UnitLoadouts : GameBehaviour
{
    public Image icon;
    public Image mapIcon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text attackValue;
    public TMP_Text healthValue;
    public TMP_Text speedValue;
    public TMP_Text maegenCost;

    [Header("Totals")]
    public TMP_Text attackTotalValue;
    public TMP_Text healthTotalValue;
    public TMP_Text speedTotalValue;

    [Header("Units")]
    public List<UnitData> currentUnits;

    private void Start()
    {
        UpdateTotals();
    }

    public void ChangeUnit(UnitID _unitID)
    {
        UnitData unit = _DATA.GetUnit(_unitID);
        icon.sprite =  unit.icon;
        mapIcon.sprite =  unit.mapIcon;
        nameText.text = unit.name;
        descriptionText.text = unit.description;
        attackValue.text = unit.stats.damage.ToString();
        healthValue.text = unit.stats.health.ToString();
        speedValue.text = unit.stats.speed.ToString();
        maegenCost.text = unit.stats.price.ToString();
    }

    public void UpdateTotals()
    {
        int healthTotal = 0;
        int attackTotal = 0;
        int speedTotal = 0;
        for(int i=0; i< currentUnits.Count;i++)
        {
            healthTotal += currentUnits[i].stats.health;
            attackTotal += currentUnits[i].stats.damage;
            speedTotal += currentUnits[i].stats.speed;
        }

        attackTotalValue.text = attackTotal.ToString();
        healthTotalValue.text = healthTotal.ToString();
        speedTotalValue.text = speedTotal.ToString();
    }

    #region Editor
    public void Setup()
    {
        foreach (UnitLoadout ul in transform.GetComponentsInChildren<UnitLoadout>())
        {
            ul.unitLoadouts = this;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(UnitLoadouts))]
    [CanEditMultipleObjects]

    public class UnitLoadoutsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            UnitLoadouts unit = (UnitLoadouts)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Setup Buttons"))
            {
                unit.Setup();
                EditorUtility.SetDirty(unit);
            }
            GUILayout.EndHorizontal();

            DrawDefaultInspector();
        }
    }
#endif
    #endregion
}
