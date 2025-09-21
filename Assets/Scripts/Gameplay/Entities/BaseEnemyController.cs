using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : Entity
{

    private readonly BaseEnemyView baseEnemyView;
    private readonly IGameObjectPool gameObjectPool;

    public Action<BaseEnemyController> Despawned;

    public int EnemyID => enemyID;

    private List<Component> components = new();

    private int enemyID;

    public BaseEnemyController(
        BaseEnemyView baseEnemyView,
        IGameObjectPool gameObjectPool)
    {
        this.baseEnemyView = baseEnemyView;
        this.gameObjectPool = gameObjectPool;
    }

    public override void AddComponentToEntity(Component entity)
    {
        components.Add(entity);
    }

    public override void ComponentCleanup()
    {
        for (int i = 0; i < components.Count; i++)
        {
            if (components[i] != null)
            {
                UnityEngine.Object.Destroy(components[i]);
            }
        }
    }

    public void Despawn()
    {
        Reset();
        Despawned?.Invoke(this);
    }

    public void SetupEnemyID(int enemyID)
    {
        this.enemyID = enemyID;
        baseEnemyView.SetupEnemyID(enemyID);
    }

    public void ResetEnemyID()
    {
        this.enemyID = -1;
        baseEnemyView.ResetEnemyID();
    }

    public void SetupEnemyPosition(Vector3 position)
    {
        baseEnemyView.SetViewPosition(position);
    }

    public override void Reset()
    {
        ResetEnemyID();
        gameObjectPool.ReturnObjectToPool(baseEnemyView.gameObject);
    }
}
