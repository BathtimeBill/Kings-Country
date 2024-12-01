using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControls : Singleton<PlayerControls>
{
    [Header("World Objects")]
    public GameObject targetDest;
    public Camera cam;
    public Camera mapCam;
    public GameObject targetPointer;
    public GameObject targetPointerGraphics;
    public GameObject mouseOverEnemy;
    public bool mouseOverEnemyBool;
    public LayerMask uILayer;
    public Tools tools;
    [Header("Tree Tool")]
    public bool treeTooClose;
    public GameObject treePlacement;
    public GameObject cantPlace;
    public GameObject treePrefab;
    public float minScale;
    public float maxScale;
    public MeshRenderer treePlacementMeshRenderer;
    public Animator errorAnimator;
    public AudioSource audioSource;
    public AudioSource worldAudioSource;
    [Header("Rune Placement")]
    public GameObject runePlacement;
    public MeshRenderer runePlacementMeshRenderer;
    public GameObject runePrefab;
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

    private void Start()
    {
        treeTooClose = false;
    }

    private void FixedUpdate()
    {
        if (_hasInput)
        {
            RayCast();
        }
    }

    private void Update()
    {
        if (_hasInput)
        {
            if (Input.GetKeyDown(KeyCode.F4))
                _GM.SpeedGame();
            if (Input.GetKeyDown(KeyCode.F3))
                _GM.SetGame();
        }
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
    public void RayCast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitPoint)/* && hitPoint.collider.tag == "Ground"*/)
        {
            switch (_GM.playmode)
            {
                case PlayMode.TreeMode:
                    treePlacement.transform.position = hitPoint.point;
                    treePlacement.SetActive(true);
                    break;
                case PlayMode.RuneMode:
                    runePlacement.transform.position = hitPoint.point;
                    runePlacement.SetActive(true);
                    break;
                case PlayMode.FyreMode:
                    tools.fyreTool.transform.position = hitPoint.point;
                    break;
                case PlayMode.StormerMode:
                    tools.stormerTool.transform.position = hitPoint.point;
                    break;
            }
            
            if (hitPoint.collider.CompareTag("Enemy"))
            {
                mouseOverEnemy = hitPoint.collider.gameObject;
                mouseOverEnemyBool = true;
                Cursor.SetCursor(cursorTextureRed, hotSpotEnemy, cursorMode);
            }
            else
            {
                mouseOverEnemyBool = false;
                Cursor.SetCursor(cursorTextureNormal, hotSpot, cursorMode);
            }
            Debug.DrawLine(ray.origin, hitPoint.point);
            targetDest.transform.position = hitPoint.point;
        }
    }

    public void RaycastClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitPoint))
        {
            if(hitPoint.collider.CompareTag("Home Tree"))
            {
                if (_GM.playmode == PlayMode.DefaultMode && _inGame)
                {
                    _SM.PlaySound(_SM.openMenuSound);
                    GameEvents.ReportOnSiteSelected(SiteID.HomeTree, true);
                    GameEvents.ReportOnSiteSelected(SiteID.Hut, false);
                    GameEvents.ReportOnSiteSelected(SiteID.Horgr, false);
                    GameEvents.ReportOnObjectSelected(hitPoint.collider.gameObject);
                }
            }
            if (hitPoint.collider.CompareTag("Horgr"))
            {
                _SM.PlaySound(_SM.openMenuSound);
                GameEvents.ReportOnSiteSelected(SiteID.HomeTree, false);
                GameEvents.ReportOnSiteSelected(SiteID.Hut, false);
                GameEvents.ReportOnSiteSelected(SiteID.Horgr, true);
                GameEvents.ReportOnObjectSelected(hitPoint.collider.gameObject);
            }
            if (hitPoint.collider.CompareTag("Hut"))
            {
                _SM.PlaySound(_SM.openMenuSound);
                GameEvents.ReportOnSiteSelected(SiteID.HomeTree, false);
                GameEvents.ReportOnSiteSelected(SiteID.Hut, true);
                GameEvents.ReportOnSiteSelected(SiteID.Horgr, false);
                GameEvents.ReportOnObjectSelected(hitPoint.collider.gameObject);
            }
            if (hitPoint.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
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
            //Trees
            case PlayMode.TreeMode:
                if (_GM.trees.Count < _GM.maxTrees)
                {
                    if (_TPlace.canPlace == true)
                    {
                        GameObject treeInstance;
                        GameObject cantDestroy;
                        Vector3 randomSize = new Vector3(1, Random.Range(minScale, maxScale), 1);
                        treeInstance = Instantiate(treePrefab, treePlacement.transform.position, treePlacement.transform.rotation);
                        treeInstance.transform.localScale = randomSize;
                        treeInstance.GetComponent<Tree>().energyMultiplier = _TPlace.maegenPerWave;
                        cantDestroy = Instantiate(cantPlace, treePlacement.transform.position, treePlacement.transform.rotation);
                        Destroy(cantDestroy, 15);
                        GameEvents.ReportOnTreePlaced(ToolID.Tree);
                        worldAudioSource = treeInstance.GetComponent<AudioSource>();
                        worldAudioSource.clip = _SM.GetTreeGrowSound();
                        worldAudioSource.Play();
                        _GM.DecreaseMaegen(_TPlace.maegenCost);
                    }
                    if (_TPlace.tooFarAway == true)
                    {
                        _UI.SetError(ErrorID.TooFar);
                    }
                    if (_TPlace.insufficientMaegen == true)
                    {
                        _UI.SetError(ErrorID.InsufficientMaegen);
                    }
                }
                else
                {
                    _UI.SetError(ErrorID.TooManyTrees);
                }
                break;
            //Runes
            case PlayMode.RuneMode:
                if (_GM.runesAvailable)
                {
                    GameObject runeInstance = Instantiate(runePrefab, runePlacement.transform.position, runePlacement.transform.rotation);
                    _GM.DecreaseMaegen(_GM.runesMaegenCost[_GM.runesCount]);
                    _GM.AddRune(runeInstance);
                    DeslectAllModes();
                    GameEvents.ReportOnRunePlaced();
                }
                break;
            //Fyre
            case PlayMode.FyreMode:
                if (_GM.fyreAvailable && _UI.fyreAvailable)
                {
                    tools.fyreTool.Use();
                    DeslectAllModes();
                }
                break;
            //Stormer
            case PlayMode.StormerMode:
                if (_GM.stormerAvailable && _UI.stormerAvailable)
                {
                    tools.stormerTool.Use();
                    DeslectAllModes();
                }
                break;
            //Default
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
        if (_hasInput)
        {
            _GM.SetPreviousState(_GM.gameState);
            _GM.ChangeGameState(GameState.Pause);
            _UI.TogglePanel(_UI.pausePanel, true);
            return;
        }

        if (_isPaused)
        {
            if (_UI.warningPanel == null)
                _GM.ChangeGameState(_GM.previousState);
            else
                _UI.TurnOffWarningPanel();
        }
    }

    private void OnControlHold(bool _holding)
    {
        canGroup = _holding;
    }

    private void OnGroupSelected(int _group)
    {
        if (canGroup)
        {
            _UM.GroupUnits(_group);
            _SM.PlaySound(_SM.controlGroup);
        }
        else
        {
            _UM.SelectGroup(_group);
            _SM.PlaySound(_SM.controlGroupSelect);
        }
    }

    private void OnUnitMove()
    {
        //Debug.Log("Target Moving");
        targetPointerGraphics.GetComponent<Animator>().SetTrigger("Move");
    }
    
    private void SelectTreeMode()
    {
        if(!_tutorialComplete)
        {
            _GM.SetPlayMode(PlayMode.TreeMode);
            treePlacement.SetActive(true);
            _UI.ShowTreeModifier(true);
        }

        if(_buildPhase)
        {
            _GM.SetPlayMode(PlayMode.TreeMode);
            treePlacement.SetActive(true);
            _UI.ShowTreeModifier(true);
        }
        _TPlace.canPlace = true;
        //_TPlace.gameObject.GetComponent<Renderer>().material = _TPlace.canPlaceMat;
    }

    private void SelectRuneMode()
    {
        _GM.SetPlayMode(PlayMode.RuneMode);
        runePlacement.SetActive(true);
    }

    private void SelectFyreMode()
    {
        _GM.SetPlayMode(PlayMode.FyreMode);
        tools.fyreTool.Select();
    }    

    private void SelectStormerMode()
    {
        _GM.SetPlayMode(PlayMode.StormerMode);
        tools.stormerTool.Select();
    }

    private void DeslectAllModes()
    {
        _GM.SetPlayMode(PlayMode.DefaultMode);
        treePlacement.SetActive(false);
        runePlacement.SetActive(false);
        tools.fyreTool.Deselect();
        tools.stormerTool.Deselect();
        _UI.ShowTreeModifier(false);
    }
    
    private void OnToolButtonPressed(ToolID _toolID)
    {
        _SM.PlaySound(_SM.buttonClickSound);
        DeslectAllModes();
        switch (_toolID)
        {
            case ToolID.Stormer:
                if (_GM.playmode != PlayMode.StormerMode)
                    SelectStormerMode();
                break;
            case ToolID.Fyre:
                if (_GM.playmode != PlayMode.FyreMode)
                    SelectFyreMode();
                break;
            case ToolID.Rune:
                if (_GM.playmode != PlayMode.RuneMode)
                    SelectRuneMode();
                break;
            case ToolID.Tree:
                if (_GM.playmode != PlayMode.TreeMode)
                    SelectTreeMode();
                break;
        }
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
    }
}

[System.Serializable]
public class Tools
{
    public FyreTool fyreTool;
    public StormerTool stormerTool;
}
