using DG.Tweening;
using UnityEngine;

public class AssaultRifle : Weapon
{

    public override void ShootGun()
    {

        AttemptToShoot(Vector3.zero, boltTransform,targetX);

    }
}
