using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
public class StatsManager : GameBehaviour
{
    [Header("Trees")]
    public TreeStats treeStats;


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
        treeStats.willowPlanted.text = willowPlanted.ToString();
        treeStats.willowDestroyed.text = willowDestroyed.ToString();
        treeStats.ficusPlanted.text = ficusPlanted.ToString();
        treeStats.ficusDestroyed.text = ficusDestroyed.ToString();
        treeStats.treePlanted.text = stats.treesPlanted.ToString();

        treeStats.totalPlanted.text = (treesPlanted + willowPlanted + ficusPlanted).ToString();
        treeStats.totalDestroyed.text = (treesDestroyed + willowDestroyed + ficusDestroyed).ToString();

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

        FillUnitStat(us, HumanID.Logger, _stats, 0);
        FillUnitStat(us, HumanID.Lumberjack, _stats, 1);
        FillUnitStat(us, HumanID.LogCutter, _stats, 2);
        FillUnitStat(us, HumanID.Poacher, _stats, 3);
        FillUnitStat(us, HumanID.Wathe, _stats, 4);
        FillUnitStat(us, HumanID.Bjornjeger, _stats, 5);
        FillUnitStat(us, HumanID.Dreng, _stats, 6);
        FillUnitStat(us, HumanID.Berserkr, _stats, 7);
        FillUnitStat(us, HumanID.Knight, _stats, 8);
        FillUnitStat(us, HumanID.Dog, _stats, 9);
        FillUnitStat(us, HumanID.Spy, _stats, 10);
        FillUnitStat(us, HumanID.Lord, _stats, 11);
    }

    private void FillUnitStat(UnitStats _us, HumanID _id, UnitKillStats _uks, int _position)
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
        _uks.ratio[_position].text = winLossPercentage.ToString()+"%";

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
        _uks.totalRatioText.text = totalWinLossPercentage.ToString() + "%";
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

