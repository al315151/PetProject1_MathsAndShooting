using Cysharp.Threading.Tasks;
using UnityEngine;

public class BaseBulletBuilder : IEntityBuilder
{
    private readonly IGameObjectPool gameObjectPool;
    private readonly BulletConfig bulletConfig;
    private readonly PlayerController playerController;
    private readonly BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor;
    private readonly MovementVisitor movementVisitor;
    private BaseBulletController bulletController;

    public BaseBulletBuilder(
        IGameObjectPool gameObjectPool,
        BulletConfig bulletConfig,
        PlayerController playerController,
        BulletAndEnemyCollisionVisitor bulletAndEnemyCollisionVisitor,
        MovementVisitor movementVisitor)
    {
        this.gameObjectPool = gameObjectPool;
        this.bulletConfig = bulletConfig;
        this.playerController = playerController;
        this.bulletAndEnemyCollisionVisitor = bulletAndEnemyCollisionVisitor;
        this.movementVisitor = movementVisitor;
    }

    public async UniTask Build()
    {
        await UniTask.WaitUntil(() => gameObjectPool.IsInitialized);

        var newBulletView = gameObjectPool.GetBaseBulletViewFromPool();

        bulletController = new BaseBulletController(
            newBulletView,
            gameObjectPool);

        BuildEntityView(newBulletView);
        BuildMovementBehaviour(newBulletView);
    }

    public void BuildEntityView(EntityView entityView)
    {
        if(bulletController == null)
        {
            Debug.Log("Set entity view called for base bullet without gameObject created! Aborting EntityView Creation");
            return;
        }

        //TODO: add collision detection between enemies and bullets.
        var bulletView = entityView as BaseBulletView;

        bulletView.AcceptVisitor(bulletAndEnemyCollisionVisitor);
    }

    public Entity GetResult()
    {
        return bulletController;
    }

    public void Reset()
    {
        bulletController.ComponentCleanup();
        
        bulletController.Reset();
        bulletController = null;
    }

    public void BuildMovementBehaviour(EntityView entityView)
    {
        var newEntityMovement = entityView.gameObject.AddComponent<EntityMovement>();
        bulletController.AddComponentToEntity(newEntityMovement);
        bulletController.SetEntityMovment(newEntityMovement);

        newEntityMovement.Initialize(playerController.GetBulletSpawnPosition());
        newEntityMovement.SetupSpeedAndDirection(bulletConfig.Direction, bulletConfig.Speed);
        newEntityMovement.AcceptVisitor(movementVisitor);
    }
}
