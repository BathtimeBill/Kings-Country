using System.Collections;
using UnityEngine;

public class PlayerControls : Singleton<PlayerControls>
{
    [Header("World Objects")]
    public GameObject targetDest;
    public Camera cam;
    public Camera mapCam;
    public GameObject playerCamera;
    public GameObject targetPointer;
    public GameObject targetPointerGraphics;
    public Material brightYellowMat;
    public Material invisibleMat;
    public GameObject mouseOverEnemy;
    public bool mouseOverEnemyBool;
    public LayerMask uILayer;
    public bool canPressEscape;
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
    [Header("Beacon Placement")]
    public GameObject beaconPlacement;
    public MeshRenderer beaconPlacementMeshRenderer;
    public GameObject beaconPrefab;
    public GameObject explosion;
    public GameObject explosion2;
    [Header("Stormer Placement")]
    public GameObject stormerPlacement;
    public MeshRenderer stormerPlacementMeshRenderer;
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


    private void Start()
    {
        treeTooClose = false;
    }

    private void FixedUpdate()
    {
        if (hasInput)
        {
            RayCast();
        }
    }

    private void SelectTreeMode()
    {
        if(_GM.peaceTime)
        {
            if(_TUTM.isTutorial)
            {
                if(_TUTM.tutorialStage == 1)
                {
                    GameEvents.ReportOnNextTutorial();
                }
            }

            _GM.playmode = PlayMode.TreeMode;
            treePlacement.SetActive(true);
            _UI.ShowTreeModifier(true);
        }
        _TPlace.canPlace = true;
        _TPlace.gameObject.GetComponent<Renderer>().material = _TPlace.canPlaceMat;
    }

    private void SelectRuneMode()
    {
        _GM.playmode = PlayMode.RuneMode;
        runePlacement.SetActive(true);
    }

    private void SelectFyreMode()
    {
        _GM.playmode = PlayMode.FyreMode;
        beaconPlacement.SetActive(true);
    }    

    private void SelectStormerMode()
    {
        _GM.playmode = PlayMode.StormerMode;
        stormerPlacement.SetActive(true);
    }

    private void DeslectAllModes()
    {
        _GM.playmode = PlayMode.DefaultMode;
        treePlacement.SetActive(false);
        runePlacement.SetActive(false);
        stormerPlacement.SetActive(false);
        beaconPlacement.SetActive(false);
        _UI.ShowTreeModifier(false);
    }

    public void MouseOverMap()
    {
        mouseOverMap = true;
    }
    public void MouseExitMap()
    {
        mouseOverMap = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (hasInput)
            {
                _GM.previousState = _GM.gameState;
                _GM.ChangeGameState(GameState.Pause);
                _UI.TogglePanel(_UI.pausePanel, true);
                return;
            }

            if (isPaused)
            {
                if (_UI.warningPanel == null)
                    _GM.ChangeGameState(_GM.previousState);
                else
                    _UI.TurnOffWarningPanel();
            }
        }

