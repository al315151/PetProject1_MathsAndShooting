using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyBuilder : IEntityBuilder
{

    private BaseEnemyController baseEnemy;
    private readonly IGameObjectPool gameObjectPool;
    private readonly EnemySpawnPositionProvider enemySpawnPositionProvider;
    private readonly MovementVisitor movementVisitor;
    private readonly GameOverVisitor gameOverVisitor;

    public EnemyBuilder(
        IGameObjectPool gameObjectPool,
        EnemySpawnPositionProvider enemySpawnPositionProvider,
        MovementVisitor movementVisitor,
        GameOverVisitor gameOverVisitor)
    {
        this.gameObjectPool = gameObjectPool;
        this.enemySpawnPositionProvider = enemySpawnPositionProvider;
        this.movementVisitor = movementVisitor;
        this.gameOverVisitor = gameOverVisitor;
    }


    public async UniTask Build()
    {
        await UniTask.WaitUntil(() => gameObjectPool.IsInitialized);

        var newObjectEnemyView = gameObjectPool.GetBaseEnemyViewFromPool();

        baseEnemy = new BaseEnemyController(
            newObjectEnemyView.gameObject,
            gameObjectPool);

        BuildEntityView(newObjectEnemyView);
        BuildMovementBehaviour(newObjectEnemyView);
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
        baseEnemy.Reset();
        baseEnemy = null;
    }

    public void BuildEntityView(EntityView entityView)
    {
        if (baseEnemy == null)
        {
            Debug.Log("Set entity view called for base enemy without gameObject created! Aborting EntityView Creation");
            return;
        }

        // Setup object position.
        var graphicsPosition = enemySpawnPositionProvider.GetEnemySpawnPosition();

        var baseEnemyView = entityView as BaseEnemyView;

        baseEnemyView.SetViewPosition(graphicsPosition);
        baseEnemyView.AcceptVisitor(gameOverVisitor);        
    }

    public void BuildMovementBehaviour(EntityView entityView)
    {
        var newEntityMovement = entityView.gameObject.AddComponent<EntityMovement>();
        baseEnemy.AddComponentToEntity(newEntityMovement);

        newEntityMovement.Initialize(entityView.gameObject.transform.position);
        newEntityMovement.AcceptVisitor(movementVisitor);
    }

}
