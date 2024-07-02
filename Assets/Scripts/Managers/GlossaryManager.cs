using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GlossaryManager : GameBehaviour
{
    public List<GlossaryItem> glossaryItems;
    [Header("Basics")]
    public UnityEngine.UI.Image glossaryImage;
    public TMP_Text glossaryDescriptionText;
    public TMP_Text glossaryTitleText;
    public CanvasGroup glossaryPanel;
    public UnityEngine.UI.Button glossaryButton;
    public Scrollbar scrollbar;
    private GlossaryID lastSelectedGlossayID;
    public List<GlossaryButton> glossaryButtons;
    [Header("New Glossary Item")]
    public UnityEngine.UI.Image newEntryLabel;
    public TMP_Text newEntryTitle;

    public GlossaryItem GetGlossaryItem(GlossaryID _id) => glossaryItems.Find(x=>x.glossaryID == _id);
    private bool glossaryAvailable(GlossaryID _id) => GetGlossaryItem(_id).available;

    private void Start()
    {
        glossaryButton.onClick.AddListener(()=> OpenGlossaryPanel());
        lastSelectedGlossayID = GlossaryID.CameraControls;
        for(int i=0; i<glossaryItems.Count; i++)
        {
            SetupGlossaryItems(GetGlossaryItem(glossaryItems[i].glossaryID));
        }
        newEntryLabel.fillAmount = 0;
    }

    private void SetupGlossaryItems(GlossaryItem _glossaryItem)
    {
        switch(_glossaryItem.glossaryID)
        {
            case GlossaryID.CameraControls:
                _glossaryItem.title = "Camera Controls";
                _glossaryItem.description =
                    "To move the camera, use the <b>‘W,A,S,D’</b> keys or move the mouse cursor to the edge of the screen.\r\n<br>Hold <b>Left Shift</b> to hasten camera movement.\r\n<br>To rotate the camera, click and drag the <b>Middle Mouse Button</b> to the left or right.\r\n<br>To zoom the camera, scroll the <b>Mouse Wheel</b> in and out.";
                break;
            case GlossaryID.CreatureMovement:
                _glossaryItem.title = "Creature Movement";
                _glossaryItem.description =
                    "To select a <color=#3A9F87><b>Creature</b></color>, click on it with the <b>Left Mouse Button</b> or click and drag over multiple <color=#3A9F87><b>Creatures</b></color> to select more than one.\r\n<br>With a selected <color=#3A9F87><b>Creature(s)</b></color>, <b>Right Cick</b> on a location to send them there. \r\n<br>Our <color=#3A9F87><b>Creatures</b></color> will defend that location if <color=#FE4E2D><b>Humans</b></color> come within their range.";
                break;
            case GlossaryID.Maegen:
                _glossaryItem.title = "Maegen";
                _glossaryItem.description =
                    "<color=#FCA343><b>Maegen</b></color> is the raw, wild energy of the <color=#9665cd><b>Grove</b></color>. It functions as the main currency of the game.\r\n<br><color=#FCA343><b>Maegen</b></color> is created by <color=#6d8659><b>Trees</b></color> at the end of each <color=#FCA343><b>Day</b></color> and is used to grow more <color=#6d8659><b>Trees</b></color> and spawn <color=#3A9F87><b>Creatures</b></color>.\r\n<br>Sometimes when a <color=#FE4E2D><b>Human</b></color> is killed, the <color=#9665cd><b>Grove</b></color> can harvest <color=#FCA343><b>Maegen</b></color> from their soul.";
                break;
            case GlossaryID.Trees:
                _glossaryItem.title = "Trees";
                _glossaryItem.description =
                    "<color=#6d8659><b>Trees</b></color> are how we increase our power and earn more <color=#FCA343><b>Maegen</b></color>.\r\n<br>The productivity of each <color=#6d8659><b>Tree</b></color> is determined by its proximity to others in the <color=#9665cd><b>Grove</b></color>. <color=#6d8659><b>Trees</b></color> clustered together are less productive but easier to defend, while those spread out yield more <color=#FCA343><b>Maegen</b></color> but are more vulnerable to attack. \r\n<br>To grow a <color=#6d8659><b>Tree</b></color>, click on the <color=#6d8659><b>Tree</b></color> button and <b>Left-Click</b> on an available space in our domain.\r\n<br><b>Right-Click</b> to deselect <color=#6d8659><b>Tree</b></color> mode.";
                break;
            case GlossaryID.Wildlife:
                _glossaryItem.title = "Wildlife";
                _glossaryItem.description =
                    "<color=#da691e><b>Wildlife</b></color> is spawned into the <color=#9665cd><b>Grove</b></color> at the end of each <color=#FCA343><b>Day</b></color>, based on the number of <color=#6d8659><b>Trees</b></color> we have and is required for us to use our Powers.\r\n<br>Hold down <b>Left-Alt</b> to see our <color=#da691e><b>Wildlife</b></color> highlighted.";
                break;
            case GlossaryID.Populous:
                _glossaryItem.title = "Populous";
                _glossaryItem.description =
                    "<color=#523c52><b>Populous</b></color> is the maximum number of <color=#3A9F87><b>Creatures</b></color> you can command in one <color=#9665cd><b>Grove</b></color>.\r\n<br>Each <color=#3A9F87><b>Creature</b></color> takes up 1 <color=#523c52><b>Populous</b></color> point.\r\n<br><color=#523c52><b>Populous</b></color> can be upgraded by +5 with a <color=#caad87><b>Perk</b></color>.";
                break;
            case GlossaryID.HomeTree:
                _glossaryItem.title = "Home Tree";
                _glossaryItem.description =
                    "This is our <color=#FEE65F><b>Home Tree</b></color>, the heart of our <color=#9665cd><b>Grove</b></color> and vital to its survival.\r\n<br>From here, you can summon <color=#3A9F87><b>Creatures</b></color> to fight for us. If the <color=#FEE65F><b>Home Tree</b></color> is ever destroyed, the game is over.\r\n<br>To open the <color=#FEE65F><b>Home Tree</b></color> menu, either click on it in the game world or press <b>Tab</b>";
                break;
            case GlossaryID.WitchsHut:
                _glossaryItem.title = "Witch's Hut";
                _glossaryItem.description =
                    "A lone witch in the woods allows the grove’s <color=#3A9F87><b>Creatures</b></color> to gather here.\r\n<br><color=#FE4E2D><b>Humans</b></color> will attempt to claim this site for themselves, which they do by being in its vicinity without opposition from any <color=#3A9F87><b>Creatures</b></color>, at which point they will begin to spawn their top tier <color=#FE4E2D><b>Human</b></color> units into the game. \r\n<br>You'll need to either defend it or attack it before the <color=#FCA343><b>Day</b></color> is over.\r\n<br>If the <color=#3A9F87><b>Creatures</b></color> outnumber the <color=#FE4E2D><b>Humans</b></color> in the vicinity of the site, it will begin to be claimed back. \r\n<br>The more units, the more quickly it will be claimed.\r\n<br><color=#3A9F87><b>Creatures</b></color> from this site cannot be purchased unless you have control of the <color=#4d705d><b>Witch's Hut</b></color>.";
                break;
            case GlossaryID.Horgr:
                _glossaryItem.title = "Horgr";
                _glossaryItem.description =
                    "This is a magical shrine that is valuable to both the <color=#FE4E2D><b>Humans</b></color> and the <color=#9665cd><b>Grove</b></color>.\r\n<br>Enemies will attempt to claim this site for themselves, at which point they will begin to spawn their own Knights into the game, so you'll need to either defend it or attack it before the wave is over.\r\n<br>If the <color=#3A9F87><b>Creatures</b></color> outnumber the <color=#FE4E2D><b>Humans</b></color> in the vicinity of the site, it will begin to be claimed back.\r\n<br>The more units, the more quickly it will be claimed.\r\n<br><color=#3A9F87><b>Creatures</b></color> cannot be purchased unless you have control of the <color=#4d705d><b>Horgr</b></color>.";
                break;
            case GlossaryID.Powers:
                _glossaryItem.title = "Powers";
                _glossaryItem.description =
                    "<color=#abb4ca><b>Rune</b></color>:<br><color=#abb4ca><b>Runes</b></color> are ancient, magical zones of wild energy that heal our <color=#3A9F87><b>Creatures</b></color> over time.\r\n<br><color=#caad87><b>Fyre</b></color>:<br><color=#caad87><b>Fyre</b></color> creates an explosion dealing damage to all <color=#FE4E2D><b>Humans</b></color> in its radius.\r\n<br><color=#aeaecf><b>Stormer</b></color>:<br>Allows you to create an intense storm that will randomly strike down <color=#FE4E2D><b>Humans</b></color> with lightning for a period of 1 minute.";
                break;
            case GlossaryID.DayNightCycle:
                _glossaryItem.title = "Day/Night Cycle";
                _glossaryItem.description =
                    "The game is divided into two phases: <color=#FCA343><b>Day</b></color> and <color=#24455A><b>Night</b></color>.<br>\r\nDuring the <color=#FCA343><b>Day</b></color>, <color=#FE4E2D><b>Human</b></color> settlers will encroach upon our <color=#9665cd><b>Grove</b></color>, seeking to cut down our <color=#6d8659><b>Trees</b></color> and hunt our <color=#da691e><b>Wildlife</b></color>. You must protect us against them until the <color=#FCA343><b>Day</b></color> is done.<br>\r\nAt <color=#24455A><b>Night</b></color>, we can recover and expand our <color=#9665cd><b>Grove</b></color>.";
                break;
            case GlossaryID.HumanClasses:
                _glossaryItem.title = "Human Classes";
                _glossaryItem.description =
                    "There are 4 <color=#FE4E2D><b>Human</b></color> classes that you will encounter.\r\n<br><color=#FFFF69><b>Woodcutters</b></color> are marked in <color=#FFFF69><b>YELLOW</b></color> on the Minimap.\r\n<br>Their primary target are your <color=#6d8659><b>Trees</b></color> and will prioritize cutting them down unless they are confronted by your <color=#3A9F87><b>Creatures</b></color>.\r\n<br><color=#30FE2C><b>Hunters</b></color> are marked in <color=#30FE2C><b>GREEN</b></color> on the Minimap.\r\n<br>Their main focus is your <color=#da691e><b>Wildlife</b></color> and will attempt to hunt your <color=#9665cd><b>Grove</b></color> into extinction unless a <color=#4d705d><b>Witch's Hut</b></color> or one of your <color=#3A9F87><b>Creatures</b></color> is closer.\r\n<br><color=#FE4E2D><b>Warriors</b></color> are marked in <color=#FE4E2D><b>RED</b></color> on the Minimap.\r\n<br>Their main goal is to kill all of your <color=#3A9F87><b>Creatures</b></color> and they will attempt to claim your <color=#4d705d><b>Horgr</b></color> if they are closer to it than they are to your <color=#3A9F87><b>Creatures</b></color>.";
                break;
            case GlossaryID.Dogs:
                _glossaryItem.title = "Dogs";
                _glossaryItem.description =
                    "These ferocious hounds have explosives strapped to their backs.<br>\r\n<br>They will appear at the beginning of a <color=#FCA343><b>Day</b></color> if there are plentiful <color=#6d8659><b>Trees</b></color> populating the <color=#9665cd><b>Grove</b></color> and will attempt to blow up your <color=#6d8659><b>trees</b></color>.<br>\r\n<br>They are easy to stop if you can intercept them but they’re fast moving.";
                break;
            case GlossaryID.Mines:
                _glossaryItem.title = "Mines";
                _glossaryItem.description =
                    "Occasionally, the <color=#FE4E2D><b>Humans</b></color> will bore through the earth and set up their iron mines, this creates a new spawn point that enemies can arrive from.<br>\r\nAny <color=#6d8659><b>Trees</b></color> in the area will be destroyed as it emerges.";
                break;
            case GlossaryID.Spies:
                _glossaryItem.title = "Spies";
                _glossaryItem.description =
                    "Spies are unique <color=#FE4E2D><b>Humans</b></color> that will attempt to sneak through your defences and attack your <color=#FEE65F><b>Home Tree</b></color> directly.<br>\r\nThey will arrive on the map at a random location and are marked in BLACK.<br>\r\nThey will move towards your <color=#FEE65F><b>Home Tree</b></color>, ignoring everything else in their path to destroy it.<br>\r\nThey will spawn in more regularly as the <color=#FCA343><b>Days</b></color> go by and can emerge at any time, even at <color=#24455A><b>Night</b></color>.";
                break;
            case GlossaryID.LordsOfTheLand:
                _glossaryItem.title = "Lords of the Land";
                _glossaryItem.description =
                    "These are high ranking members of the King’s Court, tasked with weakening the defences of the <color=#9665cd><b>Grove</b></color>.<br>\r\nThey are incredibly deadly fighters that will appear sporadically to cause as much chaos as possible.<br>\r\nIf you’re not prepared, they can easily cut through your <color=#3A9F87><b>Creatures</b></color> and destroy your <color=#FEE65F><b>Home Tree</b></color>.";
                break;
            case GlossaryID.Combat:
                _glossaryItem.title = "Combat";
                _glossaryItem.description =
                    "When a <color=#FE4E2D><b>Human</b></color> first arrives, they are invincible for 5 seconds.\r\n<br><b>Right Clicking</b> on an <color=#FE4E2D><b>Human</b></color> will order a selected <color=#3A9F87><b>Creature</b></color> to target it. They will track down the <color=#FE4E2D><b>Human</b></color> until they catch up to them.\r\n<br>The <color=#3A9F87><b>Creature</b></color> will behave differently, depending on which Combat Mode you have selected. This is represented by an icon above the <color=#3A9F87><b>Creature</b></color>.\r\n<br>Attack Mode:<br>Selecting <b>Attack Mode</b> allows the <color=#3A9F87><b>Creature</b></color> to move freely about the <color=#9665cd><b>Grove</b></color>, attacking any <color=#FE4E2D><b>Humans</b></color> that come within its range.\r\n<br>This is the default Combat Mode.\r\n<br><b>Defend Mode:</b>\r\n<br>Selecting <b>Defend Mode</b> orders the <color=#3A9F87><b>Creature</b></color> to defend its current position.\r\n<br>Its range is reduced and will move small distances to attack <color=#FE4E2D><b>Humans</b></color> but will always return to its original defence position.\r\n<br><b>Formations:</b>\r\n<br>Clicking the <b>Formations</b> button will change how spread out <color=#3A9F87><b>Creatures</b></color> are. You can choose to have them bunch them together to allow a more concentrated force or spread out to cover more ground.";
                break;
        }
    }


    public void ShowGlossaryItem(GlossaryID _glossaryID)
    {
        GlossaryItem gi = GetGlossaryItem(_glossaryID);
        glossaryImage.sprite = gi.image;
        glossaryTitleText.text = gi.title;
        glossaryDescriptionText.text = gi.description;
        lastSelectedGlossayID = _glossaryID;
        gi.available = true;
    }

    public void OpenGlossaryPanel()
    {
        ShowGlossaryItem(lastSelectedGlossayID);
        FadeX.FadeIn(glossaryPanel);
        _GM.ChangeGameState(GameState.Glossary);
    }
    public void CloseGlossaryPanel()
    {
        FadeX.FadeOut(glossaryPanel);
        _GM.ChangeGameState(GameState.Play);
        TweenX.TweenFill(newEntryLabel, _TWEENING.UIButtonTweenTime, _TWEENING.UIButtonTweenEase, 0);
    }

    public void SetInteractable(bool _interactable)
    {
        glossaryButton.interactable = _interactable;
    }

    public void NewGlossaryAvailable(GlossaryID id, string title)
    {
        newEntryTitle.text = title;
        TweenX.TweenFill(newEntryLabel, _TWEENING.UIButtonTweenTime, _TWEENING.UIButtonTweenEase, 1);
        newEntryLabel.GetComponent<Animator>().SetTrigger("TutorialAvailable");
        newEntryLabel.GetComponent<AudioSource>().Play();
        lastSelectedGlossayID = id;
    }

    #region Events
    public void OnDayBegin()
    {
        if (glossaryAvailable(GlossaryID.HumanClasses))
            return;

        ExecuteAfterSeconds(1, () => NewGlossaryAvailable(GlossaryID.HumanClasses, "Human Classes"));

        StartCoroutine(WaitToAddCombatTutorial());
    }
    IEnumerator WaitToAddCombatTutorial()
    {
        yield return new WaitForSeconds(10);
        NewGlossaryAvailable(GlossaryID.Combat, "Combat");
    }
    void OnMineSpawned()
    {
        if (!glossaryAvailable(GlossaryID.Mines))
            NewGlossaryAvailable(GlossaryID.Mines, "Mines");
    }
    void OnLordSpawned()
    {
        if (!glossaryAvailable(GlossaryID.LordsOfTheLand))
            NewGlossaryAvailable(GlossaryID.LordsOfTheLand, "Lords of the Land");
    }
    void OnSpySpawned()
    {
        if (!glossaryAvailable(GlossaryID.Spies))
            NewGlossaryAvailable(GlossaryID.Spies, "Spies");
    }
    void OnHomeTreeSelected()
    {
        if (!glossaryAvailable(GlossaryID.HomeTree) && _TUTORIAL.tutorialComplete == false)
            NewGlossaryAvailable(GlossaryID.HomeTree, "Home Tree");
    }


    private void OnEnable()
    {
        GameEvents.OnDayBegin += OnDayBegin;
        GameEvents.OnMineSpawned += OnMineSpawned;
        GameEvents.OnLordSpawned += OnLordSpawned;
        GameEvents.OnSpySpawned += OnSpySpawned;
        GameEvents.OnHomeTreeSelected += OnHomeTreeSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnDayBegin -= OnDayBegin;
        GameEvents.OnMineSpawned -= OnMineSpawned;
        GameEvents.OnLordSpawned -= OnLordSpawned;
        GameEvents.OnSpySpawned -= OnSpySpawned;
        GameEvents.OnHomeTreeSelected -= OnHomeTreeSelected;
    }
    #endregion

    #region Editor
    // IngredientDrawerUIE
    [CustomPropertyDrawer(typeof(GlossaryItem))]
    public class GlossaryItemUIE : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var idField = new PropertyField(property.FindPropertyRelative("glossaryID"));
            var imageField = new PropertyField(property.FindPropertyRelative("image"));

            // Add fields to the container.
            container.Add(idField);
            container.Add(imageField);

            return container;
        }
    }
    #endregion
}

[System.Serializable]
public class GlossaryItem
{
    public GlossaryID glossaryID;
    [HideInInspector] public string title;
    public Sprite image;
    //[TextArea]
    [HideInInspector] public string description;
    [HideInInspector] public bool available;
}