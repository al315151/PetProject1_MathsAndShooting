using System;
using UnityEngine;
using VContainer.Unity;

public class CameraProvider : MonoBehaviour
{
    public Camera MainCamera => mainCamera;

    private Camera mainCamera;

    private void OnEnable()
    {
       mainCamera = Camera.main;
    }

    private void OnDisable()
    {
        mainCamera = null;
    }

}
