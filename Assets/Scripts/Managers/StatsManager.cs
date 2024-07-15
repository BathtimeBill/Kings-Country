using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
public class StatsManager : GameBehaviour
{
    [Header("Trees")]
    public TreeStats treeStats;

    [Header("Death Stats")]
    public HumanDeathStats humanDeathStats;

    [Header("Detailed Kill Stats")]
    public UnitKillStats unit1;
    public UnitKillStats unit2;
    public UnitKillStats unit3;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject detailedPanel;

    [Header("Panel Buttons")]
    public Button mainButton;
    public Button homeTreeButton;
    public Button hutButton;
    public Button hogyrButton;

    public void Start()
    {
        mainButton.onClick.AddListener(() => FillMainPage());
        homeTreeButton.onClick.AddListener(() => FillDetailedStatPanel(BuildingID.HomeTree));
        hutButton.onClick.AddListener(() => FillDetailedStatPanel(BuildingID.Hut));
        hogyrButton.onClick.AddListener(() => FillDetailedStatPanel(BuildingID.Hogyr));

        //FillDetailedStatPanel(BuildingID.HomeTree);
        FillMainPage();
    }

    public void FillMainPage()
    {
        mainPanel.SetActive(true);
        detailedPanel.SetActive(false);

        PlayerStats stats = _SAVE.GetPlayerStats;
        if (stats == null)
            return;

        //Trees
        int treesPlanted = stats.treesPlanted;
        int treesDestroyed = stats.treesLost;
        int willowPlanted = stats.willowsPlanted;
        int willowDestroyed = stats.willowsLost;
        int ficusPlanted = stats.ficusPlanted;
        int ficusDestroyed = stats.ficusLost;
        treeStats.treePlanted.text = treesPlanted.ToString();
        treeStats.treeDestroyed.text = treesDestroyed.ToString();
        treeStats.treeRatio.text = MathX.CalculateWinLossRatio(treesPlanted, treesDestroyed).ToString("F2") + "%";
        treeStats.willowPlanted.text = willowPlanted.ToString();
        treeStats.willowDestroyed.text = willowDestroyed.ToString();
        treeStats.willowRatio.text = MathX.CalculateWinLossRatio(willowPlanted, willowDestroyed).ToString("F2") + "%";
        treeStats.ficusPlanted.text = ficusPlanted.ToString();
        treeStats.ficusDestroyed.text = ficusDestroyed.ToString();
        treeStats.ficusRatio.text = MathX.CalculateWinLossRatio(ficusPlanted, ficusDestroyed).ToString("F2") + "%";

        treeStats.totalPlanted.text = (treesPlanted + willowPlanted + ficusPlanted).ToString();
        treeStats.totalDestroyed.text = (treesDestroyed + willowDestroyed + ficusDestroyed).ToString();
        treeStats.totalRatio.text = MathX.CalculateWinLossRatio((treesPlanted + willowPlanted + ficusPlanted), (treesDestroyed + willowDestroyed + ficusDestroyed)).ToString("F2") + "%";

        //Humans
        SetHumanDeathCount();
    }

    public void FillDetailedStatPanel(BuildingID _buildingID)
    {
        mainPanel.SetActive(false);
        detailedPanel.SetActive(true);

        switch (_buildingID)
        {
            case BuildingID.HomeTree:
                GetUnitStats(unit1, CreatureID.Satyr);
                GetUnitStats(unit2, CreatureID.Orcus);
                GetUnitStats(unit3, CreatureID.Leshy);
                break;
            case BuildingID.Hut:
                GetUnitStats(unit1, CreatureID.Goblin);
                GetUnitStats(unit2, CreatureID.Skessa);
                GetUnitStats(unit3, CreatureID.Fidhain);
                break;
            case BuildingID.Hogyr:
                GetUnitStats(unit1, CreatureID.Huldra);
                GetUnitStats(unit2, CreatureID.Mistcalf);
                GetUnitStats(unit3, CreatureID.Unknown);
                break;
        }
        
    }

    public void GetUnitStats(UnitKillStats _stats, CreatureID _creatureID)
    {
        _stats.unitNameText.text = _creatureID.ToString();
        _stats.unitIcon.sprite = _DATA.GetUnit(_creatureID).icon;

        UnitStats us = _SAVE.GetUnitStats(_creatureID.ToString());
        if (us == null)
        {
            NoStats(_stats);
            return;
        }

        FillUnitStatDetailed(us, HumanID.Logger, _stats, 0);
        FillUnitStatDetailed(us, HumanID.Lumberjack, _stats, 1);
        FillUnitStatDetailed(us, HumanID.LogCutter, _stats, 2);
        FillUnitStatDetailed(us, HumanID.Poacher, _stats, 3);
        FillUnitStatDetailed(us, HumanID.Wathe, _stats, 4);
        FillUnitStatDetailed(us, HumanID.Bjornjeger, _stats, 5);
        FillUnitStatDetailed(us, HumanID.Dreng, _stats, 6);
        FillUnitStatDetailed(us, HumanID.Berserkr, _stats, 7);
        FillUnitStatDetailed(us, HumanID.Knight, _stats, 8);
        FillUnitStatDetailed(us, HumanID.Dog, _stats, 9);
        FillUnitStatDetailed(us, HumanID.Spy, _stats, 10);
        FillUnitStatDetailed(us, HumanID.Lord, _stats, 11);
    }

