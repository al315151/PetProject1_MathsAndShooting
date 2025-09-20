using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EndGameBoundsSolver : MonoBehaviour
{
    [SerializeField]
    private BoxCollider boundsCollider;

    private Bounds bounds;
    private Vector3 cachedPosition;

    private void OnEnable()
    {
        bounds = boundsCollider.bounds;
        cachedPosition = transform.position;
    }

    public bool IsTargetBoundsInside(Vector3 worldSpaceCenter, Bounds objectBounds)
    {
        var worldSpaceObjectBoundsZLimitFront = worldSpaceCenter + (objectBounds.extents.z * Vector3.forward);
        var worldSpaceObjectBoundsZLimitBack = worldSpaceCenter + (objectBounds.extents.z * Vector3.back);

        //Debug.DrawLine(worldSpaceObjectBoundsZLimitBack, worldSpaceObjectBoundsZLimitBack + Vector3.up, Color.green, 0.5f);
        //Debug.DrawLine(worldSpaceObjectBoundsZLimitFront, worldSpaceObjectBoundsZLimitFront + Vector3.up, Color.green, 0.5f);

        return bounds.Contains(worldSpaceObjectBoundsZLimitBack) || bounds.Contains(worldSpaceObjectBoundsZLimitFront);
    }
}
