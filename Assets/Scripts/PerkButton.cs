using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PerkButton : GameBehaviour
{
    Image image;
    Button button;
    public Perk perk;
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
        if(perk == Perk.satyr)
        {
            if(_PERK.satyrPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.orcus)
        {
            if (_PERK.orcusPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.leshy)
        {
            if (_PERK.leshyPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.willow)
        {
            if (_PERK.willowPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.skessa)
        {
            if (_PERK.skessaPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.goblin)
        {
            if (_PERK.goblinPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.fidhain)
        {
            if (_PERK.fidhainPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.oak)
        {
            if (_PERK.oakPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.huldra)
        {
            if (_PERK.huldraPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.golem)
        {
            if (_PERK.golemPerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.explosiveTree)
        {
            if (_PERK.explosiveTreePerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.homeTree)
        {
            if (_PERK.homeTreePerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.rune)
        {
            if (_PERK.runePerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.fyre)
        {
            if (_PERK.fyrePerk == true)
            {
                button.interactable = false;
                purchased.SetActive(true);
            }
        }
        if (perk == Perk.bear)
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
            case Perk.satyr:
                _PERK.satyrPerk = true;
                break;
            case Perk.orcus:
                _PERK.orcusPerk = true;
                break;
            case Perk.leshy:
                _PERK.leshyPerk = true;
                break;
            case Perk.willow:
                _PERK.willowPerk = true;
                break;
            case Perk.skessa:
                _PERK.skessaPerk = true;
                break;
            case Perk.goblin:
                _PERK.goblinPerk = true;
                break;
            case Perk.fidhain:
                _PERK.fidhainPerk = true;
                break;
            case Perk.oak:
                _PERK.oakPerk = true;
                break;
            case Perk.huldra:
                _PERK.huldraPerk = true;
                break;
            case Perk.golem:
                _PERK.golemPerk = true;
                break;
            case Perk.explosiveTree:
                _PERK.explosiveTreePerk = true;
                break;
            case Perk.homeTree:
                _PERK.homeTreePerk = true;
                break;
            case Perk.rune:
                _PERK.runePerk = true;
                break;
            case Perk.fyre:
                _PERK.fyrePerk = true;
                break;
            case Perk.bear:
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
