using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BasicEnemySpawner : MonoBehaviour, IDisposable
{
    private const int maxEnemyCount = 10;
    private const int timeBetweenSpawnsInMilliseconds = 1000;

    [SerializeField]
    private GameObject[] enemySpawnPositions;

    private readonly EntityDirector entityDirector;

    private List<BaseEnemy> spawnedEnemies;

    private CancellationTokenSource stopCancellationTokenSource;

    public BasicEnemySpawner(EntityDirector entityDirector)
    {
        this.entityDirector = entityDirector;
        spawnedEnemies = new List<BaseEnemy>();
        stopCancellationTokenSource = new CancellationTokenSource();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemies().Forget();
    }

    private async UniTask SpawnEnemies()
    {
        while (spawnedEnemies.Count >= maxEnemyCount)
        {
            await UniTask.Delay(timeBetweenSpawnsInMilliseconds).AttachExternalCancellation(stopCancellationTokenSource.Token);
        }
        if (stopCancellationTokenSource.IsCancellationRequested == false)
        {
            SpawnBasicEnemy();
            SpawnEnemies().Forget();
        }
    }


    private void SpawnBasicEnemy()
    {
        var randomPosition = enemySpawnPositions[UnityEngine.Random.Range(0, enemySpawnPositions.Length)].transform.position;

        var enemy = entityDirector.CreateBasicEnemy();

        enemy.SetupEnemyPosition(randomPosition);

        spawnedEnemies.Add(enemy);
    }

    public void Dispose()
    {
        stopCancellationTokenSource?.Cancel();
    }
}
