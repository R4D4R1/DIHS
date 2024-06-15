using Mirror;
using UnityEngine;

public class NetworkGunStats : NetworkBehaviour
{
    [SyncVar][SerializeField] private int ammoInMagazine;
    [SyncVar][SerializeField] private int maxAmmoInMagazine;

    public int GetAmmo()
    {
        return ammoInMagazine;
    }

    public int GetMaxAmmo()
    {
        return maxAmmoInMagazine;
    }

    public void SetAmmo(int ammo)
    {
        if (ammo < maxAmmoInMagazine)
        {
            ammoInMagazine = ammo;
        }
        else
            ammoInMagazine = maxAmmoInMagazine;
    }
}
