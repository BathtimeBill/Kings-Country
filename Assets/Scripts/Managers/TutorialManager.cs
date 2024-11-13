using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

public class TutorialManager : GameBehaviour
{
    //TEMP
    [Tooltip("Just to skip the intro camera")]
    public bool overrideStartTime = true;
    public int debugStartOffset = 0;
    public float cameraTaskTime = 3f;

    [Header("Tutorial Chunks")]
    public TutorialID currentTutorialID;
    public List<Tutorial> tutorials;
    public List<TaskLine> taskLines;

    [Header("Basic")]
    public TutorialArrow arrows;
    public CanvasGroup glossaryPanel;
    public CanvasGroup blackoutPanel;

    [Header("In Game Tutorial")]
    public TMP_Text tutorialTitle;
    public TMP_Text tutorialDescription;
    public TMP_Text taskText;
    public GameObject check;
    public CanvasGroup tutorialPanel;
    public CanvasGroup taskPanel;
    public GameObject inGameContinueButton;
    public GameObject treeButton;
    public GameObject maegenIcon;
    public int tutorialStage;
    public bool isTutorial;

/*    [HideInInspector] */public bool tutorialComplete;
    private int treeCount = 0;
    private int treeCompletionCount = 4;

    private int creatureCount = 0;
    private int creatureCompletionCount = 3;

    private int moveCreatureCount = 0;
    private int moveCreatureCompletionCount = 3;

    private float fadeStrength = 0.1f;
    private float panelDelay = 1f;

    private InGamePanels gamePanels;
    private List<CanvasGroup> arrowList = new List<CanvasGroup>();

    private Tutorial GetTutorial(TutorialID _id) => tutorials.Find(x => x.tutorialID == _id);
    private Tutorial CurrentTutorial => GetTutorial(currentTutorialID);

    #region Text Variables
    private string moveCameraTask   = "Move the camera around the Grove";
    private string rotateCameraTask = "Rotate the camera";
    private string zoomCameraTask   = "Zoom the camera";
    private string treesTask        = "Grow 4 Trees";
    private string creaturesTask    = "Summon 3 Guardians";
    private string movementTask     = "Move your Guardians";
    private string startDayTask     = "Start the Day";
    //private string winDayTask       = "Defend the GROVE from the HUMANS";

    #endregion

    void Start()
    {
        gamePanels = _UI.inGamePanels;

        for (int i = 0; i < tutorials.Count; i++)
            SetupTutorials(GetTutorial(tutorials[i].tutorialID));

        FadeX.InstantTransparent(glossaryPanel);
        FadeX.InstantTransparent(tutorialPanel);
        FadeX.InstantTransparent(blackoutPanel);
        FadeX.InstantTransparent(tutorialPanel);
        FadeX.InstantTransparent(taskPanel);
        SetArrows();
        HideArrows(true);
        inGameContinueButton.SetActive(false);
    }

    public void SkipTutorial() 
    {
        _GLOSSARY.SetButtonInteractable(true);
        gameObject.SetActive(false);
    }

    public void StartTutorial()
    {
        ShowTutorial();
        SetInitalPanels();
    }

    private void SetInitalPanels()
    {
        FadeX.InstantAlphaValue(gamePanels.dayNightPanel, fadeStrength);
        FadeX.InstantAlphaValue(gamePanels.treePanel, fadeStrength);
        FadeX.InstantAlphaValue(gamePanels.toolPanel, fadeStrength);
        FadeX.InstantAlphaValue(gamePanels.combatPanel, fadeStrength);
        FadeX.InstantAlphaValue(gamePanels.speedPanel, fadeStrength);
        FadeX.InstantAlphaValue(gamePanels.unitPanel, fadeStrength);
        FadeX.InstantAlphaValue(gamePanels.resourcesPanel, fadeStrength);
        FadeX.InstantAlphaValue(gamePanels.perksPanel, fadeStrength);
        FadeX.InstantAlphaValue(gamePanels.mapPanel, fadeStrength);

        gamePanels.dayNightPanel.GetComponent<InGamePanel>().ToggleShiny(false);
        gamePanels.treePanel.GetComponent<InGamePanel>().ToggleShiny(false);
        gamePanels.toolPanel.GetComponent<InGamePanel>().ToggleShiny(false);
        gamePanels.combatPanel.GetComponent<InGamePanel>().ToggleShiny(false);
        gamePanels.speedPanel.GetComponent<InGamePanel>().ToggleShiny(false);

        FadeX.InstantOpaque(taskPanel);
        HideArrows(true);
        _UI.SetBuildingToggleShiny(false);
        _GLOSSARY.SetButtonInteractable(false);
    }

