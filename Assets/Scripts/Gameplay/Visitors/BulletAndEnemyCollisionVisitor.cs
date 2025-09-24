using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using VContainer.Unity;

public class BulletAndEnemyCollisionVisitor : IInitializable, IDisposable
{
    private List<BaseEnemyView> enemyViews;
    private List<BaseBulletView> bulletViews;

    public Action<BaseEnemyView> CollisionDetectedForEnemy;
    public Action<BaseBulletView> CollisionDetectedForBullet;

    private bool isCollisionDetectionEnabled;
    private CancellationTokenSource cancellationTokenSource;

    public void Initialize()
    {
        isCollisionDetectionEnabled = false;
        enemyViews = new List<BaseEnemyView>();
        bulletViews = new List<BaseBulletView>();
    }

    public void Dispose()
    {
        cancellationTokenSource?.Cancel();
        cancellationTokenSource?.Dispose();

        ResetVisitors();
    }

    public void AcceptEnemyVisit(BaseEnemyView baseEnemyView)
    {
        enemyViews.Add(baseEnemyView);
    }

    public void RemoveEnemyVisit(BaseEnemyView baseEnemyView)
    {
        enemyViews.Remove(baseEnemyView);
    }

    public void AcceptBulletVisit(BaseBulletView baseBulletView)
    {
        bulletViews.Add(baseBulletView);
    }

    public void RemoveBulletVisit(BaseBulletView baseBulletView)
    {
        bulletViews.Remove(baseBulletView);
    }

    public void EnableCollisionDetection()
    {
        if (isCollisionDetectionEnabled)
        {
            DisableCollisionDetection();
        }
        cancellationTokenSource = new CancellationTokenSource();
        isCollisionDetectionEnabled = true;
        CheckForCollisions().AttachExternalCancellation(cancellationTokenSource.Token).Forget();

    }

    public void DisableCollisionDetection()
    {
        cancellationTokenSource.Cancel();
        isCollisionDetectionEnabled = false;
    }

    public void ResetVisitors()
    {
        enemyViews.Clear();
        bulletViews.Clear();
    }

    private async UniTask CheckForCollisions()
    {
        while (isCollisionDetectionEnabled)
        {

            var collisionDetected = false;

            for (int i = 0; i < enemyViews.Count; i++)
            {
                var (enemyWorldSpaceCenter, enemyBounds) = enemyViews[i].GetEnemyWorldSpaceCenterAndBounds();
                if (enemyBounds == null)
                {
                    Debug.LogError($"[Framecount: {Time.frameCount}] Enemy not found!at index: {i}");
                    continue;
                }

                for (var j = 0; j < bulletViews.Count; j++)
                {
                    var (bulletWorldSpaceCenter, bulletBounds) = bulletViews[j].GetBulletWorldSpaceCenterAndBounds();
                    if (bulletBounds == null)
                    {
                        Debug.LogError($"[Framecount: {Time.frameCount}] Bullet not found! at index: {j}");
                        continue;
                    }

                    if (CollisionBetweenTwoObjectsSolver.AreTwoObjectsColliding(
                        enemyWorldSpaceCenter,
                        enemyBounds,
                        bulletWorldSpaceCenter,
                        bulletBounds))
                    {
                        Debug.Log($"Collision detected between: {enemyViews[i].gameObject.name} and {bulletViews[j].gameObject.name}");
                        CollisionDetectedForBullet?.Invoke(bulletViews[j]);
                        CollisionDetectedForEnemy?.Invoke(enemyViews[i]);
                        collisionDetected = true;
                        break;
                    }                    
                }

                if (collisionDetected)
                {
                    break;
                }
            }

            await UniTask.WaitForEndOfFrame(cancellationTokenSource.Token);
        }
    }
}
