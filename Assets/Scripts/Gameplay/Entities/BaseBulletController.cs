using System.Collections.Generic;
using UnityEngine;

public class BaseBulletController : Entity
{
    private readonly BaseBulletView baseBulletView;
    private readonly IGameObjectPool gameObjectPool;

    private EntityMovement entityMovement;

    private List<Component> components = new();

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
                Object.Destroy(components[i]);
            }
        }
    }

    public override void Reset()
    {
        gameObjectPool.ReturnObjectToPool(baseBulletView.gameObject);
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