    private void ShowAllPanels()
    {
        //FadeX.FadeIn(gamePanels.dayNightPanel);
        FadeX.FadeIn(gamePanels.treePanel);
        FadeX.FadeIn(gamePanels.toolPanel);
        FadeX.FadeIn(gamePanels.combatPanel);
        FadeX.FadeIn(gamePanels.speedPanel);
        FadeX.FadeIn(gamePanels.unitPanel);
        FadeX.FadeIn(gamePanels.resourcesPanel);
        FadeX.FadeIn(gamePanels.perksPanel);
        FadeX.FadeIn(gamePanels.mapPanel);
    }

    private void SetArrows()
    {
        arrowList.Add(arrows.maegenArrow);
        arrowList.Add(arrows.treeToolArrow);
        arrowList.Add(arrows.treeTopArrow);
        arrowList.Add(arrows.unitArrow);
        arrowList.Add(arrows.dayNightArrow);
        arrowList.Add(arrows.populousArrow);
        arrowList.Add(arrows.wildlifeArrow);
        arrowList.Add(arrows.glossaryArrow);
    }

    private void HideArrows(bool _instant = false)
    {
        for (int i = 0; i < arrowList.Count; i++)
        {
            if(_instant)
                FadeX.InstantTransparent(arrowList[i]);
            else
                FadeX.FadeOut(arrowList[i]);
        }
    }

