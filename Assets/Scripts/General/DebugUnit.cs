using UnityEngine;

public class DebugUnit : MonoBehaviour
{
    public Unit unit;

    public Transform stopRadius;
    public Transform detectRadius;
    public Transform attackRadius;
    
    public void Setup()
    {
        //stopRadius.transform.localScale = unit.s
        float detect = unit.unitData.detectionRadius;
        detectRadius.transform.localScale = new Vector3(detect, detect, detect);

    }


}
