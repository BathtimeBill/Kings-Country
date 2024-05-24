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

        if (Input.GetKeyDown(KeyCode.L))
            _FM.WildlifeInstantiate();
    }
}
