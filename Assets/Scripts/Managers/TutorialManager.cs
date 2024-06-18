using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TutorialManager : Singleton<TutorialManager>
{
    [BV.EnumList(typeof(TutorialID))]
    public List<Tutorial> tutorials;
    public TutorialID tutorialID;

    [Header("Basic")]
    public Image tutorialImage;
    public TMP_Text tutContextText;
    public TMP_Text tutTitle;
    public TMP_Text tutCounterText;
    public GameObject tutorialPanel;
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
    public GameObject tutorialText;
    public GameObject contextArrow;
    public GameObject contextArrow2;
    public GameObject contextArrow3;
    public GameObject continueButton;
    public GameObject numberOfTextObj;
    public GameObject inWorldArrow;
    public GameObject arrowLocation2;
    public Button StartNextWaveButton;
    public TMP_Text tutText;
    public TMP_Text numberOfTreesText;
    public int tutorialStage;
    public bool isTutorial;
    public bool playerHasClaimedBoth;
    public bool canMoveTo7;
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


    //protected override void Awake()
    //{
    //    _SAVE.Load();
    //}
    void Start()
    {
        //_SAVE.Load();
        CheckTutorial();
        StartCoroutine(WaitForStartCamera());
        //if (newTutorialButton != null)
        //{
        //    CheckTutorial();
        //    StartCoroutine(WaitForStartCamera());
        //}

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
        //if (_SAVE.lvl2Complete == false && _GM.level == LevelNumber.Two)
        //{
        //    NewTutorialAvailable(7, "Witch's Hut");
        //}
        //if (_SAVE.lvl3Complete == false && _GM.level == LevelNumber.Three)
        //{
        //    NewTutorialAvailable(6, "Horgr Shrine");
        //}
    }
    public void FirstPlayTutorial()
    {
        maxTutorialCount = 3;
        OpenTutPanel();
    }
    IEnumerator WaitForToolsTutorial()
    {
        yield return new WaitForSeconds(5);
        NewTutorialAvailable(4, "Tools");
    }
    IEnumerator WaitForWavesTutorial()
    {
        yield return new WaitForSeconds(30);
        NewTutorialAvailable(8, "Waves");
    }
    IEnumerator WaitForHealthTutorial()
    {
        yield return new WaitForSeconds(180);
        NewTutorialAvailable(11, "Health Pickups");
    }
    private void Update()
    {
        if(isTutorial)
        {
            if (Input.GetMouseButton(0))
            {
                if (_GM.playmode == PlayMode.TreeMode)
                {
                    numberOfTreesText.text = "Trees placed: " + _GM.trees.Count.ToString() + "/" + "5";
                    if (_GM.trees.Count >= 5 && tutorialStage == 2)
                    {
                        GameEvents.ReportOnNextTutorial();
                    }
                }
            }
        }

        if(isTutorial && tutorialStage == 11)
        {
            if(_HM.playerOwns && _HUTM.playerOwns && playerHasClaimedBoth == false)
            {
                playerHasClaimedBoth = true;
                GameEvents.ReportOnNextTutorial();
            }
        }
    }
    public void NewTutorialAvailable(int i, string title)
    {
        tutorialCount = i;
        newTutorialTitle.text = title;
        newTutorialButton.SetActive(true);
        newTutorialButton.GetComponent<Animator>().SetTrigger("TutorialAvailable");
        newTutorialButton.GetComponent<AudioSource>().Play();
        pageObjects.SetActive(false);
    }

    public void UpdateTutorialStage()
    {
        _SM.PlaySound(_SM.nextTutorialSound);
        if(tutorialStage == 1)
        {
            continueButton.SetActive(false);
            contextArrow.SetActive(true);
            tutText.text = "We should start by placing some trees. Trees will provide us with resources at the end of each wave. Press '1' or click on the 'Tree Tool' icon to enter tree placement mode.";
        }
        if(tutorialStage == 2)
        {
            Time.timeScale = 1;
            contextArrow.SetActive(false);
            tutText.text = "Place 5 trees on the map. This will mean, at the end of the round, we will recieve 1 Maegen for each tree.";
            numberOfTextObj.SetActive(true);
        }
        if(tutorialStage == 3)
        {
            inWorldArrow.SetActive(true);
            numberOfTextObj.SetActive(false);
            tutText.text = "Now, lets recruit some guardians to fight for us. Right click to de-select the 'Tree Tool'. <br>Click on the 'Home Tree' or press 'Tab' to open the 'Home Tree' menu.";
        }
        if(tutorialStage == 4)
        {
            inWorldArrow.SetActive(false);
            tutText.text = "Use the rest of your Maegen to purchase some units.";
        }
        if(tutorialStage == 5)
        {
            continueButton.SetActive(true);
            tutText.text = "Use 'W,A,S,D' to move the camera around. Clicking and dragging the 'Middle Mouse Button will rotate the camera and scrolling will zoom in and out.";
        }
        if (tutorialStage == 6)
        {
            tutText.text = "Click on a unit to select them, then use 'Right Click' to send them to a location. Click and drag to select multiple units.<br>Place your units strategically based on where the enemies will be coming from.";
        }
        if (tutorialStage == 7)
        {
            StartNextWaveButton.interactable = true;
            continueButton.SetActive(false);
            tutText.text = "Enemies will spawn from the the banners when the wave is started. Click on the 'Start Wave' button when you're ready to begin combat.";
            contextArrow.SetActive(true) ;
            contextArrow.transform.position = arrowLocation2.transform.position;
        }
        if(tutorialStage == 8)
        {
            tutText.text = "Enemies will begin to arrive on the map. Defend the forest with your new units.";
            contextArrow.SetActive(false);
        }
        if (tutorialStage == 9)
        {
            tutText.text = "When the wave is over, you will get a breakdown of your incoming Maegen and wildlife. You also get the option to choose an upgrade that will last for the rest of the level";

        }
        if(tutorialStage == 10)
        {
            continueButton.SetActive(true);
            tutText.text = "Well done on succesully defeating the first wave, although, we're only just beginning. Let's get some more powerful units.<br>The 'Horgr' and the 'Witch's Hut' will allow us to summon new units";
        }
        if (tutorialStage == 11)
        {
            continueButton.SetActive(false);
            contextArrow2.SetActive(true);
            contextArrow3.SetActive(true);
            tutText.text = "Before we can begin using these sites, we need to claim them.<br>Send a unit to stand near its base to begin claiming. The more units that are there, the faster the site will be claimed. (Press 'F4' to speed up time, and 'F3' to return it to normal.)";
        }
        if (tutorialStage == 12)
        {
            contextArrow2.SetActive(false);
            contextArrow3.SetActive(false);
            tutText.text = "Let's summon a 'Huldra'. Huldras have the ability to transform into defensive watch towers. Click on the 'Horgr' to purchase it.";
        }
        if (tutorialStage == 13)
        {
            tutText.text = "Excellent job. Send the 'Huldra' to a good position for defence and press 'T' to turn it into a tower.";
        }
        if (tutorialStage == 14)
        {
            tutText.text = "Purchase any other units you want and begin the next wave. See if you can survive until the end of wave 3.";
        }
    }

    public void NextButton()
    {
        if(tutorialCount < maxTutorialCount)
        {
            tutorialCount++;
        }
        CheckTutorial();
    }

    public void PreviousButton()
    {
        if(tutorialCount > 0)
        {
            tutorialCount--;
        }
        CheckTutorial();
    }

    private void CheckTutorial()
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
            tutTitle.text = "Camera Controls";
            tutContextText.text = "To move the camera, use the <b>‘W,A,S,D’</b> keys or move the mouse cursor to the edge of the screen.\r\n<br>Hold <b>Left Shift</b> to hasten camera movement.\r\n<br>To rotate the camera, click and drag the <b>Middle Mouse Button</b> to the left or right.\r\n<br>To zoom the camera, scroll the <b>Mouse Wheel</b> in and out.";
        }
        if(tutorialID == TutorialID.CreatureMovement)
        {
            tutorialImage.sprite = creatureMovementSprite;
            tutTitle.text = "Creature Movement";
            tutContextText.text = "To select a <color=#3A9F87><b>Creature</b></color>, click on it with the <b>Left Mouse Button</b> or click and drag over multiple <color=#3A9F87><b>Creatures</b></color> to select more than one.\r\n<br>With a selected <color=#3A9F87><b>Creature(s)</b></color>, <b>Right Cick</b> on a location to send them there. \r\n<br>Our <color=#3A9F87><b>Creatures</b></color> will defend that location if <color=#FE4E2D><b>Humans</b></color> come within their range.";
        }
        if(tutorialID == TutorialID.Maegen)
        {
            tutorialImage.sprite = maegenSprite;
            tutTitle.text = "Maegen";
            tutContextText.text = "<color=#FCA343><b>Maegen</b></color> is the raw, wild energy of the <color=#9665cd><b>Grove</b></color>. It functions as the main currency of the game.\r\n<br><color=#FCA343><b>Maegen</b></color> is created by <color=#6d8659><b>Trees</b></color> at the end of each <color=#FCA343><b>Day</b></color> and is used to grow more <color=#6d8659><b>Trees</b></color> and spawn <color=#3A9F87><b>Creatures</b></color>.\r\n<br>Sometimes when a <color=#FE4E2D><b>Human</b></color> is killed, the <color=#9665cd><b>Grove</b></color> can harvest <color=#FCA343><b>Maegen</b></color> from their soul.";
        } 
        if(tutorialID == TutorialID.Trees)
        {
            tutorialImage.sprite = treesSprite;
            tutTitle.text = "Trees";
            tutContextText.text = "<color=#6d8659><b>Trees</b></color> are how we increase our power and earn more <color=#FCA343><b>Maegen</b></color>.\r\n<br>The productivity of each <color=#6d8659><b>Tree</b></color> is determined by its proximity to others in the <color=#9665cd><b>Grove</b></color>. <color=#6d8659><b>Trees</b></color> clustered together are less productive but easier to defend, while those spread out yield more <color=#FCA343><b>Maegen</b></color> but are more vulnerable to attack. \r\n<br>To grow a <color=#6d8659><b>Tree</b></color>, click on the <color=#6d8659><b>Tree</b></color> button and <b>Left-Click</b> on an available space in our domain.\r\n<br><b>Right-Click</b> to deselect <color=#6d8659><b>Tree</b></color> mode.";
        }
        if(tutorialID == TutorialID.Wildlife)
        {
            tutorialImage.sprite = wildlifeSprite;
            tutTitle.text = "Wildlife";
            tutContextText.text = "<color=#da691e><b>Wildlife</b></color> is spawned into the <color=#9665cd><b>Grove</b></color> at the end of each <color=#FCA343><b>Day</b></color>, based on the number of <color=#6d8659><b>Trees</b></color> we have and is required for us to use our Powers.\r\n<br>Hold down <b>Left-Alt</b> to see our <color=#da691e><b>Wildlife</b></color> highlighted.";
        }
        if(tutorialID == TutorialID.Populous)
        {
            tutorialImage.sprite = populousSprite;
            tutTitle.text = "Populous";
            tutContextText.text = "<color=#523c52><b>Populous</b></color> is the maximum number of <color=#3A9F87><b>Creatures</b></color> you can command in one <color=#9665cd><b>Grove</b></color>.\r\n<br>Each <color=#3A9F87><b>Creature</b></color> takes up 1 <color=#523c52><b>Populous</b></color> point.\r\n<br><color=#523c52><b>Populous</b></color> can be upgraded by +5 with a <color=#caad87><b>Perk</b></color>.";
        }
        if(tutorialID == TutorialID.HomeTree)
        {
            tutorialImage.sprite = homeTreeSprite;
            tutTitle.text = "Home Tree";
            tutContextText.text = "This is our <color=#FEE65F><b>Home Tree</b></color>, the heart of our <color=#9665cd><b>Grove</b></color> and vital to its survival.\r\n<br>From here, you can summon <color=#3A9F87><b>Creatures</b></color> to fight for us. If the <color=#FEE65F><b>Home Tree</b></color> is ever destroyed, the game is over.\r\n<br>To open the <color=#FEE65F><b>Home Tree</b></color> menu, either click on it in the game world or press <b>Tab</b>";
        }
        if (tutorialID == TutorialID.WitchsHut)
        {
            tutorialImage.sprite = witchsHutSprite;
            tutTitle.text = "Witch's Hut";
            tutContextText.text = "A lone witch in the woods allows the grove’s <color=#3A9F87><b>Creatures</b></color> to gather here.\r\n<br><color=#FE4E2D><b>Humans</b></color> will attempt to claim this site for themselves, which they do by being in its vicinity without opposition from any <color=#3A9F87><b>Creatures</b></color>, at which point they will begin to spawn their top tier <color=#FE4E2D><b>Human</b></color> units into the game. \r\n<br>You'll need to either defend it or attack it before the <color=#FCA343><b>Day</b></color> is over.\r\n<br>If the <color=#3A9F87><b>Creatures</b></color> outnumber the <color=#FE4E2D><b>Humans</b></color> in the vicinity of the site, it will begin to be claimed back. \r\n<br>The more units, the more quickly it will be claimed.\r\n<br><color=#3A9F87><b>Creatures</b></color> from this site cannot be purchased unless you have control of the <color=#4d705d><b>Witch's Hut</b></color>.";
        }
        if (tutorialID == TutorialID.Horgr)
        {
            tutorialImage.sprite = horgrSprite;
            tutTitle.text = "Horgr";
            tutContextText.text = "This is a magical shrine that is valuable to both the <color=#FE4E2D><b>Humans</b></color> and the <color=#9665cd><b>Grove</b></color>.\r\n<br>Enemies will attempt to claim this site for themselves, at which point they will begin to spawn their own Knights into the game, so you'll need to either defend it or attack it before the wave is over.\r\n<br>If the <color=#3A9F87><b>Creatures</b></color> outnumber the <color=#FE4E2D><b>Humans</b></color> in the vicinity of the site, it will begin to be claimed back.\r\n<br>The more units, the more quickly it will be claimed.\r\n<br><color=#3A9F87><b>Creatures</b></color> cannot be purchased unless you have control of the <color=#4d705d><b>Horgr</b></color>.";
        }
        if (tutorialID == TutorialID.Powers)
        {
            tutorialImage.sprite = powersSprite;
            tutTitle.text = "Powers";
            tutContextText.text = "<color=#abb4ca><b>Rune</b></color>:<br><color=#abb4ca><b>Runes</b></color> are ancient, magical zones of wild energy that heal our <color=#3A9F87><b>Creatures</b></color> over time.\r\n<br><color=#caad87><b>Fyre</b></color>:<br><color=#caad87><b>Fyre</b></color> creates an explosion dealing damage to all <color=#FE4E2D><b>Humans</b></color> in its radius.\r\n<br><color=#aeaecf><b>Stormer</b></color>:<br>Allows you to create an intense storm that will randomly strike down <color=#FE4E2D><b>Humans</b></color> with lightning for a period of 1 minute.";
        }
        if (tutorialID == TutorialID.DayNightCycle)
        {
            tutorialImage.sprite = dayNightCycleSprite;
            tutTitle.text = "Day/Night Cycle";
            tutContextText.text = "The game is divided into two phases: <color=#FCA343><b>Day</b></color> and <color=#24455A><b>Night</b></color>.<br>\r\nDuring the <color=#FCA343><b>Day</b></color>, <color=#FE4E2D><b>Human</b></color> settlers will encroach upon our <color=#9665cd><b>Grove</b></color>, seeking to cut down our <color=#6d8659><b>Trees</b></color> and hunt our <color=#da691e><b>Wildlife</b></color>. You must protect us against them until the <color=#FCA343><b>Day</b></color> is done.<br>\r\nAt <color=#24455A><b>Night</b></color>, we can recover and expand our <color=#9665cd><b>Grove</b></color>.";
        }
        if (tutorialID == TutorialID.HumanClasses)
        {
            tutorialImage.sprite = humanClassesSprite;
            tutTitle.text = "Human Classes";
            tutContextText.text = "There are 4 <color=#FE4E2D><b>Human</b></color> classes that you will encounter.\r\n<br><color=#FFFF69><b>Woodcutters</b></color> are marked in <color=#FFFF69><b>YELLOW</b></color> on the Minimap.\r\n<br>Their primary target are your <color=#6d8659><b>Trees</b></color> and will prioritize cutting them down unless they are confronted by your <color=#3A9F87><b>Creatures</b></color>.\r\n<br><color=#30FE2C><b>Hunters</b></color> are marked in <color=#30FE2C><b>GREEN</b></color> on the Minimap.\r\n<br>Their main focus is your <color=#da691e><b>Wildlife</b></color> and will attempt to hunt your <color=#9665cd><b>Grove</b></color> into extinction unless a <color=#4d705d><b>Witch's Hut</b></color> or one of your <color=#3A9F87><b>Creatures</b></color> is closer.\r\n<br><color=#FE4E2D><b>Warriors</b></color> are marked in <color=#FE4E2D><b>RED</b></color> on the Minimap.\r\n<br>Their main goal is to kill all of your <color=#3A9F87><b>Creatures</b></color> and they will attempt to claim your <color=#4d705d><b>Horgr</b></color> if they are closer to it than they are to your <color=#3A9F87><b>Creatures</b></color>.";
        }
        if (tutorialID == TutorialID.Dogs)
        {
            tutorialImage.sprite = dogsSprite;
            tutTitle.text = "Dogs";
            tutContextText.text = "These ferocious hounds have explosives strapped to their backs.<br>\r\n<br>They will appear at the beginning of a <color=#FCA343><b>Day</b></color> if there are plentiful <color=#6d8659><b>Trees</b></color> populating the <color=#9665cd><b>Grove</b></color> and will attempt to blow up your <color=#6d8659><b>trees</b></color>.<br>\r\n<br>They are easy to stop if you can intercept them but they’re fast moving.";
        }
        if (tutorialID == TutorialID.Mines)
        {
            tutorialImage.sprite = minesSprite;
            tutTitle.text = "Mines";
            tutContextText.text = "Occasionally, the <color=#FE4E2D><b>Humans</b></color> will bore through the earth and set up their iron mines, this creates a new spawn point that enemies can arrive from.<br>\r\nAny <color=#6d8659><b>Trees</b></color> in the area will be destroyed as it emerges.";
        }
        if (tutorialID == TutorialID.Spies)
        {
            tutorialImage.sprite = spiesprite;
            tutTitle.text = "Spies";
            tutContextText.text = "Spies are unique <color=#FE4E2D><b>Humans</b></color> that will attempt to sneak through your defences and attack your <color=#FEE65F><b>Home Tree</b></color> directly.<br>\r\nThey will arrive on the map at a random location and are marked in BLACK.<br>\r\nThey will move towards your <color=#FEE65F><b>Home Tree</b></color>, ignoring everything else in their path to destroy it.<br>\r\nThey will spawn in more regularly as the <color=#FCA343><b>Days</b></color> go by and can emerge at any time, even at <color=#24455A><b>Night</b></color>.";
        }
        if (tutorialID == TutorialID.LordsOfTheLand)
        {
            tutorialImage.sprite = lordsOfTheLandSprite;
            tutTitle.text = "Lords of the Land";
            tutContextText.text = "These are high ranking members of the King’s Court, tasked with weakening the defences of the <color=#9665cd><b>Grove</b></color>.<br>\r\nThey are incredibly deadly fighters that will appear sporadically to cause as much chaos as possible.<br>\r\nIf you’re not prepared, they can easily cut through your <color=#3A9F87><b>Creatures</b></color> and destroy your <color=#FEE65F><b>Home Tree</b></color>.";
        }
        if (tutorialID == TutorialID.Combat)
        {
            tutorialImage.sprite = combatSprite;
            tutTitle.text = "Combat";
            tutContextText.text = "When a <color=#FE4E2D><b>Human</b></color> first arrives, they are invincible for 5 seconds.\r\n<br><b>Right Clicking</b> on an <color=#FE4E2D><b>Human</b></color> will order a selected <color=#3A9F87><b>Creature</b></color> to target it. They will track down the <color=#FE4E2D><b>Human</b></color> until they catch up to them.\r\n<br>The <color=#3A9F87><b>Creature</b></color> will behave differently, depending on which Combat Mode you have selected. This is represented by an icon above the <color=#3A9F87><b>Creature</b></color>.\r\n<br>Attack Mode:<br>Selecting <b>Attack Mode</b> allows the <color=#3A9F87><b>Creature</b></color> to move freely about the <color=#9665cd><b>Grove</b></color>, attacking any <color=#FE4E2D><b>Humans</b></color> that come within its range.\r\n<br>This is the default Combat Mode.\r\n<br><b>Defend Mode:</b>\r\n<br>Selecting <b>Defend Mode</b> orders the <color=#3A9F87><b>Creature</b></color> to defend its current position.\r\n<br>Its range is reduced and will move small distances to attack <color=#FE4E2D><b>Humans</b></color> but will always return to its original defence position.\r\n<br><b>Formations:</b>\r\n<br>Clicking the <b>Formations</b> button will change how spread out <color=#3A9F87><b>Creatures</b></color> are. You can choose to have them bunch them together to allow a more concentrated force or spread out to cover more ground.";
        }
    }

    public void OpenTutPanel()
    {
        CheckTutorial();
        tutorialPanel.SetActive(true);
        newTutorialButton.SetActive(false);
        _GM.gameState = GameState.Pause;
        Time.timeScale = 0f;
    }
    public void CloseTutPanel()
    {
        maxTutorialCount = 14;
        pageObjects.SetActive(true);
        tutorialPanel.SetActive(false);
        _GM.gameState = GameState.Play;
        Time.timeScale = 1f;

    }

    public void ContinueTutorialButton()
    {

        if(tutorialStage == 0)
        {
            GameEvents.ReportOnNextTutorial();
        }
        if(tutorialStage == 5)
        {
            GameEvents.ReportOnNextTutorial();
        }
        if (tutorialStage == 6)
        {
            if(canMoveTo7 == false)
            {
                canMoveTo7 = true;

            }
            else
            {
                GameEvents.ReportOnNextTutorial();
            }
           

        }
        if(tutorialStage == 10)
        {
            GameEvents.ReportOnNextTutorial();
        }
    }

    public void OnNextTutorial()
    {
        tutorialStage++;
        UpdateTutorialStage();
        
    }

    public void OnStartNextRound()
    {
        if (firstPlay == false)
        {
            if(_GM.level == LevelNumber.One && _GM.currentWave == 1)
            {
                NewTutorialAvailable(9, "Enemy Types");
                StartCoroutine(WaitToAddCombatTutorial());
            }
        }
    }
    IEnumerator WaitToAddCombatTutorial()
    {
        yield return new WaitForSeconds(25);
        NewTutorialAvailable(14, "Combat");
    }
    public void OnWaveOver()
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
            if(tutorialStage == 9)
            {
                GameEvents.ReportOnNextTutorial();
            }
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
            NewTutorialAvailable(12, "Mines");
        }
    }
    void OnLordSpawned()
    {
        if (firstLord == false)
        {
            firstLord = true;
            NewTutorialAvailable(13, "Lords of the Land");
        }
    }
    void OnSpySpawned()
    {
        if(firstSpy == false)
        {
            firstSpy = true;
            NewTutorialAvailable(10, "Spies");
        }
    }
    void OnHomeTreeSelected()
    {
        if (firstPlay == false && firstHomeTree == false)
        {
            firstHomeTree = true;
            NewTutorialAvailable(5, "Home Tree");
        }
    }
    private void OnEnable()
    {
        GameEvents.OnNextTutorial += OnNextTutorial;
        GameEvents.OnWaveBegin += OnStartNextRound;
        GameEvents.OnWaveOver += OnWaveOver;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnLevelWin += OnLevelWin;
        GameEvents.OnMineSpawned += OnMineSpawned;
        GameEvents.OnLordSpawned += OnLordSpawned;
        GameEvents.OnSpySpawned += OnSpySpawned;
        GameEvents.OnHomeTreeSelected += OnHomeTreeSelected;
    }
    private void OnDisable()
    {
        GameEvents.OnNextTutorial -= OnNextTutorial;
        GameEvents.OnWaveBegin -= OnStartNextRound;
        GameEvents.OnWaveOver -= OnWaveOver;
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
    Perks,
}