    #region Setup
    /// <summary>
    /// Here we setup every aspect of the tutorial object
    /// </summary>
    /// <param name="_tutorial">The tutorial to setup</param>
    private void SetupTutorials(Tutorial _tutorial)
    {
        switch(_tutorial.tutorialID)
        {
            case TutorialID.Story:
                _tutorial.title = "Welcome";
                _tutorial.description =
                    "Welcome to your Grove.<br>" +
                    "It used to be peaceful and prosperous, but those days are ending.<br>" +
                    "You must protect it from those who seek to do damage.";
                _tutorial.showContinueButton = true;
                _tutorial.lockCamera = true;
                break;
            case TutorialID.CameraMove:
                _tutorial.title = "Camera Controls";
                _tutorial.description = 
                    $"{_ICONS.GetTMPIcon(_ICONS.keyboardMoveIcon)}or move the mouse cursor to the edge of the screen to MOVE the camera.<br><br>" +
                    $"Hold {_ICONS.GetTMPIcon(_ICONS.keyboardShiftIcon)} to hasten camera movement.";
                _tutorial.taskLine = moveCameraTask;
                _tutorial.unlockedGlossaryID = GlossaryID.CameraControls;
                break;
            case TutorialID.CameraRotate:
                _tutorial.title = "Camera Controls";
                _tutorial.description = $"{_ICONS.GetTMPIcon(_ICONS.mouseRotate)} to ROTATE the camera.";
                _tutorial.taskLine = rotateCameraTask;
                _tutorial.unlockedGlossaryID = GlossaryID.CameraControls;
                break;
            case TutorialID.CameraZoom:
                _tutorial.title = "Camera Controls";
                _tutorial.description = $"{_ICONS.GetTMPIcon(_ICONS.mouseZoom)} to ZOOM the camera.";
                _tutorial.taskLine = zoomCameraTask;
                _tutorial.unlockedGlossaryID = GlossaryID.CameraControls;
                break;
            case TutorialID.Maegen:
                _tutorial.title = "Maegen";
                _tutorial.description = 
                    $"This is your {GetName(ObjectID.Maegen)} {_ICONS.GetTMPIcon(_ICONS.maegenIcon)}.<br>" +
                    $"{GetName(ObjectID.Maegen)} is the wild energy within all natural things and serves as the lifeblood of our {GetName(ObjectID.Grove)}.<br>" + 
                    $"Spend {GetName(ObjectID.Maegen)} to grow {GetName(ObjectID.Tree, true)} that will, in turn, produce more {GetName(ObjectID.Maegen)} at the end of the {GetName(ObjectID.Day)}.";
                _tutorial.showContinueButton = true;
                _tutorial.showObjects.Add(gamePanels.resourcesPanel.gameObject);
                _tutorial.showObjects.Add(arrows.maegenArrow.gameObject);
                _tutorial.unlockedGlossaryID = GlossaryID.Maegen;
                _tutorial.resetCamera = true;
                _tutorial.lockCamera = true;
                _tutorial.blackout = true;
                break;
            case TutorialID.Trees:
                _tutorial.title = "Trees";
                _tutorial.description =
                    $"The productivity of each {GetName(ObjectID.Tree)} {_ICONS.GetTMPIcon(_ICONS.treeIcon)} is determined by its proximity to others in the {GetName(ObjectID.Grove)}. <br>" + 
                    $"{GetName(ObjectID.Tree, true)} clustered together are less productive but easier to defend, while those spread out yield more {GetName(ObjectID.Maegen)} but are more vulnerable to attack.";
                _tutorial.showContinueButton = true;
                _tutorial.showObjects.Add(arrows.treeTopArrow.gameObject);
                _tutorial.unlockedGlossaryID = GlossaryID.Trees;
                _tutorial.resetCamera = true;
                _tutorial.lockCamera = true;
                _tutorial.blackout = true;
                break;
            case TutorialID.PlantTree:
                _tutorial.title = "Trees";
                _tutorial.description =
                    $"To grow a {GetName(ObjectID.Tree)}, click on the {GetName(ObjectID.Tree)} tool then {_ICONS.GetTMPIcon(_ICONS.mouseLeftClick)} on an available space in our {GetName(ObjectID.Grove)}.<br>" +
                    $"To deselect the {GetName(ObjectID.Tree)} tool, {_ICONS.GetTMPIcon(_ICONS.mouseRightClick)}.<br>" +
                    $"The higher the %, the better your {GetName(ObjectID.Maegen)} yield. " +
                    $"You can only plant trees during {GetName(ObjectID.Night)}";
                _tutorial.taskLine = treesTask;
                _tutorial.showObjects.Add(gamePanels.treePanel.gameObject);
                _tutorial.showObjects.Add(arrows.treeToolArrow.gameObject);
                _tutorial.unlockedGlossaryID = GlossaryID.Trees;

                break;
            case TutorialID.Guardians:
                _tutorial.title = "Guardians";
                _tutorial.description = 
                    $"{GetName(ObjectID.Guardian, true)} are your servants. Use them to keep control of the {GetName(ObjectID.Grove)}!<br>" +
                    //$"Each {GetName(ObjectID.Creature)} requires a different {GetName(ObjectID.Maegen)} cost to summon it.<br>" +
                    $"Open the {GetName(ObjectID.HomeTree)} panel to start summoning {GetName(ObjectID.Guardian, true)}.<br>" +
                    $"You can also {_ICONS.GetTMPIcon(_ICONS.mouseLeftClick)} on the {GetName(ObjectID.HomeTree)} to start summoning {GetName(ObjectID.Guardian, true)}.<br>";
                _tutorial.taskLine = creaturesTask;
                _tutorial.showObjects.Add(gamePanels.unitPanel.gameObject);
                _tutorial.showObjects.Add(arrows.unitArrow.gameObject);
                _tutorial.unlockedGlossaryID = GlossaryID.GuardianMovement;
                break;
            case TutorialID.GuardianMovement:
                _tutorial.title = "Guardian Movement";
                _tutorial.description =
                    $"{_ICONS.GetTMPIcon(_ICONS.mouseLeftClick)} on a {GetName(ObjectID.Guardian)} to select it or {_ICONS.GetTMPIcon(_ICONS.mouseLeftClick)} and drag over multiple {GetName(ObjectID.Guardian, true)} to select more than one.<br>" +
                    $"With selected {GetName(ObjectID.Guardian)}(S), {_ICONS.GetTMPIcon(_ICONS.mouseRightClick)} on a location to send them there.<br>" +
                    $"Our {GetName(ObjectID.Guardian, true)} will defend that location if {GetName(ObjectID.Human, true)} come within their range.<br>";
                _tutorial.taskLine = movementTask;
                _tutorial.unlockedGlossaryID = GlossaryID.GuardianMovement;
                break;
            case TutorialID.Wildlife:
                _tutorial.title = "Wildlife";
                _tutorial.description =
                    $"{GetName(ObjectID.Wildlife)} {_ICONS.GetTMPIcon(_ICONS.wildlifeIcon)} are an important part of the {GetName(ObjectID.Grove)} and required for you to use your special powers.<br>" +
                    $"They will spawn in at the end of each day, based on how many trees are in your {GetName(ObjectID.Grove)}.<br>" +
                    $"Hold down the ALT button to highlight your existing {GetName(ObjectID.Wildlife)} and protect them at all costs!";
                _tutorial.showContinueButton = true;
                _tutorial.showObjects.Add(arrows.wildlifeArrow.gameObject);
                _tutorial.unlockedGlossaryID = GlossaryID.Wildlife;
                _tutorial.resetCamera = true;
                _tutorial.lockCamera = true;
                _tutorial.blackout = true;
                break;
            case TutorialID.Populous:
                _tutorial.title = "Populous";
                _tutorial.description =
                    $"There is a maximum population of {GetName(ObjectID.Guardian, true)} you can have at one time.<br>" +
                    $"The max {GetName(ObjectID.Populous)} {_ICONS.GetTMPIcon(_ICONS.populousIcon)} can be upgrade by +5 with a {GetName(ObjectID.Perk)}.<br>" +
                    $"Press DELETE with a selected {GetName(ObjectID.Guardian)} to destroy it, in order to reduce your {GetName(ObjectID.Populous)} level";
                _tutorial.showContinueButton = true;
                _tutorial.showObjects.Add(arrows.populousArrow.gameObject);
                _tutorial.unlockedGlossaryID = GlossaryID.Populous;
                _tutorial.lockCamera = true;
                _tutorial.blackout = true;
                break;
            case TutorialID.DayNightCycle:
                _tutorial.title = "Day/Night";
                _tutorial.description =
                    $"Under the cover of {GetName(ObjectID.Night)}, build and rebuild your {GetName(ObjectID.Grove)}.<br>" +
                    $"{GetName(ObjectID.Human, true)} will arrive during the day so be prepared to fight back.<br>" +
                    $"When you are ready, click the {GetName(ObjectID.Day)}/{GetName(ObjectID.Night)} button to begin defending the forest";
                _tutorial.taskLine = startDayTask;
                _tutorial.showObjects.Add(gamePanels.unitPanel.gameObject);
                _tutorial.showObjects.Add(gamePanels.treePanel.gameObject);
                _tutorial.showObjects.Add(gamePanels.dayNightPanel.gameObject);
                _tutorial.showObjects.Add(arrows.dayNightArrow.gameObject);
                _tutorial.showContinueButton = true;
                _tutorial.unlockedGlossaryID = GlossaryID.DayNightCycle;
                break;
            case TutorialID.Glossary:
                _tutorial.title = "Glossary";
                _tutorial.description =
                    "When you see this popup, it means there is new information.<br>" +
                    "Click on the highlighted text or '?' to view.<br>" +
                    "";
                _tutorial.showObjects.Add(arrows.glossaryArrow.gameObject);
                _tutorial.resetCamera = true;
                _tutorial.lockCamera = true;
                _tutorial.blackout = true;
                break;
            
        }
        SetupTaskLines();
    }

