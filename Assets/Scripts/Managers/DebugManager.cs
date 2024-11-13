using UnityEngine;

public class DebugManager : GameBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            GameEvents.ReportOnDayOver(_currentDay);

        if(Input.GetKeyDown(KeyCode.K))
            StartCoroutine(_EM.KillAllEnemies());

        if (Input.GetKeyDown(KeyCode.L))
            _GM.WildlifeInstantiate();

        if (Input.GetKeyDown(KeyCode.M))
            _GM.IncreaseMaegen(6);

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (FindObjectOfType<ExperienceMeter>())
                FindObjectOfType<ExperienceMeter>().IncreaseExperience(100);
        }
    }
}
