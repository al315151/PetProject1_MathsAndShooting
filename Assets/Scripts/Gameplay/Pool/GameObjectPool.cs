using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour, IGameObjectPool
{
    private const int GameObjectPoolCount = 200;

    public bool IsInitialized => poolInitialized;

    private List<GameObject> gameObjectPool;

    private bool poolInitialized;

    public GameObject GetGameObjectFromPool()
    {
        for (int i = 0; i < gameObjectPool.Count; i++)
        {
            if (gameObjectPool[i] == null)
            {
                continue;
            }
            var go = gameObjectPool[i];
            gameObjectPool.Remove(go);
            return go;
        }

        return null;
    }

    public void Awake()
    {
        poolInitialized = false;
        InitializeGameObjectPool();

        poolInitialized = true;        
    }

    private void InitializeGameObjectPool()
    {
        gameObjectPool = new List<GameObject>();
        for (int i = 0; i < GameObjectPoolCount; i++)
        {
            var newGameObject = new GameObject();
            newGameObject.transform.parent = transform;
            newGameObject.name = "newObject_" + i;

            gameObjectPool.Add(newGameObject);
        }
    }

    public void ReturnObjectToPool(GameObject objectToReturn)
    {
        gameObjectPool.Add(objectToReturn);
    }
}