    public void SetupTaskLines()
    {
        for (int i = 0; i < tutorials.Count; i++)
        {
            for(int j =0; j < taskLines.Count; j++)
            {
                if (tutorials[i].tutorialID == taskLines[j].taskID)
                    taskLines[j].SetText(tutorials[i].taskLine);
            }
        }
    }

    #endregion

    private void GetNextTutorial()
    {
        if (tutorials.Count == 0) 
            return;

        tutorials.Remove(CurrentTutorial);

        if (tutorials.Count == 0)
        {
            HideTutorialPanel();
            return;
        }
        else
            currentTutorialID = tutorials[0].tutorialID;
    }


    public void ShowTutorial()
    {
        if (CurrentTutorial == null)
            return;

        tutorialTitle.text = CurrentTutorial.title;
        tutorialDescription.text = CurrentTutorial.description;
        ToggleObjects();
        Blackout();
        FadeX.FadeIn(tutorialPanel);

        for (int i = 0; i < taskLines.Count; i++)
        {
            if (currentTutorialID == taskLines[i].taskID)
                taskLines[i].ActivateTask();
        }

        _GLOSSARY.UnlockGlossaryItem(CurrentTutorial.unlockedGlossaryID);
        inGameContinueButton.SetActive(CurrentTutorial.showContinueButton);

        _CAMERA.ResetCameraToStart(CurrentTutorial.resetCamera);
        _CAMERA.LockCamera(CurrentTutorial.lockCamera);

        if (currentTutorialID == TutorialID.PlantTree)
            gamePanels.treePanel.GetComponent<InGamePanel>().ToggleOnActiveShiny();

        if (currentTutorialID == TutorialID.Wildlife)
            _GM.WildlifeInstantiate(true);

        if (currentTutorialID == TutorialID.Glossary)
        {
            _GLOSSARY.NewGlossaryAvailable(GlossaryID.HomeTree, "Home Tree", false);
            _GLOSSARY.SetButtonInteractable(true);
            _GLOSSARY.SetButtonActive(true);
        }
    }

