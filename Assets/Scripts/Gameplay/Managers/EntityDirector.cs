using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class EntityDirector : IInitializable, IDisposable
{
    private readonly EnemyBuilder enemyBuilder;
    private readonly BaseBulletBuilder baseBulletBuilder;
    private readonly PlayerController playerController;

    private List<BaseEnemyController> baseEnemies;

    private List<BaseBulletController> baseBulletControllers;

    public EntityDirector(
        EnemyBuilder enemyBuilder,
        BaseBulletBuilder baseBulletBuilder,
        PlayerController playerController)
    {
        this.enemyBuilder = enemyBuilder;
        this.baseBulletBuilder = baseBulletBuilder;
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

            await UniTask.Delay(1000);
        }
    }

    public async UniTask<BaseBulletController> SpawnBullet()
    {
        var newBullet = await CreateBasicBullet();
        baseBulletControllers.Add(newBullet);
        newBullet.SetBulletStartPosition(playerController.GetBulletSpawnPosition());
        return newBullet;
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

        foreach( var bullet in baseBulletControllers)
        {
            bullet.ComponentCleanup();
            bullet.Reset();
        }
        baseBulletControllers.Clear();
    }

}
