using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float HP = 100;

    public void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log(gameObject.name + " " + HP);
        
        if(HP<0)
        {
            Destroy(gameObject);
        }
    }

}
