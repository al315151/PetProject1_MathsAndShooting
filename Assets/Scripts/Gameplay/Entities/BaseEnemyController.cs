using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : Entity
{

    private readonly GameObject gameObjectReference;

    private List<Component> components = new();

    public BaseEnemyController(GameObject gameObjectReference)
    {
        this.gameObjectReference = gameObjectReference;
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

    // Setup the enemy components from the builder, and arrange logic as intended.
    public GameObject GetGameObject()
    {
        return gameObjectReference;
    }
}
