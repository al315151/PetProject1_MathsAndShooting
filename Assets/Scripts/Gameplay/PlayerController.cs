using System;
using UnityEngine;
using VContainer.Unity;

public class PlayerController : IInitializable, IDisposable
{
    private readonly IPlayerConfig playerConfig;

    public PlayerController(IPlayerConfig playerConfig)
    {
        this.playerConfig = playerConfig;
    }

    public void Initialize()
    {
        Debug.Log($"Player speed: {playerConfig.PlayerSpeed}");
    }

    public void Dispose()
    {
        
    }

    
}
