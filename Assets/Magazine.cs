using UnityEngine;

public class Magazine : MonoBehaviour
{ 
    [SerializeField] private int ammoInMagazine;
    
    public int GetAmmo()
    {
        return ammoInMagazine;
    }

    public void SetAmmo(int ammo)
    {
        ammoInMagazine = ammo;
    }
}
