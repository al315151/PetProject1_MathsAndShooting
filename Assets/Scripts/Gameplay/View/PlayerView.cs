using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public void MovePlayer(float xAxisMovement)
    {
        // negative means left, positive means right
        transform.position = new Vector3 (xAxisMovement, transform.position.y, transform.position.z);
    }
}
