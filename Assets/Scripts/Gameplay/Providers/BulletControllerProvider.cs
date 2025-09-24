using System;
using System.Collections.Generic;
using VContainer.Unity;

public class BulletControllerProvider : IInitializable, IDisposable
{
    private List<BaseBulletController> baseBullets;

    public void Dispose()
    {
        baseBullets = null;
    }

    public void Initialize()
    {
        baseBullets = new List<BaseBulletController>();
    }

    public void AddBulletController(BaseBulletController bullet)
    {
        baseBullets.Add(bullet);
    }

    public void RemoveBulletController(BaseBulletController bullet)
    {
        baseBullets.Remove(bullet);
    }

    public void Reset()
    {
        baseBullets.Clear();
    }

    public BaseBulletController GetBulletController(BaseBulletView bulletView)
    {
        for (int i = 0; i < baseBullets.Count; i++)
        {
            if (baseBullets[i].BulletID == bulletView.BulletID)
            {
                return baseBullets[i];
            }
        }
        return null;
    }

}
