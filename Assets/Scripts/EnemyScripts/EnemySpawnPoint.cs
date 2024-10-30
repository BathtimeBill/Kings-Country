public class EnemySpawnPoint : GameBehaviour
{
    private void Awake()
    {
        _EM.AddSpawnPoint(gameObject);
    }
}
