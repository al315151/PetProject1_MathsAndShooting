using System;
using System.Collections.Generic;
using VContainer.Unity;

public class EnemyControllerProvider : IInitializable, IDisposable
{
    private Dictionary<int, BaseEnemyController> baseEnemies;

    public void Dispose()
    {
        baseEnemies = null;
    }

    public void Initialize()
    {
        baseEnemies = new Dictionary<int, BaseEnemyController>();
    }

    public void AddEnemyController(BaseEnemyController enemy)
    {
        baseEnemies.Add(enemy.EnemyID, enemy);
    }

    public void RemoveEnemyController(BaseEnemyController enemy)
    {
        baseEnemies.Remove(enemy.EnemyID);
    }

    public void Reset()
    {
        baseEnemies.Clear();
    }

    public BaseEnemyController GetEnemyController(BaseEnemyView enemyView)
    {
        return baseEnemies[enemyView.EnemyID];
    }
}
