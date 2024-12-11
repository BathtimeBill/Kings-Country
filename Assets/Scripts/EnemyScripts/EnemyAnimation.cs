using UnityEngine;
using System.Collections;

public class EnemyAnimation : GameBehaviour
{
    private Enemy enemy;
    private bool isAttacking;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();
    }

    IEnumerator PlayAttackAnimationRoutine()
    {
        animator.SetTrigger("Attack");
        //Wait for them to enter the Attacking state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            yield return null;

        //Now wait for them to finish
        while (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            yield return null;
        print("Finished Attack Animation");
        
        isAttacking = false;
        enemy.SetState();
    }
    public void PlayWalkAnimation(float _speed) => animator.SetFloat("Speed", _speed);
    public void PlayAttackAnimation(bool _attacking) => animator.SetBool("Attacking", _attacking);
    public void PlayHoldAnimation(bool _holding) => animator.SetBool("Holding", _holding);
    public void PlayImpactAnimation() => animator.SetTrigger("Impact");
    public void PlayIdleAnimation() => animator.SetTrigger("Idle");
    public void PlayRelaxAnimation() => animator.SetTrigger("Relax");
    public void PlayVictoryAnimation() => animator.SetTrigger("Cheer" + RandomCheerAnim());
    public int RandomCheerAnim(int _count = 3) => Random.Range(1, _count);

    #region Animation Events
    public void EnableWeaponCollider() => enemy.weaponCollider.enabled = true;
    public void DisableWeaponCollider() => enemy.weaponCollider.enabled = false;
    public void Attack(int _attack) => enemy.Attack(_attack);
    public void Footstep(string _foot) => enemy.Footstep(_foot);
    //Note that the draw sound needs to be in the appropriate array element
    public void PlayBowDrawSound() => enemy.PlaySound(enemy.unitData.attackSounds[1]);  
    #endregion
}
