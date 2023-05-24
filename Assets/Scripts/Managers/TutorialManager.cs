using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : GameBehaviour
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


    void Start()
    {
        CheckTutorial();
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
            tutContextText.text = "Throughout the game, you may notice various glowing objects throughout your forest. These are the Maegen and Health pickups. By sending a unit to collect these, the player will wither recieve a winfall of Maegen or it will heal their unit.";
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
}