        if (hasInput)
        {
            #region group control
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (canGroup)
                {
                    UnitSelection.Instance.Group1();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect1();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {

                if (canGroup)
                {
                    UnitSelection.Instance.Group2();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect2();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {

                if (canGroup)
                {
                    UnitSelection.Instance.Group3();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect3();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {

                if (canGroup)
                {
                    UnitSelection.Instance.Group4();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect4();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {

                if (canGroup)
                {
                    UnitSelection.Instance.Group5();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect5();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {

                if (canGroup)
                {
                    UnitSelection.Instance.Group6();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect6();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {

                if (canGroup)
                {
                    UnitSelection.Instance.Group7();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect7();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {

                if (canGroup)
                {
                    UnitSelection.Instance.Group8();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect8();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {

                if (canGroup)
                {
                    UnitSelection.Instance.Group9();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect9();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {

                if (canGroup)
                {
                    UnitSelection.Instance.Group10();
                    _SM.PlaySound(_SM.controlGroup);
                }
                else
                {
                    UnitSelection.Instance.GroupSelect10();
                    _SM.PlaySound(_SM.controlGroupSelect);
                }
            }
            #endregion

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                canGroup = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                canGroup = false;
            }
            //if (Input.GetKeyDown(KeyCode.LeftAlt))
            //{
            //    canGroup = false;
            //}
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                /*
                if (_UI.homeTreePanel.gameObject.activeInHierarchy == false)
                {
                    _GM.playmode = PlayMode.DefaultMode;
                    treePlacement.SetActive(false);
                    runePlacement.SetActive(false);
                    beaconPlacement.SetActive(false);
                    treePercentageModifier.SetActive(false);
                    _UI.homeTreePanel.SetActive(true);
                    GameEvents.ReportOnHomeTreeSelected();
                    _UI.hutPanel.SetActive(false);
                    _UI.horgrPanel.SetActive(false);
                    _UI.maegenCost.SetActive(false);
                    _UI.wildlifeCost.SetActive(false);
                    _UI.audioSource.clip = _SM.openMenuSound;
                    _UI.audioSource.Play();
                    GameEvents.ReportOnHorgrDeselected();
                    GameEvents.ReportOnHutDeselected();
                }
                else
                {
                    _UI.mouseOverUI = false;
                    _UI.homeTreePanel.SetActive(false);
                    _UI.horgrPanel.SetActive(false);
                    _UI.hutPanel.SetActive(false);
                    _UI.audioSource.clip = _SM.closeMenuSound;
                    _UI.audioSource.Play();
                    GameEvents.ReportOnHomeTreeDeselected();
                }*/
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (_UI.mouseOverUI )
                    return;
                
                if (_GM.playmode == PlayMode.TreeMode)
                {
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
                            GameEvents.ReportOnTreePlaced();
                            worldAudioSource = treeInstance.GetComponent<AudioSource>();
                            worldAudioSource.clip = _SM.GetTreeGrowSound();
                            worldAudioSource.Play();
                            _GM.DecreaseMaegen(_TPlace.maegenCost);
                        }
                        if (_TPlace.tooFarAway == true)
                        {
                            _UI.SetErrorMessageTooFar();
                            Error();
                        }
                        if (_TPlace.insufficientMaegen == true)
                        {
                            _UI.SetErrorMessageInsufficientMaegen();
                            Error();
                        }
                    }
                    else
                    {
                        _UI.SetErrorMessageTooManyTrees();
                        Error();
                    }
                }
                //Runes
                if (_GM.playmode == PlayMode.RuneMode)
                {
                    if(_GM.runesAvailable)
                    {
                        GameObject runeInstance;
                        runeInstance = Instantiate(runePrefab, runePlacement.transform.position, runePlacement.transform.rotation);
                        _GM.DecreaseMaegen(_GM.runesMaegenCost[_GM.runesCount]);
                        _GM.AddRune(runeInstance);
                        DeslectAllModes();
                        GameEvents.ReportOnRunePlaced();
                    }
                }
                //Fyre
                if (_GM.playmode == PlayMode.FyreMode)
                {
                    if (_GM.fyreAvailable && _UI.fyreAvailable)
                    {
                        if (_UM.hasUpgrade(UpgradeID.Fyre))
                        {
                            Instantiate(explosion2, beaconPlacement.transform.position, beaconPrefab.transform.rotation);
                        }
                        else
                        {
                            Instantiate(explosion, beaconPlacement.transform.position, beaconPrefab.transform.rotation);
                        }

                        beaconPlacement.SetActive(false);
                        DeslectAllModes();
                        GameEvents.ReportOnFyrePlaced();
                    }
                }
                //Stormer
                if (_GM.playmode == PlayMode.StormerMode)
                {
                    if (_GM.stormerAvailable && _UI.stormerAvailable)
                    {
                        stormerPlacement.SetActive(false);
                        DeslectAllModes();
                        GameEvents.ReportOnStormerPlaced();
                    }
                }
                //Default
                if (_GM.playmode == PlayMode.DefaultMode)
                {
                    RaycastClick();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                targetPointer.transform.position = targetDest.transform.position;
                DeslectAllModes();
                _UI.MouseCancel();
            }

            if (Input.GetKeyDown(KeyCode.F4))
            {
                if (hasInput)
                {
                    _GM.SpeedGame();
                }

            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                if (hasInput)
                {
                    _GM.SetGame();
                }

            }
        }
    }

    public void Error()
    {
        errorAnimator.SetTrigger("Error");
        audioSource.Play();
    }

    #region Raycasting
    public void RayCast()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;

        if (Physics.Raycast(ray, out hitPoint)/* && hitPoint.collider.tag == "Ground"*/)
        {
            if (_GM.playmode == PlayMode.TreeMode)
            {
                treePlacement.transform.position = hitPoint.point;
                treePlacement.SetActive(true);
            }
            if (_GM.playmode == PlayMode.RuneMode)
            {
                runePlacement.transform.position = hitPoint.point;
                runePlacement.SetActive(true);
            }
            if (_GM.playmode == PlayMode.FyreMode)
            {
                beaconPlacement.transform.position = hitPoint.point;
                beaconPlacement.SetActive(true);
            }
            if (_GM.playmode == PlayMode.StormerMode)
            {
                stormerPlacement.transform.position = hitPoint.point;
                stormerPlacement.SetActive(true);
            }
            if (hitPoint.collider.tag == "Enemy")
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
            //Vector3 targetPosition = new Vector3(targetDest.transform.position.x, transform.position.y, targetDest.transform.position.z);
            targetDest.transform.position = hitPoint.point;
            //treePlacementMeshRenderer.enabled = true;
            //runePlacementMeshRenderer.enabled = true;

        }
        else
        {
            //treePlacementMeshRenderer.enabled = false;
            //runePlacementMeshRenderer.enabled = false;
        }

    }

    public void RaycastClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;
        if (Physics.Raycast(ray, out hitPoint))
        {

            if(hitPoint.collider.tag == "Home Tree")
            {
                if (_GM.playmode == PlayMode.DefaultMode)
                {
                    _SM.PlaySound(_SM.openMenuSound);
                    GameEvents.ReportOnHomeTreeSelected();
                    GameEvents.ReportOnHutDeselected();
                    GameEvents.ReportOnHorgrDeselected();
                    if (_TUTM.isTutorial)
                    {
                        if (_TUTM.tutorialStage == 3)
                        {
                            GameEvents.ReportOnNextTutorial();
                        }
                    }
                }
            }
            if (hitPoint.collider.tag == "Horgr")
            {
                _SM.PlaySound(_SM.openMenuSound);
                GameEvents.ReportOnHorgrSelected();
                GameEvents.ReportOnHomeTreeDeselected();
                GameEvents.ReportOnHutDeselected();
            }
            if (hitPoint.collider.tag == "Hut")
            {
                _SM.PlaySound(_SM.openMenuSound);
                GameEvents.ReportOnHutSelected();
                GameEvents.ReportOnHomeTreeDeselected();
                GameEvents.ReportOnHorgrDeselected();
            }
            if (hitPoint.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                if (_UI.mouseOverUI)
                    return;
                GameEvents.ReportOnHorgrDeselected();
                GameEvents.ReportOnHomeTreeDeselected();
                GameEvents.ReportOnHutDeselected();
                GameEvents.ReportOnGroundClicked();
            }
        }
    }
    #endregion

    public void OnUnitMove()
    {
        Debug.Log("Target Moving");
        targetPointerGraphics.GetComponent<Animator>().SetTrigger("Move");
    }
    IEnumerator DisablePointerDelay()
    {
        targetPointerGraphics.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        targetPointerGraphics.SetActive(false);
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
                if(_GM.playmode != PlayMode.TreeMode)
                    SelectTreeMode();
                break;
        }
    }
    private void OnEnable()
    {
        GameEvents.OnUnitMove += OnUnitMove;
        GameEvents.OnToolButtonPressed += OnToolButtonPressed;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitMove -= OnUnitMove;
        GameEvents.OnToolButtonPressed -= OnToolButtonPressed;
    }
}
