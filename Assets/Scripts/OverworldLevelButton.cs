using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldLevelButton : GameBehaviour
{
    public GameObject selectionCircle;
    public GameObject levelCompleteImage;
    public GameObject lockImage;
    public float alphaThreshold = 0.1f;


    private void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = alphaThreshold;
    }

    public void MouseOverButton()
    {
        _Tool.SetAndShowTooltip("Wormturn Road, a forgotten expanse on the fringes of the King's realm, is a sparse, isolated region. Nestled between towering pines and twisted oaks, this grove teems with untold riches, drawing the attention of insatiable human loggers and hunters.", "Wormturn Road");
    }
    public void MouseOffButton()
    {
        _Tool.HideTooltip();
    }


}