    public void HideTutorialPanel()
    {
        FadeX.FadeOut(tutorialPanel);
    }

    private void ToggleObjects()
    {
        HideArrows();
        for (int i = 0; i < CurrentTutorial.showObjects.Count; i++)
        {
            CurrentTutorial.showObjects[i].SetActive(true);
            if (CurrentTutorial.showObjects[i].GetComponent<CanvasGroup>() != null)
                FadeX.FadeIn(CurrentTutorial.showObjects[i].GetComponent<CanvasGroup>());
        }
    }

    private void Blackout()
    {
        if (CurrentTutorial.blackout)
        {
            if (currentTutorialID == TutorialID.Wildlife)
            {
                _EFFECTS.TweenVignette(1);
                FadeX.FadeOut(blackoutPanel);
            }
            else
            {
                _EFFECTS.TweenVignetteReset();
                FadeX.FadeTo(blackoutPanel, _TWEENING.blackoutPanelFade);
            }
        }
        else
        {
            _EFFECTS.TweenVignetteReset();
            FadeX.FadeOut(blackoutPanel);
        }
    }

    public void ContinueButton()
    {
        if(currentTutorialID == TutorialID.DayNightCycle)
        {
            gamePanels.dayNightPanel.GetComponent<InGamePanel>().ToggleOnActiveShiny();
            _GM.ChangeGameState(GameState.Build);
        }

        GetNextTutorial();
        ShowTutorial();
    }

