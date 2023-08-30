using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunePlacement : Singleton<RunePlacement>
{
    public bool canPlace;

    public bool insufficientMaegen = false;
    public Material canPlaceMat;
    public Material cannotPlaceMat;
    public GameObject effectRadius;


    void Start()
    {
        gameObject.GetComponent<Renderer>().material = canPlaceMat;
        canPlace = false;
    }

    void Update()
    {
        if(_GM.runes.Count == 0)
        {
            _UI.maegenCostText.text = "2";
            _UI.wildlifeCostText.text = "5";
            if (_GM.maegen < 2 || _GM.wildlife < 5)
            {
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 2 && _GM.wildlife >= 5)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if(_GM.runes.Count == 1)
        {
            _UI.maegenCostText.text = "4";
            _UI.wildlifeCostText.text = "7";
            if (_GM.maegen < 4 || _GM.wildlife < 7)
            {
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 4 && _GM.wildlife >= 7)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (_GM.runes.Count == 2)
        {
            _UI.maegenCostText.text = "8";
            _UI.wildlifeCostText.text = "10";
            if (_GM.maegen < 8 || _GM.wildlife < 10)
            {
                
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 8 && _GM.wildlife >= 10)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (_GM.runes.Count == 3)
        {
            _UI.maegenCostText.text = "16";
            _UI.wildlifeCostText.text = "15";
            if (_GM.maegen < 16 || _GM.wildlife < 15)
            {
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 16 && _GM.wildlife >= 15)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (_GM.runes.Count > 3)
        {
            canPlace = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Tree" || other.tag == "CantPlace" || other.tag == "River")
    //    {
    //        canPlace = false;
    //        gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
    //        effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
    //    }
    //    if (other.gameObject == null)
    //    {
    //        canPlace = true;
    //        gameObject.GetComponent<Renderer>().material = canPlaceMat;
    //        effectRadius.GetComponent<Renderer>().material = canPlaceMat;
    //    }

    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Tree" || other.tag == "CantPlace")
    //    {
    //        if (!tooFarAway && !insufficientMaegen)
    //        {
    //            canPlace = true;
    //            gameObject.GetComponent<Renderer>().material = canPlaceMat;
    //        }
    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Tree" || other.tag == "CantPlace" || other.tag == "River")
        {
            canPlace = false;
            gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
            effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
        }
        if (other.gameObject == null)
        {
            canPlace = true;
            gameObject.GetComponent<Renderer>().material = canPlaceMat;
            effectRadius.GetComponent<Renderer>().material = canPlaceMat;
        }
    }

    public Transform GetClosestTree()
    {
        float closestDistance = Mathf.Infinity;
        Transform trans = null;

        foreach (GameObject go in _GM.trees)
        {
            float currentDistance;
            currentDistance = Vector3.Distance(transform.position, go.transform.position);
            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                trans = go.transform;
            }
        }
        return trans;
    }
}
