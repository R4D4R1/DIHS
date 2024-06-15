using System.Collections.Generic;
using UnityEngine;


public class StartSpawnPoints : MonoBehaviour
{

    public static StartSpawnPoints instance = null;

    [SerializeField] private List<Transform> redSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> blueSpawnPoints = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    public Transform ChooseRandomRedSpawnPoint()
    {
        return redSpawnPoints[Random.Range(0, redSpawnPoints.Count)];
    }

    public Transform ChooseRandomBlueSpawnPoint()
    {
        return blueSpawnPoints[Random.Range(0, blueSpawnPoints.Count)];
    }
}