    private IEnumerator WaitForNextTask()
    {
        CurrentTutorial.completed = true;

        if(CurrentTutorial.tutorialID == TutorialID.CameraMove || CurrentTutorial.tutorialID == TutorialID.CameraRotate || CurrentTutorial.tutorialID == TutorialID.CameraZoom)
        {
            for (int i = 0; i < taskLines.Count; i++)
            {
                if (currentTutorialID == taskLines[i].taskID)
                    CheckOffTask(taskLines[i].taskID);
            }
            yield return new WaitForSeconds(cameraTaskTime);
        }

        GetNextTutorial();
        ShowTutorial();
    }

    public void CheckCameraTutorial(TutorialID _tutorialID)
    {
        if (_DATA.currentLevelID != LevelID.Ironwood)
            return;

        if (_currentGameState != GameState.Tutorial)
            return;

        if (tutorialComplete)
            return;

        if (currentTutorialID != _tutorialID)
            return;

        if (CurrentTutorial.completed)
            return;

        StartCoroutine(WaitForNextTask());
    }

    public void ClosedGlossary()
    {
        if(_tutorialComplete || currentTutorialID != TutorialID.Glossary)
            return;

        HideArrows();
        FadeX.InstantTransparent(tutorialPanel);
        ExecuteAfterSeconds(panelDelay, () =>
        {
            GetNextTutorial();
            ShowTutorial();
        });
    }

    #region Events
    private void OnToolButtonPressed(ToolID _toolID)
    {
        if (tutorialComplete)
            return;

        if (currentTutorialID != TutorialID.PlantTree)
            return;

        if (_toolID == ToolID.Tree)
            HideTutorialPanel();
    }

    //Trees
    private void OnTreePlaced(ToolID _toolID)
    {
        if (tutorialComplete || _currentGameState != GameState.Tutorial)
            return;

        treeCount++;
        TaskLine tl = taskLines.Find(x => x.taskID == TutorialID.PlantTree);
        tl.text.text = treesTask + " (" + treeCount + "/" + treeCompletionCount + ")";
        tl.PulseTask();
        if (treeCount == treeCompletionCount)
        {
            GetNextTutorial();
            ShowTutorial();
            CheckOffTask(TutorialID.PlantTree);
            _PC.DeselectAllTools();
            _GM.SetPlayMode(PlayMode.DefaultMode);
            FadeX.FadeTo(gamePanels.treePanel, fadeStrength);
        }
    }

    //Summon Creatures
    private void OnUnitButtonPressed(UnitData _unitData)
    {
        if (tutorialComplete || currentTutorialID != TutorialID.Guardians)
            return;

        HideTutorialPanel();

        creatureCount++;
        TaskLine tl = taskLines.Find(x => x.taskID == TutorialID.Guardians);
        tl.text.text = creaturesTask + " (" + creatureCount + "/" + creatureCompletionCount + ")";
        tl.PulseTask();
        if (creatureCount == creatureCompletionCount)
        {
            GetNextTutorial();
            ShowTutorial();
            CheckOffTask(TutorialID.Guardians);
            FadeX.FadeTo(gamePanels.unitPanel, fadeStrength);
        }
    }

    private void OnUnitMove()
    {
        if (tutorialComplete || currentTutorialID != TutorialID.GuardianMovement)
            return;

        HideTutorialPanel();

        moveCreatureCount++;
        TaskLine tl = taskLines.Find(x => x.taskID == TutorialID.GuardianMovement);
        tl.text.text = movementTask + " (" + moveCreatureCount + "/" + moveCreatureCompletionCount + ")";
        tl.PulseTask();

        if (moveCreatureCount == moveCreatureCompletionCount)
        {
            ExecuteAfterSeconds(panelDelay, () =>
            {
                GetNextTutorial();
                ShowTutorial();
                CheckOffTask(TutorialID.GuardianMovement);
            });
        }
    }

