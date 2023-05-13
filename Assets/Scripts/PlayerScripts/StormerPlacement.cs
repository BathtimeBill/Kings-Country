using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormerPlacement : Singleton<StormerPlacement>
{
    public bool canPlace;
    public GameObject mesh;
    public bool insufficientMaegen = false;
    public Material canPlaceMat;
    public Material cannotPlaceMat;

    void Start()
    {
        canPlace = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_GM.wildlife >= 25 && _GM.maegen >= 1000 && _UI.stormerPlaced == false)
        {
            mesh.GetComponent<MeshRenderer>().material = canPlaceMat;
            canPlace = true;
        }
        else
        {
            mesh.GetComponent<MeshRenderer>().material = cannotPlaceMat;
            canPlace = false;
        }
    }
}
