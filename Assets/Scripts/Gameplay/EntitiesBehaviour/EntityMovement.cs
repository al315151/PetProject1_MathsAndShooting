using UnityEngine;

public class EntityMovement : MonoBehaviour, IEntityMovement
{    
    private const float entitySpeed = 3.0f;    
    private Vector3 entityDirection = Vector3.back;

    private Vector3 targetPosition;

    private bool isPositionUpdated;

    public float EntitySpeed => entitySpeed;

    public void Initialize(Vector3 position)
    {
        targetPosition = position;
        transform.position = position;
    }

    public void AcceptVisitor(MovementVisitor visitor)
    {
        visitor.VisitEntityMovement(this);
    }

    private void Update()
    {
        if (isPositionUpdated)
        {
            transform.position = targetPosition;
            isPositionUpdated = false;
        }
    }

    public void UpdatePosition(float timeSpanInSeconds)
    {
        // Avoid doing operations that would make this sync, as we want to run this async.
        targetPosition = targetPosition + (entityDirection * entitySpeed * timeSpanInSeconds);    
        isPositionUpdated = true;
    }
}
