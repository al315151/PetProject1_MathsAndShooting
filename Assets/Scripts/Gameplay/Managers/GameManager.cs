using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

public class GameManager : IInitializable, IDisposable
{
    private const int NumberOfEnemiesToSpawn = 10;

    private readonly EntityDirector entityDirector;
    private readonly MovementVisitor movementVisitor;
    private readonly GameOverVisitor gameOverVisitor;
    private readonly BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor;
    private readonly PlayerController playerController;
    private readonly GameEndController gameEndController;
    private readonly GameEndSolver gameEndSolver;
    private readonly ShootingManager shootingManager;
    private readonly IGameObjectPool gameObjectPool;

    public GameManager(
        EntityDirector entityDirector, 
        MovementVisitor movementVisitor,
        GameOverVisitor gameOverVisitor,
        BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor,
        PlayerController playerController,
        GameEndController gameEndController,
        GameEndSolver gameEndSolver,
        ShootingManager shootingManager,
        IGameObjectPool gameObjectPool)
    {
        this.entityDirector = entityDirector;
        this.movementVisitor = movementVisitor;
        this.gameOverVisitor = gameOverVisitor;
        this.bulletAndEnemyCollisionVisitor = bulletAndEnemyCollisionVisitor;
        this.playerController = playerController;
        this.gameEndController = gameEndController;
        this.gameEndSolver = gameEndSolver;
        this.shootingManager = shootingManager;
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
        playerController.EnablePlayerInput();

        await UniTask.WaitUntil(() => gameObjectPool.IsInitialized);

        entityDirector.SpawnEnemies(NumberOfEnemiesToSpawn).Forget();

        movementVisitor.EnableEntityMovement();
        bulletAndEnemyCollisionVisitor.EnableCollisionDetection();        
        shootingManager.EnableShooting();

        gameOverVisitor.EnableGameOverDetection();
        // Maybe start doing more things here?

    }

    private void Subscribe()
    {
        gameOverVisitor.GameLostDetected += OnGameOverDetected;
        gameEndSolver.GameWonDetected += OnGameOverDetected;
        gameEndController.RetryButtonPressed += OnRetryButtonPressed;
    }

    private void Unsubscribe()
    {
        gameOverVisitor.GameLostDetected -= OnGameOverDetected;
        gameEndSolver.GameWonDetected -= OnGameOverDetected;
        gameEndController.RetryButtonPressed -= OnRetryButtonPressed;
    }

    private void OnGameOverDetected()
    {
        Debug.Log($"[Framecount: {Time.frameCount}] Game Over detected, game stopped!");
        //Disable other systems until user input enables it again.
        playerController.DisablePlayerInput();
        shootingManager.DisableShooting();
        gameOverVisitor.DisableGameOverDetection();
        movementVisitor.DisableEntityMovement();
        bulletAndEnemyCollisionVisitor.DisableCollisionDetection();
    }

    private void OnRetryButtonPressed()
    {
        RestartGame().Forget();
    }

    private async UniTask RestartGame()
    {
        //First do the cleanup of all, then start the game again.
        entityDirector.Cleanup();
        gameOverVisitor.ResetVisitors();
        movementVisitor.ResetVisitors();
        bulletAndEnemyCollisionVisitor.ResetVisitors();
        gameEndSolver.ResetSolverState();

        //Wait for 1 second to make sure all objects have been destroyed / reset.
        await UniTask.Delay( 1000 );

        StartGameAsync().Forget();
    }
}
