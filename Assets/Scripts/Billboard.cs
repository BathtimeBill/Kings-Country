using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : GameBehaviour
{
    //Orients the object to be facing the main camera.
    private Camera mainCam;
    private void Awake()
    {
        mainCam = Camera.main;
    }
    void Update()
    {
        transform.LookAt(mainCam.transform.position);
    }
}
