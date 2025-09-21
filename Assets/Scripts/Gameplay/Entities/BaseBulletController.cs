using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletController : Entity
{
    private readonly BaseBulletView baseBulletView;
    private readonly IGameObjectPool gameObjectPool;

    public int BulletID => bulletID;

    public Action<BaseBulletController> Despawned;

    private EntityMovement entityMovement;

    private List<Component> components = new();

    private int bulletID;

    public BaseBulletController(
        BaseBulletView baseBulletView,
        IGameObjectPool gameObjectPool)
    {
        this.baseBulletView = baseBulletView;
        this.gameObjectPool = gameObjectPool;
    }

    public override void AddComponentToEntity(Component entity)
    {
        components.Add(entity);
    }

    public override void ComponentCleanup()
    {
        for (int i = 0; i < components.Count; i++)
        {
            if (components[i] != null)
            {
                UnityEngine.Object.Destroy(components[i]);
            }
        }
    }

    public override void Reset()
    {
        bulletID = -1;
        baseBulletView.ResetBulletID();
        gameObjectPool.ReturnObjectToPool(baseBulletView.gameObject);
    }

    public void SetBulletID(int bulletID)
    {
        this.bulletID = bulletID;
        baseBulletView.SetBulletID(bulletID);
    }

    public void Despawn()
    {
        Reset();
        Despawned?.Invoke(this);
    }

    public void EnableEntityMovement()
    {
        if (entityMovement == null)
        {
            Debug.Log($"No entity movement setup for: {baseBulletView.gameObject.name}");
            return;
        }
        entityMovement.EnableMovement();
    }

    public void DisableEntityMovement()
    {
        if (entityMovement == null)
        {
            Debug.Log($"No entity movement setup for: {baseBulletView.gameObject.name}");
            return;
        }
        entityMovement.DisableMovement();
    }

    public void SetEntityMovment(EntityMovement entityMovement)
    {
        this.entityMovement = entityMovement;
    }

    public void SetBulletStartPosition(Vector3 worldPosition)
    {
        baseBulletView.SetViewPosition(worldPosition);
    }
}
