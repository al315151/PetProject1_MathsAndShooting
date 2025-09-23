using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class GameOverVisitor : IInitializable, IDisposable
{
    private readonly EndGameBoundsProcessor endGameBoundsSolver;
    
    public Action GameLostDetected;
    public Action RanOutOfEnemiesDetected;

    private List<BaseEnemyView> enemies;

    private bool IsEndGameOverDetectionEnabled;
    private CancellationTokenSource cancellationTokenSource;

    public GameOverVisitor(
        EndGameBoundsProcessor endGameBoundsSolver)
    {
        this.endGameBoundsSolver = endGameBoundsSolver;
    }
    public void Initialize()
    {
        enemies = new List<BaseEnemyView>();
        IsEndGameOverDetectionEnabled = false;
        cancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();

        ResetVisitors();
    }

    public void AcceptBaseEnemyVisit(BaseEnemyView baseEnemyView)
    {
        enemies.Add(baseEnemyView);
    }

    public void RemoveVisitor(BaseEnemyView baseEnemyView)
    {
        enemies.Remove(baseEnemyView);
    }

    public void ResetVisitors()
    {
        enemies.Clear();
    }

    public void EnableGameOverDetection()
    {
        if (IsEndGameOverDetectionEnabled)
        {
            DisableGameOverDetection();
        }
        cancellationTokenSource = new CancellationTokenSource();
        IsEndGameOverDetectionEnabled = true;
        CheckForGameOver().AttachExternalCancellation(cancellationTokenSource.Token).Forget();

    }

    public void DisableGameOverDetection()
    {
        cancellationTokenSource?.Cancel();
        IsEndGameOverDetectionEnabled = false;
    }

    private async UniTask CheckForGameOver()
    {
        while (IsEndGameOverDetectionEnabled)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                var (worldSpaceCenter,bounds) = enemies[i].GetEnemyWorldSpaceCenterAndBounds();
                if (bounds != null && endGameBoundsSolver.IsTargetBoundsInside(worldSpaceCenter, bounds))
                {
                    GameLostDetected?.Invoke();
                    break;
                }
            }

            if (enemies.Count == 0)
            {
                RanOutOfEnemiesDetected?.Invoke();
            }

            await UniTask.WaitForEndOfFrame(cancellationTokenSource.Token);

            
        }
    }
}
