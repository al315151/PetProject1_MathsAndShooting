using System;
using UnityEngine;
using VContainer.Unity;

public class EntityDespawner : IInitializable, IDisposable
{
    private readonly BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor;
    private readonly BulletControllerProvider bulletControllerProvider;

    public EntityDespawner(
        BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor,
        BulletControllerProvider bulletControllerProvider)
    {
        this.bulletAndEnemyCollisionVisitor = bulletAndEnemyCollisionVisitor;
        this.bulletControllerProvider = bulletControllerProvider;
    }

    public void Initialize()
    {
        bulletAndEnemyCollisionVisitor.CollisionDetectedForBullet += OnCollisionDetected;
    }

    public void Dispose()
    {
        bulletAndEnemyCollisionVisitor.CollisionDetectedForBullet -= OnCollisionDetected;
    }

    private void OnCollisionDetected(BaseBulletView bulletView)
    {
        var bulletController = bulletControllerProvider.GetBulletController(bulletView);
        if (bulletController == null)
        {
            Debug.LogError($"[Framecount: {Time.frameCount}] Bullet controller not found! bullet view name: {bulletView.gameObject.name}");
        }
        bulletController?.Despawn();
    }
}
