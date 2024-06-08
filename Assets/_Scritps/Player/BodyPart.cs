using UnityEngine;

public class BodyPart : Target
{
    [Range(0f, 2f)]
    [SerializeField] private float damageCoef;
    [SerializeField] private PlayerHealth playerHealth;

    public float GetDamageCoef()
    {
        return damageCoef;
    }

    public override void TakeDamage(float damage)
    {
        playerHealth.SetHealth(HP - damage * damageCoef);

        if (HP < 0)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

}
