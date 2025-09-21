using System;
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
        bulletController?.Despawn();

        var enemyController = enemyControllerProvider.GetEnemyController(enemyView);
        enemyController?.Despawn();

    }
}
