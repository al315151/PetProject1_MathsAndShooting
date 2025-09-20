using UnityEngine;

public interface IGameObjectPool
{
    public bool IsInitialized { get; }
    public GameObject GetGameObjectFromPool();
    public BaseEnemyView GetBaseEnemyViewFromPool();

    public void ReturnObjectToPool(GameObject objectToReturn);
}
