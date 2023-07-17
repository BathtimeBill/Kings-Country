using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Beacon : GameBehaviour
{
    public float timeLeft;
    public float maxTimeLeft;
    public Slider slider;
    public GameObject explosion;
    void Start()
    {
        if(_UM.beacon)
        {
            maxTimeLeft = 30;
        }
        else
        {
            maxTimeLeft = 15;
        }
        timeLeft = maxTimeLeft;
        slider.value = CalculateTimeLeft();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= 1 * Time.deltaTime;
        slider.value = CalculateTimeLeft();

        if (timeLeft <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.Euler(-90, 0, 0));
            GameEvents.ReportOnBeaconDestroyed();
            Destroy(gameObject);
        }

    }
    float CalculateTimeLeft()
    {
        return timeLeft / maxTimeLeft;
    }
}
