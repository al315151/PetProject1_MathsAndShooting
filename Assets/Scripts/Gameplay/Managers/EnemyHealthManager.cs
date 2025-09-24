using System;
using UnityEngine;
using VContainer.Unity;

public class EnemyHealthManager : IInitializable, IDisposable
{
    private readonly BulletAndEnemyCollisionVisitor collisionVisitor;
    private readonly EnemyControllerProvider enemyControllerProvider;

    public EnemyHealthManager(
        BulletAndEnemyCollisionVisitor collisionVisitor,
        EnemyControllerProvider enemyControllerProvider)
    {
        this.collisionVisitor = collisionVisitor;
        this.enemyControllerProvider = enemyControllerProvider;
    }

    public void Dispose()
    {
        collisionVisitor.CollisionDetectedForEnemy -= OnCollisionDetected;
    }

    public void Initialize()
    {
        collisionVisitor.CollisionDetectedForEnemy += OnCollisionDetected;
    }

    private void OnCollisionDetected(BaseEnemyView enemyView)
    {
        var enemyController = enemyControllerProvider.GetEnemyController(enemyView);
        if (enemyController == null)
        {
            Debug.LogError($"[Framecount: {Time.frameCount}] enemy controller not found! eenemy view name: {enemyView.gameObject.name}");
        }
        enemyController?.DepleteHealth();
    }
}
