using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

public class InputManager : IInitializable, IDisposable, ITickable
{
    private readonly CameraProvider cameraProvider;

    public InputManager(CameraProvider cameraProvider)
    {
        this.cameraProvider = cameraProvider;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    { 
    }

    public void Tick()
    {
        OnEachFrame().Forget();
    }

    private async UniTask OnEachFrame()
    {
        if (cameraProvider.MainCamera == null)
        {
            return;
        }

        var currentCamera = cameraProvider.MainCamera;

        // On mouse button being pressed, 
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Input.mousePosition;
            Debug.Log($"Input mouse position in pixels: {mousePosition}");
        }
    }


}
