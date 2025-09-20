using System;
using UnityEngine;
using VContainer.Unity;

public class PlayerController : IInitializable, IDisposable
{
    private const float inputThreshold = 0.1f;

    private readonly IPlayerConfig playerConfig;
    private readonly PlayerView playerView;
    private readonly InputManager inputManager;

    private bool InputEnabled = true;

    public PlayerController(
        IPlayerConfig playerConfig,
        PlayerView playerView,
        InputManager inputManager)
    {
        this.playerConfig = playerConfig;
        this.playerView = playerView;
        this.inputManager = inputManager;
    }

    public void Initialize()
    {
        Debug.Log($"Player speed: {playerConfig.PlayerSpeed}");
        Subscribe();
    }

    public void Dispose()
    {
        Unsubscribe();
    }

    public void EnablePlayerInput()
    {
        InputEnabled = true;
    }

    public void DisablePlayerInput()
    {
        InputEnabled = false;
    }


    private void Subscribe()
    {
        inputManager.OnPlayerInputReceived += OnPlayerInputReceived;
    }

    private void Unsubscribe()
    {
        inputManager.OnPlayerInputReceived -= OnPlayerInputReceived;
    }
    
    private void OnPlayerInputReceived(Vector3 inputPosition)
    {
        if (InputEnabled == false)
        {
            return;
        }

        var currentPlayerPosOnXAxis = playerView.transform.position.x;
        var inputDistance = inputPosition.x - currentPlayerPosOnXAxis;
        if (Mathf.Abs(inputDistance) < inputThreshold)
        {
            return;
        }

        // Move player towards the current input position in X axis.
        var currentSpeed = Time.deltaTime * playerConfig.PlayerSpeed;
        var direction = Mathf.Sign(inputDistance);
        var travelDistance = currentSpeed * direction;
        var nextPosition = currentPlayerPosOnXAxis;
        // Snap to position if the speed is too high
        if (direction > 0)
        {
            nextPosition = Mathf.Min(nextPosition + travelDistance, inputPosition.x);
        }
        else
        {
            nextPosition = Mathf.Max(nextPosition + travelDistance, inputPosition.x);
        }
        playerView.MovePlayer(nextPosition);
    }

}