    private void FillUnitStatDetailed(UnitStats _us, HumanID _id, UnitKillStats _uks, int _position)
    {
        //int 
        //Kills
        KillStat kills = _us.iHaveKilled.Find(x => x.killedID == _id.ToString());
        int killAmount = kills == null ? 0 : kills.amount;
        _uks.kills[_position].text = killAmount.ToString();

        //Deaths
        KillStat deaths = _us.killedBy.Find(x => x.killedID == _id.ToString());
        int deathAmount = deaths == null ? 0 : deaths.amount;
        _uks.deaths[_position].text = deathAmount.ToString();

        //Ratio
        double winLossPercentage = MathX.CalculateWinLossRatio(killAmount, deathAmount);
        _uks.ratio[_position].text = winLossPercentage.ToString("F2") +"%";

        //if (winLossPercentage > 1) _uks.ratio[_position].color = _COLOUR.upgradeIncreaseColor;
        //else if (winLossPercentage < 1) _uks.ratio[_position].color = _COLOUR.upgradeDecreaseColor;
        //else
        //    _uks.ratio[_position].color = Color.white;

        //Total Kills
        int totalKillAmount = 0;
        for (int i = 0; i < _uks.kills.Count; i++)
        {
            int totalValue;
            int.TryParse(_uks.kills[i].text, out totalValue);
            totalKillAmount += totalValue;
        }
        _uks.totalKillsText.text = totalKillAmount.ToString();

        //Total Deaths
        int totalDeathAmount = 0;
        for (int i = 0; i < _uks.deaths.Count; i++)
        {
            int totalValue;
            int.TryParse(_uks.deaths[i].text, out totalValue);
            totalDeathAmount += totalValue;
        }
        _uks.totalDeathsText.text = totalDeathAmount.ToString();

        double totalWinLossPercentage = MathX.CalculateWinLossRatio(totalKillAmount, totalDeathAmount);
        _uks.totalRatioText.text = totalWinLossPercentage.ToString("F2") + "%";
        //_uks.totalRatioText.color = winLossPercentage > 1 ? _COLOUR.upgradeIncreaseColor : winLossPercentage < 0 ? _COLOUR.upgradeDecreaseColor : Color.white;
    }

    private void NoStats(UnitKillStats _stats)
    {
        for(int i=0; i < _stats.kills.Count; i++)
        {
            _stats.kills[i].text = "0";
            _stats.deaths[i].text = "0";
            _stats.ratio[i].text = "0.0%";
        }
        _stats.totalKillsText.text = "0";
        _stats.totalDeathsText.text = "0";
        _stats.totalRatioText.text = "0.0%";
    }

    public void SetHumanDeathCount()
    {
        humanDeathStats.logger.text     = _SAVE.GetDeathCount(HumanID.Logger.ToString()).ToString();
        humanDeathStats.lumberjack.text = _SAVE.GetDeathCount(HumanID.Lumberjack.ToString()).ToString();
        humanDeathStats.logCutter.text  = _SAVE.GetDeathCount(HumanID.LogCutter.ToString()).ToString();
        humanDeathStats.wathe.text      = _SAVE.GetDeathCount(HumanID.Wathe.ToString()).ToString();
        humanDeathStats.poacher.text    = _SAVE.GetDeathCount(HumanID.Poacher.ToString()).ToString();
        humanDeathStats.bjornjeger.text = _SAVE.GetDeathCount(HumanID.Bjornjeger.ToString()).ToString();
        humanDeathStats.dreng.text      = _SAVE.GetDeathCount(HumanID.Dreng.ToString()).ToString();
        humanDeathStats.berserkr.text   = _SAVE.GetDeathCount(HumanID.Berserkr.ToString()).ToString();
        humanDeathStats.knight.text     = _SAVE.GetDeathCount(HumanID.Knight.ToString()).ToString();
        humanDeathStats.spy.text        = _SAVE.GetDeathCount(HumanID.Spy.ToString()).ToString();
        humanDeathStats.dog.text        = _SAVE.GetDeathCount(HumanID.Dog.ToString()).ToString();
        humanDeathStats.lord.text       = _SAVE.GetDeathCount(HumanID.Lord.ToString()).ToString();
    }
}

[System.Serializable]
public class UnitKillStats
{
    public TMP_Text unitNameText;
    public Image unitIcon;
    public List<TMP_Text> kills;
    public List<TMP_Text> deaths;
    public List<TMP_Text> ratio;
    public TMP_Text totalKillsText;
    public TMP_Text totalDeathsText;
    public TMP_Text totalRatioText;
}

[System.Serializable]
public class TreeStats
{
    public TMP_Text treePlanted;
    public TMP_Text treeDestroyed;
    public TMP_Text treeRatio;
    public TMP_Text willowPlanted;
    public TMP_Text willowDestroyed;
    public TMP_Text willowRatio;
    public TMP_Text ficusPlanted;
    public TMP_Text ficusDestroyed;
    public TMP_Text ficusRatio;

    public TMP_Text totalPlanted;
    public TMP_Text totalDestroyed;
    public TMP_Text totalRatio;
}

[System.Serializable]
public class HumanDeathStats
{
    public TMP_Text logger;
    public TMP_Text lumberjack;
    public TMP_Text logCutter;
    public TMP_Text wathe;
    public TMP_Text poacher;
    public TMP_Text bjornjeger;
    public TMP_Text dreng;
    public TMP_Text berserkr;
    public TMP_Text knight;
    public TMP_Text spy;
    public TMP_Text dog;
    public TMP_Text lord;
}
