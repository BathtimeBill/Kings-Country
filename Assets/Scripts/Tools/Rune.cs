using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Rune : GameBehaviour
{
    public float timeLeft;
    public float maxTimeLeft;
    public Slider slider;
    public GameObject colliderObject;
    void Start()
    {
        timeLeft = maxTimeLeft;
        slider.value = CalculateTimeLeft();
    }
    void Update()
    {

        timeLeft -= 1 * Time.deltaTime;
        slider.value = CalculateTimeLeft();

        if(timeLeft < 2)
        {
            colliderObject.GetComponent<SphereCollider>().radius = 0;
        }

        if (timeLeft <= 0)
        {
            GameEvents.ReportOnRuneDestroyed();
            Destroy(gameObject);
        }

    }
    float CalculateTimeLeft()
    {
        return timeLeft / maxTimeLeft;
    }
}
