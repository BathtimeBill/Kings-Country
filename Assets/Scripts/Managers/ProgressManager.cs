using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressManager : GameBehaviour
{
    public TMP_Text currentMaegenText;
    public TMP_Text maegenChangeText;
    public TMP_Text totalDaysText;
    public TMP_Text totalLevelsComplete;
    public GameObject changeArrow;
    public ExperienceMeter experienceMeter;
    private int maegenCost = 0;
   
    void Start()
    {
        currentMaegenText.text = _SAVE.GetCurrentMaegen.ToString();
        totalDaysText.text = _SAVE.GetDaysPlayed.ToString();
        totalLevelsComplete.text = _SAVE.GetLevelCompleteCount().ToString() + "/21";
        ResetMaegenChange();
    }

    public void MaegenChange(int _newAmount)
    {
        maegenCost = _newAmount;
        currentMaegenText.text = _SAVE.GetCurrentMaegen.ToString();
        maegenChangeText.text = (_SAVE.GetCurrentMaegen - maegenCost).ToString();
        changeArrow.SetActive(true);
        changeArrow.GetComponent<Animator>().SetTrigger("Decrease");
    }

    public void ResetMaegenChange()
    {
        maegenChangeText.text = "";
        changeArrow.SetActive(false);
    }

    public void DecreaseCurrentMaegen()
    {
        _SAVE.DecrementMaegen(maegenCost);
        currentMaegenText.text = _SAVE.GetCurrentMaegen.ToString();
        ResetMaegenChange();
    }
}
