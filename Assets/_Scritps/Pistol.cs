using UnityEngine;

public class Pistol : Gun
{

    private void Start()
    {
        ShootGun();
    }

    public override void ShootGun()
    {
        Debug.Log("Shoot Glock");

        ProcessRecoil();
        DecreaseAmmo();
        //Hit
        HitSomething(Vector3.zero);


        //Sound
        AudioPoolExample audioPoolExample = this.gameObject.GetComponent<AudioPoolExample>();
        audioPoolExample.PlayClip(shootSound);

    }   
}
