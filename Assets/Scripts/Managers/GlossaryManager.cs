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
    public List<GlossaryID> unlockedItems;

    public GlossaryItem GetGlossaryItem(GlossaryID _id) => glossaryItems.Find(x=>x.glossaryID == _id);
    private bool glossaryAvailable(GlossaryID _id) => unlockedItems.Contains(_id);

    private void Start()
    {
        glossaryButton.onClick.AddListener(()=> OpenGlossaryPanel());
        lastSelectedGlossayID = GlossaryID.CameraControls;
        for(int i=0; i<glossaryItems.Count; i++)
        {
            SetupGlossaryItems(GetGlossaryItem(glossaryItems[i].glossaryID));
        }
        newEntryLabel.fillAmount = 0;

        unlockedItems = _SAVE.GetGlossaryItemsUnlocked;

        ExecuteNextFrame(() =>
        {
            for (int i = 0; i < glossaryButtons.Count; i++)
                glossaryButtons[i].Setup();
        });
    }

    private void SetupGlossaryItems(GlossaryItem _glossaryItem)
    {
        switch(_glossaryItem.glossaryID)
        {
            case GlossaryID.CameraControls:
                _glossaryItem.title = "Camera Controls";
                _glossaryItem.description =
                    $"{_ICONS.GetTMPIcon(_ICONS.keyboardMoveIcon)}or move the mouse cursor to the edge of the screen to MOVE the camera.<br><br>" +
                    $"Hold {_ICONS.GetTMPIcon(_ICONS.keyboardShiftIcon)} to hasten camera movement.<br><br>" +
                    $"{_ICONS.GetTMPIcon(_ICONS.mouseRotate)} to ROTATE the camera.<br><br>" +
                    $"{_ICONS.GetTMPIcon(_ICONS.mouseZoom)} to ZOOM the camera.";
                break;
            case GlossaryID.CreatureMovement:
                _glossaryItem.title = "Creature Movement";
                _glossaryItem.description =
                    $"To select a {GetName(ObjectID.Creature)}, click on it with the <b>Left Mouse Button</b> or click and drag over multiple {GetName(ObjectID.Creature, true)} to select more than one.<br>" + 
                    $"With a selected {GetName(ObjectID.Creature)}, <b>Right Cick</b> on a location to send them there.<br>" +
                    $"Our {GetName(ObjectID.Creature, true)} will defend that location if {GetName(ObjectID.Human, true)} come within their range.";
                break;
            case GlossaryID.Maegen:
                _glossaryItem.title = "Maegen";
                _glossaryItem.description =
                    $"{GetName(ObjectID.Maegen)} is the raw, wild energy of the {GetName(ObjectID.Grove)}.<br>" + 
                    $"It functions as the main currency of the game.<br><br>" +
                    $"{GetName(ObjectID.Maegen)} is created by {GetName(ObjectID.Tree, true)} at the end of each {GetName(ObjectID.Day)} and is used to grow more {GetName(ObjectID.Tree, true)} and spawn {GetName(ObjectID.Creature, true)}.<br><br>" +
                    $"Sometimes when a {GetName(ObjectID.Human)} is killed, the {GetName(ObjectID.Grove)} can harvest {GetName(ObjectID.Maegen)} from their soul.";
                break;
            case GlossaryID.Trees:
                _glossaryItem.title = "Trees";
                _glossaryItem.description =
                    $"{GetName(ObjectID.Tree, true)} are how we increase our power and earn more {GetName(ObjectID.Maegen)}.<br>" +
                    $"The productivity of each {GetName(ObjectID.Tree)} is determined by its proximity to others in the {GetName(ObjectID.Grove)}.<br>" +
                    $"{GetName(ObjectID.Tree, true)} clustered together are less productive but easier to defend, while those spread out yield more {GetName(ObjectID.Maegen)} but are more vulnerable to attack.<br> " +
                    $"To grow a {GetName(ObjectID.Tree)}, click on the {GetName(ObjectID.Tree)} button and {_ICONS.GetTMPIcon(_ICONS.mouseLeftClick)} on an available space in our {GetName(ObjectID.Grove)}.<br>" +
                    $"{_ICONS.GetTMPIcon(_ICONS.mouseRightClick)} to deselect {GetName(ObjectID.Tree)} mode.";
                break;
            case GlossaryID.Wildlife:
                _glossaryItem.title = "Wildlife";
                _glossaryItem.description =
                    $"{GetName(ObjectID.Wildlife)} is spawned into the {GetName(ObjectID.Grove)} at the end of each {GetName(ObjectID.Day)}, based on the number of {GetName(ObjectID.Tree, true)}.<br><br>" + 
                    $"{GetName(ObjectID.Wildlife)} is required for us to use our Powers.<br><br>"+
                    $"Hold down <b>Left-Alt</b> to see our {GetName(ObjectID.Wildlife)} highlighted.";
                break;
            case GlossaryID.Populous:
                _glossaryItem.title = "Populous";
                _glossaryItem.description =
                    $"{GetName(ObjectID.Populous)} is the maximum number of {GetName(ObjectID.Creature, true)} you can command in one {GetName(ObjectID.Grove)}.<br><br>" +
                    $"Each {GetName(ObjectID.Creature)} takes up 1 {GetName(ObjectID.Populous)} point.<br><br>" +
                    $"{GetName(ObjectID.Populous)} can be upgraded by +5 with a {GetName(ObjectID.Perk)}.";
                break;
            case GlossaryID.HomeTree:
                _glossaryItem.title = "Home Tree";
                _glossaryItem.description =
                    $"This is our {GetName(ObjectID.HomeTree)}, the heart of our {GetName(ObjectID.Grove)} and vital to its survival.<br><br>" +
                    $"From here, you can summon {GetName(ObjectID.Creature, true)} to fight for us.<br><br>"+
                    $"If the {GetName(ObjectID.HomeTree)} is ever destroyed, the game is over.<br><br>"+
                    $"To open the {GetName(ObjectID.HomeTree)} menu, either click on it in the game world or press <b>Tab</b>";
                break;
            case GlossaryID.WitchsHut:
                _glossaryItem.title = "Witch's Hut";
                _glossaryItem.description =
                    $"A lone witch in the woods allows the {GetName(ObjectID.Grove)}’s {GetName(ObjectID.HomeTree, true)} to gather here.<br><br>"+
                    $"{GetName(ObjectID.Human, true)} will attempt to claim this site for themselves, which they do by being in its vicinity without opposition from any {GetName(ObjectID.Creature, true)}, at which point they will begin to spawn their top tier {GetName(ObjectID.Human)} units into the game.<br><br>"+
                    $"You'll need to either defend it or attack it before the {GetName(ObjectID.Day)} is over.<br><br>" +
                    $"If the {GetName(ObjectID.Creature, true)} outnumber the {GetName(ObjectID.Human, true)} in the vicinity of the site, it will begin to be claimed back. The more units, the more quickly it will be claimed.<br><br>" +
                    $"{GetName(ObjectID.Creature, true)} from this site cannot be purchased unless you have control of the {GetName(ObjectID.Hut)}.";
                break;
            case GlossaryID.Horgr:
                _glossaryItem.title = "Horgr";
                _glossaryItem.description =
                    $"This is a magical shrine that is valuable to both the {GetName(ObjectID.Human, true)} and the {GetName(ObjectID.Grove)}.<br><br>" +
                    $"Enemies will attempt to claim this site for themselves, at which point they will begin to spawn their own {GetName(ObjectID.Knight, true)} into the game, so you'll need to either defend it or attack it before the wave is over.<br><br>" +
                    $"If the {GetName(ObjectID.Creature, true)} outnumber the {GetName(ObjectID.Human, true)} in the vicinity of the site, it will begin to be claimed back. The more units, the more quickly it will be claimed.<br><br>" +
                    $"{GetName(ObjectID.Creature, true)} cannot be purchased unless you have control of the {GetName(ObjectID.Horgr, true)}.";
                break;
            case GlossaryID.Powers:
                _glossaryItem.title = "Powers";
                _glossaryItem.description =
                    $"{GetName(ObjectID.Rune, true)} are ancient, magical zones of wild energy that heal our {GetName(ObjectID.Creature, true)} over time.<br><br>" +
                    $"{GetName(ObjectID.Fyre)} creates an explosion dealing damage to all {GetName(ObjectID.Human, true)} in its radius.<br><br>" +
                    $"{GetName(ObjectID.Stormer)} allows you to create an intense storm that will randomly strike down {GetName(ObjectID.Human, true)} with lightning for a period of 1 minute.";
                break;
            case GlossaryID.DayNightCycle:
                _glossaryItem.title = "Day/Night Cycle";
                _glossaryItem.description =
                    $"The game is divided into two phases: {GetName(ObjectID.Day)} and {GetName(ObjectID.Night)}.<br><br>" +
                    $"During the {GetName(ObjectID.Day)}, {GetName(ObjectID.Human)} settlers will encroach upon our {GetName(ObjectID.Grove)}, seeking to cut down our {GetName(ObjectID.Tree, true)} and hunt our {GetName(ObjectID.Wildlife)}. You must protect the {GetName(ObjectID.Grove)} against them until the {GetName(ObjectID.Day)} is done.<br><br>" +
                    $"At {GetName(ObjectID.Night)}, we can recover and expand our {GetName(ObjectID.Grove)}.";
                break;
            case GlossaryID.HumanClasses:
                _glossaryItem.title = "Human Classes";
                _glossaryItem.description =
                    $"There are 4 {GetName(ObjectID.Human)} classes that you will encounter.<br><br>" +
                    $"{GetName(ObjectID.Woodcutter, true)} primary target are your {GetName(ObjectID.Tree, true)} and will prioritize cutting them down unless they are confronted by your {GetName(ObjectID.Creature, true)}.<br><br>" +
                    $"{GetName(ObjectID.Hunter, true)} main focus are your {GetName(ObjectID.Wildlife)} and will attempt to hunt your {GetName(ObjectID.Grove)} into extinction unless a {GetName(ObjectID.Hut)} or one of your {GetName(ObjectID.Creature, true)} is closer.<br><br>" +
                    $"{GetName(ObjectID.Warrior, true)} main goal are to kill all of your {GetName(ObjectID.Creature, true)} and they will attempt to claim your {GetName(ObjectID.Horgr, true)} if they are closer to it than they are to your {GetName(ObjectID.Creature, true)}.";
                break;
            case GlossaryID.Dogs:
                _glossaryItem.title = "Dogs";
                _glossaryItem.description =
                    $"These ferocious hounds have explosives strapped to their backs.<br><br>" +
                    $"They will appear at the beginning of a {GetName(ObjectID.Day)} if there are plentiful {GetName(ObjectID.Tree, true)} populating the {GetName(ObjectID.Grove, true)} and will attempt to blow up your {GetName(ObjectID.Tree, true)}.<br><br>" +
                    $"They are easy to stop if you can intercept them but they’re fast moving!";
                break;
            case GlossaryID.Mines:
                _glossaryItem.title = "Mines";
                _glossaryItem.description =
                    $"Occasionally, the {GetName(ObjectID.Human, true)} will bore through the earth and set up their Iron Mines<br><br>" +
                    $"This creates a new spawn point that {GetName(ObjectID.Human, true)} can arrive from.<br><br>" +
                    $"Any {GetName(ObjectID.Tree, true)} in the area will be destroyed as it emerges.";
                break;
            case GlossaryID.Spies:
                _glossaryItem.title = "Spies";
                _glossaryItem.description =
                    $"{GetName(ObjectID.Spy, true)} are unique {GetName(ObjectID.Human, true)} that will attempt to sneak through your defences and attack your {GetName(ObjectID.HomeTree, true)} directly.<br><br>" +
                    $"They will arrive on the map at a random location and are marked in BLACK.<br><br>" +
                    $"They will move towards your {GetName(ObjectID.HomeTree, true)}, ignoring everything else in their path to destroy it.<br><br>" +
                    $"They will spawn in more regularly as the {GetName(ObjectID.Day, true)} go by and can emerge at any time, even at {GetName(ObjectID.Night)}.";
                break;
            case GlossaryID.LordsOfTheLand:
                _glossaryItem.title = "Lords of the Land";
                _glossaryItem.description =
                    $"These are high ranking members of the King’s Court, tasked with weakening the defences of the {GetName(ObjectID.Grove)}.<br><br>" +
                    $"They are incredibly deadly fighters that will appear sporadically to cause as much chaos as possible.<br><br>" +
                    $"If you’re not prepared, they can easily cut through your {GetName(ObjectID.Creature, true)} and destroy your {GetName(ObjectID.HomeTree, true)}.";
                break;
            case GlossaryID.Combat:
                _glossaryItem.title = "Combat";
                _glossaryItem.description =
                    $"When a {GetName(ObjectID.Human)} first arrives, they are invincible for 5 seconds.<br><br>" +
                    $"<b>Right Clicking</b> on an {GetName(ObjectID.Human)} will order a selected {GetName(ObjectID.Creature)} to target it. They will track down the {GetName(ObjectID.Human)} until they catch up to them.<br>The {GetName(ObjectID.Creature)} will behave differently, depending on which Combat Mode you have selected. This is represented by an icon above the {GetName(ObjectID.Creature)}.<br><br>" +
                    $"Attack Mode:<br>Selecting <b>Attack Mode</b> allows the {GetName(ObjectID.Creature)} to move freely about the {GetName(ObjectID.Grove)}, attacking any {GetName(ObjectID.Human, true)} that come within its range.<br>This is the default Combat Mode.<br><br>" +
                    $"<b>Defend Mode:</b><br>Selecting <b>Defend Mode</b> orders the {GetName(ObjectID.Creature)} to defend its current position.<br>It's range is reduced and will move small distances to attack {GetName(ObjectID.Human, true)} but will always return to its original defence position.<br><br>" +
                    $"<b>Formations:</b><br>Clicking the <b>Formations</b> button will change how spread out {GetName(ObjectID.Creature, true)} are. You can choose to have them bunch them together to allow a more concentrated force or spread out to cover more ground.";
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
    }

    public void OpenGlossaryPanel()
    {
        ShowGlossaryItem(lastSelectedGlossayID);
        FadeX.FadeIn(glossaryPanel);
        if(!_inTutorial)
            _GM.SetPreviousState(_currentGameState);
        _GM.ChangeGameState(GameState.Glossary);
    }
    public void CloseGlossaryPanel()
    {
        FadeX.FadeOut(glossaryPanel);
        _GM.ChangeGameState(_previousGameState);
        TweenX.TweenFill(newEntryLabel, _TWEENING.UIButtonTweenTime, _TWEENING.UIButtonTweenEase, 0);
        _TUTORIAL.ClosedGlossary();
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
        UnlockGlossaryItem(id);
    }


    public void UnlockGlossaryItem(GlossaryID _id)
    {
        unlockedItems = _SAVE.GetGlossaryItemsUnlocked;

        if (unlockedItems.Contains(_id))
            return;

        unlockedItems.Add(_id);
        _SAVE.SetGlossaryItemsUnlocked(unlockedItems);

        if (glossaryButtons.Find(x => x.glossaryID == _id) != null)
            glossaryButtons.Find(x=>x.glossaryID == _id).Unlock(true);
    }

    #region Events
    public void OnDayBegin()
    {
        if (!glossaryAvailable(GlossaryID.HumanClasses))
            ExecuteAfterSeconds(1, () => NewGlossaryAvailable(GlossaryID.HumanClasses, "Human Classes"));

        if (!glossaryAvailable(GlossaryID.Combat))
            ExecuteAfterSeconds(10, () => NewGlossaryAvailable(GlossaryID.Combat, "Combat"));
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
}