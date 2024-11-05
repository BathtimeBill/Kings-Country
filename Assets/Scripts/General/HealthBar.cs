using UnityEngine;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public Transform healthBarFill;
    public SpriteRenderer combatModeIcon;
    public TMPro.TMP_Text groupNumber;
    
    public void AdjustHealthBar(float _currentHealth, float _maxHealth) => healthBarFill.DOScaleX(MathX.MapTo01(_currentHealth, 0, _maxHealth), 0.2f);
    public void ChangeGroupNumber(string _groupNumber) => groupNumber.text = _groupNumber;
    public void ChangeCombatModeIcon(Sprite _icon) => combatModeIcon.sprite = _icon;
    
}
