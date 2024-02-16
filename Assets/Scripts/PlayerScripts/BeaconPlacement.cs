using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconPlacement : Singleton<BeaconPlacement>
{
    public bool canPlace;

    public bool insufficientMaegen = false;
    public Material canPlaceMat;
    public Material cannotPlaceMat;
    public GameObject effectRadius;

    void Start()
    {
        canPlace = false;
        effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
    }

    void Update()
    {
        if(_UI.beaconPlaced)
        {
            canPlace = false;
            effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
        }
        if (_GM.beacons.Length < _UI.fyreCost && _UI.beaconPlaced == false)
        {
            if(_GM.wildlife >= _UI.fyreCost)
            {
                canPlace = true;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
            else
            {
                canPlace = false;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tree" || other.tag == "CantPlace" || other.tag == "River")
        {
            canPlace = false;
            effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
        }
        if (other.gameObject == null)
        {
            canPlace = true;
            effectRadius.GetComponent<Renderer>().material = canPlaceMat;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Tree" || other.tag == "CantPlace" || other.tag == "River")
        {
            canPlace = false;
            effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
        }
        if (other.gameObject == null)
        {
            canPlace = true;
            effectRadius.GetComponent<Renderer>().material = canPlaceMat;
        }
    }

    private void OnBeaconUpgrade()
    {
        effectRadius.transform.localScale = effectRadius.transform.localScale * 2;
    }

    private void OnEnable()
    {
        GameEvents.OnBeaconUpgrade += OnBeaconUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnBeaconUpgrade -= OnBeaconUpgrade;
    }
}
