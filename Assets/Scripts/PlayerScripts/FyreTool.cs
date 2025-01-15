using UnityEngine;

public class FyreTool : Tool
{
    public GameObject explosion;
    public GameObject explosion2;
    public GameObject effectRadius;

    public override void Select()
    {
        base.Select();
    }

    public override void Deselect()
    {
        base.Deselect();
    }

    public override void Use()
    {
        base.Use();
        Instantiate(_DATA.HasPerk(PerkID.Fyre) ? explosion2 : explosion, selectObjects.transform.position, selectObjects.transform.rotation);
        _CAMERA.CameraShake(_SETTINGS.cameraSettings.fyreShakeIntensity);
        GameEvents.ReportOnFyrePlaced();
        Deselect();
    }
    private void OnBeaconUpgrade()
    {
        effectRadius.transform.localScale = effectRadius.transform.localScale * 2;
    }

    
    private void OnEnable()
    {
        GameEvents.OnBeaconUpgrade += OnBeaconUpgrade;
    }

    private void OnDisable()
    {
        GameEvents.OnBeaconUpgrade -= OnBeaconUpgrade;
    }
}
