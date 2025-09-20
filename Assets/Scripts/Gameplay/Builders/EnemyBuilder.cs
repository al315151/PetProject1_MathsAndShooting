using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyBuilder : IEntityBuilder
{

    private BaseEnemy baseEnemy;
    private readonly IGameObjectPool gameObjectPool;
    private readonly EnemySpawnPositionProvider enemySpawnPositionProvider;

    public EnemyBuilder(
        IGameObjectPool gameObjectPool,
        EnemySpawnPositionProvider enemySpawnPositionProvider)
    {
        this.gameObjectPool = gameObjectPool;
        this.enemySpawnPositionProvider = enemySpawnPositionProvider;
    }


    public async UniTask Build()
    {
        await UniTask.WaitUntil(() => gameObjectPool.IsInitialized);

        var newObjectEnemyView = gameObjectPool.GetBaseEnemyViewFromPool();

        baseEnemy = new BaseEnemy(newObjectEnemyView.gameObject);

        BuildEntityView(newObjectEnemyView);
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

    public void BuildEntityView(EntityView baseEnemyView)
    {
        if (baseEnemy == null)
        {
            Debug.Log("Set entity view called for base enemy without gameObject created! Aborting EntityView Creation");
            return;
        }

        // Setup object position.
        var graphicsPosition = enemySpawnPositionProvider.GetEnemySpawnPosition();

        baseEnemyView.SetViewPosition(graphicsPosition);
        
        baseEnemy.AddComponentToEntity(baseEnemyView);
    }
}
