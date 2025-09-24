using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField]
    private BulletSpawnPositionProvider spawnPositionProvider;

    public void MovePlayer(float xAxisMovement)
    {
        // negative means left, positive means right
        transform.position = new Vector3 (xAxisMovement, transform.position.y, transform.position.z);
    }

    public Vector3 GetBulletSpawnPosition()
    {
        return spawnPositionProvider.GetBulletSpawnPosition();
    }
}
