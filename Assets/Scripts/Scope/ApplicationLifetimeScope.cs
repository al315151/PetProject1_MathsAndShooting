using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ApplicationLifetimeScope : LifetimeScope
{
    [SerializeField]
    private CameraProvider cameraProvider;  

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<PlayerConfig>(Lifetime.Scoped).As<IPlayerConfig>();
        builder.Register<PlayerController>(Lifetime.Scoped).As<IInitializable, IDisposable>();
        builder.Register<InputManager>(Lifetime.Scoped).As<IInitializable, IDisposable, ITickable, InputManager>();
        builder.RegisterInstance(cameraProvider);

    }
}
