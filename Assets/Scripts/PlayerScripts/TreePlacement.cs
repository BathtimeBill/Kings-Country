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
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI resultText;



    void Start()
    {
        gameObject.GetComponent<Renderer>().material = canPlaceMat;
        canPlace = true;
    }

    void Update()
    {
        //percentagePanel.transform.position = Input.mousePosition;
        //resultNumber = 5 / energyMultiplier;
        //distanceFromTree = Vector3.Distance(GetClosestTree().transform.position, transform.position);
        //energyMultiplier = 1f / 100 * distanceFromTree;
        //percentageText.text = energyMultiplier.ToString("0.0" + "%");
        //resultText.text = "1 Maegen every " + resultNumber.ToString("0.0") + " seconds";

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

        if(_GM.maegen <1 && insufficientMaegen == false)
        {
            insufficientMaegen = true;
            canPlace = false;
            gameObject.GetComponent<Renderer>().material = cannotPlaceMat;
        }
        if (_GM.maegen >= 1 && insufficientMaegen == true)
        {
            insufficientMaegen = false;
            canPlace = true;
            gameObject.GetComponent<Renderer>().material = canPlaceMat;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tree" || other.tag == "CantPlace" || other.tag == "River")
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
        if (other.tag == "Tree" || other.tag == "CantPlace" || other.tag == "River")
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
