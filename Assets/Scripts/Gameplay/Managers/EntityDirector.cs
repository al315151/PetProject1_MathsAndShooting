using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class EntityDirector : IInitializable, IDisposable
{
    private readonly EnemyBuilder enemyBuilder;
    private readonly BaseBulletBuilder baseBulletBuilder;
    private readonly BulletControllerProvider bulletControllerProvider;
    private readonly EnemyControllerProvider enemyControllerProvider;
    private readonly PlayerController playerController;

    private List<BaseEnemyController> baseEnemies;

    private List<BaseBulletController> baseBulletControllers;

    public Action FinishedSpawnOfEnemies;

    public EntityDirector(
        EnemyBuilder enemyBuilder,
        BaseBulletBuilder baseBulletBuilder,
        BulletControllerProvider bulletControllerProvider,
        EnemyControllerProvider enemyControllerProvider,
        PlayerController playerController)
    {
        this.enemyBuilder = enemyBuilder;
        this.baseBulletBuilder = baseBulletBuilder;
        this.bulletControllerProvider = bulletControllerProvider;
        this.enemyControllerProvider = enemyControllerProvider;
        this.playerController = playerController;
    }


    public void Dispose()
    {
        enemyBuilder.Reset();
        baseBulletBuilder.Reset();
    }

    public void Initialize()
    {
        baseBulletControllers = new List<BaseBulletController>();
    }

    public async UniTask SpawnEnemies()
    {
        baseEnemies = new List<BaseEnemyController>();

        for (int i = 0; i < 10; i++)
        {
            var baseEnemy = await CreateBasicEnemy();
            baseEnemies.Add(baseEnemy);

            enemyControllerProvider.AddEnemyController(baseEnemy);
            baseEnemy.Despawned += OnEnemyDespawned;

            await UniTask.Delay(1000);
        }

        FinishedSpawnOfEnemies?.Invoke();
    }

    public async UniTask<BaseBulletController> SpawnBullet()
    {
        var newBullet = await CreateBasicBullet();
        baseBulletControllers.Add(newBullet);

        bulletControllerProvider.AddBulletController(newBullet);
        newBullet.Despawned += OnBulletDespawned;        
        return newBullet;
    }

    private void OnBulletDespawned(BaseBulletController controller)
    {
        controller.ComponentCleanup();
        controller.Despawned -= OnBulletDespawned;
        baseBulletControllers.Remove(controller);
        bulletControllerProvider.RemoveBulletController(controller);
    }

    private void OnEnemyDespawned(BaseEnemyController controller)
    {
        controller.ComponentCleanup();
        controller.Despawned -= OnEnemyDespawned;
        baseEnemies.Remove(controller);
        enemyControllerProvider.RemoveEnemyController(controller);
    }

    public async UniTask<BaseEnemyController> CreateBasicEnemy()
    {        
        await enemyBuilder.Build();
        var enemy = enemyBuilder.GetResult();
        return (BaseEnemyController)enemy;
    }

    public async UniTask<BaseBulletController> CreateBasicBullet()
    {
        await baseBulletBuilder.Build();
        var bullet = baseBulletBuilder.GetResult();
        return (BaseBulletController)bullet;
    }

    public void Cleanup()
    {
        foreach (var enemy in baseEnemies)
        {
            enemy.ComponentCleanup();
            enemy.Reset();
        }
        baseEnemies.Clear();
        enemyControllerProvider.Reset();

        foreach ( var bullet in baseBulletControllers)
        {
            bullet.ComponentCleanup();
            bullet.Reset();
        }
        baseBulletControllers.Clear();
        bulletControllerProvider.Reset();

    }

}
