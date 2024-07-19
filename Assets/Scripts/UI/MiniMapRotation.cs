using UnityEngine;

public class MiniMapRotation : GameBehaviour
{
    public Camera miniMapCamera;
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

    private void OnGameStateChanged(GameState _gameState)
    {
        miniMapCamera.enabled = _gameState == GameState.Play || _gameState == GameState.Build || _gameState == GameState.Tutorial;
    }

    private void OnEnable()
    {
        GameEvents.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
    }
}
