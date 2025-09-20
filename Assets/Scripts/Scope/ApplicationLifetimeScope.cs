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
    private EndGameBoundsSolver endGameBoundsProvider;

    [SerializeField]
    private GameOverPopup gameOverPopup;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<PlayerConfig>(Lifetime.Scoped).As<IPlayerConfig>();
        builder.Register<PlayerController>(Lifetime.Scoped).As<IInitializable, IDisposable, PlayerController>();
        builder.Register<InputManager>(Lifetime.Scoped).As<IInitializable, IDisposable, ITickable, InputManager>();

        builder.Register<EnemyBuilder>(Lifetime.Scoped).AsSelf();

        builder.RegisterInstance(cameraProvider);
        builder.RegisterInstance(playerView);
        builder.RegisterInstance(gameObjectPool).As<IGameObjectPool>();
        builder.RegisterInstance(enemySpawnPositionProvider).As<EnemySpawnPositionProvider>();
        builder.RegisterInstance(endGameBoundsProvider).As<EndGameBoundsSolver>();
        builder.RegisterInstance(gameOverPopup).AsSelf();

        builder.Register<EntityDirector>(Lifetime.Scoped).As<IInitializable, IDisposable, EntityDirector>();
        builder.Register<MovementVisitor>(Lifetime.Scoped).As<IInitializable, IDisposable, MovementVisitor>();
        builder.Register<GameOverVisitor>(Lifetime.Scoped).As<IInitializable, IDisposable, GameOverVisitor>();

        builder.Register<GameManager>(Lifetime.Scoped).As<IInitializable, IDisposable>();
    }
}
