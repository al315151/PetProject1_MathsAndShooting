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
    private bool physicsScaleUpdated;

    private void OnEnable()
    {
        enemyBounds = enemyGraphicsBoundsCollider.bounds;
        targetLocalScale = 1.0f;
    }

    private void Update()
    {
        cachedPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (physicsScaleUpdated)
        {
            enemyBounds = enemyGraphicsBoundsCollider.bounds;
            physicsScaleUpdated = false;
            Debug.Log($"[Framecount: {Time.frameCount}] Changed bounds of object: {gameObject.name} to: {enemyBounds.size}");
        }
    }

    private void LateUpdate()
    {
        if (graphicsScaleUpdated)
        {
            m_EnemyGraphicsPrefab.transform.localScale = Vector3.one * targetLocalScale;
            graphicsScaleUpdated = false;
            Debug.Log($"[Framecount: {Time.frameCount}] Changed scale of object: {gameObject.name} to: {targetLocalScale}");
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
        Debug.Log($"[Framecount: {Time.frameCount}] object: {gameObject.name} hit points changed to: {hitPoints}");
        var expectedSize = Mathf.Clamp(hitPoints, minSizeHitPoints, maxSizeHitPoints);
        var normalizedExpectedSize = expectedSize / maxSizeHitPoints;

        var sampledSize = sizeAnimationCurve.Evaluate(normalizedExpectedSize);
        var graphicsSize = Mathf.Lerp(minSizeGraphicsScale, maxSizeGraphicsScale, sampledSize);

        targetLocalScale = graphicsSize;
        graphicsScaleUpdated = true;
        physicsScaleUpdated = true;
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
