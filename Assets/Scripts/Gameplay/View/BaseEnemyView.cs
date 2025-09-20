using UnityEngine;

public class BaseEnemyView : EntityView
{
    [SerializeField]
    private GameObject m_EnemyGraphicsPrefab;

    [SerializeField]
    private Collider enemyGraphicsBoundsCollider;


    private Bounds enemyBounds;
    private Vector3 cachedPosition;

    private void OnEnable()
    {
        enemyBounds = enemyGraphicsBoundsCollider.bounds;
    }

    private void Update()
    {
        cachedPosition = transform.position;
    }

    public (Vector3, Bounds) GetEnemyWorldSpaceCenterAndBounds()
    {
        return (cachedPosition,enemyBounds);
    }

    public void AcceptVisitor(GameOverVisitor visitor)
    {
         visitor.AcceptBaseEnemyVisit(this);
    }

}
