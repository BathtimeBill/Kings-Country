using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : Singleton<WaveManager>
{
    public TMP_Text waveTitleText;
    public GameObject testButton;
    public List<GameObject> upgradeButtonList;
    public GameObject slot1;
    public GameObject slot2;
    public Button continueButton;

    private Button slot1Button;
    private Button slot2Button;

    public GameObject slot1Obj;
    public GameObject slot2Obj;

    private GameObject selectedSLot;

    public GameObject slot1Box;
    public GameObject slot2Box;

    public int arrayNumber;
    public int arrayNumber2;

    public Animator animator;

    public bool mouseOverSlot1;
    public bool mouseOverSlot2;

    public int winLevel;

    [Header("Stats")]
    public int totalMaegen;
    public int totalTrees;
    public int totalMaegenDrops;
    public TMP_Text totalMaegenText;
    public TMP_Text totalTreesText;
    public TMP_Text treeBonusText;
    public TMP_Text totalMaegenDropsText;
    public TMP_Text penaltyText;

    public GameObject treeTextGroup;
    public GameObject bonusTextGroup;
    public GameObject penaltyTextGroup;
    public GameObject totalTextGroup;

    
    IEnumerator ActivateTextGroups()
    {
        Debug.Log("Bringing in text groups");
        yield return new WaitForSecondsRealtime(1.6f);
        _SM.PlaySound(_SM.textGroupSound);
        treeTextGroup.SetActive(true);
        yield return new WaitForSecondsRealtime(0.6f);
        _SM.PlaySound(_SM.textGroupSound);
        bonusTextGroup.SetActive(true);
        yield return new WaitForSecondsRealtime(0.6f);
        _SM.PlaySound(_SM.textGroupSound);
        penaltyTextGroup.SetActive(true);
        yield return new WaitForSecondsRealtime(0.6f);
        _SM.PlaySound(_SM.textGroupSound);
        totalTextGroup.SetActive(true);
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        //GameEvents.ReportOnWaveOver();
    //        GameEvents.ReportOnGameWin();
    //    }
    //}

    private void Start()
    {
        if(_TUTM.isTutorial)
        {
            winLevel = 3;
        }
        else
        {
            winLevel = 10;
        }
    }

    private int GetRandomUpgrade()
    {
        int rnd = Random.Range(0, upgradeButtonList.Count);
        return rnd;
    }
    int GetRandom()
    {
        int rand = Random.Range(0, upgradeButtonList.Count);
        while (rand == arrayNumber)
            rand = Random.Range(0, upgradeButtonList.Count);
        arrayNumber2 = rand;
        return rand;
    }
    public void OnWaveOver()
    {
        if(_GM.currentWave != winLevel)
        {
            if (upgradeButtonList.Count > 1)
            {
                continueButton.interactable = false;
                StartCoroutine(ActivateTextGroups());
                arrayNumber = GetRandomUpgrade();

                animator.SetTrigger("PanelEnter");
                _SM.PlaySound(_SM.waveOverSound);
                _SM.PlaySound(_SM.menuDragSound);
                waveTitleText.text = "Wave " + _GM.currentWave.ToString() + " is complete!";
                GameObject go1 = Instantiate(upgradeButtonList[arrayNumber], slot1.transform.position, slot1.transform.rotation);
                go1.transform.SetParent(slot1.transform, false);
                go1.transform.position = slot1.transform.position;
                //upgradeButtonList.Remove(go1);
                arrayNumber2 = GetRandom();
                GameObject go2 = Instantiate(upgradeButtonList[arrayNumber2], slot2.transform.position, slot2.transform.rotation);
                go2.transform.SetParent(slot2.transform, false);
                go2.transform.position = slot2.transform.position;
                //upgradeButtonList.Remove(go2);
                slot1Button = go1.GetComponent<Button>();
                slot2Button = go2.GetComponent<Button>();
                slot1Button.interactable = true;
                slot2Button.interactable = true;
                slot1Obj = go1;
                slot2Obj = go2;

                totalTrees = _GM.trees.Count;
                totalMaegenDrops = GameObject.FindGameObjectsWithTag("MaegenDrop").Length;
                totalMaegen = totalTrees + totalMaegenDrops + GetTreeBonusTotal();

                penaltyText.text = "+" + _FM.numberOfWildlifeToSpawn.ToString();
                totalTreesText.text = totalTrees.ToString();
                treeBonusText.text = "(+" +  GetTreeBonusTotal().ToString() + ")";
                totalMaegenDropsText.text = totalMaegenDrops.ToString();
                totalMaegenText.text = "+" + totalMaegen.ToString();
            }
        }
        else
        {
            GameEvents.ReportOnGameWin();
        }

    }
    private int GetTreeBonusTotal()
    {
        int treeBonus = 0;
        foreach (GameObject i in _GM.trees)
        {
            treeBonus = treeBonus + i.GetComponent<Tree>().energyMultiplier;
        }
        int totalTreeBonus = treeBonus - totalTrees;
        return totalTreeBonus;
    }
    public void MouseOverSlot1()
    {
        mouseOverSlot1 = true;
    }
    public void MouseOverSlot2()
    {
        mouseOverSlot2 = true;
    }
    public void MouseExitSlot1()
    {
        mouseOverSlot1 = false;
    }
    public void MouseExitSlot2()
    {
        mouseOverSlot2 = false;
    }
    public void OnContinueButton()
    {
        Destroy(slot1Button.gameObject);
        Destroy(slot2Button.gameObject);
        _SM.PlaySound(_SM.closeMenuSound);
        upgradeButtonList.Remove(selectedSLot);
        slot1Box.SetActive(false);
        slot2Box.SetActive(false);

        treeTextGroup.SetActive(false);

        bonusTextGroup.SetActive(false);

        penaltyTextGroup.SetActive(false);

        totalTextGroup.SetActive(false);
    }
    public void OnUpgradeSelected()
    {
        _SM.PlaySound(_SM.upgradeSound);
        if(mouseOverSlot1)
        {
            selectedSLot = upgradeButtonList[arrayNumber];
            slot1Box.SetActive(true); 
        }
        if(mouseOverSlot2)
        {
            selectedSLot = upgradeButtonList[arrayNumber2];
            slot2Box.SetActive(true);
        }
        slot1Button.interactable = false;
        slot2Button.interactable = false;
        continueButton.interactable = true;
    }

    private void OnEnable()
    {
        GameEvents.OnWaveOver += OnWaveOver;
        GameEvents.OnContinueButton += OnContinueButton;
        GameEvents.OnUpgradeSelected += OnUpgradeSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnWaveOver -= OnWaveOver;
        GameEvents.OnContinueButton -= OnContinueButton;
        GameEvents.OnUpgradeSelected -= OnUpgradeSelected;
    }
}
