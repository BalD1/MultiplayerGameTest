using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    [SerializeField] private bool useCustomPosition;
    [SerializeField] private Vector3 customPosition;

    private void Start()
    {
        Vector3 position = this.transform.position;
        if (useCustomPosition)
            position = customPosition;

        PhotonNetwork.Instantiate(objectToSpawn.name, position, Quaternion.identity);
    }
}
