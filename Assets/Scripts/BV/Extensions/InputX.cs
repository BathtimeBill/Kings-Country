using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.OnScreen;
using static UnityEngine.InputSystem.InputAction;


public class InputX : MonoBehaviour 
{
    #region Enums
    public enum ButtonState { None, Pressed, Holding, Released}
    public enum ButtonMap { Start, Select, North, East, South, West, LShoulder, RShoulder, Home}
    #endregion

    #region Variables
    public ButtonState startButtonState { get; private set; }
    public ButtonState selectButtonState { get; private set; }
    public ButtonState northButtonState { get; private set; }
    public ButtonState eastButtonState { get; private set; }
    public ButtonState southButtonState { get; private set; }
    public ButtonState westButtonState { get; private set; }
    public ButtonState leftShoulderButtonState { get; private set; }
    public ButtonState rightShoulderButtonState { get; private set; }
    public ButtonState homeButtonState { get; private set; }

    #endregion

    #region Unity Methods
    private void Awake()
    {
        // find the "move" action, and keep the reference to it, for use in Update
        //leftStickAction = actions.FindActionMap("Gameplay").FindAction("Cursor");

        //actions.FindActionMap("Creator").FindAction("StartButton").performed += OnStartButton;
        //actions.FindActionMap("Creator").FindAction("SelectButton").performed += OnSelectButton;
        //actions.FindActionMap("Creator").FindAction("HomeButton").performed += OnHomeButton;

        //actions.FindActionMap("Creator").FindAction("NorthButton").performed += OnNorthButton;
        //actions.FindActionMap("Creator").FindAction("EastButton").performed += OnEastButton;
        //actions.FindActionMap("Creator").FindAction("SouthButton").performed += OnSouthButton; 
        //actions.FindActionMap("Creator").FindAction("WestButton").performed += OnWestButton;

        //actions.FindActionMap("Creator").FindAction("LeftShoulderButton").performed += OnLeftShoulderButton;
        //actions.FindActionMap("Creator").FindAction("RightShoulderButton").performed += OnRightShoulderButton;
        
    }
    #endregion

    #region Public Methods
    public float GetHorizontalAxis(InputAction _action)
    {
        return _action.ReadValue<Vector2>().x;
    }
    public float GetVerticalAxis(InputAction _action)
    {
        return _action.ReadValue<Vector2>().y;
    }
    public Vector2 GetAxis(InputAction _action)
    {
        return _action.ReadValue<Vector2>();
    }
    public void OnStartButton(CallbackContext context) => ButtonAction(context, startButtonState);
    public void OnSelectButton(CallbackContext context) => ButtonAction(context, selectButtonState);
    public void OnHomeButton(CallbackContext context) => ButtonAction(context, homeButtonState);
    public void OnNorthButton(CallbackContext context) => ButtonAction(context, northButtonState);
    public void OnEastButton(CallbackContext context) => ButtonAction(context, eastButtonState);
    public void OnSouthButton(CallbackContext context) => ButtonAction(context, southButtonState);
    public void OnWestButton(CallbackContext context) => ButtonAction(context, westButtonState);
    public void OnLeftShoulderButton(CallbackContext context) => ButtonAction(context, leftShoulderButtonState);
    public void OnRightShoulderButton(CallbackContext context) => ButtonAction(context, rightShoulderButtonState);

    public bool GetButtonPressed(ButtonMap _button)
    {
        switch(_button)
        {
            case ButtonMap.Start:
                return startButtonState == ButtonState.Pressed;
            case ButtonMap.Select:
                return selectButtonState == ButtonState.Pressed;
            case ButtonMap.Home:
                return homeButtonState == ButtonState.Pressed;
            case ButtonMap.North:
                return northButtonState == ButtonState.Pressed;
            case ButtonMap.East:
                return eastButtonState == ButtonState.Pressed;
            case ButtonMap.South:
                return southButtonState == ButtonState.Pressed;
            case ButtonMap.West:
                return westButtonState == ButtonState.Pressed;
            case ButtonMap.LShoulder:
                return leftShoulderButtonState == ButtonState.Pressed;
            case ButtonMap.RShoulder:
                return rightShoulderButtonState == ButtonState.Pressed;
            default: 
                return false;
        }
    }
    public bool GetButtonHolding(ButtonMap _button)
    {
        switch (_button)
        {
            case ButtonMap.Start:
                return startButtonState == ButtonState.Holding;
            case ButtonMap.Select:
                return selectButtonState == ButtonState.Holding;
            case ButtonMap.Home:
                return homeButtonState == ButtonState.Holding;
            case ButtonMap.North:
                return northButtonState == ButtonState.Holding;
            case ButtonMap.East:
                return eastButtonState == ButtonState.Holding;
            case ButtonMap.South:
                return southButtonState == ButtonState.Holding;
            case ButtonMap.West:
                return westButtonState == ButtonState.Holding;
            case ButtonMap.LShoulder:
                return leftShoulderButtonState == ButtonState.Holding;
            case ButtonMap.RShoulder:
                return rightShoulderButtonState == ButtonState.Holding;
                
            default:
                return false;
        }
    }
    public bool GetButtonReleased(ButtonMap _button)
    {
        switch (_button)
        {
            case ButtonMap.Start:
                return startButtonState == ButtonState.Released;
            case ButtonMap.Select:
                return selectButtonState == ButtonState.Released;
            case ButtonMap.Home:
                return homeButtonState == ButtonState.Released;
            case ButtonMap.North:
                return northButtonState == ButtonState.Released;
            case ButtonMap.East:
                return eastButtonState == ButtonState.Released;
            case ButtonMap.South:
                return southButtonState == ButtonState.Released;
            case ButtonMap.West:
                return westButtonState == ButtonState.Released;
            case ButtonMap.LShoulder:
                return leftShoulderButtonState == ButtonState.Released;
            case ButtonMap.RShoulder:
                return rightShoulderButtonState == ButtonState.Released;
            default:
                return false;
        }
    }

    public void Vibrate(float _low, float _high, float _seconds = 0.1f)
    {
        Gamepad.current.SetMotorSpeeds(_low, _high);
        StopAllCoroutines();
        StartCoroutine(StopVibration(_seconds));
    }
    public void SetLightColor(Color _color)
    {
        if(Gamepad.current is DualShockGamepad)
            DualShockGamepad.current.SetLightBarColor(_color);
    }
    public IEnumerator StopVibration(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        Gamepad.current.SetMotorSpeeds(0,0);
    }
    #endregion

    #region Private Methods
    private void ButtonAction(CallbackContext _context, ButtonState _buttonState)
    {
        if (_context.action.WasPressedThisFrame())
        {
            _buttonState = ButtonState.Pressed;
            ExecuteNextFrame(() => _buttonState = ButtonState.Holding);
        }

        if (_context.action.WasReleasedThisFrame())
        {
            _buttonState = ButtonState.Released;
            ExecuteNextFrame(() => _buttonState = ButtonState.None);
        }
    }

    /// <summary>
    /// Executes the Action block as a Coroutine on the next frame.
    /// </summary>
    /// <param name="func">The Action block</param>
    protected void ExecuteNextFrame(Action func)
    {
        StartCoroutine(ExecuteAfterFramesCoroutine(1, func));
    }
    private IEnumerator ExecuteAfterFramesCoroutine(int frames, Action func)
    {
        for (int f = 0; f < frames; f++)
            yield return new WaitForEndOfFrame();
        func();
    }
    #endregion

    #region Protected Methods
    #endregion

    #region Event Handlers
    #endregion
}