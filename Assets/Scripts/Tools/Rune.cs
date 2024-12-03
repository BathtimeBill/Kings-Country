public class Rune : GameBehaviour
{
    public void OnDayOver(int _day)
    {
        if(!_DATA.HasPerk(PerkID.Rune))
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnDayOver += OnDayOver;
    }

    private void OnDisable()
    {
        GameEvents.OnDayOver -= OnDayOver;
    }
}
