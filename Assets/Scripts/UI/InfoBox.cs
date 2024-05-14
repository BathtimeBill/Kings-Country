using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.ProBuilder.Shapes;

public class InfoBox : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TMP_Text description;
    public TMP_Text healthStat;
    public TMP_Text damageStat;
    public TMP_Text speedStat;
    public string defaultText = "";

    private void Start()
    {
        OnButtonExit();
    }

    public void OnButtonHover(string _description, Stats _stats)
    {
        description.text = _description;
        healthStat.text = _stats.health.ToString();
        damageStat.text = _stats.damage.ToString();
        speedStat.text = _stats.speed.ToString();
    }

    public void OnButtonExit()
    {
        description.text = defaultText;
        healthStat.text = "-";
        damageStat.text = "-";
        speedStat.text = "-";
    }
}
