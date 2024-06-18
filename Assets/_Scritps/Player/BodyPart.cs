using Mirror;
using UnityEngine;

public class BodyPart : NetworkBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float damageCoef;
    [SerializeField] private HealthAndSpawnPlayer playerHealth;

    private void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }
    }

    public float GetDamageCoef()
    {
        return damageCoef;
    }

    public void TakeDamage(float damage)
    {
        playerHealth.SetHealth(playerHealth.GetHP() - damage * damageCoef);
        Debug.Log(playerHealth.GetHP());

    }

}
