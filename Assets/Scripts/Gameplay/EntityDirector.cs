using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

public class EntityDirector : IInitializable, IDisposable
{
    private readonly EnemyBuilder enemyBuilder;

    private IEntityBuilder entityBuilder;

    public EntityDirector(EnemyBuilder enemyBuilder)
    {
        this.enemyBuilder = enemyBuilder;
    }


    public void Dispose()
    {
        if (entityBuilder != null)
        {
            entityBuilder.Reset();
        }

    }

    public void Initialize()
    {
        SpawnEnemies().Forget();
    }

    private async UniTask SpawnEnemies()
    {
        await UniTask.Delay(5000);

        for (int i = 0; i < 10; i++)
        {
            CreateBasicEnemy();

            await UniTask.Delay(1000);
        }
    }

    public BaseEnemy CreateBasicEnemy()
    {
        entityBuilder = enemyBuilder;
        
        entityBuilder.Build();
        var enemy = entityBuilder.GetResult();
        return (BaseEnemy)enemy;
    }
}
