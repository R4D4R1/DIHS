using Mirror;
using UnityEngine;

public class Target : NetworkBehaviour
{
    [SyncVar]
    [SerializeField] private float HP;

    public float GetHP()
    {
        return HP;
    }

    public void SetHP(float hp)
    {
        this.HP = hp;
    }

    public virtual void TakeDamage(float damage)
    {
        SetHP(GetHP() - damage);

        Debug.Log(gameObject.name + " " + HP);
        
        if(GetHP() <= 0)
        {
            Debug.Log(gameObject.name + " Dead");

            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
