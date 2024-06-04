using UnityEngine;

public class ShotGun : Gun
{

    public override void ShootGun()
    {
        Debug.Log("Shoot ShotGun");

        ProcessRecoil();
        DecreaseAmmo();

        //Для каждой пули свои дробинки

        for (int i = 0; i < 9; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-5,5)/100f,Random.Range(-5,5)/100f,Random.Range(-5,5)/100f);
            HitSomething(offset);
        }

        //Sound
        AudioPoolExample audioPoolExample = this.gameObject.GetComponent<AudioPoolExample>();
        audioPoolExample.PlayClip(shootSound);
    }
}
