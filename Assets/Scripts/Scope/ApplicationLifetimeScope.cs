using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ApplicationLifetimeScope : LifetimeScope
{
    [SerializeField]
    private CameraProvider cameraProvider;  

    [SerializeField]
    private PlayerView playerView;

    [SerializeField]
    private GameObjectPool gameObjectPool;

    [SerializeField]
    private EnemySpawnPositionProvider enemySpawnPositionProvider;

    [SerializeField]
    private EndGameBoundsProcessor endGameBoundsProvider;

    [SerializeField]
    private GameEndPopup gameOverPopup;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<PlayerConfig>(Lifetime.Scoped).As<IPlayerConfig>();
        builder.Register<PlayerController>(Lifetime.Scoped).As<IInitializable, IDisposable, PlayerController>();
        builder.Register<InputManager>(Lifetime.Scoped).As<ITickable, InputManager>();

        builder.Register<EnemyBuilder>(Lifetime.Scoped).AsSelf();
        builder.Register<BaseBulletBuilder>(Lifetime.Scoped).AsSelf();

        builder.Register<EnemyConfig>(Lifetime.Scoped).AsSelf();
        builder.Register<BulletConfig>(Lifetime.Scoped).AsSelf();

        builder.RegisterInstance(cameraProvider);
        builder.RegisterInstance(playerView);
        builder.RegisterInstance(gameObjectPool).As<IGameObjectPool>();
        builder.RegisterInstance(enemySpawnPositionProvider).As<EnemySpawnPositionProvider>();
        builder.RegisterInstance(endGameBoundsProvider).As<EndGameBoundsProcessor>();
        builder.RegisterInstance(gameOverPopup).AsSelf();

        builder.Register<EntityDirector>(Lifetime.Scoped).As<IInitializable, IDisposable, EntityDirector>();
        builder.Register<MovementVisitor>(Lifetime.Scoped).As<IInitializable, IDisposable, MovementVisitor>();
        builder.Register<GameOverVisitor>(Lifetime.Scoped).As<IInitializable, IDisposable, GameOverVisitor>();
        builder.Register<BulletAndEnemyCollisionVisitor>(Lifetime.Scoped).As<IInitializable, IDisposable, BulletAndEnemyCollisionVisitor>();
        builder.Register<EntityDespawner>(Lifetime.Scoped).As<IDisposable, IInitializable, EntityDespawner>();
        builder.Register<GameEndController>(Lifetime.Scoped).As<IInitializable, IDisposable, GameEndController>();
        builder.Register<GameEndSolver>(Lifetime.Scoped).As<IInitializable, IDisposable, GameEndSolver>();

        builder.Register<EnemyControllerProvider>(Lifetime.Scoped).As<IInitializable, IDisposable, EnemyControllerProvider>();
        builder.Register<BulletControllerProvider>(Lifetime.Scoped).As<IInitializable, IDisposable, BulletControllerProvider>();

        builder.Register<ShootingManager>(Lifetime.Scoped).As<IInitializable, IDisposable, ShootingManager>();
        builder.Register<EnemyHealthManager>(Lifetime.Scoped).As<IInitializable, IDisposable, EnemyHealthManager>();
        builder.Register<GameManager>(Lifetime.Scoped).As<IInitializable, IDisposable>();
        
    }
}
