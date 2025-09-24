using System;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

public class ShootingManager : IInitializable, IDisposable
{
    private const float TimeoutBetweenShotsInSeconds = 0.5f;

    private readonly InputManager inputManager;
    private readonly EntityDirector entityDirector;
    private readonly PlayerController playerController;
    private bool shootingEnabled;

    public ShootingManager(
        InputManager inputManager,
        EntityDirector entityDirector,
        PlayerController playerController)
    {
        this.inputManager = inputManager;
        this.entityDirector = entityDirector;
        this.playerController = playerController;
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
        newBullet.SetBulletStartPosition(playerController.GetBulletSpawnPosition());
        newBullet.EnableEntityMovement();

        ApplyShootingTimeout().Forget();
    }

    private async UniTask ApplyShootingTimeout()
    {
        //Disable shooting until timeout has passed.
        shootingEnabled = false;
        await UniTask.WaitForSeconds(TimeoutBetweenShotsInSeconds).ContinueWith(() => shootingEnabled = true);
    }
}
