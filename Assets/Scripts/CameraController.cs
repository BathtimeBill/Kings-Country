using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : Singleton<CameraController>
{
    public Transform cameraTransform;

    public float movementModifier = 1;  //Maybe remove. Need to get consistent speeds
    public float movementTime;
    public float normalSpeed;
    public float fastSpeed;
    public float edgeScrollThreshold = 20f;
    public bool isMouseOnEdge = false;
    private Vector3 scrollDirection;
    public Vector3 zoomAmount;

    public Vector3 newPosition;
    public Vector3 newZoom;
    public Quaternion newRotation;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;

    public float minZ;
    public float maxZ;
    public float minY;
    public float maxY;

    public float minMovementX;
    public float maxMovementX;
    public float minMovementZ;
    public float maxMovementZ;

    public bool hasReachMaxZ;
    public bool hasReachMaxY;

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

        //HandleMovementInput();
        //HandleMouseInput();
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
        float mouseX = _cursorPosition.x;
        float mouseY = _cursorPosition.y;

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    movementSpeed = fastSpeed;
        //}
        //else
        //{
        //    movementSpeed = normalSpeed;
        //}

        if (mouseY > 0)
            newPosition += transform.forward * _PLAYER.movementSpeed * movementModifier;
        if (mouseY < 0)
            newPosition -= transform.forward * _PLAYER.movementSpeed * movementModifier;
        if (mouseX > 0)
            newPosition += transform.right * _PLAYER.movementSpeed * movementModifier;
        if (mouseX < 0)
            newPosition -= transform.right * _PLAYER.movementSpeed * movementModifier;

        _TUTORIAL.CheckCameraTutorial(TutorialID.CameraMove);
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

    #region old
    void HandleMouseInput()
    {
        newZoom.z = Mathf.Clamp(newZoom.z, minZ, maxZ);
        newZoom.y = Mathf.Clamp(newZoom.y, minY, maxY);

        if (Input.mouseScrollDelta.y != 0)
        {
            if (newZoom.y != minY || newZoom.y != maxY)
                newZoom += Input.mouseScrollDelta.y * zoomAmount;
            _TUTORIAL.CheckCameraTutorial(TutorialID.CameraZoom);
        }

        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));

        }
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

    void HandleMovementInput()
    {
        newPosition.x = Mathf.Clamp(newPosition.x, minMovementX, maxMovementX);
        newPosition.z = Mathf.Clamp(newPosition.z, minMovementZ, maxMovementZ);
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementModifier = fastSpeed;
        }
        else
        {
            movementModifier = normalSpeed;
        }
        if (Input.GetKey(KeyCode.W) || mouseY > Screen.height - edgeScrollThreshold)
        {
            newPosition += (transform.forward * movementModifier * Time.deltaTime);
            _TUTORIAL.CheckCameraTutorial(TutorialID.CameraMove);
        }
        if (Input.GetKey(KeyCode.S) || mouseY < edgeScrollThreshold)
        {
            newPosition += (transform.forward * -movementModifier * Time.deltaTime);
            _TUTORIAL.CheckCameraTutorial(TutorialID.CameraMove);
        }
        if (Input.GetKey(KeyCode.D) || mouseX > Screen.width - edgeScrollThreshold)
        {
            newPosition += (transform.right * movementModifier * Time.deltaTime);
            _TUTORIAL.CheckCameraTutorial(TutorialID.CameraMove);
        }
        if (Input.GetKey(KeyCode.A) || mouseX < edgeScrollThreshold)
        {
            newPosition += (transform.right * -movementModifier * Time.deltaTime);
            _TUTORIAL.CheckCameraTutorial(TutorialID.CameraMove);
        }


        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
    }
    void EdgeScroll()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        // Check for scrolling left
        if (mouseX < edgeScrollThreshold)
        {
            isMouseOnEdge = true;
            scrollDirection = Vector3.left;
        }
        // Check for scrolling right
        else if (mouseX > Screen.width - edgeScrollThreshold)
        {
            isMouseOnEdge = true;
            scrollDirection = Vector3.right;
        }
        // Check for scrolling down
        else if (mouseY < edgeScrollThreshold)
        {
            isMouseOnEdge = true;
            scrollDirection = Vector3.back;
        }
        // Check for scrolling up
        else if (mouseY > Screen.height - edgeScrollThreshold)
        {
            isMouseOnEdge = true;
            scrollDirection = Vector3.forward;
        }
        else
        {
            isMouseOnEdge = false;
        }

        // Move the camera in the determined direction if the cursor is on the edge
        if (isMouseOnEdge)
        {
            transform.Translate(scrollDirection * normalSpeed * Time.deltaTime, Space.World);
        }
    }
    #endregion

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
    }

    private void OnDisable()
    {
        InputManager.OnCameraMove -= OnCameraMove;
        InputManager.OnCameraZoom -= OnCameraZoom;
        InputManager.OnCameraRotate -= OnCameraRotate;
    }
}
