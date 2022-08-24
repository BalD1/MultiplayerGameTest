using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject player_PF;
    [SerializeField] private Transform spawnPointsParent;
    private Transform[] spawnPoints;

    private void Start()
    {
        if (spawnPoints.Length == 0)
            PopulateSpawnPoints();

        int rand = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[rand];

        PhotonNetwork.Instantiate(player_PF.name, spawnPoint.position, Quaternion.identity);
    }

    public void PopulateSpawnPoints()
    {
        spawnPoints = new Transform[spawnPointsParent.childCount];

        for (int i = 0; i < spawnPointsParent.childCount; i++)
        {
            spawnPoints[i] = spawnPointsParent.GetChild(i);
        }
    }
}
