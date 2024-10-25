using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.InputAction;


public enum JoystickMotor
{
    Left,
    Right
}

public enum InputMap { Menu, Gameplay}
public enum InputScheme { Keyboard, Controller}

public class InputManager : InputX 
{
    #region Enums
    #endregion

    #region Variables
    [Space]
    public InputScheme playerInput;
    public InputMap inputMap;
    public InputActionAsset actions;

    private Vector2 cameraVector, cursorVector;

    //Mouse
    public Vector2 bias = new Vector2(0f, -1f);
    public Vector2 sensitivity = new Vector2(1500f, 1500f);
    private Vector2 mousePosition;
    private Vector2 warpPosition;
    private Vector2 overflow;

    public bool southButtonPressed { get; private set; }

    // private field to store move action reference
    private InputAction cursorAction, cameraMovementAction, cameraRotationAction, cameraZoomAction;

    //In Game Stuff
    //public static event Action<float> OnCameraRotate = null;
    public static event Action<float> OnCameraZoom = null;
    public static event Action<Vector2> OnCameraMove = null;
    public static event Action<Vector2> OnCameraRotate = null;
    public static event Action<Vector2> OnCursorMove = null;
    public static event Action<bool> OnCameraHaste = null;

    // Menu Stuff
    public static event Action OnMenuInputSelect = null;
    public static event Action OnMenuInputCancel = null;
    public static event Action OnMenuInputUp = null;
    public static event Action OnMenuInputDown = null;
    public static event Action OnMenuInputLeft = null;
    public static event Action OnMenuInputRight = null;

    public static event Action OnSelectButtonPressed = null;
    public static event Action OnSelectButtonHolding = null;
    public static event Action OnSelectButtonReleased = null;
    public static event Action OnDeselectButtonPressed = null;
    public static event Action OnEscapeButtonPressed = null;

    public static event Action OnLeftShoulderDown = null;
    public static event Action OnLeftShoulderUp = null;
    public static event Action OnRightShoulderDown = null;
    public static event Action OnRightShoulderUp = null;

    
    #endregion

    #region Unity Methods
    private void Awake()
    {
        cursorAction = actions.FindActionMap("Gameplay").FindAction("Cursor");

        cameraMovementAction = actions.FindActionMap("Gameplay").FindAction("CameraMovement");
        cameraRotationAction = actions.FindActionMap("Gameplay").FindAction("CameraRotation");
        cameraZoomAction = actions.FindActionMap("Gameplay").FindAction("CameraZoom");

        //actions.FindActionMap("Gameplay").FindAction("Select").performed += OnWestButton;
        actions.FindActionMap("Gameplay").FindAction("Select").performed += SouthButton;
        actions.FindActionMap("Gameplay").FindAction("Deselect").performed += EastButton;
        actions.FindActionMap("Gameplay").FindAction("Escape").performed += StartButton;
        actions.FindActionMap("Gameplay").FindAction("CameraHaste").performed += HasteButton;

        //Menus
        //menuMoveAction = actions.FindActionMap("Menus").FindAction("MenuMove");
        //actions.FindActionMap("Menus").FindAction("MenuSelect").performed += OnMenuSelect;
        //actions.FindActionMap("Menus").FindAction("MenuCancel").performed += OnMenuCancel;

        ChangeInputMap(inputMap);
    }

    void Update()
    {
        //CursorMovement();

        if (inputMap == InputMap.Gameplay)
        {
            cameraVector = GetAxis(cameraMovementAction);
            OnCameraMove?.Invoke(cameraVector);

            float cameraZoom = cameraZoomAction.ReadValue<float>();
            OnCameraZoom?.Invoke(cameraZoom);

            Vector2 cameraRotation = cameraRotationAction.ReadValue<Vector2>();
            OnCameraRotate?.Invoke(cameraRotation);

            //Button Functionality
            if (GetButtonPressed(ButtonMap.South))
                OnSelectButtonPressed?.Invoke();
            if (GetButtonHolding(ButtonMap.South))
                OnSelectButtonHolding?.Invoke();
            if (GetButtonReleased(ButtonMap.South))
                OnSelectButtonReleased?.Invoke();

            if (GetButtonPressed(ButtonMap.East))
                OnDeselectButtonPressed?.Invoke();

            if (GetButtonPressed(ButtonMap.Start))
                OnEscapeButtonPressed?.Invoke();


            if (GetButtonPressed(ButtonMap.LShoulder))
                OnLeftShoulderDown?.Invoke();
            if (GetButtonReleased(ButtonMap.LShoulder))
                OnLeftShoulderUp?.Invoke();
            if (GetButtonPressed(ButtonMap.RShoulder))
                OnRightShoulderDown?.Invoke();
            if (GetButtonReleased(ButtonMap.RShoulder))
                OnRightShoulderUp?.Invoke();

            if (GetButtonHolding(ButtonMap.LStick))
                OnCameraHaste?.Invoke(true);
            if (GetButtonReleased(ButtonMap.LStick))
                OnCameraHaste?.Invoke(false);
        }

       
    }

