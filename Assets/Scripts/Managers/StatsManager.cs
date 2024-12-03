using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
public class StatsManager : GameBehaviour
{
    [Header("General")]
    public TMP_Text totalMaegen;
    public TMP_Text daysWon;
    public TMP_Text levelsComplete;
    public TMP_Text upgradesObtained;
    public TMP_Text achievementsObtained;
    public TMP_Text timePlayed;

    [Header("Trees")]
    public TreeStats treeStats;

    [Header("Tools")]
    public ToolStats toolStats;

    [Header("Wildlife")]
    public WildlifeStats wildlifeStats;

    [Header("Unit Stats")]
    //[BV.EnumList(typeof(CreatureID))]
    public List<CreatureStats> creatureStats;
    public StatsTotal creatureStatsTotal;
    [BV.EnumList(typeof(HumanID))]
    public List<HumanStats> humanStats;
    public StatsTotal humanStatsTotal;

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
        homeTreeButton.onClick.AddListener(() => FillDetailedStatPanel(SiteID.HomeTree));
        hutButton.onClick.AddListener(() => FillDetailedStatPanel(SiteID.Hut));
        hogyrButton.onClick.AddListener(() => FillDetailedStatPanel(SiteID.Horgr));

        //FillDetailedStatPanel(SiteID.HomeTree);
        FillMainPage();
    }

    public void FillMainPage()
    {
        mainPanel.SetActive(true);
        detailedPanel.SetActive(false);

        SetGeneralStats();
        SetTreeStats();
        SetToolStats();
        SetWildlifeStats();
        SetHumanStats();
        SetCreatureStats();
    }

    #region General Stats
    private void SetGeneralStats()
    {
        totalMaegen.text = _SAVE.GetTotalMaegen.ToString();
        daysWon.text = _SAVE.GetDaysWon.ToString() + " of " + _SAVE.GetDaysPlayed.ToString();
        levelsComplete.text = _SAVE.GetLevelCompleteCount().ToString() + " of 21";
        upgradesObtained.text = "0 of 46";
        upgradesObtained.text = "0 of 32";
        timePlayed.text = _SAVE.GetElapsedTimeFormatted();
    }

    private void SetTreeStats()
    {
        int treesPlanted = _SAVE.GetTreePlantedStats(TreeID.Tree);
        int treesDestroyed = _SAVE.GetTreeLostStats(TreeID.Tree);
        int willowPlanted = _SAVE.GetTreePlantedStats(TreeID.Willow);
        int willowDestroyed = _SAVE.GetTreeLostStats(TreeID.Willow);
        int ficusPlanted = _SAVE.GetTreePlantedStats(TreeID.Ficus);
        int ficusDestroyed = _SAVE.GetTreeLostStats(TreeID.Ficus);
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
    }

    private void SetToolStats()
    {
        toolStats.fyreUsed.text = _SAVE.GetFyreUsed.ToString();
        toolStats.fyreKills.text = _SAVE.GetToolKillCount(ToolID.Fyre).ToString();
        toolStats.stormerUsed.text = _SAVE.GetStormerUsed.ToString();
        toolStats.stormerKills.text = _SAVE.GetToolKillCount(ToolID.Stormer).ToString();
    }

    private void SetWildlifeStats()
    {
        wildlifeStats.rabbitsSpawned.text = _SAVE.GetWildlifeSpawnCount(WildlifeID.Rabbit).ToString();
        wildlifeStats.rabbitsKilled.text = _SAVE.GetWildlifeKilledCount(WildlifeID.Rabbit).ToString();
        wildlifeStats.deerSpawned.text = _SAVE.GetWildlifeSpawnCount(WildlifeID.Deer).ToString();
        wildlifeStats.deerKilled.text = _SAVE.GetWildlifeKilledCount(WildlifeID.Deer).ToString();
        wildlifeStats.boarsSpawned.text = _SAVE.GetWildlifeSpawnCount(WildlifeID.Boar).ToString();
        wildlifeStats.boarsKilled.text = _SAVE.GetWildlifeKilledCount(WildlifeID.Boar).ToString();
        wildlifeStats.bearsSpawned.text = _SAVE.GetWildlifeSpawnCount(WildlifeID.Bear).ToString();
        wildlifeStats.bearsKilled.text = _SAVE.GetWildlifeKilledCount(WildlifeID.Bear).ToString();
    }

    private void SetHumanStats()
    {
        int totalSummons = 0;
        int totalKills = 0;
        int totalDeaths = 0;
        for (int i = 0; i < humanStats.Count; i++)
        {
            UnitStats us = _SAVE.GetUnitStats(humanStats[i].humanID.ToString());
            if (us != null)
            {
                int summons = us.summonCount;
                totalSummons += summons;
                humanStats[i].spawnCount.text = summons.ToString();

                int kill = _SAVE.GetCreatureKillCount(humanStats[i].humanID.ToString());
                totalKills += kill;
                humanStats[i].killCount.text = kill.ToString();

                int deaths = _SAVE.GetCreatureDeathCount(humanStats[i].humanID.ToString());
                totalDeaths += deaths;
                humanStats[i].deathCount.text = deaths.ToString();
            }
        }
        humanStatsTotal.summonCount.text = totalSummons.ToString();
        humanStatsTotal.killCount.text = totalKills.ToString();
        humanStatsTotal.deathCount.text = totalDeaths.ToString();
    }

    private void SetCreatureStats()
    {
        int totalSummons = 0;
        int totalKills = 0;
        int totalDeaths = 0;
        for (int i = 0; i < creatureStats.Count; i++)
        {
            UnitStats us = _SAVE.GetUnitStats(creatureStats[i].creatureID.ToString());
            if (us != null)
            {
                int summons = us.summonCount;
                totalSummons += summons;
                creatureStats[i].summonCount.text = summons.ToString();

                int kill = _SAVE.GetCreatureKillCount(creatureStats[i].creatureID.ToString());
                totalKills += kill;
                creatureStats[i].killCount.text = kill.ToString();

                int deaths = _SAVE.GetCreatureDeathCount(creatureStats[i].creatureID.ToString());
                totalDeaths += deaths;
                creatureStats[i].deathCount.text = deaths.ToString();
            }
        }
        creatureStatsTotal.summonCount.text = totalSummons.ToString();
        creatureStatsTotal.killCount.text = totalKills.ToString();
        creatureStatsTotal.deathCount.text = totalDeaths.ToString();
    }
    #endregion

    #region Detailed Stats
    private void FillDetailedStatPanel(SiteID siteID)
    {
        mainPanel.SetActive(false);
        detailedPanel.SetActive(true);

        switch (siteID)
        {
            case SiteID.HomeTree:
                GetUnitStats(unit1, CreatureID.Satyr);
                GetUnitStats(unit2, CreatureID.Orcus);
                GetUnitStats(unit3, CreatureID.Leshy);
                break;
            case SiteID.Hut:
                GetUnitStats(unit1, CreatureID.Goblin);
                GetUnitStats(unit2, CreatureID.Skessa);
                GetUnitStats(unit3, CreatureID.Fidhain);
                break;
            case SiteID.Horgr:
                GetUnitStats(unit1, CreatureID.Huldra);
                GetUnitStats(unit2, CreatureID.Mistcalf);
                GetUnitStats(unit3, CreatureID.Unknown);
                break;
        }
        
    }

    private void GetUnitStats(UnitKillStats _stats, CreatureID _creatureID)
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
    #endregion

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
public class HumanStats
{
    public HumanID humanID;
    public TMP_Text spawnCount;
    public TMP_Text killCount;
    public TMP_Text deathCount;
}

[System.Serializable]
public class CreatureStats
{
    public CreatureID creatureID;
    public TMP_Text summonCount;
    public TMP_Text killCount;
    public TMP_Text deathCount;
}
[System.Serializable]
public class StatsTotal
{
    public TMP_Text summonCount;
    public TMP_Text killCount;
    public TMP_Text deathCount;
}
[System.Serializable]
public class ToolStats
{
    public TMP_Text fyreUsed;
    public TMP_Text fyreKills;
    public TMP_Text stormerUsed;
    public TMP_Text stormerKills;
    public TMP_Text runesUsed;

}

[System.Serializable]
public class WildlifeStats
{
    public TMP_Text rabbitsSpawned;
    public TMP_Text rabbitsKilled;
    public TMP_Text deerSpawned;
    public TMP_Text deerKilled;
    public TMP_Text boarsSpawned;
    public TMP_Text boarsKilled;
    public TMP_Text bearsSpawned;
    public TMP_Text bearsKilled;

}