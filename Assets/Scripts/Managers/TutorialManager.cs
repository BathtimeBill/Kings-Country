using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;

public class TutorialManager : GameBehaviour
{
    //TEMP
    [Tooltip("Will override the save data values")]
    public bool overrideTutorial;
    [DrawIf("overrideTutorial", true)]
    public bool showTutorial;
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
    public bool hasCompletedTask;
    public GameObject contextArrow;
    public GameObject inWorldArrow;
    public int tutorialStage;
    public bool isTutorial;

    [HideInInspector] public bool tutorialComplete;
    private int treeCount = 0;
    private int treeCompletionCount = 4;

    private int creatureCount = 0;
    private int creatureCompletionCount = 3;

    private float fadeStrength = 0.1f;

    private InGamePanels gamePanels;
    private List<CanvasGroup> arrowList = new List<CanvasGroup>();

    private Tutorial GetTutorial(TutorialID _id) => tutorials.Find(x => x.tutorialID == _id);
    private Tutorial CurrentTutorial => GetTutorial(currentTutorialID);

    #region Text Variables
    private string moveCameraTask   = "Move the camera around the GROVE";
    private string rotateCameraTask = "Rotate the camera";
    private string zoomCameraTask   = "Zoom the camera";
    private string treesTask        = "Grow 4 Trees";
    private string creaturesTask    = "Summon 4 CREATURES";
    private string startDayTask     = "Start the Day";
    private string winDayTask       = "Defend the GROVE from the HUMANS";

    #endregion

