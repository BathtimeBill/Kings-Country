using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerDecor : MonoBehaviour
{
    // Randomises the rotation of the flower's Y axis.
    void Start()
    {
        int rndRotation;
        rndRotation = Random.Range(0, 360);
        transform.rotation = Quaternion.Euler(0, rndRotation, 0);
    }
}
