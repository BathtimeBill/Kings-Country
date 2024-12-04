using UnityEngine;
public class Huldra : Unit
{
    [Header("Huldra Specific")]
    public GameObject towerUnit;
    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_NoEnemies)
            return;

        PlaySound(unitData.attackSounds);
        ParticlesX.PlayParticles(attackParticles, rightHand.transform.position);
    }

    protected override void Tower()
    {
        base.Tower();
        Vector3 offset = new Vector3(0, -1.5f, 0);
        Instantiate(towerUnit, transform.position + offset, Quaternion.Euler(-90, 0, 0));
    }
}
