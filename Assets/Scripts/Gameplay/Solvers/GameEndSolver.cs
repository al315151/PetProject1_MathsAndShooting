using System;
using VContainer.Unity;

public class GameEndSolver : IInitializable, IDisposable
{
    private readonly EntityDirector entityDirector;
    private readonly GameOverVisitor gameOverVisitor;

    public Action GameWonDetected;

    public bool HasEnemiesSpawnFinished;

    public GameEndSolver(EntityDirector entityDirector,
                         GameOverVisitor gameOverVisitor)
    {
        this.entityDirector = entityDirector;
        this.gameOverVisitor = gameOverVisitor;
    }
    

    public void Initialize()
    {
        entityDirector.FinishedSpawnOfEnemies += OnEnemiesSpawnFinished;
        gameOverVisitor.GameLostDetected += OnGameLostDetected;
        gameOverVisitor.RanOutOfEnemiesDetected += OnRanOutOfEnemiesDetected;
    }

    public void Dispose()
    {
        entityDirector.FinishedSpawnOfEnemies -= OnEnemiesSpawnFinished;
        gameOverVisitor.GameLostDetected -= OnGameLostDetected;
        gameOverVisitor.RanOutOfEnemiesDetected -= OnRanOutOfEnemiesDetected;
    }

    public void ResetSolverState()
    {
        HasEnemiesSpawnFinished = false;
    }

    private void OnRanOutOfEnemiesDetected()
    {
        if (HasEnemiesSpawnFinished)
        {
            GameWonDetected?.Invoke();
        }
    }

    private void OnGameLostDetected()
    {
        ResetSolverState();
    }

    private void OnEnemiesSpawnFinished()
    {
        HasEnemiesSpawnFinished = true;
    }
}
