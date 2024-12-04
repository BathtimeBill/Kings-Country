using UnityEngine;

public class EnemyAnimation : GameBehaviour
{
    public Enemy enemy;
    public Collider weaponCollider;
    public bool isAttacking;
    
    public void PlayAttack()
    {
        if (!isAttacking)
        {
            //animator.SetTrigger("Attack" + GetRandomAnimation());
            isAttacking = true;
        }
    }
    public void CheckAttack()
    {
        isAttacking = false;
    }
    
    #region Animation Events
    public void EnableWeaponCollider() => weaponCollider.enabled = true;
    public void DisableWeaponCollider() => weaponCollider.enabled = false;
    public void Attack(int _attack) => enemy.Attack(_attack);
    public void Footstep(string _foot) => enemy.Footstep(_foot);
    public void CheckState() => enemy.CheckState();
    //Note that the draw sound needs to be in the appropriate array element
    public void PlayBowDrawSound() => enemy.PlaySound(enemy.unitData.attackSounds[1]);  
    #endregion
}
