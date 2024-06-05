using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : Singleton<PerkManager>
{
    public List<PerkID> availablePerks;
    public List<PerkID> currentPerks;

    public bool HasPerk(PerkID perkID) => currentPerks.Contains(perkID);

    public void RemovePerk(PerkID perkID) => availablePerks.Remove(perkID);

    public void AddBackPerk(PerkID perkID) => availablePerks.Add(perkID);

    public void AddPerk(PerkID perkID) => currentPerks.Add(perkID);
    public PerkData GetPerk(PerkID perkID) => _DATA.GetPerk(perkID);

    public PerkID GetRandomPerk() => ListX.GetRandomItemFromList(availablePerks);
    public bool CanObtainPerk => availablePerks.Count > 1;

    private void Start()
    {
        for (int i = 0; i < _DATA.perkData.Count; i++)
        {
            availablePerks.Add(_DATA.perkData[i].id);
        }
    }
}