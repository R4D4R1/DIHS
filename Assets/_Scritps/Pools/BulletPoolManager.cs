using Mirror;
using Org.BouncyCastle.Asn1.Cmp;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : NetworkBehaviour
{
    public static BulletPoolManager instance = null;

    [SerializeField] private int spawnCount;
    public int bulletDestroyTime;
    public GameObject bulletHolePrefab;
    [SyncVar]
    public List<GameObject> bulletHoleList;

    public override void OnStartServer()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab) as GameObject;
            NetworkServer.Spawn(bulletHole);
            bulletHoleList.Add(bulletHole);
            bulletHole.transform.parent = this.transform;
            bulletHole.SetActive(false);

        }

        base.OnStartServer();
    }

    //protected virtual void Awake()
    //{
    //    if(instance == null)
    //    {
    //        instance = this;
    //    }
    //    else if(instance == this)
    //    {
    //        Destroy(gameObject);
    //    }

    //    for(int i = 0; i < spawnCount;i++)
    //    {
    //        GameObject bulletHole = Instantiate(bulletHolePrefab) as GameObject;
    //        NetworkServer.Spawn(bulletHole);
    //        bulletHoleList.Add(bulletHole);
    //        bulletHole.transform.parent = this.transform;
    //        bulletHole.SetActive(false);

    //    }
    //}
}