    public void OnDayBegin(int _day)
    {
        if (tutorialComplete)
            return;

        CheckOffTask(TutorialID.DayNightCycle);
        ShowAllPanels();
        HideArrows();
        HideTutorialPanel();
        tutorialComplete = true;
        _SAVE.SetTutorialComplete();

        gamePanels.toolPanel.GetComponent<InGamePanel>().ToggleOnActiveShiny();
        gamePanels.combatPanel.GetComponent<InGamePanel>().ToggleOnActiveShiny();
        gamePanels.speedPanel.GetComponent<InGamePanel>().ToggleOnActiveShiny();

        GameEvents.ReportOnTutorialFinished();

        ExecuteAfterSeconds(3, () => FadeX.FadeOut(taskPanel));
    }

    private void CheckOffTask(TutorialID _id)
    {
        if (taskLines.Find(x => x.taskID == _id))
            taskLines.Find(x => x.taskID == _id).CheckOffTask();
    }

    void OnHomeTreeSelected()
    {
        //if (firstPlay == false && firstHomeTree == false)
        //{
        //    firstHomeTree = true;
        //    NewTutorialAvailable(TutorialID.HomeTree, "Home Tree");
        //}
    }
    
    private void OnEnable()
    {
        GameEvents.OnToolButtonPressed += OnToolButtonPressed;
        GameEvents.OnTreePlaced += OnTreePlaced;
        GameEvents.OnUnitButtonPressed += OnUnitButtonPressed;
        GameEvents.OnUnitMove += OnUnitMove;
        GameEvents.OnDayBegin += OnDayBegin;
        GameEvents.OnHomeTreeSelected += OnHomeTreeSelected;
    }

    

    private void OnDisable()
    {
        GameEvents.OnToolButtonPressed -= OnToolButtonPressed;
        GameEvents.OnTreePlaced -= OnTreePlaced;
        GameEvents.OnUnitButtonPressed -= OnUnitButtonPressed;
        GameEvents.OnUnitMove -= OnUnitMove;
        GameEvents.OnDayBegin -= OnDayBegin;
        GameEvents.OnHomeTreeSelected -= OnHomeTreeSelected;
    }
    #endregion

    #region Editor
    #if UNITY_EDITOR
    // IngredientDrawerUIE
    [CustomPropertyDrawer(typeof(Tutorial))]
    public class TutorialUIE : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var idField = new PropertyField(property.FindPropertyRelative("tutorialID"));
            var imageField = new PropertyField(property.FindPropertyRelative("image"));
            //var continueField = new PropertyField(property.FindPropertyRelative("showContinueButton"));
            var showField = new PropertyField(property.FindPropertyRelative("showObjects"));
            var hideField = new PropertyField(property.FindPropertyRelative("hideObjects"));

            // Add fields to the container.
            container.Add(idField);
            //container.Add(imageField);
            //container.Add(continueField);
            //container.Add(showField);
            //container.Add(hideField);

            //int i = container.childCount;
            //container.style.backgroundColor = i % 1 == 0 ? Color.green : Color.black;

            return container;
        }
    }
#endif
#endregion
}

[System.Serializable]
public class Tutorial
{
    public TutorialID tutorialID;
    public string title;
    public Sprite image;
    public bool showContinueButton;
    public bool closePanelAfter;
    public bool resetCamera;
    public bool lockCamera;
    public bool blackout;
    public string description;
    public string taskLine;
    public bool completed;
    public List<GameObject> showObjects;
    public List<GameObject> hideObjects;
    public GlossaryID unlockedGlossaryID;
}

[System.Serializable]
public class TutorialArrow
{
    public CanvasGroup maegenArrow;
    public CanvasGroup treeToolArrow;
    public CanvasGroup treeTopArrow;
    public CanvasGroup wildlifeArrow;
    public CanvasGroup populousArrow;
    public CanvasGroup unitArrow;
    public CanvasGroup dayNightArrow;
    public CanvasGroup glossaryArrow;
}