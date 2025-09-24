using System;
using UnityEngine;
using VContainer.Unity;

public class EntityDespawner : IInitializable, IDisposable
{
    private readonly BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor;
    private readonly BulletControllerProvider bulletControllerProvider;
    private readonly EnemyControllerProvider enemyControllerProvider;

    public EntityDespawner(
        BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor,
        BulletControllerProvider bulletControllerProvider,
        EnemyControllerProvider enemyControllerProvider)
    {
        this.bulletAndEnemyCollisionVisitor = bulletAndEnemyCollisionVisitor;
        this.bulletControllerProvider = bulletControllerProvider;
        this.enemyControllerProvider = enemyControllerProvider;
    }

    public void Initialize()
    {
        bulletAndEnemyCollisionVisitor.CollisionDetected += OnCollisionDetected;
    }

    public void Dispose()
    {
        bulletAndEnemyCollisionVisitor.CollisionDetected -= OnCollisionDetected;
    }

    private void OnCollisionDetected(BaseEnemyView enemyView, BaseBulletView bulletView)
    {
        var bulletController = bulletControllerProvider.GetBulletController(bulletView);
        if (bulletController == null)
        {
            Debug.LogError($"[Framecount: {Time.frameCount}] Bullet controller not found! bullet view name: {bulletView.gameObject.name}");
        }
        bulletController?.Despawn();

        var enemyController = enemyControllerProvider.GetEnemyController(enemyView);
        if (enemyController == null)
        {
            Debug.LogError($"[Framecount: {Time.frameCount}] enemy controller not found! eenemy view name: {enemyView.gameObject.name}");
        }
        enemyController?.Despawn();

    }
}
