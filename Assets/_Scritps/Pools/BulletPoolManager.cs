using Mirror;
using UnityEngine;

public class BulletPoolManager : NetworkBehaviour
{
    public static BulletPoolManager instance = null;

    [SerializeField] private int spawnCount;
    public int bulletDestroyTime;
    public GameObject bulletHolePrefab;

    public readonly SyncList<NetworkIdentity> bulletHoleSyncList = new SyncList<NetworkIdentity>();

    [Server]
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

        SpawnDecals();

        base.OnStartServer();
    }

    [Server]
    public void SpawnDecals()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject bulletHole = Instantiate(bulletHolePrefab);
            NetworkServer.Spawn(bulletHole);
            bulletHoleSyncList.Add(bulletHole.GetComponent<NetworkIdentity>());
            //bulletHole.transform.parent = this.transform;
        }       
    }
}