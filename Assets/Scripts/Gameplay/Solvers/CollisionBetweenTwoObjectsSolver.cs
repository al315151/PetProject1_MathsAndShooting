using UnityEngine;

public static class CollisionBetweenTwoObjectsSolver
{
    public static bool AreTwoObjectsColliding(
        Vector3 worldPositionObject1,
        Bounds boundsObject1,
        Vector3 worldPositionObject2, 
        Bounds boundsObject2)
    {
        // Stolen formula from Bounds.Intersects(bounds).
        // We need to adapt it so that the bounds are actually on the expected worldPosition center.

        var boundsObject1Max = worldPositionObject1 + boundsObject1.max;
        var boundsObject1Min = worldPositionObject1 + boundsObject1.min;
        var boundsObject2Max = worldPositionObject2 + boundsObject2.max;
        var boundsObject2Min = worldPositionObject2 + boundsObject2.min;

        var areObjectsColliding = boundsObject1Min.x <= boundsObject2Max.x && boundsObject1Max.x >= boundsObject2Min.x &&
                       boundsObject1Min.y <= boundsObject2Max.y && boundsObject1Max.y >= boundsObject2Min.y &&
                       boundsObject1Min.z <= boundsObject2Max.z && boundsObject1Max.z >= boundsObject2Min.z;

        return areObjectsColliding;
    }
}
