using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreePlacement : Singleton<TreePlacement>
{
    public bool canPlace = true;
    public bool tooFarAway = false;
    public bool insufficientMaegen = false;
    public Material canPlaceMat;
    public Material cannotPlaceMat;
    public GameObject percentagePanel;
    
    [Header("Tree Stats")]
    public float distanceFromTree;
    public float energyMultiplier;
    private float resultNumber;
    public int maegenPerWave;
    public int maegenCost;
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI resultText;



    void Start()
    {
        gameObject.GetComponent<Renderer>().material = canPlaceMat;
        canPlace = true;
    }

    void Update()
    {
        if(_GM.trees.Count != 0)
        {
            distanceFromTree = Vector3.Distance(GetClosestTree().transform.position, transform.position);
        }
        else
        {
            distanceFromTree = 67;
        }

        percentagePanel.transform.position = Input.mousePosition;
        energyMultiplier = 1f / 100 * distanceFromTree;
        resultNumber = 5 / energyMultiplier;
        if(distanceFromTree > 0 && distanceFromTree < 33)
        {
            maegenPerWave = 1;
            _UI.maegenCostText.text = "2";
        }
        if (distanceFromTree > 32 && distanceFromTree < 66)
        {
            maegenPerWave = 2;
            _UI.maegenCostText.text = "3";
        }
        if (distanceFromTree > 65 && distanceFromTree <= 100)
        {
            maegenPerWave = 3;
            _UI.maegenCostText.text = "4";
        }
        maegenCost = maegenPerWave + 1;
        percentageText.text = energyMultiplier.ToString("0.0" + "%");
        resultText.text = maegenPerWave.ToString() + " Maegen every wave.<br>Cost: " + maegenCost + " Maegen";

        if (distanceFromTree >= 100 && tooFarAway == false)
        {
            tooFarAway = true;
            canPlace = false;
            gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
        }
        if (distanceFromTree < 99 && tooFarAway == true)
        {
            if(!insufficientMaegen)
            {
                tooFarAway = false;
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;

            }
        }

        if(maegenPerWave <= 1)
        {
            if (_GM.maegen < 2 && insufficientMaegen == false)
            {
                insufficientMaegen = true;
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 2 && insufficientMaegen == true)
            {
                insufficientMaegen = false;
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (maegenPerWave == 2)
        {
            if (_GM.maegen < 3 && insufficientMaegen == false)
            {
                insufficientMaegen = true;
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 3 && insufficientMaegen == true)
            {
                insufficientMaegen = false;
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
        if (maegenPerWave == 3)
        {
            if (_GM.maegen < 4 && insufficientMaegen == false)
            {
                insufficientMaegen = true;
                canPlace = false;
                gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
            }
            if (_GM.maegen >= 4 && insufficientMaegen == true)
            {
                insufficientMaegen = false;
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tree" || other.tag == "CantPlace" || other.tag == "River" || other.tag == "Wildlife")
        {
            Debug.Log(other.tag);
            canPlace = false;
            gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
        }
        if (other.gameObject == null)
        {
            canPlace = true;
            gameObject.GetComponent<Renderer>().material = canPlaceMat;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tree" || other.tag == "CantPlace" || other.tag == "River" || other.tag == "Home Tree" || other.tag == "Wildlife")
        {
            if(!tooFarAway && !insufficientMaegen)
            {
                canPlace = true;
                gameObject.GetComponent<Renderer>().material = canPlaceMat;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Tree" || other.tag == "CantPlace" || other.tag == "River")
        {
            canPlace = false;
            gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
        }
        if (other.gameObject == null)
        {
            canPlace = true;
            gameObject.GetComponent<Renderer>().material = canPlaceMat;
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
