using UnityEngine;

public class EntityMovement : MonoBehaviour, IEntityMovement
{

    public bool IsEntityMovementAllowed => isEntityMovementAllowed;

    private float entitySpeed;    
    private Vector3 entityDirection;

    private Vector3 targetPosition;
    private bool isPositionUpdated;
    private bool isEntityMovementAllowed;

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

    public void EnableMovement()
    {
        isEntityMovementAllowed = true;
    }

    public void DisableMovement()
    { 
        isEntityMovementAllowed = false; 
    }    

    public void UpdatePosition(float timeSpanInSeconds)
    {
        if (IsEntityMovementAllowed == false)
        {
            return;
        }
        // Avoid doing operations that would make this sync, as we want to run this async.
        targetPosition = targetPosition + (entityDirection * entitySpeed * timeSpanInSeconds);    
        isPositionUpdated = true;
    }

    public void SetupSpeedAndDirection(Vector3 direction, float speed)
    {
        entitySpeed = speed;
        entityDirection = direction;
    }
}
