using System.Collections;
using System.Collections.Generic;
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

    [Header("Tree Tool")]
    public bool treeTooClose;
    public GameObject treePlacement;
    public GameObject treePercentageModifier;
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
    [Header("Stormer Placement")]
    public GameObject stormerPlacement;
    public MeshRenderer stormerPlacementMeshRenderer;

    [Header("Map")]
    public bool mouseOverMap;

    private void Start()
    {
        treeTooClose = false;
    }

    private void FixedUpdate()
    {
        RayCast();

    }

    private void SelectTreeMode()
    {
        if(_GM.downTime)
        {
            if(_TUTM.isTutorial)
            {
                if(_TUTM.tutorialStage == 1)
                {
                    GameEvents.ReportOnNextTutorial();
                }
            }

            _GM.playmode = PlayMode.TreeMode;
            runePlacement.SetActive(false);
            stormerPlacement.SetActive(false);
            treePlacement.SetActive(true);
            beaconPlacement.SetActive(false);
            treePercentageModifier.SetActive(true);
            _UI.homeTreePanel.SetActive(false);
            _UI.treeToolSelectionBox.SetActive(true);
            _UI.runeToolSelectionBox.SetActive(false);
            _UI.beaconToolSelectionBox.SetActive(false);
            _UI.stormerToolSelectionBox.SetActive(false);
            _UI.maegenCost.SetActive(true);
            _UI.wildlifeCost.SetActive(false);
        }
        else
        {
            _UI.SetErrorMessageCantPlaceTrees();
            Error();
        }
    }

    private void SelectRuneMode()
    {
        _GM.playmode = PlayMode.RuneMode;
        runePlacement.SetActive(true);
        treePlacement.SetActive(false);
        stormerPlacement.SetActive(false);
        beaconPlacement.SetActive(false);
        treePercentageModifier.SetActive(false);
        _UI.homeTreePanel.SetActive(false);
        _UI.runeToolSelectionBox.SetActive(true);
        _UI.stormerToolSelectionBox.SetActive(false);
        _UI.treeToolSelectionBox.SetActive(false);
        _UI.beaconToolSelectionBox.SetActive(false);
        _UI.maegenCost.SetActive(true);
        _UI.wildlifeCost.SetActive(true);
    }

    private void SelectBeaconMode()
    {
        _GM.playmode = PlayMode.BeaconMode;
        beaconPlacement.SetActive(true);
        stormerPlacement.SetActive(false);
        runePlacement.SetActive(false);
        treePlacement.SetActive(false);
        treePercentageModifier.SetActive(false);
        _UI.homeTreePanel.SetActive(false);
        _UI.beaconToolSelectionBox.SetActive(true);
        _UI.stormerToolSelectionBox.SetActive(false);
        _UI.treeToolSelectionBox.SetActive(false);
        _UI.runeToolSelectionBox.SetActive(false);
        _UI.maegenCost.SetActive(true);
        _UI.wildlifeCost.SetActive(true);
        _UI.wildlifeCostText.text = "10";
        _UI.maegenCostText.text = "0";
    }    

    private void SelectStormerMode()
    {
        _GM.playmode = PlayMode.StormerMode;
        stormerPlacement.SetActive(true);
        beaconPlacement.SetActive(false);
        runePlacement.SetActive(false);
        treePlacement.SetActive(false);
        treePercentageModifier.SetActive(false);
        _UI.homeTreePanel.SetActive(false);
        _UI.stormerToolSelectionBox.SetActive(true);
        _UI.beaconToolSelectionBox.SetActive(false);
        _UI.treeToolSelectionBox.SetActive(false);
        _UI.runeToolSelectionBox.SetActive(false);
        _UI.maegenCost.SetActive(true);
        _UI.wildlifeCost.SetActive(true);
        _UI.wildlifeCostText.text = "20";
        _UI.maegenCostText.text = "0";
    }

    private void DeslectAllModes()
    {
        _GM.playmode = PlayMode.DefaultMode;
        treePlacement.SetActive(false);
        runePlacement.SetActive(false);
        stormerPlacement.SetActive(false);
        beaconPlacement.SetActive(false);
        treePercentageModifier.SetActive(false);
        _UI.treeToolSelectionBox.SetActive(false);
        _UI.runeToolSelectionBox.SetActive(false);
        _UI.stormerToolSelectionBox.SetActive(false);
        _UI.beaconToolSelectionBox.SetActive(false);
        _UI.maegenCost.SetActive(false);
        _UI.wildlifeCost.SetActive(false);
    }

    public void ClickOnTreeMode()
    {
        if (_GM.playmode != PlayMode.TreeMode)
        {
            SelectTreeMode();
            _UI.audioSource.clip = _SM.buttonClickSound;
            _UI.audioSource.Play();
        }
        else
        {
            DeslectAllModes();
        }
    }

    public void ClickOnRuneMode()
    {
        if (_GM.playmode != PlayMode.RuneMode)
        {
            SelectRuneMode();
            _UI.audioSource.clip = _SM.buttonClickSound;
            _UI.audioSource.Play();
        }
        else
        {
            DeslectAllModes();
        }
    }

    public void ClickOnBeaconMode()
    {
        if (_GM.playmode != PlayMode.BeaconMode)
        {
            SelectBeaconMode();
            _UI.audioSource.clip = _SM.buttonClickSound;
            _UI.audioSource.Play();
        }
        else
        {
            DeslectAllModes();
        }
    }

    public void ClickOnStormerMode()
    {
        if (_GM.playmode != PlayMode.StormerMode)
        {
            SelectStormerMode();
            _UI.audioSource.clip = _SM.buttonClickSound;
            _UI.audioSource.Play();
        }
        else
        {
            DeslectAllModes();
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_GM.playmode != PlayMode.TreeMode)
            {
                SelectTreeMode();
            }
            else
            {
                DeslectAllModes();
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_GM.playmode != PlayMode.RuneMode)
            {
                SelectRuneMode();
            }
            else
            {
                DeslectAllModes();
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_GM.playmode != PlayMode.BeaconMode)
            {
                SelectBeaconMode();
            }
            else
            {
                DeslectAllModes();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (_GM.playmode != PlayMode.StormerMode)
            {
                SelectStormerMode();
            }
            else
            {
                DeslectAllModes();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_UI.homeTreePanel.gameObject.activeInHierarchy == false)
            {
                _GM.playmode = PlayMode.DefaultMode;
                treePlacement.SetActive(false);
                runePlacement.SetActive(false);
                beaconPlacement.SetActive(false);
                treePercentageModifier.SetActive(false);
                _UI.homeTreePanel.SetActive(true);
                _UI.hutPanel.SetActive(false);
                _UI.horgrPanel.SetActive(false);
                _UI.maegenCost.SetActive(false);
                _UI.wildlifeCost.SetActive(false);
                _UI.runeToolSelectionBox.SetActive(false);
                _UI.treeToolSelectionBox.SetActive(false);
                _UI.beaconToolSelectionBox.SetActive(false);
                _UI.audioSource.clip = _SM.openMenuSound;
                _UI.audioSource.Play();
                if (_TUTM.isTutorial)
                {
                    if(_TUTM.tutorialStage == 3)
                    {
                        GameEvents.ReportOnNextTutorial();
                    }
                }
            }
            else
            {
                _UI.homeTreePanel.SetActive(false);
                _UI.horgrPanel.SetActive(false);
                _UI.hutPanel.SetActive(false);
                _UI.audioSource.clip = _SM.closeMenuSound;
                _UI.audioSource.Play();
            }


        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_UI.pausePanel.gameObject.activeInHierarchy == false)
            {
                _GM.isPaused = true;
                _GM.gameState = GameState.Pause;
                _UI.pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                _GM.isPaused = false;
                _GM.gameState = GameState.Play;
                _UI.pausePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {


            if (_GM.playmode == PlayMode.TreeMode)
            {
                if (_GM.trees.Length < _GM.maxTrees)
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
            if (_GM.playmode == PlayMode.RuneMode)
            {
                if (_RPlace.canPlace == true)
                {
                    GameObject runeInstance;
                    runeInstance = Instantiate(runePrefab, runePlacement.transform.position, runePlacement.transform.rotation);
                    _GM.maegen -= 2;
                    _GM.CheckRunes();
                }
                if (_RPlace.canPlace == false)
                {
                    _UI.SetErrorMessageInsufficientResources();
                    Error();
                }
            }
            if (_GM.playmode == PlayMode.BeaconMode)
            {
                if (_BPlace.canPlace == true)
                {
                    Instantiate(beaconPrefab, beaconPlacement.transform.position, beaconPrefab.transform.rotation);
                    _GM.CheckBeacons();
                    GameEvents.ReportOnBeaconPlaced();
                }
                if (_BPlace.canPlace == false)
                {
                    if (_UI.beaconPlaced)
                    {
                        _UI.SetErrorMessageBeaconCooldown();
                        Error();
                    }
                    else
                    {
                        _UI.SetErrorMessageInsufficientResources();
                        Error();
                    }
                }
            }
            if (_GM.playmode == PlayMode.StormerMode)
            {
                if (_SPlace.canPlace == true)
                {
                    GameEvents.ReportOnStormerPlaced();
                }
                else
                {
                    if (_UI.stormerPlaced == false)
                    {
                        _UI.SetErrorMessageInsufficientResources();
                        Error();
                    }
                    else
                    {
                        _UI.SetErrorMessageBeaconCooldown();
                        Error();
                    }

                }
            }
            if (_GM.playmode == PlayMode.DefaultMode)
            {
                RaycastClick();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            targetPointer.transform.position = targetDest.transform.position;
            _GM.playmode = PlayMode.DefaultMode;
            treePlacement.SetActive(false);
            stormerPlacement.SetActive(false);
            runePlacement.SetActive(false);
            beaconPlacement.SetActive(false);
            treePercentageModifier.SetActive(false);
            _UI.maegenCost.SetActive(false);
            _UI.wildlifeCost.SetActive(false);
            _UI.runeToolSelectionBox.SetActive(false);
            _UI.stormerToolSelectionBox.SetActive(false);
            _UI.treeToolSelectionBox.SetActive(false);
            _UI.beaconToolSelectionBox.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (_GM.gameState == GameState.Play)
            {
                Time.timeScale = 3.0f;
            }

        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (_GM.gameState == GameState.Play)
            {
                Time.timeScale = 1.0f;
            }

        }

    }

    public void Error()
    {
        errorAnimator.SetTrigger("Error");
        audioSource.Play();
    }
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
            if (_GM.playmode == PlayMode.BeaconMode)
            {
                beaconPlacement.transform.position = hitPoint.point;
                beaconPlacement.SetActive(true);
            }
            if (_GM.playmode == PlayMode.StormerMode)
            {
                stormerPlacement.transform.position = hitPoint.point;
                stormerPlacement.SetActive(true);
            }

            Debug.DrawLine(ray.origin, hitPoint.point);
            //Vector3 targetPosition = new Vector3(targetDest.transform.position.x, transform.position.y, targetDest.transform.position.z);
            targetDest.transform.position = hitPoint.point;
            treePlacementMeshRenderer.enabled = true;
            runePlacementMeshRenderer.enabled = true;
        }
        else
        {
            treePlacementMeshRenderer.enabled = false;
            runePlacementMeshRenderer.enabled = false;
        }


    }

    public void RaycastClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;
        if (Physics.Raycast(ray, out hitPoint) && hitPoint.collider.tag == "Home Tree")
        {
            if (_GM.playmode == PlayMode.DefaultMode)
            {
                _UI.homeTreePanel.SetActive(true);
                _UI.horgrPanel.SetActive(false);
                _UI.hutPanel.SetActive(false);

                if (_TUTM.isTutorial)
                {
                    if (_TUTM.tutorialStage == 3)
                    {
                        GameEvents.ReportOnNextTutorial();
                    }
                }
            }

            _UI.audioSource.clip = _SM.openMenuSound;
            _UI.audioSource.Play();
        }
        if (Physics.Raycast(ray, out hitPoint) && hitPoint.collider.tag == "Horgr")
        {
            Debug.Log("Clicked on Horgr");
            if (_GM.playmode == PlayMode.DefaultMode)
                _UI.horgrPanel.SetActive(true);
            _UI.hutPanel.SetActive(false);
            _UI.homeTreePanel.SetActive(false);
            _UI.audioSource.clip = _SM.openMenuSound;
            _UI.audioSource.Play();
        }
        if (Physics.Raycast(ray, out hitPoint) && hitPoint.collider.tag == "Hut")
        {
            Debug.Log("Clicked on Hut");
            if (_GM.playmode == PlayMode.DefaultMode)
                _UI.hutPanel.SetActive(true);
            _UI.homeTreePanel.SetActive(false);
            _UI.horgrPanel.SetActive(false);
            _UI.audioSource.clip = _SM.openMenuSound;
            _UI.audioSource.Play();
        }

    }

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

    private void OnEnable()
    {
        GameEvents.OnUnitMove += OnUnitMove;
    }

    private void OnDisable()
    {
        GameEvents.OnUnitMove -= OnUnitMove;
    }
}
