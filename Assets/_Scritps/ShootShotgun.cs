using UnityEngine;

public class ShotGun : Gun
{

    public override void ShootGun()
    {
        Debug.Log("Shoot ShotGun");

        particleSystem.Play();
        
        ProcessRecoil();
        //Для каждой дробинки своя пуля

        for (int i = 0; i < 9; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-10,10)/100f,Random.Range(-10,10)/100f,Random.Range(-10,10)/100f);
            HitSomething(offset);
        }

        //Sound
        AudioPoolExample audioPoolExample = this.gameObject.GetComponent<AudioPoolExample>();
        audioPoolExample.PlayClip(shootSound);
    }
}
