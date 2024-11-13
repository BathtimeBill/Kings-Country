using UnityEngine;

public class DebugManager : GameBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameEvents.ReportOnWaveOver();
            //GameEvents.ReportOnGameWin();
        }

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