    public void CursorMovement()
    {
        cursorVector = GetAxis(cursorAction);
        //if(controller.currentControlScheme.Equals("Controller"))
        {
            // Prevent annoying jitter when not using joystick
            if (cursorVector.magnitude < 0.1f) return;

            // Get the current mouse position to add to the joystick movement
            mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // Precise value for desired cursor position, which unfortunately cannot be used directly
            warpPosition = mousePosition + bias + overflow + sensitivity * Time.deltaTime * cursorVector;

            // Keep the cursor in the game screen (behavior gets weird out of bounds)
            warpPosition = new Vector2(Mathf.Clamp(warpPosition.x, 0, Screen.width), Mathf.Clamp(warpPosition.y, 0, Screen.height));

            // Store floating point values so they are not lost in WarpCursorPosition (which applies FloorToInt)
            overflow = new Vector2(warpPosition.x % 1, warpPosition.y % 1);

            // Move the cursor
            Mouse.current.WarpCursorPosition(warpPosition);
        }
    }
    #endregion

    #region Public Methods

    void OnMenuSelect(CallbackContext context)
    {
        //GameEvents.ReportMenuInputSelect();
    }

    void OnMenuCancel (CallbackContext context)
    {
        //GameEvents.ReportMenuInputCancel();
    }


    /*public void Vibrate(HitAccuracy accuracy)
    {
        if (!_SETTINGS.vibration.vibrateOn || accuracy == HitAccuracy.Missed)
            return;

        float intensity = accuracy == HitAccuracy.Perfect ? accuracy == HitAccuracy.Great ? _SETTINGS.vibration.vibrateStrength[1] : _SETTINGS.vibration.vibrateStrength[2] : _SETTINGS.vibration.vibrateStrength[3];
        _INPUT.input.Vibrate(intensity, intensity);
    }

    public void Vibrate(StemShakeIntensity shakeIntensity)
    {
        if (!_SETTINGS.vibration.vibrateOn || shakeIntensity == StemShakeIntensity.None)
            return;

        float intensity = shakeIntensity == StemShakeIntensity.Max ? shakeIntensity == StemShakeIntensity.Mid ? _SETTINGS.vibration.vibrateStrength[0] : _SETTINGS.vibration.vibrateStrength[1] : _SETTINGS.vibration.vibrateStrength[2];
        _INPUT.input.Vibrate(intensity, intensity);
    }*/
    #endregion

    #region Private Methods

    #endregion

    #region Protected Methods
    #endregion

    #region Event Handlers

    private void ChangeInputMap(InputMap _inputMap)
    {
        inputMap = _inputMap;
        if (inputMap == InputMap.Menu)
        {
            actions.FindActionMap("Menu").Enable();
            actions.FindActionMap("Gameplay").Disable();
        }
        else
        {
            actions.FindActionMap("Gameplay").Enable();
            actions.FindActionMap("Menu").Disable();
        }
    }
    private void OnGameStateChanged(GameState _gameState)
    {
        if(_gameState == GameState.Play)
            ChangeInputMap(InputMap.Gameplay);
        else
            ChangeInputMap(InputMap.Menu);
    }

    void OnEnable()
    {
        //actions.FindActionMap("Gameplay").Enable();
        //GameEvents.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDisable()
    {
        //actions.FindActionMap("Gameplay").Disable();
        //GameEvents.OnGameStateChanged -= OnGameStateChanged;
    }
    #endregion

}