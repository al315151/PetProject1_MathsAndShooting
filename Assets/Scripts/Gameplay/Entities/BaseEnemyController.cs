using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : Entity
{

    private readonly GameObject gameObjectReference;
    private readonly IGameObjectPool gameObjectPool;
    private List<Component> components = new();

    public BaseEnemyController(
        GameObject gameObjectReference,
        IGameObjectPool gameObjectPool)
    {
        this.gameObjectReference = gameObjectReference;
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

    public void SetupEnemyPosition(Vector3 position)
    {
        gameObjectReference.transform.position = position;
    }

    public void Reset()
    {
        gameObjectPool.ReturnObjectToPool(gameObjectReference);
    }
}
