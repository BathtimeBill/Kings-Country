using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.ProBuilder.Shapes;

public class InfoBox : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TMP_Text description;
    public TMP_Text stats;
    public string defaultText = "";

    private void Start()
    {
        OnButtonExit();
    }

    public void OnButtonHover(string _description, string _stats)
    {
        description.text = _description;
        stats.text = _stats;
    }

    public void OnButtonExit()
    {
        description.text = defaultText;
        stats.text = "";
    }
}
