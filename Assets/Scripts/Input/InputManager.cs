using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

public class InputManager : ITickable
{
    private readonly int interactionLayer = LayerMask.GetMask("PlayerInteraction");

    private readonly CameraProvider cameraProvider;

    public Action<Vector3> OnPlayerMovementInputReceived;

    public Action PlayerShootInputReceived;

    public InputManager(CameraProvider cameraProvider)
    {
        this.cameraProvider = cameraProvider;
    }

    public void Tick()
    {
        OnEachFrame().Forget();
    }

    private UniTask OnEachFrame()
    {
        if (cameraProvider.MainCamera == null)
        {
            return UniTask.CompletedTask;
        }

        var currentCamera = cameraProvider.MainCamera;

        // On mouse button being pressed, 
        if (Input.GetMouseButton(0))
        {
            GetWorldPositionFromScreenPosition(Input.mousePosition);
        }
        if (Input.GetMouseButtonDown(1))
        {
            PlayerShootInputReceived?.Invoke();
        }

        return UniTask.CompletedTask;
    }

    private void GetWorldPositionFromScreenPosition(Vector3 screenPosition)
    {
        var currentCamera = cameraProvider.MainCamera;
        var originPosition = currentCamera.transform.position;

        var endVectorPos = new Vector3(screenPosition.x, screenPosition.y, currentCamera.nearClipPlane);
        var worldPosition = currentCamera.ScreenToWorldPoint(endVectorPos);

        var direction = worldPosition - originPosition;

        //Debug.Log($"Input mouse position in pixels: {screenPosition}");
        //Debug.Log($"Input mouse position in world coordinates: {worldPosition}");

        var raycastResults = new RaycastHit[5];
        var numberOfHits = Physics.RaycastNonAlloc(new Ray(originPosition, direction), raycastResults, 20.0f, interactionLayer);
        Debug.DrawRay(originPosition, direction, Color.red, 1.0f);

        if (numberOfHits > 0)
        {
            for (var i = 0; i < numberOfHits; i++)
            {
                //Debug.Log($"Position obtained by raycast: {raycastResults[i].point}");
                OnPlayerMovementInputReceived?.Invoke(raycastResults[i].point);
            }
        }

    }



}
