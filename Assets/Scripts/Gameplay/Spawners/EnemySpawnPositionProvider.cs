using System;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawnPositionProvider : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemySpawnPositions;

    public Vector3 GetEnemySpawnPosition()
    {
        var index = UnityEngine.Random.Range(0, enemySpawnPositions.Length);
        return enemySpawnPositions[index].transform.position;
    }
}
