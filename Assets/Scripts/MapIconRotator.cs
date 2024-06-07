using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapIconRotator : MonoBehaviour
{
    public GameObject playerCam;
    public float targetYRotation;
    public Quaternion targetRotation;
    public float targetScale;


    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
        targetScale = transform.localScale.z - transform.localScale.z*2;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, targetScale);
    }

    private void Update()
    {
        targetRotation = Quaternion.Euler(0, playerCam.transform.localEulerAngles.y, 0);
        transform.rotation = targetRotation;
    }

}
