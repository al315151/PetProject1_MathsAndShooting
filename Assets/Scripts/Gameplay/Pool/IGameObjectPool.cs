using UnityEngine;

public interface IGameObjectPool
{
    public bool IsInitialized { get; }
    public GameObject GetGameObjectFromPool();

    public void ReturnObjectToPool(GameObject objectToReturn);
}
