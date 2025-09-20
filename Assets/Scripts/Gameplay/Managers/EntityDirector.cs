using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using VContainer.Unity;

public class EntityDirector : IInitializable, IDisposable
{
    private readonly EnemyBuilder enemyBuilder;

    private IEntityBuilder entityBuilder;

    private List<BaseEnemyController> baseEnemies;

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
    }

    public async UniTask SpawnEnemies()
    {
        baseEnemies = new List<BaseEnemyController>();

        for (int i = 0; i < 10; i++)
        {
            var baseEnemy = CreateBasicEnemy();
            baseEnemies.Add(baseEnemy);

            await UniTask.Delay(1000);
        }
    }

    public BaseEnemyController CreateBasicEnemy()
    {
        entityBuilder = enemyBuilder;
        
        entityBuilder.Build();
        var enemy = entityBuilder.GetResult();
        return (BaseEnemyController)enemy;
    }
}
