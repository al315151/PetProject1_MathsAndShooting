using UnityEngine;

public interface IEntityMovement
{
    public float EntitySpeed { get; }

    public void Initialize(Vector3 position);

    public void AcceptVisitor(MovementVisitor visitor);

    public void UpdatePosition(float timeSpanInSeconds);
}
