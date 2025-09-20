using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

public class GameManager : IInitializable, IDisposable
{
    private readonly EntityDirector entityDirector;
    private readonly MovementVisitor movementVisitor;
    private readonly GameOverVisitor gameOverVisitor;
    private readonly IGameObjectPool gameObjectPool;

    public GameManager(
        EntityDirector entityDirector, 
        MovementVisitor movementVisitor,
        GameOverVisitor gameOverVisitor,
        IGameObjectPool gameObjectPool)
    {
        this.entityDirector = entityDirector;
        this.movementVisitor = movementVisitor;
        this.gameOverVisitor = gameOverVisitor;
        this.gameObjectPool = gameObjectPool;
    }

    public void Dispose()
    {
        Unsubscribe();
    }

    public void Initialize()
    {
        StartGameAsync().Forget();
        Subscribe();
    }

    private async UniTask StartGameAsync()
    {
        await UniTask.WaitUntil(() => gameObjectPool.IsInitialized);

        entityDirector.SpawnEnemies().Forget();

        movementVisitor.EnableEntityMovement();

        gameOverVisitor.EnableGameOverDetection();
        // Maybe start doing more things here?

    }

    private void Subscribe()
    {
        gameOverVisitor.GameOverDetected += OnGameOverDetected;
    }

    private void Unsubscribe()
    {
        gameOverVisitor.GameOverDetected -= OnGameOverDetected;
    }

    private void OnGameOverDetected()
    {
        Debug.Log($"[Framecount: {Time.frameCount}] Game Over detected, game stopped!");
        //Disable other systems until user input enables it again.
        gameOverVisitor.DisableGameOverDetection();
        movementVisitor.DisableEntityMovement();

    }
}
