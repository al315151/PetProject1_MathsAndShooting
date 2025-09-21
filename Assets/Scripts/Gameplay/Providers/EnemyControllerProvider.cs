using System;
using System.Collections.Generic;
using VContainer.Unity;

public class EnemyControllerProvider : IInitializable, IDisposable
{
    private List<BaseEnemyController> baseEnemies;

    public void Dispose()
    {
        baseEnemies = null;
    }

    public void Initialize()
    {
        baseEnemies = new List<BaseEnemyController>();
    }

    public void AddEnemyController(BaseEnemyController enemy)
    {
        baseEnemies.Add(enemy);
    }

    public void RemoveEnemyController(BaseEnemyController enemy)
    {
        baseEnemies.Remove(enemy);
    }

    public BaseEnemyController GetEnemyController(BaseEnemyView enemyView)
    {
        for (int i = 0; i < baseEnemies.Count; i++)
        {
            if (baseEnemies[i].EnemyID == enemyView.EnemyID)
            {
                return baseEnemies[i];
            }
        }
        return null;
    }
}
