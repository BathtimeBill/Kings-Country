using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : GameBehaviour
{
    public bool isHut;

    private void Start()
    {
        if(isHut)
        {
            _HUTM.spawnLocations.Add(gameObject);
        }
        else
        {
            _HM.spawnLocations.Add(gameObject);
        }
    }

}
