using UnityEngine;

public class BaseEnemyView : EntityView
{
    [SerializeField]
    private GameObject m_EnemyGraphicsPrefab;

    [SerializeField]
    private Collider enemyGraphicsBoundsCollider;

    [SerializeField]
    private AnimationCurve sizeAnimationCurve;

    [SerializeField]
    private int maxSizeGraphicsScale;

    [SerializeField]
    private int minSizeGraphicsScale;

    public int EnemyID => enemyID;

    private Bounds enemyBounds;
    private Vector3 cachedPosition;
    private float targetLocalScale;

    private int enemyID;
    private int minSizeHitPoints;
    private int maxSizeHitPoints;

    private bool graphicsScaleUpdated;

    private void OnEnable()
    {
        enemyBounds = enemyGraphicsBoundsCollider.bounds;
        targetLocalScale = 1.0f;
    }

    private void Update()
    {
        cachedPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (graphicsScaleUpdated)
        {
            m_EnemyGraphicsPrefab.transform.localScale = new Vector3(1.0f,targetLocalScale, 1.0f);
            graphicsScaleUpdated = false;
        }        
    }

    public void SetupEnemyID(int enemyID)
    {
        this.enemyID = enemyID;
    }

    public void ResetEnemyID()
    {
        enemyID = -1;
    }

    public void SetupInitialHitPointThreshold(int minHitPoints, int maxHitPoints)
    {
        minSizeHitPoints = minHitPoints;
        maxSizeHitPoints = maxHitPoints;
    }

    public void SetupHitPointsGraphics(int hitPoints)
    {
        var expectedSize = Mathf.Clamp(hitPoints, minSizeHitPoints, maxSizeHitPoints);
        var normalizedExpectedSize = expectedSize / maxSizeHitPoints;

        var sampledSize = sizeAnimationCurve.Evaluate(normalizedExpectedSize);
        var graphicsSize = Mathf.Lerp(minSizeGraphicsScale, maxSizeGraphicsScale, sampledSize);

        targetLocalScale = graphicsSize;
        graphicsScaleUpdated = true;
    }

    public void ResetView()
    {
        ResetEnemyID();
        m_EnemyGraphicsPrefab.transform.localScale = Vector3.one;
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
