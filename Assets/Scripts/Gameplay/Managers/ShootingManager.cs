using System;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class ShootingManager : IInitializable, IDisposable
{
    private readonly InputManager inputManager;
    private readonly EntityDirector entityDirector;

    private bool shootingEnabled;

    public ShootingManager(
        InputManager inputManager,
        EntityDirector entityDirector)
    {
        this.inputManager = inputManager;
        this.entityDirector = entityDirector;
    }

    public void Initialize()
    {
        inputManager.PlayerShootInputReceived += OnPlayerShootingInputReceived;
    }

    public void Dispose()
    {
        inputManager.PlayerShootInputReceived -= OnPlayerShootingInputReceived;
    }

    public void EnableShooting()
    {
        shootingEnabled = true;
    }

    public void DisableShooting()
    { 
        shootingEnabled = false; 
    }

    private void OnPlayerShootingInputReceived()
    {
        if (shootingEnabled == false)
        {
            return;
        }
        SpawnAndShootBullet().Forget();
    }

    private async UniTask SpawnAndShootBullet()
    {
        var newBullet = await entityDirector.SpawnBullet();
        newBullet.EnableEntityMovement();
    }
}
