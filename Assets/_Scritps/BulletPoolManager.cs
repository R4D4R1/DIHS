using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager instance = null;

    [SerializeField] private int spawnCount;
    public int bulletDestroyTime;
    public GameObject bulletHolePrefab;
    public List<GameObject> bulletHoleList;




    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance == this)
        {
            Destroy(gameObject);
        }

        for(int i = 0; i < spawnCount;i++)
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab) as GameObject;
            bulletHoleList.Add(bulletHole);
            bulletHole.transform.parent = this.transform;
            bulletHole.SetActive(false);

        }
    }   
}