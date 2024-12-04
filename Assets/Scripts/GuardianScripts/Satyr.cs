public class Satyr : Unit
{
    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_NoEnemies)
            return;

        PlaySound(unitData.attackSounds);
        ParticlesX.PlayParticles(attackParticles, rightHand.transform.position);
    }
}
