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
    private SeasonID selectedSeason;

    [Header("UI")]
    public TMP_Text levelTitle;
    public TMP_Text levelDescription;
    public Image levelIcon;
    public TMP_Text seasonUnlockCondition;
    [Header("Level Stats")]
    public TMP_Text levelDays;
    public TMP_Text spawnPoints;
    public TMP_Text bestEXP;
    public List<OverworldLevelButton> levelButtons;
    public StartLevelButton startLevelButton;
    [BV.EnumList(typeof(SeasonID))]
    public SeasonToggle[] seasonToggles;
    public ToggleGroup toggleGroup;

    [Header("Level Icons")]
    public BuildingLevelIcons buildingLevelIcons;
    public HumanLevelIcons humanLevelIcons;
    public Image[] levelStars;

    private LevelData levelData = null;

    private void Start()
    {
        mapModel.DOLocalMoveY(9,0);
        ShowNoLevel();
    }

    public void ToggleInMap(bool _show)
    {
        ParticlesX.ToggleParticles(mapParticles, !_show);
        mapLight.DOIntensity(_show ? 200 : 2000, mapTweenTime).SetEase(mapEase);
        mapModel.DOLocalMoveY(_show ? 16 : 9, mapTweenTime).SetEase(mapEase);
        if(!_show)
            ShowNoLevel();
    }

    public void ShowLevel(LevelID _levelID)
    {
        levelData = _DATA.GetLevel(_levelID);

        //UI Stuff
        if (_DATA.levelUnlocked(_levelID))
        {
            levelTitle.text = levelData.name;
            levelDescription.text = levelData.description;
            levelIcon.sprite = levelData.icon;
            levelDays.text = levelData.days.ToString();
            spawnPoints.text = levelData.spawnPoints.ToString();
            selectedLevel = _levelID;
        }
        else //Leaving in in case we want to hide the values
        {
            levelTitle.text = levelData.name;
            levelDescription.text = levelData.description;
            levelIcon.sprite = levelData.icon;
            levelDays.text = levelData.days.ToString();
            spawnPoints.text = levelData.spawnPoints.ToString();
            selectedLevel = _levelID;
        }

        startLevelButton.SetInteractable(_DATA.levelUnlocked(_levelID));
        ShowIcons(levelData);
        SetSeasons(levelData);

        //3D Map Pieces
        for (int i=0;i<levelButtons.Count;i++)
        {
            if(levelButtons[i].thisLevel == _levelID)
            {
                levelButtons[i].SetSelected(true);
                if (_DATA.levelUnlocked(levelButtons[i].thisLevel))
                {
                    levelButtons[i].TweenScale(true);
                    levelButtons[i].TweenColor(_COLOUR.mapUnlockedSelectedColor);
                }
                else
                {
                    levelButtons[i].TweenColor(_COLOUR.mapLockedSelectedColor);
                }
            }
            else
            {
                levelButtons[i].SetSelected(false);
                levelButtons[i].TweenScale(false);
                if (_DATA.levelUnlocked(levelButtons[i].thisLevel))
                    levelButtons[i].TweenColor(_COLOUR.mapUnlockedColor);
                else
                    levelButtons[i].TweenColor(_COLOUR.mapLockedColor);
            }
        }
    }

    private void ShowNoLevel()
    {
        selectedLevel = LevelID.None;
        levelData = _DATA.GetLevel(selectedLevel);
        startLevelButton.SetInteractable(false);
        levelTitle.text = levelData.name;
        levelDescription.text = levelData.description;
        levelIcon.sprite = levelData.icon;
        levelDays.text = levelData.days.ToString();
        spawnPoints.text = levelData.spawnPoints.ToString();
        ShowIcons(levelData);
        SeasonHover(SeasonID.Spring, false);

        for (int i = 0; i < seasonToggles.Length; i++)
            seasonToggles[i].SetInteractable(false);
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
        humanLevelIcons.hunterIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Poacher) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.bjornjegerIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Bjornjeger) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.drengIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Dreng) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.bezerkrIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Berserkr) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.knightIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Knight) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.lordIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Lord) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.dogIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Dog) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);
        humanLevelIcons.spyIcon.DOColor(_DATA.LevelContains(_levelData.id, HumanID.Spy) ? _COLOUR.levelHasColor : _COLOUR.levelHasNotColor, levelIconsTweenTime);

        ShowStars(_levelData);
    }

    private void ShowStars(LevelData _levelData)
    {
        int amount = (int)_levelData.difficultyRating;
        for(int i=0;i<levelStars.Length;i++)
        {
            if(i <= amount)
                levelStars[i].DOColor(_COLOUR.toggleIconHighlightColor, levelIconsTweenTime);
            else
                levelStars[i].DOColor(Color.black, levelIconsTweenTime);
        }

    }

    public void ChangeSeason(SeasonID _seasonID)
    {
        selectedSeason = _seasonID;
        levelTitle.text = levelData.name + " - " + _seasonID.ToString();
    }

    public void SetSeasons(LevelData _levelData)
    {
        for (int i = 0; i < seasonToggles.Length; i++)
            seasonToggles[i].SetInteractable(false);

        if (_levelData.unlocked)
        {
            int seasonCount = _levelData.unlockedSeasons.Count-1;
            for (int i = 0; i < seasonToggles.Length; i++)
            {
                seasonToggles[i].SetInteractable(i <= seasonCount);
            }
            //TODO ??? This section is a bit of a mess but works for now
            toggleGroup.SetAllTogglesOff();
            IList<Toggle> toggles = toggleGroup.GetToggles();
            for(int i=0; i < toggles.Count; i++)
            {
                if (toggles[i].name.Contains("Spring"))
                    toggles[i].isOn = true;
            }
            ChangeSeason(SeasonID.Spring);
        }
    }

    public void SeasonHover(SeasonID _seasonID, bool _show)
    {
        seasonUnlockCondition.text = "";

        if (!_show)
            return;

        if (levelData.unlockedSeasons.Contains(_seasonID) && levelData.unlocked)
        {
            seasonUnlockCondition.text = "Select " + _seasonID.ToString() + "?";
        }
        else
        {
            //seasonUnlockCondition.text = "Unlock at EXP lvl 21 (Grove Master)";
            seasonUnlockCondition.text = "Not available in this demo.";
        }
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
