using UnityEngine;
using System.Collections;
using Unity.IntegerTime;

public class EnemyAnimation : GameBehaviour
{
    public Enemy enemy;
    public Collider weaponCollider;
    public bool isAttacking;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation()
    {
        if (isAttacking)
            return;
        
        isAttacking = true;
        StartCoroutine(PlayAttackAnimationRoutine());
    }

    IEnumerator PlayAttackAnimationRoutine()
    {
        animator.SetTrigger("Attack");

        //print("Starting Attack Animation");
        //Wait for them to enter the Attacking state
        while (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            yield return null;

        //Now wait for them to finish
        while (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            yield return null;
        //print("Finished Attack Animation");
        
        isAttacking = false;
        enemy.ChangeState(EnemyState.Work);
    }

    public void PlayWalkAnimation(float _speed) => animator.SetFloat("Speed", _speed);
    public void PlayImpactAnimation() => animator.SetTrigger("Impact");
    public void PlayIdleAnimation() => animator.SetTrigger("Idle");
    public void PlayRelaxAnimation() => animator.SetTrigger("Relax");
    public void PlayVictoryAnimation() => animator.SetTrigger("Cheer" + RandomCheerAnim());
    public int RandomCheerAnim(int _count = 3) => Random.Range(1, _count);

    #region Animation Events
    public void EnableWeaponCollider() => weaponCollider.enabled = true;
    public void DisableWeaponCollider() => weaponCollider.enabled = false;
    public void Attack(int _attack) => enemy.Attack(_attack);
    public void Footstep(string _foot) => enemy.Footstep(_foot);
    //Note that the draw sound needs to be in the appropriate array element
    public void PlayBowDrawSound() => enemy.PlaySound(enemy.unitData.attackSounds[1]);  
    #endregion
}
