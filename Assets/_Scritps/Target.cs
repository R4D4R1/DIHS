using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float HP = 100;
    [Serializable] public enum ImpactMaterialEnum
    {
        Wood,
        Metal,
        Flesh
    };

    [field: SerializeField] public ImpactMaterialEnum ImpactMaterial
    {
        get;
        private set;
    }

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
