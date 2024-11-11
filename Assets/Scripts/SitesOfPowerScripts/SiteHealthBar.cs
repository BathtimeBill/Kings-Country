using UnityEngine;

public class SiteHealthBar : GameBehaviour
{
    public Transform healthBarFill;
    public SpriteRenderer healthBarFillSprite;
    public SpriteRenderer claimedIcon;
    
    public void ChangeClaimedIcon(Sprite _icon) => claimedIcon.sprite = _icon;
    
    public void AdjustHealthBar(float _current, float _max, Color _color)
    {
        Vector3 temp = healthBarFill.localScale;
        temp.x = MathX.MapTo01(_current, 0, _max);
        healthBarFill.localScale = temp;
        healthBarFillSprite.color = _color;
    }
}
