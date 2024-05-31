using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TutorialManager : Singleton<TutorialManager>
{
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
    public Sprite oneSprite;
    public Sprite twoSprite;
    public Sprite threeSprite;
    public Sprite fourSprite;
    public Sprite fiveSprite;
    public Sprite sixSprite;
    public Sprite sevenSprite;
    public Sprite eightSprite;
    public Sprite nineSprite;
    public Sprite tenSprite;
    public Sprite elevenSprite;
    public Sprite twelveSprite;
    public Sprite thirteenSprite;
    public Sprite fouteenSprite;

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
        if (tutorialCount == 0)
        {
            tutorialImage.sprite = welcomeSprite;
            tutTitle.text = "Tutorial";
            tutContextText.text = "Welcome to Grove Keeper, thanks for playing!\r\n<br>The game is in development and we would love your feedback.\r\n<br>If you return to the main menu, you will see a link to a feedback form where you can give your thoughts and feelings about the game.";
        }
        if(tutorialCount == 1)
        {
            tutorialImage.sprite = oneSprite;
            tutTitle.text = "Camera Controls";
            tutContextText.text = "Use 'W,A,S,D' to control the movement of the camera.\r\n<br>Clicking and dragging 'Middle Mouse Button' will roatate and scrolling zooms in and out.\r\n<br>Hold 'Shift' to increase the speed of the camera";
        }
        if(tutorialCount == 2)
        {
            tutorialImage.sprite = twoSprite;
            tutTitle.text = "Controls";
            tutContextText.text = "To select a unit, left click on it.\r\n<br>To order them to do something, right click on your desired target with them selected. If you right click on an enemy with a unit selected they will target that enemy.\r\n<br>To select multiple units, click and drag the left mouse button around the desired units.";
        }
        if( tutorialCount == 3)
        {
            tutorialImage.sprite = threeSprite;
            tutTitle.text = "Resources";
            tutContextText.text = "The game has 4 resources. These include, Maegen, Trees, Wildlife, and Populous.\r\n<br>Maegen:<br>This is the main currency of the game. Maegen represents the raw energy of the grove, and you use this to recruit units. Maegen is collected at the end of each wave, based on the amount and quality of the trees standing.\r\n<br>Trees:<br>Once placed, trees produce Maegen based on their location to other trees at the end of each wave.\r\n<br>Wildlife:<br>This represents the amount of wildlife you have in your grove. Wildlife includes rabbits, deer, and boar. Wildlife spawns in at the end of each round based on the number of trees are in the grove; 1 wildlife for every 5 trees. Your special abilities require a certain amount of wildlife for them to be used.\r\n<br>Populous:<br>This represents how many units you can have in your army at any one time (game starts with 10 populous). This number can be increased through acquiring upgrades.";
        } 
        if(tutorialCount == 4)
        {
            tutorialImage.sprite = fourSprite;
            tutTitle.text = "Tools";
            tutContextText.text = "Tree Tool:<br>Clicking on the tree icon at the bottom of the screen will allow you to place a tree by left clicking on an available ground space. Trees cannot be placed on top of one another and neither on rocks nor swamp sections. A tree produces Maegen at the end of each wave and its productivity is determined by its distance from other trees. (Trees cannot be placed while the enemy is attacking).\r\n<br>Rune Tool:<br>Clicking on the rune icon at the bottom of the screen allows you to place a ‘Rune’. This is a blue dome of magical energy that heals units that are inside it. The rune will last for 1 round and multiple can be placed at an increasing cost each time.\r\n<br>Fyre Tool:<br>Clicking on the icon at the bottom of the screen allows you to create a fiery explosion, dealing damage to all enemy units in its radius.\r\n<br>Stormer Tool:<br>Clicking on the icon at the bottom of the screen allows you to create an intense storm that requires at least 20 wildlife. Lightning bolts will randomly strike enemies down for a period of 1 minute.";
        }
        if(tutorialCount == 5)
        {
            tutorialImage.sprite = fiveSprite;
            tutTitle.text = "Home Tree";
            tutContextText.text = "The home tree is the heart of your forest and must be protected at all costs. Units can be spawned from this ancient tree to help defend the forest. If the Home Tree is ever destroyed, the game is over.\r\n<br>To open the Home Tree menu, either click on it in the game world or press 'Tab'";
        }
        if(tutorialCount == 6)
        {
            tutorialImage.sprite = sixSprite;
            tutTitle.text = "Horgr";
            tutContextText.text = "This is a magical shrine that is valuable to both the humans and the grove.\r\n<br>From this location, you can purchase a 'Huldra', which can transform into a watch tower to defend your grove.\r\n<br>You can also call upon the ‘Mistclif’, an enormous stone golem with tremendous power but that can’t be healed.\r\n<br>Enemies will attempt to claim this site for themselves, at which point they will begin to spawn their own knights into the game, so you'll need to either defend it or attack it before the wave is over.\r\n<br>If the player outnumbers the enemy in the vicinity of the site, it will begin to be claimed back. The more units, the more quickly it will be claimed.\r\n<br>Units cannot be purchased unless you have control of the Horgr.";
        }
        if(tutorialCount == 7)
        {
            tutorialImage.sprite = sevenSprite;
            tutTitle.text = "Witch's Hut";
            tutContextText.text = "A lone witch in the woods allows the grove’s minions to gather here. From this location, you can purchase the 'Skessa', a fast and powerful female troll, the 'Goblin Archer', A reliable ranged unit with fire arrows, and the Fidhain, a wiry monster made from plants that spits acid and can transform into an acid tower.\r\n<br>Enemies will attempt to claim this site for themselves, at which point they will begin to spawn their own crossbowmen into the game, so you'll need to either defend it or attack it before the wave is over.\r\n<br>If the player outnumbers the enemy in the vicinity of the site, it will begin to be claimed back. The more units, the more quickly it will be claimed.\r\n<br>Units cannot be purchased unless you have control of the Witch’s Hut.";
        }
        if (tutorialCount == 8)
        {
            tutorialImage.sprite = eightSprite;
            tutTitle.text = "Waves";
            tutContextText.text = "The game is broken up into 'waves'.\r\n<br>Before a wave starts, you have the opportunity to place trees and purchase units in preparation for the incoming enemy force.\r\n<br>When you're ready to begin the wave, click the button pictured above and the enemies will begin to attack.\r\n<br>Once all enemies have been taken care of, the wave will end and you will receive a breakdown of your incoming resources for the next round and a choice between two upgrades.\r\n<br>Press ‘F4’ to speed up time\r\n<br>Press ‘F3’ to return to regular speed.";
        }
        if (tutorialCount == 9)
        {
            tutorialImage.sprite = nineSprite;
            tutTitle.text = "Enemy Types";
            tutContextText.text = "There are 4 enemy types that you will encounter.\r\n<br>Woodcutters:<br>Woodcutters are marked in 'YELLOW' on the map. Their primary target are your trees and will prioritise cutting them down unless they are confronted by your units.\r\n<br>Hunters:<br>Hunters are marked in 'GREEN' on the map and their main focus is your wildlife. They will attempt to hunt your grove into extinction unless a 'Witch's Hut' or one of your units is closer.\r\n<br>Warriors:<br>Warriors are marked in 'RED' on the map and their main goal is to kill all of your units. They will attempt to claim your 'Horgr' if they are closer to it than they are to your units.";
        }
        if (tutorialCount == 10)
        {
            tutorialImage.sprite = tenSprite;
            tutTitle.text = "Spies";
            tutContextText.text = "Spies are unique enemies that will attempt to sneak through your defences and attack your Home Tree directly.\r\n<br>They will arrive on the map at a random location and are marked in 'BLACK'.\r\n<br>They will move towards your home tree, ignoring everything else in their path to destroy it.\r\n<br>They will spawn in more regularly the higher the current wave is and can emerge at any time, even in between waves.";
        }
        if (tutorialCount == 11)
        {
            tutorialImage.sprite = elevenSprite;
            tutTitle.text = "Health Pickups";
            tutContextText.text = "Health:<br>During the game you might notice some pink objects appearing around the map. These are 'Health' pickups.\r\n<br>If you send a unit to their location, they will heal any units that are nearby at the time.";
        }
        if (tutorialCount == 12)
        {
            tutorialImage.sprite = twelveSprite;
            tutTitle.text = "Mines";
            tutContextText.text = "The humans are coming out of holes in the ground!\r\n<br>Occasionally, the humans will bore through the earth and set up their iron mines, this creates a new spawn point that enemies can arrive from.\r\n<br>Any trees in the area will be destroyed as it emerges.";
        }
        if (tutorialCount == 13)
        {
            tutorialImage.sprite = thirteenSprite;
            tutTitle.text = "Lords of the Land";
            tutContextText.text = "These are high ranking members of the King’s Court, tasked with weakening the defences of the grove.\r\n<br>They are incredibly deadly warriors that will appear sporadically to cause as much chaos as possible.\r\n<br>If you’re not prepared, they can easily cut through your units and destroy your Home Tree.";
        }
        if (tutorialCount == 14)
        {
            tutorialImage.sprite = fouteenSprite;
            tutTitle.text = "Combat";
            tutContextText.text = "Right clicking on an enemy will order a selected unit to target it. They will track down the enemy until they catch up to them.\r\n<br>Otherwise, the unit will behave differently, depending on which Combat Mode you have selected. This is represented by this icon above the unit.\r\n<br>Attack Mode:<br>Selecting Attack Mode allows the unit to move freely about the map, attacking any enemies that come within its range.<br>This is the default Combat Mode.\r\n<br>Defend Mode:<br>Selecting Defend Mode orders the unit to defend its current position. Its range is reduced and will move small distances to attack enemies but will always return to its original defence position.\r\n<br>Formations:<br>Clicking the Formations button will change how spread out units are. You can choose to have them bunch them together to allow a more concentrated force or spread out to cover more ground.";
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
