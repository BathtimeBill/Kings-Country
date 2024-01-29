using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile 
{
    [Header("General")]
    public int levelsUnlocked;
    public int overworldMaegen;
    [Header("Level 1")]
    public int LVL1Score;
    public int LVL1HighScore;
    public bool hasCompletedLVL1;
    [Header("Level 2")]
    public int LVL2Score;
    public int LVL2HighScore;
    public bool hasCompletedLVL2;
}
