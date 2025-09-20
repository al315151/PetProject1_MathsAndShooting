using System;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class GameManager : IInitializable, IDisposable
{
    private readonly EntityDirector entityDirector;
    private readonly MovementVisitor movementVisitor;
    private readonly IGameObjectPool gameObjectPool;

    public GameManager(
        EntityDirector entityDirector, 
        MovementVisitor movementVisitor, 
        IGameObjectPool gameObjectPool)
    {
        this.entityDirector = entityDirector;
        this.movementVisitor = movementVisitor;
        this.gameObjectPool = gameObjectPool;
    }

    public void Dispose()
    {
    }

    public void Initialize()
    {
        InitializeAsync().Forget();
    }

    private async UniTask InitializeAsync()
    {
        await UniTask.WaitUntil(() => gameObjectPool.IsInitialized);

        entityDirector.SpawnEnemies().Forget();

        movementVisitor.EnableEntityMovement();

        // Maybe start doing more things here?

    }
}
