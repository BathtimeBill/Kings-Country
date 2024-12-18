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

    public BV.Range yZoom;
    public BV.Range zZoom;
    public BV.Range xMovement;
    public BV.Range zMovement;

    public LayerMask groundLayer;

    private Vector3 startZoom;
    private Vector3 startPos;
    private Quaternion startRot;
    private bool lockCamera = false;
    public Vector3 siteOffset = new Vector3(0, 0, 5);

    private void Start()
    {
        startPos = newPosition = transform.position;
        startRot = newRotation = transform.rotation;
        startZoom = newZoom = cameraTransform.localPosition;
        movementModifier = normalSpeed;
    }

    private void Update()
    {
        if (lockCamera || !_HasInput)
            return;

        HandleLerps();
        HandleMouseInput();
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
        newZoom.z = Mathf.Clamp(newZoom.z, zZoom.min, zZoom.max);
        newZoom.y = Mathf.Clamp(newZoom.y, yZoom.min, yZoom.max);

        if (_zoom != 0)
        {
            //TODO When change to cinemachine, fix so no go though objects
            //RaycastHit hit;
            //if(Physics.Raycast(cameraTransform.transform.position, Vector3.down, out hit, minY, groundLayer))
            //{
            //    return;
            //}

            if (newZoom.y != yZoom.min || newZoom.y != yZoom.max)
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
        newPosition.x = Mathf.Clamp(newPosition.x, xMovement.min, xMovement.max);
        newPosition.z = Mathf.Clamp(newPosition.z, zMovement.min, zMovement.max);
        float posX = _cursorPosition.x;
        float posY = _cursorPosition.y;
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        //print("posX: " + posX + " | posY: " + posY + " | mouseX: " + mouseX + " | mouseY: " + mouseY);

        if (posY > 0 || (_SAVE.GetCameraEdgeScrolling && mouseY > Screen.height - edgeScrollThreshold))
            newPosition += transform.forward * _PLAYER.movementSpeed * movementModifier;
        if (posY < 0 || (_SAVE.GetCameraEdgeScrolling && mouseY < edgeScrollThreshold))
            newPosition -= transform.forward * _PLAYER.movementSpeed * movementModifier;
        if (posX > 0 || (_SAVE.GetCameraEdgeScrolling && mouseX > Screen.width - edgeScrollThreshold))
            newPosition += transform.right * _PLAYER.movementSpeed * movementModifier;
        if (posX < 0 || (_SAVE.GetCameraEdgeScrolling && mouseX < edgeScrollThreshold))
            newPosition -= transform.right * _PLAYER.movementSpeed * movementModifier;

        _TUTORIAL.CheckCameraTutorial(TutorialID.CameraMove);
    }

    private void OnCameraHaste(bool _held)
    {
        movementModifier = _held ? fastSpeed : normalSpeed;
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonUp(2))
        {
            _TUTORIAL.CheckCameraTutorial(TutorialID.CameraRotate);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_PC.mouseOverMap == true)
            {
                print("Map over");
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
        newPosition = startPos;
        newRotation = startRot;
        newZoom = startZoom;
        transform.DOLocalMove(newPosition, resetTime).SetEase(Ease.OutSine);
        transform.DOLocalRotateQuaternion(newRotation, resetTime).SetEase(Ease.OutSine);
        cameraTransform.DOLocalMove(newZoom, resetTime).SetEase(Ease.OutSine);
        //ExecuteAfterSeconds(resetTime + 0.1f, () => lockCamera = false);
    }

    public void TweenCameraPosition(Vector3 _position, float _speed)
    {
        newPosition = _position;
        newZoom = startZoom;
        transform.DOLocalMove(_position - siteOffset, _speed).SetEase(Ease.OutSine);
        cameraTransform.DOLocalMove(newZoom, _speed).SetEase(Ease.OutSine);
    }

    public void LockCamera(bool _lock) => lockCamera = _lock;

    private void OnSiteSelected(SiteID _siteID, bool _active)
    {
        if (!_active)
            return;
        
        switch (_siteID)
        {
            case SiteID.HomeTree:
                TweenCameraPosition(_GM.homeTree.transform.position, _TWEENING.focusTweenTime);
                break;
            case SiteID.Hut:
                TweenCameraPosition(_GM.hut.transform.position, _TWEENING.focusTweenTime);
                break;
            case SiteID.Horgr:
                TweenCameraPosition(_GM.horgr.transform.position, _TWEENING.focusTweenTime);
                break;
            case SiteID.Unknown:
                break;
            case SiteID.Unknown2:
                break;
        }
    }
    
    private void OnEnable()
    {
        InputManager.OnCameraMove += OnCameraMove;
        InputManager.OnCameraZoom += OnCameraZoom;
        InputManager.OnCameraRotate += OnCameraRotate;
        InputManager.OnCameraHaste += OnCameraHaste;
        
        GameEvents.OnSiteSelected += OnSiteSelected;
    }

    private void OnDisable()
    {
        InputManager.OnCameraMove -= OnCameraMove;
        InputManager.OnCameraZoom -= OnCameraZoom;
        InputManager.OnCameraRotate -= OnCameraRotate;
        InputManager.OnCameraHaste -= OnCameraHaste;
        
        GameEvents.OnSiteSelected -= OnSiteSelected;
    }
}
