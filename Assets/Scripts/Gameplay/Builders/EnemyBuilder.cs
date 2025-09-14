using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyBuilder : IEntityBuilder
{

    private BaseEnemy baseEnemy;
    private readonly IGameObjectPool gameObjectPool;

    public EnemyBuilder(IGameObjectPool gameObjectPool)
    {
        this.gameObjectPool = gameObjectPool;
    }


    public async UniTask Build()
    {
        await UniTask.WaitUntil(() => gameObjectPool.IsInitialized);

        var newObject = gameObjectPool.GetGameObjectFromPool();
        newObject.name = "BaseEnemy";

        baseEnemy = new BaseEnemy(newObject);

        BuildEntityView(baseEnemy);
    }

    public Entity GetResult()
    {
        return baseEnemy;
    }

    public void Reset()
    {
        //Remove the elements which were built onto the object here.
        baseEnemy.ComponentCleanup();

        // Return empty gameObject to pool
        gameObjectPool.ReturnObjectToPool(baseEnemy.GetGameObject());
        baseEnemy = null;
    }

    public void BuildEntityView(BaseEnemy baseEnemy)
    {
        if (baseEnemy == null)
        {
            Debug.Log("Set entity view called for base enemy without gameObject created! Aborting EntityView Creation");
            return;
        }

        var entityView = baseEnemy.GetGameObject().AddComponent<EntityView>();
        baseEnemy.AddComponentToEntity(entityView);
    }
}
