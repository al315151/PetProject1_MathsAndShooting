using UnityEngine;

public class BulletSpawnPositionProvider : MonoBehaviour
{
    public Vector3 GetBulletSpawnPosition()
    {
        return transform.position;
    }
}
