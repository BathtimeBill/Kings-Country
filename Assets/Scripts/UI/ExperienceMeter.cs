using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ExperienceMeter : GameBehaviour
{
    public TMP_Text titleText;
    public TMP_Text currentLevelText;
    public TMP_Text currentEXPText;
    public TMP_Text nextEXPText;
    public Image progressFill;
    //public Settings settings;

    private ExperienceObject experience;
    private int currentLevel;
    private float currentEXP;
    private int nextEXP;
    private bool leveledUp = false;

    //public Experience _EX(int i) => settings.experience.Find(x => x.level == i);
    private bool atMaxLevel => currentLevel == _SETTINGS.experience.Count;

    private string CurrentTitle => _EXP(currentLevel).title;
    private int CurrentRequirement => _EXP(currentLevel).requirement;

    private int NextRequirement => !atMaxLevel ? _EXP(currentLevel + 1).requirement : CurrentRequirement;
    private int EXPLimit => _EXP(_SETTINGS.experience.Count).requirement;

    public void IncreaseLevel() => experience.currentLevel++;

    void Start()
    {
        ExecuteNextFrame(() =>
        {
            experience = _SAVE.GetExperience();
            currentLevel = experience.currentLevel;
            currentEXP = experience.currentEXP;
            nextEXP = NextRequirement;
            SetStartValues();
        });
    }

    private void SetStartValues()
    {
        currentLevelText.text = currentLevel.ToString();
        currentEXPText.text = currentEXP.ToString();
        nextEXPText.text = nextEXP.ToString();
        titleText.text = CurrentTitle;
        progressFill.fillAmount = atMaxLevel ? 1 : MathX.MapTo01(currentEXP, _EXP(currentLevel).requirement, nextEXP);
    }
    
    public void IncreaseExperience(int _amount)
    {
        if (atMaxLevel)
            return;

        int newEXP = (int)currentEXP + _amount <= EXPLimit ? (int)currentEXP + _amount : EXPLimit;
        

        DOTween.To(() => currentEXP, x => currentEXP = x, newEXP, _TWEENING.experienceDuration).SetEase(_TWEENING.experienceEase).OnUpdate(()=>
        {
            if (atMaxLevel)
                return;

            currentEXPText.text = currentEXP.ToString("F0");
            progressFill.fillAmount = MathX.MapTo01(currentEXP, _EXP(currentLevel).requirement, nextEXP);

            if (currentEXP >= nextEXP && !leveledUp)
                UpdateLevel();
        });
    }

    private void UpdateLevel()
    {
        if (atMaxLevel)
            return;

        leveledUp = true;
        currentLevel++;
        nextEXP = NextRequirement;
        nextEXPText.text = nextEXP.ToString();
        progressFill.fillAmount = atMaxLevel ? 1 : 0;
        currentLevelText.alpha = 0;
        currentLevelText.transform.localScale = Vector3.one * 3;
        currentLevelText.text = currentLevel.ToString();
        currentLevelText.DOFade(1, _TWEENING.UITweenTime / 2);
        currentLevelText.transform.DOScale(1, _TWEENING.UITweenTime).SetEase(_TWEENING.experienceEase);
        
        titleText.alpha = 0;
        titleText.transform.localScale = Vector3.one * 3;
        titleText.text = CurrentTitle;
        titleText.DOFade(1, _TWEENING.UITweenTime / 2);
        titleText.transform.DOScale(1, _TWEENING.UITweenTime).SetEase(_TWEENING.experienceEase).SetDelay(_TWEENING.UITweenTime);
        
        experience.currentLevel = currentLevel;
        experience.currentEXP = (int)currentEXP;
        //_SAVE.SetExperience(experience);
        leveledUp = false;
    }
}
