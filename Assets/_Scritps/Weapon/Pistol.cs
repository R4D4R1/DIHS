using JetBrains.Annotations;
using UnityEngine;

public class Pistol : Weapon
{
    public override void ShootGun()
    {        
        AttemptToShoot(Vector3.zero, boltTransform, targetX);
    }   
}
