using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour, IGameObjectPool
{
    private const int GameObjectPoolCount = 50;

    [SerializeField]
    private GameObject basicEnemyReference;

    public bool IsInitialized => poolInitialized;

    private List<GameObject> gameObjectPool;

    private List<BaseEnemyView> baseEnemyViewPool;

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
            go.SetActive(true);
            gameObjectPool.Remove(go);
            return go;
        }

        return null;
    }

    public BaseEnemyView GetBaseEnemyViewFromPool()
    {
        for(int i = 0; i < baseEnemyViewPool.Count; i++)
        {
            if (baseEnemyViewPool[i] == null)
            {
                continue;
            }
            var go = baseEnemyViewPool[i];
            go.gameObject.SetActive(true);
            baseEnemyViewPool.Remove(go);
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
            newGameObject.SetActive(false);

            gameObjectPool.Add(newGameObject);
        }

        baseEnemyViewPool = new List<BaseEnemyView>();
        for (int i = 0;i < GameObjectPoolCount; i++)
        {
            var newBaseEnemyView = Instantiate(basicEnemyReference, transform);
            newBaseEnemyView.name = newBaseEnemyView.name + "_" + i;
            newBaseEnemyView.SetActive(false);

            baseEnemyViewPool.Add(newBaseEnemyView.GetComponent<BaseEnemyView>());
        }

    }

    public void ReturnObjectToPool(GameObject objectToReturn)
    {
        if (objectToReturn == null)
        {
            return;
        }

        var isObjectEnemyView = objectToReturn.TryGetComponent<BaseEnemyView>(out var baseEnemyView);

        if (isObjectEnemyView)
        {
            baseEnemyViewPool.Add(baseEnemyView);
        }
        else
        {
            gameObjectPool.Add(objectToReturn);
        }
            
        objectToReturn.SetActive(false);
    }
}
