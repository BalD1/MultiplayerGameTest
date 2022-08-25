using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("PlayerManager Instance could not be found. Force create.");
                instance = ForceCreate();
            }
            return instance;
        }
    }

    private static PlayerManager ForceCreate()
    {
        GameObject playerManager = new GameObject();
        playerManager.name = "PlayerManager";

        playerManager.AddComponent<PlayerManager>();
        instance = playerManager.GetComponent<PlayerManager>();

        return instance;
    }

    [System.Serializable]
    public struct PlayerByNetwork
    {
        public GameObject playerObject;
        public Player playerNetwork;
    }

    public List<PlayerByNetwork> playersByNetworks = new List<PlayerByNetwork>();

    private void Awake()
    {
        instance = this;
    }
}
