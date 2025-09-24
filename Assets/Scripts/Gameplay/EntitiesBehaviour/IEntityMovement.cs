using UnityEngine;

public interface IEntityMovement
{
    public void Initialize(Vector3 position);

    public void SetupSpeedAndDirection(Vector3 direction,  float speed);

    public void AcceptVisitor(MovementVisitor visitor);

    public void UpdatePosition(float timeSpanInSeconds);
}
