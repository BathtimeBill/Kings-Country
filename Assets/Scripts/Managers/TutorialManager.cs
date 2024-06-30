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
    [Tooltip("Untick to skip tutorial")]
    public bool playTutorial;
    [Tooltip("Just to skip the intro camera")]
    public bool overrideStartTime = true;

    [Header("Tutorial Chunks")]
    public TutorialID currentTutorialID;
    public List<Tutorial> tutorials;
    [BV.EnumList(typeof(TutorialID))]
    public List<TutorialID> tutorialList;
    public List<TaskLine> taskLines;

    [Header("Basic")]
    public CanvasGroup glossaryPanel;
    public int tutorialCount = 0;
    public int maxTutorialCount;

    [Header("In Game Tutorial")]
    public TMP_Text tutorialTitle;
    public TMP_Text tutorialDescription;
    public TMP_Text taskText;
    public GameObject check;
    public CanvasGroup inGameTutorialPanel;
    public GameObject taskPanel;
    public GameObject inGameContinueButton;
    public GameObject treeButton;
    public GameObject maegenIcon;
    public bool hasCompletedTask;
    public GameObject contextArrow;
    public GameObject inWorldArrow;
    public int tutorialStage;
    public bool isTutorial;

    [Header("New")]
    public GameObject newTutorialButton;
    public TMP_Text newTutorialTitle;
    public bool firstPlay;
    public bool firstWave;
    public bool firstMine;
    public bool firstLord;
    public bool firstLevel2;
    public bool firstSpy;
    public bool firstHomeTree;

    public Tutorial GetTutorial(TutorialID _id) => tutorials.Find(x => x.tutorialID == _id);
    private Tutorial CurrentTutorial => GetTutorial(currentTutorialID);
    public TutorialID current => tutorialList[0];

    void Start()
    {
        //TODO save and load from file
        //_SAVE.Load();
        //CheckTutorial();
        //CheckInGameTutorial();
        //StartCoroutine(WaitForStartCamera());
        //if (newTutorialButton != null)
        //{
        //    CheckTutorial();
        //    StartCoroutine(WaitForStartCamera());
        //}

        for (int i = 0; i < tutorials.Count; i++)
            SetupTutorials(GetTutorial(tutorials[i].tutorialID));
        
        currentTutorialID = tutorialList[0];

        FadeX.InstantTransparent(glossaryPanel);
        FadeX.InstantTransparent(inGameTutorialPanel);

        if (playTutorial)
        {
            _GM.ChangeGameState(GameState.Tutorial);
            ShowTutorial(currentTutorialID);
            ExecuteAfterSeconds(overrideStartTime ? 1 : _SETTINGS.general.introCameraDuration, () => FadeX.FadeIn(inGameTutorialPanel));
        }
        else
            FadeX.InstantTransparent(inGameTutorialPanel);

        inGameContinueButton.SetActive(false);
    }

    private void SetupTutorials(Tutorial _tutorial)
    {
        switch(_tutorial.tutorialID)
        {
            case TutorialID.CameraMove:
                _tutorial.title = "Camera Controls";
                _tutorial.description = "To MOVE the camera, use the ‘W,A,S,D’ keys or move the mouse cursor to the edge of the screen.\r\n<br>Hold shift to hasten camera movement.";
                break;
            case TutorialID.CameraRotate:
                _tutorial.title = "Camera Controls";
                _tutorial.description = "To ROTATE the camera, click and drag the ‘Middle Mouse Button’ to the left or right.";
                break;
            case TutorialID.CameraZoom:
                _tutorial.title = "Camera Controls";
                _tutorial.description = "To ZOOM the camera, scroll the Mouse Wheel in and out.";
                break;
            case TutorialID.Maegen:
                _tutorial.title = "Maegen";
                _tutorial.description = "This is your MAEGEN. \r\n<br>MAEGEN is the wild energy within all natural things and serves as the lifeblood of our grove. Spend MAEGEN to grow TREES that will, in turn, produce more MAEGEN at the end of the DAY.";
                break;
            case TutorialID.Trees:
                _tutorial.title = "Trees";
                _tutorial.description = "The productivity of each TREE is determined by its proximity to others in the GROVE. TREES clustered together are less productive but easier to defend, while those spread out yield more MAEGEN but are more vulnerable to attack.\r\n<br>To grow a tree, click on the TREE button and Left-Click on an available space in our domain.";
                break;
        }
    }

    public void ShowTutorial(TutorialID _tutorialID)
    {
        if (CurrentTutorial == null)
            return;

        tutorialTitle.text = CurrentTutorial.title;
        tutorialDescription.text = CurrentTutorial.description;
        ObjectX.ToggleObjects(CurrentTutorial.showObjects, true);
        ObjectX.ToggleObjects(CurrentTutorial.hideObjects, false);

        for (int i = 0; i < taskLines.Count; i++)
        {
            if (currentTutorialID == taskLines[i].taskID)
                taskLines[i].ActivateTask();
        }

        //tutorialList.Remove(t.tutorialID);
        CheckTaskList();
    }

    public void NewTutorialAvailable(TutorialID id, string title)
    {
        currentTutorialID = id;
        newTutorialTitle.text = title;
        newTutorialButton.SetActive(true);
        newTutorialButton.GetComponent<Animator>().SetTrigger("TutorialAvailable");
        newTutorialButton.GetComponent<AudioSource>().Play();
    }

    public void ContinueButton()
    {
        tutorialStage++;
        CheckInGameTutorial();
        CheckTaskList();
    }

    public void CheckInGameTutorial()
    {
        ShowTutorial(currentTutorialID);
        /*if(tutorialStage == 0)
        {
            tutorialTitle.text = "Camera Controls";
            tutorialDescription.text = "To MOVE the camera, use the ‘W,A,S,D’ keys or move the mouse cursor to the edge of the screen.\r\n<br>Hold shift to hasten camera movement.";
        }
        if (tutorialStage == 1)
        {
            tutorialTitle.text = "Camera Controls";
            tutorialDescription.text = "To ROTATE the camera, click and drag the ‘Middle Mouse Button’ to the left or right.";
        }
        if (tutorialStage == 2)
        {
            tutorialTitle.text = "Camera Controls";
            tutorialDescription.text = "To ZOOM the camera, scroll the Mouse Wheel in and out.";
        }
        if (tutorialStage == 3)
        {
            FadeX.FadeOut(inGameTutorialPanel);
            taskPanel.SetActive(false);
            currentTutorialID = TutorialID.DayNightCycle;
         
        }
        if (tutorialStage == 4)
        {
            tutorialTitle.text = "Maegen";
            tutorialDescription.text = "This is your MAEGEN. \r\n<br>MAEGEN is the wild energy within all natural things and serves as the lifeblood of our grove. Spend MAEGEN to grow TREES that will, in turn, produce more MAEGEN at the end of the DAY.";
            inGameContinueButton.SetActive(true);
            contextArrow.SetActive(true);
            contextArrow.transform.rotation = Quaternion.Euler(0, 0, 180);
            contextArrow.transform.position = maegenIcon.transform.position;
        }
        if (tutorialStage == 5)
        {
            tutorialTitle.text = "Trees";
            tutorialDescription.text = "The productivity of each TREE is determined by its proximity to others in the GROVE. TREES clustered together are less productive but easier to defend, while those spread out yield more MAEGEN but are more vulnerable to attack.\r\n<br>To grow a tree, click on the TREE button and Left-Click on an available space in our domain.";
            contextArrow.SetActive(true);
            inGameContinueButton.SetActive(false);
            contextArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
            contextArrow.transform.position = treeButton.transform.position;
        }*/
    }
    public void CheckTaskList()
    {
        StartCoroutine(WaitForNextTask());

        /*if (currentTutorialID == TutorialID.CameraMove)
        {
            taskText.text = "Move the camera around the Grove";
            if(hasCompletedTask)
            {
                StartCoroutine(WaitForNextTask());
            }
        }
        if (currentTutorialID == TutorialID.CameraRotate)
        {
            taskText.text = "Rotate the camera";
            if (hasCompletedTask)
            {
                StartCoroutine(WaitForNextTask());
            }
        }
        if (currentTutorialID == TutorialID.CameraZoom)
        {
            taskText.text = "Zoom the camera";
            if (hasCompletedTask)
            {
                StartCoroutine(WaitForNextTask());
            }
        }*/
    }
    private IEnumerator WaitForNextTask()
    {
        CurrentTutorial.completed = true;
        for (int i = 0; i < taskLines.Count; i++)
        {
            if(currentTutorialID == taskLines[i].taskID)
                taskLines[i].CheckOffTask();
        }

        //check.SetActive(true);
        yield return new WaitForSeconds(3);
        //tutorialList.Remove(currentTutorialID);
        tutorialStage++;
        hasCompletedTask = false;
        //check.SetActive(false);
        CheckTaskList();
        CheckInGameTutorial();
    }

    public void CheckCameraTutorial(TutorialID _tutorialID)
    {
        if (currentTutorialID != _tutorialID)
            return;

        if (CurrentTutorial.completed)
            return;

        ShowTutorial(_tutorialID);
        print(_tutorialID.ToString());
        //if (isTutorial && current == _tutorialID)
        //{
        //    if (hasCompletedTask == false)
        //    {
        //        hasCompletedTask = true;
        //        CheckTaskList();
        //    }
        //}
    }

    #region Events
    public void OnDayBegin()
    {
        if (!playTutorial)
            return;

        if (firstPlay == false)
        {
            if(_GM.level == LevelNumber.One && _GM.currentDay == 1)
            {
                NewTutorialAvailable(TutorialID.HumanClasses, "Human Classes");
                StartCoroutine(WaitToAddCombatTutorial());
            }
        }
    }
    IEnumerator WaitToAddCombatTutorial()
    {
        yield return new WaitForSeconds(10);
        NewTutorialAvailable(TutorialID.Combat, "Combat");
    }
    public void OnDayOver()
    {
        if(isTutorial)
        {
            if(tutorialStage == 8)
            {
                GameEvents.ReportOnNextTutorial();
            }
        }
    }
    public void OnContinueButton()
    {
        if(isTutorial)
        {
            //if(tutorialStage == 9)
            //{
            //    GameEvents.ReportOnNextTutorial();
            //}
        }
    }
    void OnLevelWin(LevelID _levelID, int _score, int _maegen)
    {
        firstPlay = true;
    }
    void OnMineSpawned()
    {
        if(firstMine == false)
        {
            firstMine = true;
            NewTutorialAvailable(TutorialID.Mines, "Mines");
        }
    }
    void OnLordSpawned()
    {
        if (firstLord == false)
        {
            firstLord = true;
            NewTutorialAvailable(TutorialID.LordsOfTheLand, "Lords of the Land");
        }
    }
    void OnSpySpawned()
    {
        if(firstSpy == false)
        {
            firstSpy = true;
            NewTutorialAvailable(TutorialID.Spies, "Spies");
        }
    }
    void OnHomeTreeSelected()
    {
        if (firstPlay == false && firstHomeTree == false)
        {
            firstHomeTree = true;
            NewTutorialAvailable(TutorialID.HomeTree, "Home Tree");
        }
    }
    private void OnEnable()
    {
        GameEvents.OnDayBegin += OnDayBegin;
        GameEvents.OnDayOver += OnDayOver;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnLevelWin += OnLevelWin;
        GameEvents.OnMineSpawned += OnMineSpawned;
        GameEvents.OnLordSpawned += OnLordSpawned;
        GameEvents.OnSpySpawned += OnSpySpawned;
        GameEvents.OnHomeTreeSelected += OnHomeTreeSelected;
    }
    private void OnDisable()
    {
        GameEvents.OnDayBegin -= OnDayBegin;
        GameEvents.OnDayOver -= OnDayOver;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnLevelWin -= OnLevelWin;
        GameEvents.OnMineSpawned -= OnMineSpawned;
        GameEvents.OnLordSpawned -= OnLordSpawned;
        GameEvents.OnSpySpawned -= OnSpySpawned;
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
            var showField = new PropertyField(property.FindPropertyRelative("showObjects"));
            var hideField = new PropertyField(property.FindPropertyRelative("hideObjects"));

            // Add fields to the container.
            container.Add(idField);
            container.Add(imageField);
            container.Add(showField);
            container.Add(hideField);

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
    [HideInInspector] public string description;
    [HideInInspector] public bool completed;
    public GameObject[] showObjects;
    public GameObject[] hideObjects;
}