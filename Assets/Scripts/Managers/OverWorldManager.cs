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
    private float levelIconsTweenTime = 0.3f;
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
    public List<OverworldLevelButton> levelButtons;
    public StartLevelButton startLevelButton;

    [Header("Level Icons")]
    public BuildingLevelIcons buildingLevelIcons;
    public HumanLevelIcons humanLevelIcons;

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
        ShowIcons(levelData);

        if (_DATA.levelUnlocked(_levelID))
        {
            levelTitle.text = levelData.name;
            levelDescription.text = levelData.description;
            levelIcon.sprite = levelData.icon;
            levelDays.text = levelData.days.ToString();
            spawnPoints.text = levelData.spawnPoints.ToString();
            selectedLevel = _levelID;
        }
        else
        {
            levelTitle.text = levelData.name;
            levelDescription.text = "Locked";// levelData.description;
            levelIcon.sprite = levelData.icon;
            levelDays.text = levelData.days.ToString();
            spawnPoints.text = levelData.spawnPoints.ToString();
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

    private void ShowIcons(LevelData _levelData)
    {
        buildingLevelIcons.homeTreeIcon.DOColor(_DATA.LevelContains(_levelData.id, BuildingID.HomeTree) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        buildingLevelIcons.hutIcon.DOColor(_DATA.LevelContains(_levelData.id, BuildingID.Hut) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        buildingLevelIcons.hogyrIcon.DOColor(_DATA.LevelContains(_levelData.id, BuildingID.Hogyr) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        buildingLevelIcons.unknownIcon.DOColor(_DATA.LevelContains(_levelData.id, BuildingID.Unknown) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        buildingLevelIcons.unknown2Icon.DOColor(_DATA.LevelContains(_levelData.id, BuildingID.Unknown2) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);

        humanLevelIcons.loggerIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Logger) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.lumberjackIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Lumberjack) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.logCutterIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.LogCutter) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.watheIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Wathe) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.hunterIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Hunter) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.bjornjegerIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Bjornjeger) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.drengIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Dreng) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.bezerkrIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Berserkr) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.knightIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Knight) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.lordIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Lord) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.dogIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Dog) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.spyIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Spy) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
    }
}

[System.Serializable]
public class HumanLevelIcons
{
    public Image loggerIcon;
    public Image lumberjackIcon;
    public Image logCutterIcon;
    public Image watheIcon;
    public Image hunterIcon;
    public Image bjornjegerIcon;
    public Image drengIcon;
    public Image bezerkrIcon;
    public Image knightIcon;
    public Image lordIcon;
    public Image dogIcon;
    public Image spyIcon;
}

[System.Serializable]
public class BuildingLevelIcons
{
    public Image homeTreeIcon;
    public Image hutIcon;
    public Image hogyrIcon;
    public Image unknownIcon;
    public Image unknown2Icon;
}
