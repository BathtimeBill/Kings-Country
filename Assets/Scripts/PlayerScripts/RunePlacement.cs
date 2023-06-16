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
        if(_GM.runes.Length == 0)
        {
            _UI.maegenCostText.text = "2";
            _UI.wildlifeCostText.text = "2";
            if (_GM.maegen < 2 || _GM.wildlife < 2)
            {
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 2 && _GM.wildlife >= 2)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if(_GM.runes.Length == 1)
        {
            _UI.maegenCostText.text = "4";
            _UI.wildlifeCostText.text = "3";
            if (_GM.maegen < 4 || _GM.wildlife < 3)
            {
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 4 && _GM.wildlife >= 3)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (_GM.runes.Length == 2)
        {
            _UI.maegenCostText.text = "8";
            _UI.wildlifeCostText.text = "4";
            if (_GM.maegen < 8 || _GM.wildlife < 4)
            {
                
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 8 && _GM.wildlife >= 4)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (_GM.runes.Length == 3)
        {
            _UI.maegenCostText.text = "16";
            _UI.wildlifeCostText.text = "5";
            if (_GM.maegen < 16 || _GM.wildlife < 5)
            {
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 16 && _GM.wildlife >= 5)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (_GM.runes.Length > 3)
        {
            canPlace = false;
        }
    }

    private void OnTriggerEnter(Collider other)
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
    private void OnTriggerExit(Collider other)
    {
        //if (other.tag == "Tree" || other.tag == "CantPlace")
        //{
        //    if (!tooFarAway && !insufficientMaegen)
        //    {
        //        canPlace = true;
        //        gameObject.GetComponent<Renderer>().material = canPlaceMat;
        //    }
        //}
    }

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
