using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PerkButton : GameBehaviour
{
    Image image;
    Button button;
    public PerkID perk;
    public Sprite avatar;
    public int myPrice;
    public GameObject purchased;
    public bool hasBeenPurchased;


    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }
    void Start()
    {
        image.sprite = avatar;
        StartCoroutine(CheckMaegen());

    }
    void Setup()
    {
        if(perk == PerkID.satyr)
        {
            if(_PERK.satyrPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.orcus)
        {
            if (_PERK.orcusPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.leshy)
        {
            if (_PERK.leshyPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.willow)
        {
            if (_PERK.willowPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.skessa)
        {
            if (_PERK.skessaPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.goblin)
        {
            if (_PERK.goblinPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.fidhain)
        {
            if (_PERK.fidhainPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.oak)
        {
            if (_PERK.oakPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.huldra)
        {
            if (_PERK.huldraPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.golem)
        {
            if (_PERK.golemPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.explosiveTree)
        {
            if (_PERK.explosiveTreePerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.homeTree)
        {
            if (_PERK.homeTreePerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.rune)
        {
            if (_PERK.runePerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.fyre)
        {
            if (_PERK.fyrePerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == PerkID.bear)
        {
            if (_PERK.bearPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }

    }
    IEnumerator CheckMaegen()
    {
        yield return new WaitForSeconds(0.2f);
        if(hasBeenPurchased == false)
        {
            if (_OM.overWorldMaegenTotal >= myPrice)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
        Setup();
        //StartCoroutine(WaitForSave());
    }
    public void PressButton()
    {
        _OM.overWorldMaegenTotal -= myPrice;
        hasBeenPurchased = true;
        purchased.SetActive(true);
        switch (perk)
        {
            case PerkID.satyr:
                _PERK.satyrPerk = true;
                break;
            case PerkID.orcus:
                _PERK.orcusPerk = true;
                break;
            case PerkID.leshy:
                _PERK.leshyPerk = true;
                break;
            case PerkID.willow:
                _PERK.willowPerk = true;
                break;
            case PerkID.skessa:
                _PERK.skessaPerk = true;
                break;
            case PerkID.goblin:
                _PERK.goblinPerk = true;
                break;
            case PerkID.fidhain:
                _PERK.fidhainPerk = true;
                break;
            case PerkID.oak:
                _PERK.oakPerk = true;
                break;
            case PerkID.huldra:
                _PERK.huldraPerk = true;
                break;
            case PerkID.golem:
                _PERK.golemPerk = true;
                break;
            case PerkID.explosiveTree:
                _PERK.explosiveTreePerk = true;
                break;
            case PerkID.homeTree:
                _PERK.homeTreePerk = true;
                break;
            case PerkID.rune:
                _PERK.runePerk = true;
                break;
            case PerkID.fyre:
                _PERK.fyrePerk = true;
                break;
            case PerkID.bear:
                _PERK.bearPerk = true;
                break;
        }
        button.interactable = false;
        _OM.maegenText.text = _OM.overWorldMaegenTotal.ToString();
        GameEvents.ReportOnPerkButtonPressed();

    }
    IEnumerator WaitForSave()
    {
        yield return new WaitForEndOfFrame();
        _OM.maegenText.text = _OM.overWorldMaegenTotal.ToString();
        //_SAVE.Save();
    }

    private void OnPerkButtonPressed()
    {
        StartCoroutine(CheckMaegen());
    }
    private void OnEnable()
    {
        GameEvents.OnPerkButtonPressed += OnPerkButtonPressed;
    }

    private void OnDisable()
    {
        GameEvents.OnPerkButtonPressed -= OnPerkButtonPressed;
    }
}
