using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitUpgradeButton : InteractableButton
{
    public UnitUpgrades unitUpgrades;
    public Image fillImage;
    public float fillSpeed = 0.5f;
    bool filling = false;
    private Vector3 startPosition;

    public float timePeriod = 2; // Time taken to complete left and right motion
    public float height = 30f;   // Amplitude of the sine wave
    public  float currentTime;
    private float timeSinceStart;
    private Vector3 pivot;

    public override void Start()
    {
        base.Start();
        startPosition = transform.position;
        pivot = transform.position;
        currentTime = timePeriod * 3;
    }
    void Update()
    {
        if (filling)
        {
            fillImage.fillAmount += fillSpeed * Time.deltaTime;
            if (fillImage.fillAmount >= 1)
            {
                UpgradeIt(unitUpgrades.ActiveUnit);
            }
            //TODO
            //Improve upgrade button shake
            currentTime -= Time.deltaTime / 3;
            currentTime = Mathf.Clamp(currentTime, 0.01f, 1);
            Vector3 nextPos = transform.position;
            nextPos.y = pivot.y + height + height * Mathf.Sin(((Mathf.PI * 2) / currentTime) * timeSinceStart);
            timeSinceStart += Time.deltaTime;
            transform.position = nextPos;
        }
        else
        {
            fillImage.fillAmount -= (fillSpeed * 3) * Time.deltaTime;
            transform.position = startPosition;
            currentTime = timePeriod * 3;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        filling = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        filling = false;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        unitUpgrades.ShowStatsUpgrade();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        unitUpgrades.ShowStats();
        filling = false;
    }

    public void UpgradeIt(UnitID _unitId)
    {

    }
}
