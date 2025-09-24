using System;
using System.Collections.Generic;
using VContainer.Unity;

public class BulletControllerProvider : IInitializable, IDisposable
{
    private Dictionary<int, BaseBulletController> baseBullets;

    public void Dispose()
    {
        baseBullets = null;
    }

    public void Initialize()
    {
        baseBullets = new Dictionary<int, BaseBulletController>();
    }

    public void AddBulletController(BaseBulletController bullet)
    {
        baseBullets.Add(bullet.BulletID, bullet);
    }

    public void RemoveBulletController(BaseBulletController bullet)
    {
        baseBullets.Remove(bullet.BulletID);
    }

    public void Reset()
    {
        baseBullets.Clear();
    }

    public BaseBulletController GetBulletController(BaseBulletView bulletView)
    {
        return baseBullets[bulletView.BulletID];
    }

}
