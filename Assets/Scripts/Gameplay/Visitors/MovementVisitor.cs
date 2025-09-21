using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class MovementVisitor : IInitializable, IDisposable
{
    private bool IsEntityMovementEnabled;
    private List<EntityMovement> registeredEntities;
    private CancellationTokenSource cancellationTokenSource;

    public void Initialize()
    {
        IsEntityMovementEnabled = false;
        registeredEntities = new List<EntityMovement>();
        cancellationTokenSource = new CancellationTokenSource();
    }

    public void Dispose()
    {
        cancellationTokenSource?.Dispose();
        registeredEntities.Clear();
    }

    public void EnableEntityMovement()
    {
        if (IsEntityMovementEnabled)
        {
            DisableEntityMovement();
        }

        cancellationTokenSource = new CancellationTokenSource();
        IsEntityMovementEnabled = true;
        UpdatePositionOnEntities().AttachExternalCancellation(cancellationTokenSource.Token);
    }

    public void DisableEntityMovement()
    {
        cancellationTokenSource.Cancel();
        IsEntityMovementEnabled = false;
    }

    public void VisitEntityMovement(EntityMovement entityMovement)
    {
        if (registeredEntities.Contains(entityMovement))
        {
            return;
        }
        registeredEntities.Add(entityMovement);
    }

    public void RemoveVisitor(EntityMovement visitor)
    {
        if ( registeredEntities.Contains(visitor) == false)
        {
            return;
        }
        registeredEntities.Remove(visitor);
    }

    public void ResetVisitors()
    {
        registeredEntities.Clear();
    }

    private async UniTask UpdatePositionOnEntities()
    {
        var cachedTime = DateTime.UtcNow;
        while (IsEntityMovementEnabled)
        {
            var currentTimeSpan = (DateTime.UtcNow - cachedTime).TotalMilliseconds;
            for (int i = 0; i < registeredEntities.Count; i++)
            {
                if (registeredEntities[i].IsEntityMovementAllowed == false)
                {
                    continue;
                }
                registeredEntities[i].UpdatePosition((float)currentTimeSpan / 1000f);
            }
            cachedTime = DateTime.UtcNow;
            
            await UniTask.WaitForEndOfFrame(cancellationTokenSource.Token);
        }
    }

}
