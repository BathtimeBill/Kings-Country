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
    [Header("Damage")]
    public TMP_Text damageValue;
    public TMP_Text damageChangeValue;
    public Image damageChangeIcon;
    Animator damageAnim;
    [Header("Health")]
    public TMP_Text healthValue;
    public TMP_Text healthChangeValue;
    public Image healthChangeIcon;
    Animator healthAnim;
    [Header("Speed")]
    public TMP_Text speedValue;
    public TMP_Text speedChangeValue;
    public Image speedChangeIcon;
    Animator speedAnim;
    [Header("Maegen")]
    public TMP_Text maegenValue;
    public TMP_Text maegenChangeValue;
    public Image maegenChangeIcon;
    Animator maegenAnim;

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

    [Header("Units")]
    public List<UnitData> currentUnits;
    private UnitData activeUnit;
    public UnitID ActiveUnit => activeUnit.id;

    private int currentDamage = 0;
    private int currentHealth = 0;
    private int currentSpeed = 0;
    private int currentMaegen = 0;

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
        damageAnim = damageChangeIcon.GetComponent<Animator>();
        healthAnim = healthChangeIcon.GetComponent<Animator>();
        speedAnim = speedChangeIcon.GetComponent<Animator>();
        maegenAnim = maegenChangeIcon.GetComponent<Animator>();

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
        damageValue.text = "-";
        healthValue.text = "-";
        speedValue.text = "-";
        maegenValue.text = "-";
        RemoveUpgradeValues();
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
        currentDamage = activeUnit.damage;
        currentHealth = activeUnit.health;
        currentSpeed = activeUnit.speed;
        currentMaegen = activeUnit.cost;

        damageValue.text = currentDamage.ToString();
        healthValue.text = currentHealth.ToString();
        speedValue.text = currentSpeed.ToString();
        maegenValue.text = currentMaegen.ToString();

        RemoveUpgradeValues();
        ShowTotal(); 
    }

    private void RemoveUpgradeValues()
    {
        ToggleChangeIcons(false);
        damageChangeValue.text = "";
        healthChangeValue.text = "";
        speedChangeValue.text = "";
        maegenChangeValue.text = "";

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
        damageUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageChange(currentDamage, 10));
        healthUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageChange(currentHealth, 10));
        speedUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageChange(currentSpeed, 10));
        maegenUpgradePercentage = Mathf.RoundToInt(MathX.GetPercentageIncrease(currentMaegen, 10));

        int attackUpgrade = currentDamage + damageUpgradePercentage;
        int healthUpgrade = currentHealth + healthUpgradePercentage;
        int speedUpgrade = currentSpeed + speedUpgradePercentage;
        int maegenUpgrade = currentMaegen + maegenUpgradePercentage;

        CheckStatColours(damageChangeValue, currentDamage, attackUpgrade, damageChangeIcon, damageAnim);
        CheckStatColours(healthChangeValue, currentHealth, healthUpgrade, healthChangeIcon, healthAnim);
        CheckStatColours(speedChangeValue, currentSpeed, speedUpgrade, speedChangeIcon, speedAnim);
        CheckStatColours(maegenChangeValue, currentMaegen, maegenUpgrade, maegenChangeIcon, maegenAnim, true);

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
            damageTotal += currentDamage;
            healthTotal += currentHealth;
            speedTotal += currentSpeed;
            maegenTotal += currentMaegen;
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
        damageChangeIcon.gameObject.SetActive(_on);
        healthChangeIcon.gameObject.SetActive(_on);
        speedChangeIcon.gameObject.SetActive(_on);
        maegenChangeIcon.gameObject.SetActive(_on);

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
