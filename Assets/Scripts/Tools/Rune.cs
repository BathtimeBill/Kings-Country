using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Rune : GameBehaviour
{
    //public float timeLeft;
    //public float maxTimeLeft;
    //public Slider slider;
    public bool hasUpgrade;
    public GameObject colliderObject;
    void Start()
    {
        //timeLeft = maxTimeLeft;
        //slider.value = CalculateTimeLeft();

        if(_UM.rune)
        {
            hasUpgrade = true;
        }
    }
    //void Update()
    //{

    //    timeLeft -= 1 * Time.deltaTime;
    //    slider.value = CalculateTimeLeft();

    //    if (timeLeft < 2)
    //    {
    //        colliderObject.GetComponent<SphereCollider>().radius = 0;
    //    }

    //    if (timeLeft <= 0)
    //    {
    //        GameEvents.ReportOnRuneDestroyed();
    //        Destroy(gameObject);
    //    }

    //}
    //float CalculateTimeLeft()
    //{
    //    return timeLeft / maxTimeLeft;
    //}

    public void OnWaveOver()
    {
        if(!hasUpgrade)
        {
            Destroy(gameObject);
        }
        else
        {
            hasUpgrade = false;
        }

    }
    public void OnRuneUpgrade()
    {
        hasUpgrade = true;
    }

    private void OnEnable()
    {
        GameEvents.OnWaveOver += OnWaveOver;
        GameEvents.OnRuneUpgrade += OnRuneUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnWaveOver -= OnWaveOver;
        GameEvents.OnRuneUpgrade -= OnRuneUpgrade;
    }
}
