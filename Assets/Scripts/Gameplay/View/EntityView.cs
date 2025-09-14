using UnityEngine;

public class EntityView : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log($"Object: {gameObject.name} , created Entity View!");
    }
}
