using System;
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
        CreateBasicEnemy();
    }

    public BaseEnemy CreateBasicEnemy()
    {
        entityBuilder = enemyBuilder;
        
        entityBuilder.Build();
        var enemy = entityBuilder.GetResult();
        return (BaseEnemy)enemy;
    }
}
