using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSync : MonoBehaviour
{
    public Camera syncCamera;
    Camera thisCamera;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        thisCamera.fieldOfView = syncCamera.fieldOfView;
    }
}
