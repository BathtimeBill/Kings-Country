using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class OverWorldManager : GameBehaviour
{
    public Transform mapModel;
    public float mapTweenTime = 1f;
    public Ease mapEase;
    public Light mapLight;
    public ParticleSystem[] mapParticles;
    public LevelID selectedLevel;

    [Header("UI")]
    public TMP_Text levelTitle;
    public TMP_Text levelDescription;
    public Image levelIcon;
    public TMP_Text levelDays;
    public TMP_Text spawnPoints;
    public TMP_Text bestEXP;
    public TMP_Text enemyCount;
    public List<OverworldLevelButton> levelButtons;
    public StartLevelButton startLevelButton;

    private void Start()
    {
        mapModel.DOLocalMoveY(9,0);
    }

    public void ToggleInMap(bool _show)
    {
        ParticlesX.ToggleParticles(mapParticles, !_show);
        mapLight.DOIntensity(_show ? 200 : 2000, mapTweenTime).SetEase(mapEase);
        mapModel.DOLocalMoveY(_show ? 16 : 9, mapTweenTime).SetEase(mapEase);
    }

    public void ShowLevel(LevelID _levelID)
    {
        LevelData levelData = _DATA.GetLevel(_levelID);
        startLevelButton.SetInteractable(_DATA.levelUnlocked(_levelID));

        if (_DATA.levelUnlocked(_levelID))
        {
            levelTitle.text = levelData.name;
            levelDescription.text = levelData.description;
            levelIcon.sprite = levelData.icon;
            levelDays.text = levelData.days.ToString();
            spawnPoints.text = levelData.spawnPoints.ToString();
            enemyCount.text = levelData.availableHunters.Count.ToString();
            selectedLevel = _levelID;
        }
        else
        {
            levelTitle.text = levelData.name;
            levelDescription.text = "Locked";// levelData.description;
            levelIcon.sprite = levelData.icon;
            levelDays.text = levelData.days.ToString();
            spawnPoints.text = levelData.spawnPoints.ToString();
            enemyCount.text = levelData.availableHunters.Count.ToString();
            selectedLevel = _levelID;
        }


        for(int i=0;i<levelButtons.Count;i++)
        {
            if(levelButtons[i].thisLevel == _levelID)
            {
                if (_DATA.levelUnlocked(levelButtons[i].thisLevel))
                {
                    levelButtons[i].TweenScale(true);
                    levelButtons[i].TweenColor(_COLOUR.mapSelectedColor);
                }
                else
                {
                    levelButtons[i].TweenColor(_COLOUR.cooldownColor);
                }
            }
            else
            {
                levelButtons[i].TweenScale(false);
                if (_DATA.levelUnlocked(levelButtons[i].thisLevel))
                    levelButtons[i].TweenColor(_COLOUR.mapUnlockedColor);
                else
                    levelButtons[i].TweenColor(_COLOUR.mapLockedColor);
            }
        }
    }
}
