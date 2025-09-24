using UnityEngine;

public class BulletConfig : IEntityMovementConfig
{
    public float Speed => 4.0f;

    public Vector3 Direction => Vector3.forward;

}
