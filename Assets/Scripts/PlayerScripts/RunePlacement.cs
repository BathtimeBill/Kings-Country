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
    [Header("Price for 1 Rune")]
    public int maegenCost1;
    public int wildlifeCost1;
    [Header("Price for 2 Runes")]
    public int maegenCost2;
    public int wildlifeCost2;
    [Header("Price for 3 Runes")]
    public int maegenCost3;
    public int wildlifeCost3;
    [Header("Price for 4 Runes")]
    public int maegenCost4;
    public int wildlifeCost4;


    void Start()
    {
        gameObject.GetComponent<Renderer>().material = canPlaceMat;
        canPlace = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_GM.runes.Count == 0)
        {
            _UI.maegenCostText.text = maegenCost1.ToString();
            _UI.wildlifeCostText.text = wildlifeCost1.ToString();
            if (_GM.maegen < maegenCost1 || _GM.wildlife < wildlifeCost1)
            {
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= maegenCost1 && _GM.wildlife >= wildlifeCost1)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (_GM.runes.Count == 1)
        {
            _UI.maegenCostText.text = maegenCost2.ToString();
            _UI.wildlifeCostText.text = wildlifeCost2.ToString();
            if (_GM.maegen < maegenCost2 || _GM.wildlife < wildlifeCost2)
            {
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= maegenCost2 && _GM.wildlife >= wildlifeCost2)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (_GM.runes.Count == 2)
        {
            _UI.maegenCostText.text = maegenCost3.ToString();
            _UI.wildlifeCostText.text = wildlifeCost3.ToString();
            if (_GM.maegen < maegenCost3 || _GM.wildlife < wildlifeCost3)
            {

                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= maegenCost3 && _GM.wildlife >= wildlifeCost3)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
                effectRadius.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (_GM.runes.Count == 3)
        {
            _UI.maegenCostText.text = maegenCost4.ToString();
            _UI.wildlifeCostText.text = wildlifeCost4.ToString();
            if (_GM.maegen < maegenCost4 || _GM.wildlife < wildlifeCost4)
            {
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
                effectRadius.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= maegenCost4 && _GM.wildlife >= wildlifeCost4)
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
