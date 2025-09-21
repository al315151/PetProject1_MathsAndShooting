using UnityEngine;

public class BaseEnemyView : EntityView
{
    [SerializeField]
    private GameObject m_EnemyGraphicsPrefab;

    [SerializeField]
    private Collider enemyGraphicsBoundsCollider;

    public int EnemyID => enemyID;

    private Bounds enemyBounds;
    private Vector3 cachedPosition;

    private int enemyID;

    private void OnEnable()
    {
        enemyBounds = enemyGraphicsBoundsCollider.bounds;
    }

    private void Update()
    {
        cachedPosition = transform.position;
    }

    public void SetupEnemyID(int enemyID)
    {
        this.enemyID = enemyID;
    }

    public void ResetEnemyID()
    {
        enemyID = -1;
    }

    public (Vector3, Bounds) GetEnemyWorldSpaceCenterAndBounds()
    {
        return (cachedPosition,enemyBounds);
    }

    public void AcceptVisitor(GameOverVisitor visitor)
    {
         visitor.AcceptBaseEnemyVisit(this);
    }

    public void AcceptVisitor(BulletAndEnemyCollisionVisitor visitor)
    {
        visitor.AcceptEnemyVisit(this);
    }
}
