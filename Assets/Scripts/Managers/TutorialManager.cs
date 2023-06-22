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

    [Header("Sprites")]
    public Sprite welcomeSprite;
    public Sprite oneSprite;
    public Sprite twoSprite;
    public Sprite threeSprite;
    public Sprite fourSprite;
    public Sprite fiveSprite;
    public Sprite sixSprite;
    public Sprite sevenSprite;

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


    void Start()
    {
        CheckTutorial();
        if(isTutorial)
        {
            Time.timeScale = 0;
            StartNextWaveButton.interactable = false;
        }
    }
    private void Update()
    {
        if(isTutorial)
        {
            if (Input.GetMouseButton(0))
            {
                if (_GM.playmode == PlayMode.TreeMode)
                {
                    numberOfTreesText.text = "Trees placed: " + _GM.trees.Length.ToString() + "/" + "5";
                    if (_GM.trees.Length >= 5 && tutorialStage == 2)
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
        tutCounterText.text = tutorialCount.ToString() + "/" + maxTutorialCount.ToString();
        if (tutorialCount == 0)
        {
            tutorialImage.sprite = welcomeSprite;
            tutTitle.text = "Tutorial";
            tutContextText.text = "Welcome to King's Country, thanks for playing!<br>The game is in early development and we would love your feedback. If you return to the main menu, you will see a link to a feedback form where you can give your thoughts and feelings about the game.";
        }
        if(tutorialCount == 1)
        {
            tutorialImage.sprite = oneSprite;
            tutTitle.text = "Camera Controls";
            tutContextText.text = "Use 'W,A,S,D' to control the movement of the camera. Clicking and dragging 'Middle Mouse Button' will roatate and scrolling zooms in and out. Hold 'Shift' to increase the speed of the camera";
        }
        if(tutorialCount == 2)
        {
            tutorialImage.sprite = twoSprite;
            tutTitle.text = "Controls";
            tutContextText.text = "To select multiple units, click and drag the left mouse button around the desired units. To order them to do something, right click on your desired target with them selected.\r\nThe number buttons will choose which tool is being used. Pressing a number button once will select that tool, pressing it again will deselect.\r\nPressing escape will open the pause menu, from which you can access the game settings, and exit the game.";
        }
        if( tutorialCount == 3)
        {
            tutorialImage.sprite = threeSprite;
            tutTitle.text = "Resources";
            tutContextText.text = "The game has 4 resources. These include, Maegen, Trees, Wildlife, and Populous.\r\n<br>Maegen:<br>This is the main currency of the game. The player uses this to recruit units and purchase upgrades.\r\n<br>Trees:<br>Once placed, and at the cost of some maegen, trees produce maegen slowly over time. Upgrades can be purchased to increase the rate of maegen production.\r\n<br>Wildlife:<br>This represents the amount of wildlife you have in your forest. Wildlife includes rabbits, deer and boar (In that order). Wildlife slowly spawns over time and some buildings, tools and units require a certain amount of wildlife to be purchased.\r\n<br>Populous:<br>This represents how many units you can have in your army at any one time (game starts with 10 populous). This number can be increased through purchasing upgrades.";
        } 
        if(tutorialCount == 4)
        {
            tutorialImage.sprite = fourSprite;
            tutTitle.text = "Tools";
            tutContextText.text = "Tree Tool:<br>Pressing ‘1’ will allow the player to place a tree by left clicking on an available ground space at the cost of 15 maegen. Trees cannot be placed on top of one another and neither on rocks nor swamp sections. A tree produces Maegen over time and its productivity is determined by its distance from other trees.\r\n<br>Rune Tool:<br>Pressing ‘2’ allows the player to place a ‘Rune’. This is a blue dome of magical energy that heals units that are inside it and increases the maegen production rate of all trees inside it (+5 maegen every 20-40 seconds). It’s on a timer of 5 mins and when it runs out the rune is destroyed. One Rune costs 150 maegen and requires 5 wildlife, a second is 150 maegen and 10 wildlife, a third is 150 and 20 wildlife and so on.\r\n<br>Fyre Beacon Tool:<br>Pressing ‘3’ allows the player to place a beacon at the cost of 200 maegen and requires at least 15 wildlife. The beacon must be placed within the forested section of the map (near at least 3 trees) and will draw all enemies to it. It appears as a hazy dome with fairies dancing around a toadstool. After 1 minute, the beacon will explode in flames, killing all enemies and destroying all trees in its radius.";
        }
        if(tutorialCount == 5)
        {
            tutorialImage.sprite = fiveSprite;
            tutTitle.text = "Home Tree";
            tutContextText.text = "This is the main interface for the player. They can spawn creatures here and purchase upgrades for their units and tools. If the Home Tree is ever destroyed, the game is over.\r\n<br> To open the Home Tree menu, either click on it in the game world or press 'Tab'";
        }
        if(tutorialCount == 6)
        {
            tutorialImage.sprite = sixSprite;
            tutTitle.text = "Horgr";
            tutContextText.text = "This is a magical shrine that is valuable to both the humans and the forest. At the beginning of the game, a Horgr is placed randomly on the map and it belongs to no one. To claim one, the player or the enemy needs to have a unit close to it where a slider will slowly fill up, at which point the horgr is claimed. Once claimed, the player will recieve a regular income of Maegen and the ability to spawn unique units. If the enemy claims it, they will leave some units there to defend it and all their units damage and health are buffed by 30%.";
        }
        if(tutorialCount == 7)
        {
            tutorialImage.sprite = sevenSprite;
            tutTitle.text = "Pickups";
            tutContextText.text = "Throughout the game, you may notice various glowing objects throughout your forest. These are the Maegen and Health pickups. By sending a unit to collect these, the player will either recieve a winfall of Maegen or it will heal their unit.";
        }
    }

    public void OpenTutPanel()
    {
        tutorialPanel.SetActive(true);
        _GM.gameState = GameState.Pause;
        Time.timeScale = 0f;
    }
    public void CloseTutPanel()
    {
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
        if(isTutorial)
        {
            if (tutorialStage == 7)
            {
                GameEvents.ReportOnNextTutorial();
            }
        }

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
    private void OnEnable()
    {
        GameEvents.OnNextTutorial += OnNextTutorial;
        GameEvents.OnStartNextRound += OnStartNextRound;
        GameEvents.OnWaveOver += OnWaveOver;
        GameEvents.OnContinueButton += OnContinueButton;
    }
    private void OnDisable()
    {
        GameEvents.OnNextTutorial -= OnNextTutorial;
        GameEvents.OnStartNextRound -= OnStartNextRound;
        GameEvents.OnWaveOver -= OnWaveOver;
        GameEvents.OnContinueButton -= OnContinueButton;
    }
}
