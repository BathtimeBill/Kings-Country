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
        detectRange.transform.localScale = new Vector3(_detect, _detect, _detect);
        attackRange.transform.localScale = new Vector3(_attack, _attack, _attack);
        stopRange.transform.localScale = new Vector3(_stop, _stop, _stop);
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
