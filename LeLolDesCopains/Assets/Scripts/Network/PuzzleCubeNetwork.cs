using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PuzzleCubeNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField] private PuzzleCube ownerScript;

    private bool newIsGrabbed = false;
    private bool updateState = false;

    public void ChangeGrabState(bool newState)
    {
        base.photonView.RequestOwnership();

        newIsGrabbed = newState;
        updateState = true;

        base.photonView.RPC(nameof(RPC_ChangeGrabState), RpcTarget.Others, newState);
    }

    [PunRPC]
    private void RPC_ChangeGrabState(bool _newState)
    {
        ownerScript.OnGrabByOther(_newState);
    }

}
