using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapRotation : GameBehaviour
{
    public GameObject playerCam;
    public float targetYRotation;
    public Quaternion targetRotation;


    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
    }

    private void Update()
    {
        targetRotation = Quaternion.Euler(90, playerCam.transform.localEulerAngles.y, 0);
        transform.rotation = targetRotation;
    }
}
