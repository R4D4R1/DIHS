using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] protected float HP { get; set; } = 100;

    public virtual void TakeDamage(float damage)
    {
        HP -= damage;
        //Debug.Log(gameObject.name + " " + HP);
        
        if(HP<0)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
