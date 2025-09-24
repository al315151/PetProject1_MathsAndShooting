using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : Entity
{

    private readonly BaseEnemyView baseEnemyView;
    private readonly IGameObjectPool gameObjectPool;
    private readonly BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor;
    private readonly MovementVisitor movementVisitor;
    private readonly GameOverVisitor gameOverVisitor;

    public Action<BaseEnemyController> Despawned;

    public int EnemyID => enemyID;

    private List<Component> components = new();
    private int enemyID;
    private EntityMovement entityMovement;
    private int HealthPoints;

    public BaseEnemyController(
        BaseEnemyView baseEnemyView,
        EnemyConfig enemyConfig,
        IGameObjectPool gameObjectPool,
        BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor,
        MovementVisitor movementVisitor,
        GameOverVisitor gameOverVisitor)
    {
        this.baseEnemyView = baseEnemyView;
        this.gameObjectPool = gameObjectPool;
        this.bulletAndEnemyCollisionVisitor = bulletAndEnemyCollisionVisitor;
        this.movementVisitor = movementVisitor;
        this.gameOverVisitor = gameOverVisitor;

        SetupEnemyID(GetHashCode());
        baseEnemyView.SetupInitialHitPointThreshold
            (enemyConfig.MinInitialHitPoints, 
            enemyConfig.MaxInitialHitPoints);

        // Setup initial life
        HealthPoints = UnityEngine.Random.Range(
            enemyConfig.MinInitialHitPoints, 
            enemyConfig.MaxInitialHitPoints + 1);
        UpdateHealthOnView();
    }

    public override void AddComponentToEntity(Component entity)
    {
        components.Add(entity);
        if (entity is EntityMovement)
        {
            entityMovement = entity as EntityMovement;
        }
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
        entityMovement = null;
    }

    public void Despawn()
    {
        Reset();
        Despawned?.Invoke(this);
    }

    public void UpdateHealthOnView()
    {
        baseEnemyView.SetupHitPointsGraphics(HealthPoints);
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

    public void DepleteHealth()
    {
        HealthPoints = HealthPoints - 1;
        UpdateHealthOnView();
        if (HealthPoints <= 0)
        {
            Despawn();
        }
    }

    public void SetupEnemyPosition(Vector3 position)
    {
        baseEnemyView.SetViewPosition(position);
    }

    public override void Reset()
    {
        ResetEnemyID();
        baseEnemyView.ResetView();
        bulletAndEnemyCollisionVisitor.RemoveEnemyVisit(baseEnemyView);
        gameOverVisitor.RemoveVisitor(baseEnemyView);
        movementVisitor.RemoveVisitor(entityMovement);
        gameObjectPool.ReturnObjectToPool(baseEnemyView.gameObject);
    }
}
