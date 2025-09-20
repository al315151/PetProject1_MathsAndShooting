using UnityEngine;

public abstract class Entity
{
    // To be used as a "template" of objects returned by the builders.

    public abstract void AddComponentToEntity(Component entity);

    public abstract void ComponentCleanup();
}
