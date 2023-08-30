using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile 
{
    [Header("Level 1")]
    public int levelsUnlocked;
    public int LVL1Score;
    public int LVL1HighScore;
    public bool hasCompletedLVL1;
}
