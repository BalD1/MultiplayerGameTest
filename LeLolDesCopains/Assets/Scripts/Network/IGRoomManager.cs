using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class IGRoomManager : MonoBehaviourPunCallbacks
{
    private static IGRoomManager instance;
    public static IGRoomManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("RoomManager instance could not be found.");
                return null;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name.Equals("MainScene"))
        {
            PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
        }
    }
}
