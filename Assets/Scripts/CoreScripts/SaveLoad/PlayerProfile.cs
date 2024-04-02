using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile 
{
    [Header("General")]
    public int levelsUnlocked;
    public int overworldMaegen;
    public int overworldMaegenTotal;
    public bool hasComeFromWin;
    public bool firstPlay;
    public bool firstWave;
    public bool firstMine;
    public bool firstLord;
    public bool firstLevel2;
    [Header("Level 1")]
    public int LVL1Score;
    public int LVL1HighScore;
    public bool hasCompletedLVL1;
    [Header("Level 2")]
    public int LVL2Score;
    public int LVL2HighScore;
    public bool hasCompletedLVL2;
    [Header("Level 3")]
    public int LVL3Score;
    public int LVL3HighScore;
    public bool hasCompletedLVL3;
    [Header("Level 4")]
    public int LVL4Score;
    public int LVL4HighScore;
    public bool hasCompletedLVL4;
    [Header("Level 5")]
    public int LVL5Score;
    public int LVL5HighScore;
    public bool hasCompletedLVL5;
    [Header("Perks")]
    public bool satyrPerk;
    public bool orcusPerk;
    public bool leshyPerk;
    public bool willowPerk;
    public bool skessaPerk;
    public bool goblinPerk;
    public bool fidhainPerk;
    public bool oakPerk;
    public bool huldraPerk;
    public bool golemPerk;
    public bool explosiveTreePerk;
    public bool homeTreePerk;
    public bool runePerk;
    public bool fyrePerk;
    public bool bearPerk;
}
