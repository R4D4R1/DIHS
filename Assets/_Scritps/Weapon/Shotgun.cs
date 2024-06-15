//using UnityEngine;
//using UnityEngine.Events;

//public class ShotGun : Weapon
//{
//    [Range(0,10)]
//    [SerializeField] private int recoilOffset;
//    [Range(0, 10)]
//    [SerializeField] private int maxAmmo;
//    [SerializeField] private UnityEvent hasSpaceForAmmo;

//    public override void ShootGun()
//    { 
//        AttemptToShootShotGun(recoilOffset, targetX);
//        hasSpaceForAmmo.Invoke();
//    }

//    public int AddAmmo()
//    {
//        if(ammoInMagazine<maxAmmo)
//        {
//            return ++ammoInMagazine;
//        }
//        else
//        {
//            return maxAmmo;
//        }
//    }

//    public int GetCurrentAmmo()
//    {
//        return ammoInMagazine;
//    }

//    public int GetMaxAmmo()
//    {
//        return maxAmmo;
//    }
//}
