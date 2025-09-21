using UnityEngine;

public class EnemyConfig : IEntityMovementConfig
{
    public float Speed =>3.0f;
    public Vector3 Direction => Vector3.back;
}
