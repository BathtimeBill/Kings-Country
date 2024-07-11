using UnityEngine;

public class MiniMapRotation : GameBehaviour
{
    private GameObject playerCam;
    private Quaternion targetRotation;
    private Quaternion defaultRotation;

    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
        defaultRotation = transform.rotation;
    }

    private void Update()
    {
        if (!_PLAYER.miniMapRotation)
        {
            transform.rotation = defaultRotation;
            return;
        }


        targetRotation = Quaternion.Euler(90, playerCam.transform.localEulerAngles.y, 0);
        transform.rotation = targetRotation;
    }
}
