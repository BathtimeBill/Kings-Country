using UnityEngine;

public class PlayerControls : Singleton<PlayerControls>
{
    [Header("World Objects")]
    public GameObject targetDest;
    public Camera cam;
    public Camera mapCam;
    public GameObject targetPointer;
    public GameObject targetPointerGraphics;
    public GameObject mouseOverEnemy;
    public LayerMask uILayer;
    [Header("Tools")]
    public Tools tools;
    [Header("Control Groups")]
    public bool canGroup;
    [Header("Map")]
    public bool mouseOverMap;
    [Header("Cursors")]
    public Texture2D cursorTextureNormal;
    public Texture2D cursorTextureRed;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public Vector2 hotSpotEnemy;

    public void DeselectAllTools() => DeslectAllModes();
    
    private void FixedUpdate()
    {
        if (_HasInput)
            RayCast();
    }

    public void MouseOverMap()
    {
        mouseOverMap = true;
    }
    public void MouseExitMap()
    {
        mouseOverMap = false;
    }

    #region Raycasting
    private void RayCast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitPoint)/* && hitPoint.collider.tag == "Ground"*/)
        {
            switch (_GAME.playmode)
            {
                case PlayMode.TreeMode:
                    tools.treeTool.SetPosition(hitPoint.point);
                    break;
                case PlayMode.RuneMode:
                    tools.runeTool.SetPosition(hitPoint.point);
                    break;
                case PlayMode.FyreMode:
                    tools.fyreTool.SetPosition(hitPoint.point);
                    break;
                case PlayMode.StormerMode:
                    tools.stormerTool.SetPosition(hitPoint.point);
                    break;
            }
            
            if (hitPoint.collider.CompareTag("Enemy"))
            {
                mouseOverEnemy = hitPoint.collider.gameObject;
                Cursor.SetCursor(cursorTextureRed, hotSpotEnemy, cursorMode);
            }
            else
            {
                Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode);
            }
            Debug.DrawLine(ray.origin, hitPoint.point);
            targetDest.transform.position = hitPoint.point;
        }
    }

    private void RaycastClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitPoint))
        {
            if(hitPoint.collider.CompareTag("Home Tree"))
            {
                if (_GAME.playmode == PlayMode.DefaultMode && _InGame)
                {
                    _SM.PlaySound(_SM.openMenuSound);
                    GameEvents.ReportOnSiteSelected(SiteID.Hut, false);
                    GameEvents.ReportOnSiteSelected(SiteID.Horgr, false);
                    GameEvents.ReportOnSiteSelected(SiteID.HomeTree, true); //NOTE the true site must be the last one
                    GameEvents.ReportOnGuardianSelected(null);
                }
            }
            else if (hitPoint.collider.CompareTag("Horgr"))
            {
                _SM.PlaySound(_SM.openMenuSound);
                GameEvents.ReportOnSiteSelected(SiteID.HomeTree, false);
                GameEvents.ReportOnSiteSelected(SiteID.Hut, false);
                GameEvents.ReportOnSiteSelected(SiteID.Horgr, true);  //NOTE the true site must be the last one
                GameEvents.ReportOnGuardianSelected(null);
            }
            else if (hitPoint.collider.CompareTag("Hut"))
            {
                _SM.PlaySound(_SM.openMenuSound);
                GameEvents.ReportOnSiteSelected(SiteID.HomeTree, false);
                GameEvents.ReportOnSiteSelected(SiteID.Horgr, false);
                GameEvents.ReportOnSiteSelected(SiteID.Hut, true);  //NOTE the true site must be the last one
                GameEvents.ReportOnGuardianSelected(null);
            }
            else if (hitPoint.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                if (_UI.mouseOverUI)
                    return;
                GameEvents.ReportOnSiteSelected(SiteID.HomeTree, false);
                GameEvents.ReportOnSiteSelected(SiteID.Hut, false);
                GameEvents.ReportOnSiteSelected(SiteID.Horgr, false);
                GameEvents.ReportOnGroundClicked();
            }
        }
    }
    #endregion
    
    private void OnSelectButtonPressed()
    {
        if (_UI.mouseOverUI)
            return;

        switch (_CurrentPlayMode)
        {
            case PlayMode.TreeMode:
                tools.treeTool.Use();
                break;
            case PlayMode.RuneMode:
                if (_GAME.runesAvailable)
                {
                    tools.runeTool.Use();
                    DeslectAllModes();
                }
                break;
            case PlayMode.FyreMode:
                if (_GAME.fyreAvailable && _UI.fyreAvailable)
                {
                    tools.fyreTool.Use();
                    DeslectAllModes();
                }
                break;
            case PlayMode.StormerMode:
                if (_GAME.stormerAvailable && _UI.stormerAvailable)
                {
                    tools.stormerTool.Use();
                    DeslectAllModes();
                }
                break;
            case PlayMode.DefaultMode:
                RaycastClick();
                break;
            default: break;
        }
    }

    private void OnDeselectButtonPressed()
    {
        targetPointer.transform.position = targetDest.transform.position;
        DeslectAllModes();
        _UI.MouseCancel();
    }

    private void OnEscapeButtonPressed()
    {
        if (_HasInput)
        {
            _GAME.SetPreviousState(_GAME.gameState);
            _GAME.ChangeGameState(GameState.Pause);
            _UI.TogglePanel(_UI.pausePanel, true);
            return;
        }

        if (_IsPaused)
        {
            if (_UI.warningPanel == null)
                _GAME.ChangeGameState(_GAME.previousState);
            else
                _UI.TurnOffWarningPanel();
        }
    }
    
    private void OnGroupSelected(int _group)
    {
        if (canGroup)
        {
            _GM.GroupUnits(_group);
            _SM.PlaySound(_SM.controlGroup);
        }
        else
        {
            _GM.SelectGroup(_group);
            _SM.PlaySound(_SM.controlGroupSelect);
        }
    }

    private void OnControlHold(bool _holding) => canGroup = _holding;
    private void OnUnitMove() => targetPointerGraphics.GetComponent<Animator>().SetTrigger("Move");
    
    private void SelectTreeMode(TreeID _treeID)
    {
        if(!_TutorialComplete || _BuildPhase)
        {
            _GAME.SetPlayMode(PlayMode.TreeMode);
            tools.treeTool.Select(_treeID);
        }
    }

    private void SelectRuneMode()
    {
        _GAME.SetPlayMode(PlayMode.RuneMode);
        tools.runeTool.Select();
    }

    private void SelectFyreMode()
    {
        _GAME.SetPlayMode(PlayMode.FyreMode);
        tools.fyreTool.Select();
    }    

    private void SelectStormerMode()
    {
        _GAME.SetPlayMode(PlayMode.StormerMode);
        tools.stormerTool.Select();
    }

    private void DeslectAllModes()
    {
        _GAME.SetPlayMode(PlayMode.DefaultMode);
        tools.runeTool.Deselect();
        tools.fyreTool.Deselect();
        tools.stormerTool.Deselect();
        tools.treeTool.Deselect();
        _UI.ShowTreeModifier(false);
    }
    
    private void OnToolButtonPressed(ToolID _toolID)
    {
        _SM.PlaySound(_SM.buttonClickSound);
        DeslectAllModes();
        switch (_toolID)
        {
            case ToolID.Stormer:
                if (_GAME.playmode != PlayMode.StormerMode)
                    SelectStormerMode();
                break;
            case ToolID.Fyre:
                if (_GAME.playmode != PlayMode.FyreMode)
                    SelectFyreMode();
                break;
            case ToolID.Rune:
                if (_GAME.playmode != PlayMode.RuneMode)
                    SelectRuneMode();
                break;
        }
    }
    
    private void OnTreeButtonPressed(TreeID _treeID)
    {
        _SM.PlaySound(_SM.buttonClickSound);
        DeslectAllModes();
        if (_GAME.playmode != PlayMode.TreeMode)
            SelectTreeMode(_treeID);
    }

    private void OnEnable()
    {
        InputManager.OnSelectButtonPressed += OnSelectButtonPressed;
        InputManager.OnDeselectButtonPressed += OnDeselectButtonPressed;
        InputManager.OnEscapeButtonPressed += OnEscapeButtonPressed;
        InputManager.OnControlHold += OnControlHold;
        InputManager.OnGroupSelected += OnGroupSelected;
        GameEvents.OnUnitMove += OnUnitMove;
        GameEvents.OnToolButtonPressed += OnToolButtonPressed;
        GameEvents.OnTreeButtonPressed += OnTreeButtonPressed;
    }

    private void OnDisable()
    {
        InputManager.OnSelectButtonPressed -= OnSelectButtonPressed;
        InputManager.OnDeselectButtonPressed -= OnDeselectButtonPressed;
        InputManager.OnEscapeButtonPressed -= OnEscapeButtonPressed;
        InputManager.OnControlHold -= OnControlHold;
        InputManager.OnGroupSelected -= OnGroupSelected;
        GameEvents.OnUnitMove -= OnUnitMove;
        GameEvents.OnToolButtonPressed -= OnToolButtonPressed;
        GameEvents.OnTreeButtonPressed -= OnTreeButtonPressed;
    }
}

[System.Serializable]
public class Tools
{
    public TreeTool treeTool;
    public FyreTool fyreTool;
    public StormerTool stormerTool;
    public RuneTool runeTool;
}
