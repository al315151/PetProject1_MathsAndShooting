using UnityEngine;

public class BaseBulletView : EntityView
{
    [SerializeField]
    private Collider bulletBoundsCollider;

    public int BulletID => bulletID;

    private Bounds bulletBounds;
    private Vector3 cachedPosition;

    private int bulletID;

    private void OnEnable()
    {
        bulletBounds = bulletBoundsCollider.bounds;
    }

    private void Update()
    {
        cachedPosition = transform.position;
    }

    public void SetBulletID(int bulletID)
    {
        this.bulletID = bulletID;
    }

    public void ResetBulletID()
    {
        this.bulletID = -1;
    }

    public (Vector3, Bounds) GetBulletWorldSpaceCenterAndBounds()
    {
        return (cachedPosition, bulletBounds);
    }

    public void AcceptVisitor(BulletAndEnemyCollisionVisitor visitor)
    {
        visitor.AcceptBulletVisit(this);
    }

}
