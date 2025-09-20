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

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<PlayerConfig>(Lifetime.Scoped).As<IPlayerConfig>();
        builder.Register<PlayerController>(Lifetime.Scoped).As<IInitializable, IDisposable>();
        builder.Register<InputManager>(Lifetime.Scoped).As<IInitializable, IDisposable, ITickable, InputManager>();

        builder.Register<EnemyBuilder>(Lifetime.Scoped).AsSelf();

        builder.RegisterInstance(cameraProvider);
        builder.RegisterInstance(playerView);
        builder.RegisterInstance(gameObjectPool).As<IGameObjectPool>();
        builder.RegisterInstance(enemySpawnPositionProvider).As<EnemySpawnPositionProvider>();

        builder.Register<EntityDirector>(Lifetime.Scoped).As<IInitializable, IDisposable>();
    }
}
