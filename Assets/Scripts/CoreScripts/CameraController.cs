using UnityEngine;
using DG.Tweening;

public class CameraController : Singleton<CameraController>
{
    public Transform cameraTransform;
    [ReadOnly] public float movementModifier = 1;  //Maybe remove. Need to get consistent speeds
    public float normalSpeed;
    public float fastSpeed;
    public float movementTime;
    public float edgeScrollThreshold = 20f;

    public Vector3 zoomAmount;
    [ReadOnly] public Vector3 newPosition;
    [ReadOnly] public Vector3 newZoom;
    [ReadOnly] public Quaternion newRotation;

    public float minZ;
    public float maxZ;
    public float minY;
    public float maxY;

    public float minMovementX;
    public float maxMovementX;
    public float minMovementZ;
    public float maxMovementZ;

    public LayerMask groundLayer;

    private Vector3 startZoom;
    private Vector3 startPos;
    private Quaternion startRot;
    private bool lockCamera = false;

    private void Start()
    {
        startPos = newPosition = transform.position;
        startRot = newRotation = transform.rotation;
        startZoom = newZoom = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (lockCamera || !_hasInput)
            return;

        HandleLerps();
        HandleMouseInputNew();
    }

    void HandleLerps()
    {
        //Panning
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        //Rotating
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        //Zooming
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }

    private void OnCameraZoom(float _zoom)
    {
        newZoom.z = Mathf.Clamp(newZoom.z, minZ, maxZ);
        newZoom.y = Mathf.Clamp(newZoom.y, minY, maxY);

        if (_zoom != 0)
        {
            //TODO When change to cinemachine, fix so no go though objects
            //RaycastHit hit;
            //if(Physics.Raycast(cameraTransform.transform.position, Vector3.down, out hit, minY, groundLayer))
            //{
            //    return;
            //}

            if (newZoom.y != minY || newZoom.y != maxY)
                newZoom += (_zoom * _PLAYER.zoomSpeed) * zoomAmount;
            _TUTORIAL.CheckCameraTutorial(TutorialID.CameraZoom);
        }
    }

    private void OnCameraRotate(Vector2 _rotate)
    {
        newRotation *= Quaternion.Euler(Vector3.up * _rotate.x * _PLAYER.rotationSpeed);
    }

    private void OnCameraMove(Vector2 _cursorPosition)
    {
        newPosition.x = Mathf.Clamp(newPosition.x, minMovementX, maxMovementX);
        newPosition.z = Mathf.Clamp(newPosition.z, minMovementZ, maxMovementZ);
        float posX = _cursorPosition.x;
        float posY = _cursorPosition.y;
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        //print("posX: " + posX + " | posY: " + posY + " | mouseX: " + mouseX + " | mouseY: " + mouseY);

        if (posY > 0 || mouseY > Screen.height - edgeScrollThreshold)
            newPosition += transform.forward * _PLAYER.movementSpeed * movementModifier;
        if (posY < 0 || mouseY < edgeScrollThreshold)
            newPosition -= transform.forward * _PLAYER.movementSpeed * movementModifier;
        if (posX > 0 || mouseX > Screen.width - edgeScrollThreshold)
            newPosition += transform.right * _PLAYER.movementSpeed * movementModifier;
        if (posX < 0 || mouseX < edgeScrollThreshold)
            newPosition -= transform.right * _PLAYER.movementSpeed * movementModifier;

        _TUTORIAL.CheckCameraTutorial(TutorialID.CameraMove);
    }

    private void OnCameraHaste(bool _held)
    {
        movementModifier = _held ? fastSpeed : normalSpeed;
    }

    void HandleMouseInputNew()
    {
        if (Input.GetMouseButtonUp(2))
        {
            _TUTORIAL.CheckCameraTutorial(TutorialID.CameraRotate);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_PC.mouseOverMap == true)
            {
                Ray ray = _PC.mapCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitPoint;
                if (Physics.Raycast(ray, out hitPoint))
                    newPosition = hitPoint.point;
            }
        }
    }

    public void CameraShake(float _shakeIntensity)
    {
        transform.DOShakeRotation(1.5f, new Vector3(_shakeIntensity, _shakeIntensity, _shakeIntensity), _SETTINGS.cameraShake.shakeVibrato, _SETTINGS.cameraShake.shakeRandomness, true);
    }

    public void ResetCameraToStart(bool _reset)
    {
        if (!_reset)
            return;

        float resetTime = 2f;
        lockCamera = true;
        newRotation = startRot;
        newPosition = startPos;
        newZoom = startZoom;
        transform.DOLocalMove(startPos, resetTime).SetEase(Ease.OutSine);
        cameraTransform.DOLocalMove(startZoom, resetTime).SetEase(Ease.OutSine);
        transform.DOLocalRotateQuaternion(startRot, resetTime).SetEase(Ease.OutSine);
        //ExecuteAfterSeconds(resetTime + 0.1f, () => lockCamera = false);
    }

    public void LockCamera(bool _lock) => lockCamera = _lock;

    private void OnEnable()
    {
        InputManager.OnCameraMove += OnCameraMove;
        InputManager.OnCameraZoom += OnCameraZoom;
        InputManager.OnCameraRotate += OnCameraRotate;
        InputManager.OnCameraHaste += OnCameraHaste;
    }

    private void OnDisable()
    {
        InputManager.OnCameraMove -= OnCameraMove;
        InputManager.OnCameraZoom -= OnCameraZoom;
        InputManager.OnCameraRotate -= OnCameraRotate;
        InputManager.OnCameraHaste -= OnCameraHaste;
    }
}
