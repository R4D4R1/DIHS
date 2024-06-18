using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class HealthAndSpawnPlayer : NetworkBehaviour
{
    [SyncVar] private int connNumber = 0;

    [SyncVar] [SerializeField] private float HP;
    [SyncVar] [SerializeField] private float respawnTime;
    [SyncVar] [SerializeField] private NetworkRig networkRig;

    [SerializeField] private List<MeshRenderer> playersBodyPartsMesh;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Volume volume;


    [System.Serializable]
    public enum Teams
    {
        Red,
        Blue
    };

    [field: SerializeField]
    public Teams team
    {
        get;
        private set;
    }

    private void Start()
    { 

        if (!isLocalPlayer)
        {
            this.enabled = false;
        }

        // TEAM RED
        if (connNumber == 0)
        {
            team = Teams.Red;
            foreach(var part in playersBodyPartsMesh)
            {
                part.material = redMaterial;
            }

            connNumber = 1;

        }
        // TEAM BLUE
        else if (connNumber == 1)
        {
            team = Teams.Blue;
            foreach (var part in playersBodyPartsMesh)
            {
                part.material = blueMaterial;
            }

            connNumber = 0;

        }

        //SpawnPlayer();
    }

    public void SetHealth(float hp)
    {
        if (HP > 0) 
        {
            this.HP = hp;
        }

        if (HP <= 0)
        {
            StartCoroutine(RespawnPlayer(respawnTime));
        }
    }

    IEnumerator RespawnPlayer(float respawnTime)
    {
        Debug.Log("PLayer Died");

        if (volume.profile.TryGet(out ColorAdjustments CA))
        {
            CA.saturation.value = -100;
        }
        yield return new WaitForSecondsRealtime(respawnTime);

        CA.saturation.value = 0;

        SpawnPlayer();

        Debug.Log("Player Respawned");
    }

    private void SpawnPlayer()
    {
        if (team == Teams.Red)
        {
            var spawnPoint = StartSpawnPoints.instance.ChooseRandomRedSpawnPoint();
            networkRig.hardwareRig.gameObject.SetActive(false);
            networkRig.hardwareRig.transform.position = spawnPoint.transform.position;
            Debug.Log("Red Player Repawned");
        }
        if (team == Teams.Blue)
        {
            var spawnPoint = StartSpawnPoints.instance.ChooseRandomBlueSpawnPoint();
            networkRig.hardwareRig.gameObject.SetActive(false);
            networkRig.hardwareRig.transform.position = spawnPoint.transform.position;
            networkRig.hardwareRig.gameObject.SetActive(true);
            Debug.Log("Blue Playe Repawned");
        }
    }

    public float GetHP()
    {
        return this.HP;
    }

}
