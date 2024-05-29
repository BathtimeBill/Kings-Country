using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class UnitUpgrades : GameBehaviour
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
    private UnitData activeUnit;

    private int healthTotal = 0;
    private int attackTotal = 0;
    private int speedTotal = 0;
    private int attackUpgradePercentage = 0;
    private int healthUpgradePercentage = 0;
    private int speedUpgradePercentage = 0;
    private int maegenUpgradePercentage = 0;

    public Canvas unitCanvas;
    Vector3 worldPos = new Vector3(8, 0, 0.1f);
    Vector3 worldScale = new Vector3(0.013f, 0.013f, 0.013f);
    Vector3 worldRotation = new Vector3(90, 0, 0);
    bool worldCanvas = false;

    private void Start()
    {
        ShowTotal();
        ShowNoUnit();
    }

    private void ShowNoUnit()
    {
        icon.sprite = null;
        mapIcon.sprite = null;
        nameText.text = "Unit Management";
        descriptionText.text = "Select a unit to view and upgrade";
        attackValue.text = "-";
        healthValue.text = "-";
        speedValue.text = "-";
        maegenCost.text = "-";
    }

    public void ChangeUnit(UnitID _unitID)
    {
        activeUnit = _DATA.GetUnit(_unitID);
        icon.sprite =  activeUnit.icon;
        mapIcon.sprite =  activeUnit.mapIcon;
        nameText.text = activeUnit.name;
        descriptionText.text = activeUnit.description;
        ShowStats();
    }

    public void ShowStats()
    {
        attackValue.text = activeUnit.stats.damage.ToString();
        healthValue.text = activeUnit.stats.health.ToString();
        speedValue.text = activeUnit.stats.speed.ToString();
        maegenCost.text = activeUnit.stats.price.ToString();

        ShowTotal();
    }

    public void ShowStatsUpgrade()
    {
        if (activeUnit == null) return;

        attackUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageChange(activeUnit.stats.damage, 10));
        healthUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageChange(activeUnit.stats.health, 10));
        speedUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageChange(activeUnit.stats.speed, 10));
        maegenUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageIncrease(activeUnit.stats.price, 10));

        int attackUpgrade = activeUnit.stats.damage + attackUpgradePercentage;
        int healthUpgrade = activeUnit.stats.health + healthUpgradePercentage;
        int speedUpgrade = activeUnit.stats.speed + speedUpgradePercentage;

        attackValue.text = activeUnit.stats.damage + "<color=#00FF00> > " + attackUpgrade;
        healthValue.text = activeUnit.stats.health + "<color=#00FF00> > " + healthUpgrade;
        speedValue.text = activeUnit.stats.speed + "<color=#00FF00> > " + speedUpgrade;
        maegenCost.text = activeUnit.stats.price + "<color=#FF0000> > " + maegenUpgradePercentage;

        ShowTotalUpgrade();
    }

    public void ShowTotal()
    {
        healthTotal = 0;
        attackTotal = 0;
        speedTotal = 0;
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

    public void ShowTotalUpgrade()
    {
        int attackTotalUpgrade = attackTotal + attackUpgradePercentage;
        int healthTotalUpgrade = healthTotal + healthUpgradePercentage;
        int speedTotalUpgrade = speedTotal + speedUpgradePercentage;

        attackTotalValue.text = attackTotal + "<color=#00FF00> > " + attackTotalUpgrade;
        healthTotalValue.text = healthTotal + "<color=#00FF00> > " + healthTotalUpgrade;
        speedTotalValue.text = speedTotal + "<color=#00FF00> > " + speedTotalUpgrade;
    }

    public void ToggleCanvasPosition()
    {
        worldCanvas = !worldCanvas;

        unitCanvas.renderMode = worldCanvas ? RenderMode.WorldSpace : RenderMode.ScreenSpaceOverlay;
        unitCanvas.GetComponent<Transform>().localPosition = worldPos;
        unitCanvas.GetComponent<Transform>().localEulerAngles = worldRotation;
        unitCanvas.GetComponent<Transform>().localScale = worldScale;
    }

    #region Editor
    public void Setup()
    {
        foreach (UnitUpgrade ul in transform.GetComponentsInChildren<UnitUpgrade>())
        {
            ul.unitUpgrades = this;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(UnitUpgrades))]
    [CanEditMultipleObjects]

    public class UnitLoadoutsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(20);
            GUI.backgroundColor = Color.green;
            UnitUpgrades unit = (UnitUpgrades)target;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Setup Buttons"))
            {
                unit.Setup();
                EditorUtility.SetDirty(unit);
            }
            GUILayout.EndHorizontal();

            
        }
    }
#endif
    #endregion
}