    void Start()
    {
        gamePanels = _UI.inGamePanels;

        for (int i = 0; i < tutorials.Count; i++)
            SetupTutorials(GetTutorial(tutorials[i].tutorialID));

        FadeX.InstantTransparent(glossaryPanel);
        FadeX.InstantTransparent(tutorialPanel);
        SetArrows();

        //if(debugStartOffset > 0)
        //{
        //    for(int  i = 0;i <= tutorials.Count;i++)
        //    {
        //        tutorials.Remove(tutorials[i]);
        //    }
        //    currentTutorialID = tutorials[0].tutorialID;
        //}

        //Debug Stuff
        if (overrideTutorial)
            tutorialComplete = !showTutorial;
        else
            tutorialComplete = _SAVE.TutorialComplete;

        if (tutorialComplete)
        {
            FadeX.InstantTransparent(tutorialPanel);
            FadeX.InstantTransparent(taskPanel);
            HideArrows(true);
            _GLOSSARY.SetInteractable(true);
        }
        else
        {
            _GM.ChangeGameState(GameState.Tutorial);
            ShowTutorial();
            SetInitalPanels();
            ExecuteAfterSeconds(overrideStartTime ? 1 : _SETTINGS.general.introCameraDuration, () => FadeX.FadeIn(tutorialPanel));
        }

        inGameContinueButton.SetActive(false);
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

        FadeX.InstantOpaque(taskPanel);
        HideArrows(true);
        _GLOSSARY.SetInteractable(false);
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
        arrowList.Add(arrows.unitArrow);
        arrowList.Add(arrows.dayNightArrow);
        arrowList.Add(arrows.populousArrow);
        arrowList.Add(arrows.wildlifeArrow);
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
            case TutorialID.CameraMove:
                _tutorial.title = "Camera Controls";
                _tutorial.description = "To MOVE the camera, use the ‘W,A,S,D’ keys or move the mouse cursor to the edge of the screen.\r\n<br>Hold shift to hasten camera movement.";
                _tutorial.taskLine = moveCameraTask;
                break;
            case TutorialID.CameraRotate:
                _tutorial.title = "Camera Controls";
                _tutorial.description = "To ROTATE the camera, click and drag the ‘Middle Mouse Button’ to the left or right.";
                _tutorial.taskLine = rotateCameraTask;
                break;
            case TutorialID.CameraZoom:
                _tutorial.title = "Camera Controls";
                _tutorial.description = "To ZOOM the camera, scroll the Mouse Wheel in and out.";
                _tutorial.taskLine = zoomCameraTask;
                break;
            case TutorialID.Maegen:
                _tutorial.title = "Maegen";
                _tutorial.description = "This is your MAEGEN. \r\n<br>MAEGEN is the wild energy within all natural things and serves as the lifeblood of our grove. Spend MAEGEN to grow TREES that will, in turn, produce more MAEGEN at the end of the DAY.";
                _tutorial.showContinueButton = true;
                _tutorial.showObjects.Add(gamePanels.resourcesPanel.gameObject);
                _tutorial.showObjects.Add(arrows.maegenArrow.gameObject);
                break;
            case TutorialID.Trees:
                _tutorial.title = "Trees";
                _tutorial.description = "The productivity of each TREE is determined by its proximity to others in the GROVE. <br><br>TREES clustered together are less productive but easier to defend, while those spread out yield more MAEGEN but are more vulnerable to attack.\r\n<br>To grow a tree, click on the TREE button and Left-Click on an available space in our domain.<br> You can only plant trees during NIGHT";
                _tutorial.taskLine = treesTask;
                _tutorial.showObjects.Add(gamePanels.treePanel.gameObject);
                _tutorial.showObjects.Add(arrows.treeToolArrow.gameObject);
                break;
            case TutorialID.CreatureMovement:
                _tutorial.title = "Creatures";
                _tutorial.description = 
                    "CREATURES are your servants. Use them to keep control of the GROVE!<br>" +
                    "Each unit requires a different MAEGEN cost to summon it.<br>" +
                    "Open the HOME TREE panel to start summoning creatures.<br>";
                _tutorial.taskLine = creaturesTask;
                _tutorial.showObjects.Add(gamePanels.unitPanel.gameObject);
                _tutorial.showObjects.Add(arrows.unitArrow.gameObject);
                break;
            case TutorialID.HomeTree:
                _tutorial.title = "Home Tree";
                _tutorial.description =
                    "This is our HOME TREE, the heart of our GROVE and vital to its survival.<br>" +
                    "If the HOME TREE is destroyed, the forest will fall.<br>" +
                    "When you click on the HOME TREE, you can bring up the CREATURES that you can summon from it.<br>" + 
                    "Use CREATURES to defend the GROVE<br>";
                _tutorial.showContinueButton = true;
                break;
            case TutorialID.Wildlife:
                _tutorial.title = "Wildlife";
                _tutorial.description =
                    "This represents your WILDLIFE.<br>" +
                    "WILDLIFE are an important part of the GROVE and required for you to use your special powers.<br>" +
                    "They will spawn in at the end of each wave, based on how many trees are in your GROVE.<br>" +
                    "Hold down the ALT button to highlight your existing WILDLIFE and protect them at all costs!";
                _tutorial.showContinueButton = true;
                _tutorial.showObjects.Add(arrows.wildlifeArrow.gameObject);
                break;
            case TutorialID.Populous:
                _tutorial.title = "Populous";
                _tutorial.description =
                    "There is a maximum population of CREATURES you can have at one time.<br>" +
                    "The max POPULOUS can be upgrade by +5 with a Perk.<br>" +
                    "Press DELETE with a selected CREATURE to destroy it, in order to reduce your POPULOUS level";
                _tutorial.showContinueButton = true;
                _tutorial.showObjects.Add(arrows.populousArrow.gameObject);
                break;
            case TutorialID.DayNightCycle:
                _tutorial.title = "Day/Night";
                _tutorial.description =
                    "Under the cover of night, build and rebuild your GROVE.<br>" +
                    "Humans will arrive during the day so be prepared to fight back.<br>" +
                    "When you are ready, click the Day/Night button to begin defending the forest";
                _tutorial.taskLine = startDayTask;
                _tutorial.showObjects.Add(gamePanels.unitPanel.gameObject);
                _tutorial.showObjects.Add(gamePanels.treePanel.gameObject);
                _tutorial.showObjects.Add(gamePanels.dayNightPanel.gameObject);
                _tutorial.showObjects.Add(arrows.dayNightArrow.gameObject);
                _tutorial.showContinueButton = true;
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
        FadeX.FadeIn(tutorialPanel);

        for (int i = 0; i < taskLines.Count; i++)
        {
            if (currentTutorialID == taskLines[i].taskID)
                taskLines[i].ActivateTask();
        }

        inGameContinueButton.SetActive(CurrentTutorial.showContinueButton);
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
            if (CurrentTutorial.showObjects[i].GetComponent<CanvasGroup>() != null)
                FadeX.FadeIn(CurrentTutorial.showObjects[i].GetComponent<CanvasGroup>());
        }
        //for (int i = 0; i < CurrentTutorial.hideObjects.Count; i++)
        //{
        //    if (CurrentTutorial.hideObjects[i].GetComponent<CanvasGroup>() != null)
        //        FadeX.FadeOut(CurrentTutorial.hideObjects[i].GetComponent<CanvasGroup>());
        //}
    }

    public void ContinueButton()
    {
        GetNextTutorial();
        ShowTutorial();

        //if (CurrentTutorial.closePanelAfter)
        //    HideTutorialPanel();
    }

    private IEnumerator WaitForNextTask()
    {
        CurrentTutorial.completed = true;

        if(CurrentTutorial.tutorialID == TutorialID.CameraMove || CurrentTutorial.tutorialID == TutorialID.CameraRotate || CurrentTutorial.tutorialID == TutorialID.CameraZoom)
        {
            for (int i = 0; i < taskLines.Count; i++)
            {
                if (currentTutorialID == taskLines[i].taskID)
                    taskLines[i].CheckOffTask();
            }
            yield return new WaitForSeconds(cameraTaskTime);
        }

        GetNextTutorial();
        ShowTutorial();
    }

    public void CheckCameraTutorial(TutorialID _tutorialID)
    {
        if (tutorialComplete)
            return;

        if (currentTutorialID != _tutorialID)
            return;

        if (CurrentTutorial.completed)
            return;

        StartCoroutine(WaitForNextTask());
    }

    #region Events
    private void OnToolButtonPressed(ToolID _toolID)
    {
        if (tutorialComplete)
            return;

        if (currentTutorialID != TutorialID.Trees)
            return;

        if (_toolID == ToolID.Tree)
            HideTutorialPanel();
    }

    //Trees
    private void OnTreePlaced(ToolID _toolID)
    {
        if (tutorialComplete)
            return;

        treeCount++;
        taskLines.Find(x => x.taskID == TutorialID.Trees).text.text = "Grow 4 trees (" + treeCount + "/" + treeCompletionCount + ")";
        if (treeCount == treeCompletionCount)
        {
            GetNextTutorial();
            ShowTutorial();
            taskLines.Find(x => x.taskID == TutorialID.Trees).CheckOffTask();
            _PC.DeselectAllTools();
            _GM.SetPlayMode(PlayMode.DefaultMode);
            FadeX.FadeTo(gamePanels.treePanel, fadeStrength);
        }
    }

    //Summon Creatures
    private void OnUnitButtonPressed(UnitData _unitData)
    {
        if (tutorialComplete)
            return;

        if (currentTutorialID != TutorialID.CreatureMovement)
            return;

        HideTutorialPanel();

        creatureCount++;
        taskLines.Find(x => x.taskID == TutorialID.CreatureMovement).text.text = "Summon 3 Creatures (" + creatureCount + "/" + creatureCompletionCount + ")";
        if (creatureCount == creatureCompletionCount)
        {
            GetNextTutorial();
            ShowTutorial();
            taskLines.Find(x => x.taskID == TutorialID.CreatureMovement).CheckOffTask();
            FadeX.FadeTo(gamePanels.unitPanel, fadeStrength);
        }
    }


    public void OnDayBegin()
    {
        if (tutorialComplete)
            return;

        taskLines.Find(x => x.taskID == TutorialID.DayNightCycle).CheckOffTask();
        ShowAllPanels();
        HideArrows();
        HideTutorialPanel();
        _GLOSSARY.SetInteractable(true);
        tutorialComplete = true;
        _SAVE.SetTutorialComplete();
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
        GameEvents.OnDayBegin += OnDayBegin;
        GameEvents.OnHomeTreeSelected += OnHomeTreeSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnToolButtonPressed -= OnToolButtonPressed;
        GameEvents.OnTreePlaced -= OnTreePlaced;
        GameEvents.OnUnitButtonPressed -= OnUnitButtonPressed;
        GameEvents.OnDayBegin -= OnDayBegin;
        GameEvents.OnHomeTreeSelected -= OnHomeTreeSelected;
    }
    #endregion

    #region Editor
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
    #endregion
}

[System.Serializable]
public class Tutorial
{
    public TutorialID tutorialID;
    [HideInInspector] public string title;
    public Sprite image;
    public bool showContinueButton;
    public bool closePanelAfter;
    [HideInInspector] public string description;
    [HideInInspector] public string taskLine;
    [HideInInspector] public bool completed;
    public List<GameObject> showObjects;
    public List<GameObject> hideObjects;
}

[System.Serializable]
public class TutorialArrow
{
    public CanvasGroup maegenArrow;
    public CanvasGroup treeToolArrow;
    public CanvasGroup wildlifeArrow;
    public CanvasGroup populousArrow;
    public CanvasGroup unitArrow;
    public CanvasGroup dayNightArrow;
}