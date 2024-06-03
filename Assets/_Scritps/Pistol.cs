using UnityEngine;

public class Pistol : Gun
{

    public override void ShootGun()
    {
        Debug.Log("Shoot Glock");

        particleSystem.Play();

        ProcessRecoil();

        //Hit
        HitSomething(Vector3.zero);

        //Sound
        AudioPoolExample audioPoolExample = this.gameObject.GetComponent<AudioPoolExample>();
        audioPoolExample.PlayClip(shootSound);
    }
}
