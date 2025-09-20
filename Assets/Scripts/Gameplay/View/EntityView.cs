using UnityEngine;

public class EntityView : MonoBehaviour
{
    private void OnEnable()
    {
        //Debug.Log($"Object: {gameObject.name} , created Entity View!");
    }

    public void SetViewPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

}
