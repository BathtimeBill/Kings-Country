using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class UpgradePanel : GameBehaviour
{
    [Header("Core")]
    public Image icon;
    public Image mapIcon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    [Header("Stat 1")]      //Units - Maegen
    public Image stat1Icon;
    public TMP_Text stat1Value;
    public Image stat1ChangeIcon;
    public TMP_Text stat1ValueChange;
    Animator stat1Anim;
    [Header("Stat 2")]      //Units - Damage
    public Image stat2Icon;
    public TMP_Text stat2Value;
    public Image stat2ChangeIcon;
    public TMP_Text stat2ChangeValue;
    Animator stat2Anim;
    [Header("Stat 3")]      //Units - Health
    public Image stat3Icon;
    public TMP_Text stat3Value;
    public Image stat3ChangeIcon;
    public TMP_Text stat3ChangeValue;
    Animator stat3Anim;
    [Header("Stat 4")]      //Units - Speed
    public Image stat4Icon;
    public TMP_Text stat4Value;
    public Image stat4ChangeIcon;
    public TMP_Text stat4ChangeValue;
    Animator stat4Anim;
    
    [Header("Totals")]
    public TMP_Text damageTotalValue;
    public TMP_Text damageTotalChangeValue;
    public Image damageTotalIcon;
    Animator damageTotalAnim;
    [Space]
    public TMP_Text healthTotalValue;
    public TMP_Text healthTotalChangeValue;
    public Image healthTotalIcon;
    Animator healthTotalAnim;
    [Space]
    public TMP_Text speedTotalValue;
    public TMP_Text speedTotalChangeValue;
    public Image speedTotalIcon;
    Animator speedTotalAnim;
    [Space]
    public TMP_Text maegenTotalValue;
    public TMP_Text maegenTotalChangeValue;
    public Image maegenTotalIcon;
    Animator maegenTotalAnim;

    [Header("Level Stars")]
    public Image level1Icon;
    public Image level2Icon;
    public Image level3Icon;
    Animator level1Anim;
    Animator level2Anim;
    Animator level3Anim;

    [Header("Stats")]

    [Header("Units")]
    public List<UnitData> currentUnits;
    private UnitData activeUnit;
    public CreatureID ActiveUnit => activeUnit.id;

    private ToolData activeTool;
    public ToolID ActiveTool => activeTool.id;

    private int currentStat1 = 0;
    private int currentStat2 = 0;
    private int currentStat3 = 0;
    private int currentStat4 = 0;

    private int damageTotal = 0;
    private int healthTotal = 0;
    private int speedTotal = 0;
    private int maegenTotal = 0;
    private int damageUpgradePercentage = 0;
    private int healthUpgradePercentage = 0;
    private int speedUpgradePercentage = 0;
    private int maegenUpgradePercentage = 0;

    public Canvas unitCanvas;
    Vector3 worldPos = new Vector3(8, 0, 0.1f);
    Vector3 worldScale = new Vector3(0.013f, 0.013f, 0.013f);
    Vector3 worldRotation = new Vector3(90, 0, 0);
    bool worldCanvas = false;


    private void Awake()
    {
        stat1Anim = stat1ChangeIcon.GetComponent<Animator>();
        stat2Anim = stat2ChangeIcon.GetComponent<Animator>();
        stat3Anim = stat3ChangeIcon.GetComponent<Animator>();
        stat4Anim = stat4ChangeIcon.GetComponent<Animator>();

        damageTotalAnim = damageTotalIcon.GetComponent<Animator>();
        healthTotalAnim = healthTotalIcon.GetComponent<Animator>();
        speedTotalAnim = speedTotalIcon.GetComponent<Animator>();
        maegenTotalAnim = maegenTotalIcon.GetComponent<Animator>();

        level1Anim = level1Icon.GetComponent<Animator>();
        level2Anim = level2Icon.GetComponent<Animator>();
        level3Anim = level3Icon.GetComponent<Animator>();
    }

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
        stat1Value.text = "-";
        stat2Value.text = "-";
        stat3Value.text = "-";
        stat4Value.text = "-";
        RemoveUpgradeValues();
    }

    public void ChangeUpgrade(CreatureID _unitID)
    {
        activeUnit = _DATA.GetUnit(_unitID);
        icon.sprite =  activeUnit.icon;
        mapIcon.sprite =  activeUnit.mapIcon;
        nameText.text = activeUnit.name;
        descriptionText.text = activeUnit.description;
        currentStat1 = activeUnit.cost;
        currentStat2 = activeUnit.damage;
        currentStat3 = activeUnit.health;
        currentStat4 = activeUnit.speed;

        stat1Icon.sprite = _ICONS.maegenIcon;
        stat2Icon.sprite = _ICONS.damageIcon;
        stat3Icon.sprite = _ICONS.healthIcon;
        stat4Icon.sprite = _ICONS.speedIcon;

        SetStatValues();
    }

    public void ChangeUpgrade(ToolID _toolID)
    {
        activeTool = _DATA.GetTool(_toolID);
        icon.sprite = activeTool.icon;
        mapIcon.sprite = _ICONS.emptyIcon;
        nameText.text = activeTool.name;
        descriptionText.text = activeTool.description;
        currentStat1 = activeTool.maegenPrice;
        currentStat2 = activeTool.wildlifePrice;
        currentStat3 = activeTool.cooldownTime;
        currentStat4 = 0;

        stat1Icon.sprite = _ICONS.maegenIcon;
        stat2Icon.sprite = _ICONS.wildlifeIcon;
        stat3Icon.sprite = _ICONS.cooldownIcon;
        stat4Icon.sprite = _ICONS.emptyIcon;

        SetStatValues();
    }

    public void ChangeUpgrade(WildlifeID _wildlifeID)
    {

    }

    public void SetStatValues()
    {
        stat1Value.text = currentStat1.ToString();
        stat2Value.text = currentStat2.ToString();
        stat3Value.text = currentStat3.ToString();
        stat4Value.text = currentStat4.ToString();

        RemoveUpgradeValues();
        ShowTotal(); 
    }

    public void ShowToolStats()
    {

    }

    private void RemoveUpgradeValues()
    {
        ToggleChangeIcons(false);
        stat1ValueChange.text = "";
        stat2ChangeValue.text = "";
        stat3ChangeValue.text = "";
        stat4ChangeValue.text = "";

        damageTotalChangeValue.text = "";
        healthTotalChangeValue.text = "";
        speedTotalChangeValue.text = "";
        maegenTotalChangeValue.text = "";

        level2Anim.SetTrigger("Reset");
    }

    public void ShowStatsUpgrade()
    {
        if (activeUnit == null) return;
        
        ToggleChangeIcons(true);

        //Stats
        damageUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageChange(currentStat2, 10));
        healthUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageChange(currentStat3, 10));
        speedUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageChange(currentStat4, 10));
        maegenUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageIncrease(currentStat1, 10));

        int attackUpgrade = currentStat2 + damageUpgradePercentage;
        int healthUpgrade = currentStat3 + healthUpgradePercentage;
        int speedUpgrade = currentStat4 + speedUpgradePercentage;
        int maegenUpgrade = currentStat1 + maegenUpgradePercentage;

        CheckStatColours(stat1ValueChange, currentStat1, maegenUpgrade, stat1ChangeIcon, stat1Anim, true);
        CheckStatColours(stat2ChangeValue, currentStat2, attackUpgrade, stat2ChangeIcon, stat2Anim);
        CheckStatColours(stat3ChangeValue, currentStat3, healthUpgrade, stat3ChangeIcon, stat3Anim);
        CheckStatColours(stat4ChangeValue, currentStat4, speedUpgrade, stat4ChangeIcon, stat4Anim);

        //Totals
        int damageTotalUpgrade = damageTotal + damageUpgradePercentage;
        int healthTotalUpgrade = healthTotal + healthUpgradePercentage;
        int speedTotalUpgrade = speedTotal + speedUpgradePercentage;
        int maegenTotalUpgrade = maegenTotal + speedUpgradePercentage;

        CheckStatColours(damageTotalChangeValue, damageTotal, damageTotalUpgrade, damageTotalIcon, damageTotalAnim);
        CheckStatColours(healthTotalChangeValue, healthTotal, healthTotalUpgrade, healthTotalIcon, healthTotalAnim);
        CheckStatColours(speedTotalChangeValue, speedTotal, speedTotalUpgrade, speedTotalIcon, speedTotalAnim);
        CheckStatColours(maegenTotalChangeValue, maegenTotal, maegenTotalUpgrade, maegenTotalIcon, maegenTotalAnim, true);

        //Level Stars
        //TODO actually hook into level values
        level2Anim.SetTrigger("Pulse");
    }

    private void CheckStatColours(TMP_Text _text, int _current, int _change, Image _icon, Animator _anim, bool _isMaegen = false)
    {
        bool increase = _isMaegen ? _change < _current : _change > _current;
        string increaseString = "<color=#" + _COLOUR.GetIncreaseColorString + ">";
        string decreaseString = "<color=#" + _COLOUR.GetDecreaseColorString + ">";
        _icon.color = increase ? _COLOUR.upgradeIncreaseColor : _COLOUR.upgradeDecreaseColor;
        _anim.SetTrigger(increase ? "Increase" : "Decrease");
        _text.text = (increase ? increaseString : decreaseString) + _change;
    }

    public void ShowTotal()
    {
        healthTotal = 0;
        damageTotal = 0;
        speedTotal = 0;
        maegenTotal = 0;
        for(int i=0; i< currentUnits.Count;i++)
        {
            damageTotal += currentStat2;
            healthTotal += currentStat3;
            speedTotal += currentStat4;
            maegenTotal += currentStat1;
        }

        damageTotalValue.text = damageTotal.ToString();
        healthTotalValue.text = healthTotal.ToString();
        speedTotalValue.text = speedTotal.ToString();
        maegenTotalValue.text = maegenTotal.ToString();
    }

    public void ToggleCanvasPosition()
    {
        worldCanvas = !worldCanvas;

        unitCanvas.renderMode = worldCanvas ? RenderMode.WorldSpace : RenderMode.ScreenSpaceOverlay;
        unitCanvas.GetComponent<Transform>().localPosition = worldPos;
        unitCanvas.GetComponent<Transform>().localEulerAngles = worldRotation;
        unitCanvas.GetComponent<Transform>().localScale = worldScale;
    }

    private void ToggleChangeIcons(bool _on)
    {
        stat1ChangeIcon.gameObject.SetActive(_on);
        stat2ChangeIcon.gameObject.SetActive(_on);
        stat3ChangeIcon.gameObject.SetActive(_on);
        stat4ChangeIcon.gameObject.SetActive(_on);

        damageTotalIcon.gameObject.SetActive(_on);
        healthTotalIcon.gameObject.SetActive(_on);
        speedTotalIcon.gameObject.SetActive(_on);
        maegenTotalIcon.gameObject.SetActive(_on);
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
    [CustomEditor(typeof(UpgradePanel))]
    [CanEditMultipleObjects]

    public class UnitLoadoutsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(20);
            GUI.backgroundColor = Color.green;
            UpgradePanel unit = (UpgradePanel)target;
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


