using UnityEngine;

public class MapIconRotator : GameBehaviour
{
    private GameObject playerCam;
    private Quaternion targetRotation;
    private float targetScale;


    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
        targetScale = transform.localScale.z - transform.localScale.z*2;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, targetScale);
    }

    private void Update()
    {
        if (!_PS.miniMapRotation)
            return;

        targetRotation = Quaternion.Euler(0, playerCam.transform.localEulerAngles.y, 0);
        transform.rotation = targetRotation;
    }

}
