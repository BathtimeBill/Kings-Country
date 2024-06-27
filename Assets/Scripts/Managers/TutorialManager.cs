using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    [BV.EnumList(typeof(TutorialID))]
    public List<Tutorial> tutorials;
    public TutorialID tutorialID;

    [Header("Basic")]
    public Image tutorialImage;
    public TMP_Text glossaryDescriptionText;
    public TMP_Text glossaryTitleText;
    public TMP_Text tutCounterText;
    public CanvasGroup glossaryPanel;
    public int tutorialCount = 0;
    public int maxTutorialCount;
    public Scrollbar scrollbar;
    public GameObject pageObjects;

    [Header("Sprites")]
    public Sprite welcomeSprite;
    public Sprite cameraControlsSprite;
    public Sprite creatureMovementSprite;
    public Sprite maegenSprite;
    public Sprite treesSprite;
    public Sprite wildlifeSprite;
    public Sprite populousSprite;
    public Sprite homeTreeSprite;
    public Sprite witchsHutSprite;
    public Sprite horgrSprite;
    public Sprite powersSprite;
    public Sprite dayNightCycleSprite;
    public Sprite humanClassesSprite;
    public Sprite dogsSprite;
    public Sprite minesSprite;
    public Sprite spiesprite;
    public Sprite lordsOfTheLandSprite;
    public Sprite combatSprite;

    [Header("In Game Tutorial")]
    public TMP_Text tutorialTitle;
    public TMP_Text tutorialText;
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

    //TEMP
    public bool playTutorial;


    void Start()
    {
        //_SAVE.Load();
        //CheckTutorial();
        //CheckInGameTutorial();
        //StartCoroutine(WaitForStartCamera());
        //if (newTutorialButton != null)
        //{
        //    CheckTutorial();
        //    StartCoroutine(WaitForStartCamera());
        //}
        FadeX.InstantTransparent(glossaryPanel);
        if (playTutorial)
        {
            FadeX.InstantOpaque(inGameTutorialPanel);
            _GM.ChangeGameState(GameState.Tutorial);
        }
        else
            FadeX.InstantTransparent(inGameTutorialPanel);

        inGameContinueButton.SetActive(false);
    }
    IEnumerator WaitForStartCamera()
    {
        yield return new WaitForSeconds(10);
        if (firstPlay == false && _GM.level == LevelNumber.One)
        {

            StartCoroutine(WaitForWavesTutorial());
            StartCoroutine(WaitForHealthTutorial());
            StartCoroutine(WaitForToolsTutorial());
            FirstPlayTutorial();
        }
    }

    public void GlossaryButton(TutorialID id)
    {
        tutorialID = id;
        CheckTutorial();
    }
    public void FirstPlayTutorial()
    {
        maxTutorialCount = 3;
        OpenGlossaryPanel();
    }
    IEnumerator WaitForToolsTutorial()
    {
        yield return new WaitForSeconds(5);
        NewTutorialAvailable(TutorialID.Powers, "Powers");
    }
    IEnumerator WaitForWavesTutorial()
    {
        yield return new WaitForSeconds(30);
        NewTutorialAvailable(TutorialID.DayNightCycle, "Day/Night Cycle");
    }
    IEnumerator WaitForHealthTutorial()
    {
        yield return new WaitForSeconds(180);
        NewTutorialAvailable(TutorialID.Health, "Health Pickups");
    }

    public void NewTutorialAvailable(TutorialID id, string title)
    {
        tutorialID = id;
        newTutorialTitle.text = title;
        newTutorialButton.SetActive(true);
        newTutorialButton.GetComponent<Animator>().SetTrigger("TutorialAvailable");
        newTutorialButton.GetComponent<AudioSource>().Play();
        pageObjects.SetActive(false);
    }

    public void ContinueButton()
    {
        tutorialStage++;
        CheckInGameTutorial();
        CheckTaskList();
    }

    //public void PreviousButton()
    //{
    //    if(tutorialCount > 0)
    //    {
    //        tutorialCount--;
    //    }
    //    CheckTutorial();
    //}

    public void CheckInGameTutorial()
    {
        if(tutorialStage == 0)
        {
            tutorialTitle.text = "Camera Controls";
            tutorialText.text = "To MOVE the camera, use the ‘W,A,S,D’ keys or move the mouse cursor to the edge of the screen.\r\n<br>Hold shift to hasten camera movement.";
        }
        if (tutorialStage == 1)
        {
            tutorialTitle.text = "Camera Controls";
            tutorialText.text = "To ROTATE the camera, click and drag the ‘Middle Mouse Button’ to the left or right.";
        }
        if (tutorialStage == 2)
        {
            tutorialTitle.text = "Camera Controls";
            tutorialText.text = "To ZOOM the camera, scroll the Mouse Wheel in and out.";
        }
        if (tutorialStage == 3)
        {
            FadeX.FadeOut(inGameTutorialPanel);
            taskPanel.SetActive(false);



            tutorialID = TutorialID.DayNightCycle;
            OpenGlossaryPanel();
         
        }
        if (tutorialStage == 4)
        {
            tutorialTitle.text = "Maegen";
            tutorialText.text = "This is your MAEGEN. \r\n<br>MAEGEN is the wild energy within all natural things and serves as the lifeblood of our grove. Spend MAEGEN to grow TREES that will, in turn, produce more MAEGEN at the end of the DAY.";
            inGameContinueButton.SetActive(true);
            contextArrow.SetActive(true);
            contextArrow.transform.rotation = Quaternion.Euler(0, 0, 180);
            contextArrow.transform.position = maegenIcon.transform.position;
        }
        if (tutorialStage == 5)
        {
            tutorialTitle.text = "Trees";
            tutorialText.text = "The productivity of each TREE is determined by its proximity to others in the GROVE. TREES clustered together are less productive but easier to defend, while those spread out yield more MAEGEN but are more vulnerable to attack.\r\n<br>To grow a tree, click on the TREE button and Left-Click on an available space in our domain.";
            contextArrow.SetActive(true);
            inGameContinueButton.SetActive(false);
            contextArrow.transform.rotation = Quaternion.Euler(0, 0, 0);
            contextArrow.transform.position = treeButton.transform.position;
        }
    }
    public void CheckTaskList()
    {
        if(tutorialStage == 0)
        {
            taskText.text = "Move the camera around the Grove";
            if(hasCompletedTask)
            {
                StartCoroutine(WaitForNextTask());
            }
        }
        if (tutorialStage == 1)
        {
            taskText.text = "Rotate the camera";
            if (hasCompletedTask)
            {
                StartCoroutine(WaitForNextTask());
            }
        }
        if (tutorialStage == 2)
        {
            taskText.text = "Zoom the camera";
            if (hasCompletedTask)
            {
                StartCoroutine(WaitForNextTask());
            }
        }
    }
    IEnumerator WaitForNextTask()
    {
        check.SetActive(true);
        yield return new WaitForSeconds(3);
        tutorialStage++;
        hasCompletedTask = false;
        check.SetActive(false);
        CheckTaskList();
        CheckInGameTutorial();
    }
    public void CheckTutorial()
    {
        scrollbar.value = 1;
        tutCounterText.text = tutorialCount.ToString() + "/" + maxTutorialCount.ToString();
        //if (tutorialCount == 0)
        //{
        //    tutorialImage.sprite = welcomeSprite;
        //    tutTitle.text = "Tutorial";
        //    tutContextText.text = "Welcome to Grove Keeper, thanks for playing!\r\n<br>The game is in development and we would love your feedback.\r\n<br>If you return to the main menu, you will see a link to a feedback form where you can give your thoughts and feelings about the game.";
        //}
        if(tutorialID == TutorialID.CameraControls)
        {
            tutorialImage.sprite = cameraControlsSprite;
            glossaryTitleText.text = "Camera Controls";
            glossaryDescriptionText.text = "To move the camera, use the <b>‘W,A,S,D’</b> keys or move the mouse cursor to the edge of the screen.\r\n<br>Hold <b>Left Shift</b> to hasten camera movement.\r\n<br>To rotate the camera, click and drag the <b>Middle Mouse Button</b> to the left or right.\r\n<br>To zoom the camera, scroll the <b>Mouse Wheel</b> in and out.";
        }
        if(tutorialID == TutorialID.CreatureMovement)
        {
            tutorialImage.sprite = creatureMovementSprite;
            glossaryTitleText.text = "Creature Movement";
            glossaryDescriptionText.text = "To select a <color=#3A9F87><b>Creature</b></color>, click on it with the <b>Left Mouse Button</b> or click and drag over multiple <color=#3A9F87><b>Creatures</b></color> to select more than one.\r\n<br>With a selected <color=#3A9F87><b>Creature(s)</b></color>, <b>Right Cick</b> on a location to send them there. \r\n<br>Our <color=#3A9F87><b>Creatures</b></color> will defend that location if <color=#FE4E2D><b>Humans</b></color> come within their range.";
        }
        if(tutorialID == TutorialID.Maegen)
        {
            tutorialImage.sprite = maegenSprite;
            glossaryTitleText.text = "Maegen";
            glossaryDescriptionText.text = "<color=#FCA343><b>Maegen</b></color> is the raw, wild energy of the <color=#9665cd><b>Grove</b></color>. It functions as the main currency of the game.\r\n<br><color=#FCA343><b>Maegen</b></color> is created by <color=#6d8659><b>Trees</b></color> at the end of each <color=#FCA343><b>Day</b></color> and is used to grow more <color=#6d8659><b>Trees</b></color> and spawn <color=#3A9F87><b>Creatures</b></color>.\r\n<br>Sometimes when a <color=#FE4E2D><b>Human</b></color> is killed, the <color=#9665cd><b>Grove</b></color> can harvest <color=#FCA343><b>Maegen</b></color> from their soul.";
        } 
        if(tutorialID == TutorialID.Trees)
        {
            tutorialImage.sprite = treesSprite;
            glossaryTitleText.text = "Trees";
            glossaryDescriptionText.text = "<color=#6d8659><b>Trees</b></color> are how we increase our power and earn more <color=#FCA343><b>Maegen</b></color>.\r\n<br>The productivity of each <color=#6d8659><b>Tree</b></color> is determined by its proximity to others in the <color=#9665cd><b>Grove</b></color>. <color=#6d8659><b>Trees</b></color> clustered together are less productive but easier to defend, while those spread out yield more <color=#FCA343><b>Maegen</b></color> but are more vulnerable to attack. \r\n<br>To grow a <color=#6d8659><b>Tree</b></color>, click on the <color=#6d8659><b>Tree</b></color> button and <b>Left-Click</b> on an available space in our domain.\r\n<br><b>Right-Click</b> to deselect <color=#6d8659><b>Tree</b></color> mode.";
        }
        if(tutorialID == TutorialID.Wildlife)
        {
            tutorialImage.sprite = wildlifeSprite;
            glossaryTitleText.text = "Wildlife";
            glossaryDescriptionText.text = "<color=#da691e><b>Wildlife</b></color> is spawned into the <color=#9665cd><b>Grove</b></color> at the end of each <color=#FCA343><b>Day</b></color>, based on the number of <color=#6d8659><b>Trees</b></color> we have and is required for us to use our Powers.\r\n<br>Hold down <b>Left-Alt</b> to see our <color=#da691e><b>Wildlife</b></color> highlighted.";
        }
        if(tutorialID == TutorialID.Populous)
        {
            tutorialImage.sprite = populousSprite;
            glossaryTitleText.text = "Populous";
            glossaryDescriptionText.text = "<color=#523c52><b>Populous</b></color> is the maximum number of <color=#3A9F87><b>Creatures</b></color> you can command in one <color=#9665cd><b>Grove</b></color>.\r\n<br>Each <color=#3A9F87><b>Creature</b></color> takes up 1 <color=#523c52><b>Populous</b></color> point.\r\n<br><color=#523c52><b>Populous</b></color> can be upgraded by +5 with a <color=#caad87><b>Perk</b></color>.";
        }
        if(tutorialID == TutorialID.HomeTree)
        {
            tutorialImage.sprite = homeTreeSprite;
            glossaryTitleText.text = "Home Tree";
            glossaryDescriptionText.text = "This is our <color=#FEE65F><b>Home Tree</b></color>, the heart of our <color=#9665cd><b>Grove</b></color> and vital to its survival.\r\n<br>From here, you can summon <color=#3A9F87><b>Creatures</b></color> to fight for us. If the <color=#FEE65F><b>Home Tree</b></color> is ever destroyed, the game is over.\r\n<br>To open the <color=#FEE65F><b>Home Tree</b></color> menu, either click on it in the game world or press <b>Tab</b>";
        }
        if (tutorialID == TutorialID.WitchsHut)
        {
            tutorialImage.sprite = witchsHutSprite;
            glossaryTitleText.text = "Witch's Hut";
            glossaryDescriptionText.text = "A lone witch in the woods allows the grove’s <color=#3A9F87><b>Creatures</b></color> to gather here.\r\n<br><color=#FE4E2D><b>Humans</b></color> will attempt to claim this site for themselves, which they do by being in its vicinity without opposition from any <color=#3A9F87><b>Creatures</b></color>, at which point they will begin to spawn their top tier <color=#FE4E2D><b>Human</b></color> units into the game. \r\n<br>You'll need to either defend it or attack it before the <color=#FCA343><b>Day</b></color> is over.\r\n<br>If the <color=#3A9F87><b>Creatures</b></color> outnumber the <color=#FE4E2D><b>Humans</b></color> in the vicinity of the site, it will begin to be claimed back. \r\n<br>The more units, the more quickly it will be claimed.\r\n<br><color=#3A9F87><b>Creatures</b></color> from this site cannot be purchased unless you have control of the <color=#4d705d><b>Witch's Hut</b></color>.";
        }
        if (tutorialID == TutorialID.Horgr)
        {
            tutorialImage.sprite = horgrSprite;
            glossaryTitleText.text = "Horgr";
            glossaryDescriptionText.text = "This is a magical shrine that is valuable to both the <color=#FE4E2D><b>Humans</b></color> and the <color=#9665cd><b>Grove</b></color>.\r\n<br>Enemies will attempt to claim this site for themselves, at which point they will begin to spawn their own Knights into the game, so you'll need to either defend it or attack it before the wave is over.\r\n<br>If the <color=#3A9F87><b>Creatures</b></color> outnumber the <color=#FE4E2D><b>Humans</b></color> in the vicinity of the site, it will begin to be claimed back.\r\n<br>The more units, the more quickly it will be claimed.\r\n<br><color=#3A9F87><b>Creatures</b></color> cannot be purchased unless you have control of the <color=#4d705d><b>Horgr</b></color>.";
        }
        if (tutorialID == TutorialID.Powers)
        {
            tutorialImage.sprite = powersSprite;
            glossaryTitleText.text = "Powers";
            glossaryDescriptionText.text = "<color=#abb4ca><b>Rune</b></color>:<br><color=#abb4ca><b>Runes</b></color> are ancient, magical zones of wild energy that heal our <color=#3A9F87><b>Creatures</b></color> over time.\r\n<br><color=#caad87><b>Fyre</b></color>:<br><color=#caad87><b>Fyre</b></color> creates an explosion dealing damage to all <color=#FE4E2D><b>Humans</b></color> in its radius.\r\n<br><color=#aeaecf><b>Stormer</b></color>:<br>Allows you to create an intense storm that will randomly strike down <color=#FE4E2D><b>Humans</b></color> with lightning for a period of 1 minute.";
        }
        if (tutorialID == TutorialID.DayNightCycle)
        {
            tutorialImage.sprite = dayNightCycleSprite;
            glossaryTitleText.text = "Day/Night Cycle";
            glossaryDescriptionText.text = "The game is divided into two phases: <color=#FCA343><b>Day</b></color> and <color=#24455A><b>Night</b></color>.<br>\r\nDuring the <color=#FCA343><b>Day</b></color>, <color=#FE4E2D><b>Human</b></color> settlers will encroach upon our <color=#9665cd><b>Grove</b></color>, seeking to cut down our <color=#6d8659><b>Trees</b></color> and hunt our <color=#da691e><b>Wildlife</b></color>. You must protect us against them until the <color=#FCA343><b>Day</b></color> is done.<br>\r\nAt <color=#24455A><b>Night</b></color>, we can recover and expand our <color=#9665cd><b>Grove</b></color>.";
        }
        if (tutorialID == TutorialID.HumanClasses)
        {
            tutorialImage.sprite = humanClassesSprite;
            glossaryTitleText.text = "Human Classes";
            glossaryDescriptionText.text = "There are 4 <color=#FE4E2D><b>Human</b></color> classes that you will encounter.\r\n<br><color=#FFFF69><b>Woodcutters</b></color> are marked in <color=#FFFF69><b>YELLOW</b></color> on the Minimap.\r\n<br>Their primary target are your <color=#6d8659><b>Trees</b></color> and will prioritize cutting them down unless they are confronted by your <color=#3A9F87><b>Creatures</b></color>.\r\n<br><color=#30FE2C><b>Hunters</b></color> are marked in <color=#30FE2C><b>GREEN</b></color> on the Minimap.\r\n<br>Their main focus is your <color=#da691e><b>Wildlife</b></color> and will attempt to hunt your <color=#9665cd><b>Grove</b></color> into extinction unless a <color=#4d705d><b>Witch's Hut</b></color> or one of your <color=#3A9F87><b>Creatures</b></color> is closer.\r\n<br><color=#FE4E2D><b>Warriors</b></color> are marked in <color=#FE4E2D><b>RED</b></color> on the Minimap.\r\n<br>Their main goal is to kill all of your <color=#3A9F87><b>Creatures</b></color> and they will attempt to claim your <color=#4d705d><b>Horgr</b></color> if they are closer to it than they are to your <color=#3A9F87><b>Creatures</b></color>.";
        }
        if (tutorialID == TutorialID.Dogs)
        {
            tutorialImage.sprite = dogsSprite;
            glossaryTitleText.text = "Dogs";
            glossaryDescriptionText.text = "These ferocious hounds have explosives strapped to their backs.<br>\r\n<br>They will appear at the beginning of a <color=#FCA343><b>Day</b></color> if there are plentiful <color=#6d8659><b>Trees</b></color> populating the <color=#9665cd><b>Grove</b></color> and will attempt to blow up your <color=#6d8659><b>trees</b></color>.<br>\r\n<br>They are easy to stop if you can intercept them but they’re fast moving.";
        }
        if (tutorialID == TutorialID.Mines)
        {
            tutorialImage.sprite = minesSprite;
            glossaryTitleText.text = "Mines";
            glossaryDescriptionText.text = "Occasionally, the <color=#FE4E2D><b>Humans</b></color> will bore through the earth and set up their iron mines, this creates a new spawn point that enemies can arrive from.<br>\r\nAny <color=#6d8659><b>Trees</b></color> in the area will be destroyed as it emerges.";
        }
        if (tutorialID == TutorialID.Spies)
        {
            tutorialImage.sprite = spiesprite;
            glossaryTitleText.text = "Spies";
            glossaryDescriptionText.text = "Spies are unique <color=#FE4E2D><b>Humans</b></color> that will attempt to sneak through your defences and attack your <color=#FEE65F><b>Home Tree</b></color> directly.<br>\r\nThey will arrive on the map at a random location and are marked in BLACK.<br>\r\nThey will move towards your <color=#FEE65F><b>Home Tree</b></color>, ignoring everything else in their path to destroy it.<br>\r\nThey will spawn in more regularly as the <color=#FCA343><b>Days</b></color> go by and can emerge at any time, even at <color=#24455A><b>Night</b></color>.";
        }
        if (tutorialID == TutorialID.LordsOfTheLand)
        {
            tutorialImage.sprite = lordsOfTheLandSprite;
            glossaryTitleText.text = "Lords of the Land";
            glossaryDescriptionText.text = "These are high ranking members of the King’s Court, tasked with weakening the defences of the <color=#9665cd><b>Grove</b></color>.<br>\r\nThey are incredibly deadly fighters that will appear sporadically to cause as much chaos as possible.<br>\r\nIf you’re not prepared, they can easily cut through your <color=#3A9F87><b>Creatures</b></color> and destroy your <color=#FEE65F><b>Home Tree</b></color>.";
        }
        if (tutorialID == TutorialID.Combat)
        {
            tutorialImage.sprite = combatSprite;
            glossaryTitleText.text = "Combat";
            glossaryDescriptionText.text = "When a <color=#FE4E2D><b>Human</b></color> first arrives, they are invincible for 5 seconds.\r\n<br><b>Right Clicking</b> on an <color=#FE4E2D><b>Human</b></color> will order a selected <color=#3A9F87><b>Creature</b></color> to target it. They will track down the <color=#FE4E2D><b>Human</b></color> until they catch up to them.\r\n<br>The <color=#3A9F87><b>Creature</b></color> will behave differently, depending on which Combat Mode you have selected. This is represented by an icon above the <color=#3A9F87><b>Creature</b></color>.\r\n<br>Attack Mode:<br>Selecting <b>Attack Mode</b> allows the <color=#3A9F87><b>Creature</b></color> to move freely about the <color=#9665cd><b>Grove</b></color>, attacking any <color=#FE4E2D><b>Humans</b></color> that come within its range.\r\n<br>This is the default Combat Mode.\r\n<br><b>Defend Mode:</b>\r\n<br>Selecting <b>Defend Mode</b> orders the <color=#3A9F87><b>Creature</b></color> to defend its current position.\r\n<br>Its range is reduced and will move small distances to attack <color=#FE4E2D><b>Humans</b></color> but will always return to its original defence position.\r\n<br><b>Formations:</b>\r\n<br>Clicking the <b>Formations</b> button will change how spread out <color=#3A9F87><b>Creatures</b></color> are. You can choose to have them bunch them together to allow a more concentrated force or spread out to cover more ground.";
        }
    }

    public void OpenGlossaryPanel()
    {
        CheckTutorial();
        FadeX.FadeIn(glossaryPanel);
        newTutorialButton.SetActive(false);
        _GM.ChangeGameState(GameState.Glossary);
    }
    public void CloseGlossaryPanel()
    {
        maxTutorialCount = 14;
        pageObjects.SetActive(true);
        FadeX.FadeOut(glossaryPanel);
        _GM.ChangeGameState(GameState.Play);
        if (isTutorial && tutorialStage == 3)
        {
            tutorialStage++;
            hasCompletedTask = true;
            FadeX.FadeIn(inGameTutorialPanel);
            CheckTaskList();
            CheckInGameTutorial();

        }
    }

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
}

[System.Serializable]
public class Tutorial
{
    
    public TutorialID tutorialID;
    [TextArea]
    public string description;
    public bool completed;
}

public enum TutorialID 
{ 
    CameraControls,
    CreatureMovement,
    Maegen,
    Trees,
    Wildlife,
    Populous,
    HomeTree,
    WitchsHut,
    Horgr,
    Powers,
    DayNightCycle,
    HumanClasses,
    Dogs,
    Mines,
    Spies,
    LordsOfTheLand,
    Combat,
    Health,
}

