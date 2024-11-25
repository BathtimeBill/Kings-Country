using UnityEngine;
using System.Collections.Generic;

public class ObjectPoolManager : MonoBehaviour
{
    public List<Enemy> enemyPool;

    public void AddToEnemyPool(Enemy _enemy)
    {
        if (enemyPool.Contains(_enemy))
            return;
        enemyPool.Add(_enemy);
    }

    public GameObject GetEnemyFromPool(HumanID _humanID)
    {
        List<Enemy> available = enemyPool.FindAll(x=> x.unitID == _humanID);
        if (available.Count == 0)
            return null;

        for (int i = 0; i < available.Count; i++)
        {
            if(!available[i].gameObject.activeSelf)
                return available[i].gameObject;
        }

        return null;
    }
    
}
