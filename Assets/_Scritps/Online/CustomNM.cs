using Mirror;
using UnityEngine;

public class CustomNM : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log($"Added {numPlayers + 1}  player");

        Debug.LogWarning(BulletPoolManager.instance);

        //BulletPoolManager.instance.SpawnDecals();

        base.OnServerAddPlayer(conn);
    }
}
