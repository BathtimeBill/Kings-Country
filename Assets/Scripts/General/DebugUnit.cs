using UnityEngine;

public class DebugUnit : MonoBehaviour
{
    public Transform detectRange;
    public Transform attackRange;
    public Transform stopRange;

    private bool showRange = false;
    public void AdjustRange(float _detect, float _attack, float _stop)
    {
        ToggleObjects();
        detectRange.transform.localScale = new Vector3(_detect*2, _detect*2, _detect*2);
        attackRange.transform.localScale = new Vector3(_attack*2, _attack*2, _attack*2);
        stopRange.transform.localScale = new Vector3(_stop*2, _stop*2, _stop*2);
    }

    private void OnShowRangeButton(bool _show)
    {
        showRange = !showRange;
        ToggleObjects();
    }

    private void ToggleObjects()
    {
        detectRange.gameObject.SetActive(showRange);
        attackRange.gameObject.SetActive(showRange);
        stopRange.gameObject.SetActive(showRange);
    }
    
    private void OnEnable()
    {
        InputManager.OnShowRangeButton += OnShowRangeButton;
    }
    private void OnDisable()
    {
        InputManager.OnShowRangeButton -= OnShowRangeButton;
    }
    
}
