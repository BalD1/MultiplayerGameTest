using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PhotonView view;

    private void Start()
    {
        if (view.IsMine)
        {
            CreateController();
        }
    }

    private void CreateController()
    {

    }
}
