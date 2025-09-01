using System;
using VContainer;
using VContainer.Unity;

public class ApplicationLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<PlayerConfig>(Lifetime.Scoped).As<IPlayerConfig>();
        builder.Register<PlayerController>(Lifetime.Scoped).As<IInitializable, IDisposable>();

    }
}
