using UnityEngine;

public class MiniMapRotation : GameBehaviour
{
    private GameObject playerCam;
    private Quaternion targetRotation;


    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam");
    }

    private void Update()
    {
        if (!_PLAYER.miniMapRotation)
            return;

        targetRotation = Quaternion.Euler(90, playerCam.transform.localEulerAngles.y, 0);
        transform.rotation = targetRotation;
    }
}
