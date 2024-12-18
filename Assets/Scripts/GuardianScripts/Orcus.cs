public class Orcus : Guardian
{
    public override void Attack(int _attack)
    {
        base.Attack(_attack);
        if (_NoEnemies)
            return;

        PlaySound(guardianData.attackSounds);
        ParticlesX.PlayParticles(attackParticles, rightHand.transform.position);
    }
}
