using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class ReadyButton : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private Toggle toggle;

    [SerializeField] private RoomWindow roomWindow;

    private Player playerItem;

    private bool isReady = false;
    public bool IsReady { get => isReady; }

    public void SetToggle(Player player)
    {
        toggle.isOn = false;
        playerItem = player;

        if (player.Equals(PhotonNetwork.LocalPlayer)) toggle.interactable = true;
        else toggle.interactable = false;
    }

    public void OnToggleUpdate(bool _toggle)
    {
        SetToggleState(_toggle);

        base.photonView.RPC(nameof(RPC_ChangeReadyState), RpcTarget.Others, PhotonNetwork.LocalPlayer, isReady);

        roomWindow.CheckIfEveryoneIsReady();
    }

    public void SetToggleState(bool _toggle)
    {
        isReady = _toggle;

        toggle.SetIsOnWithoutNotify(_toggle);

        readyText.text = isReady ? "Ready" : "Not Ready";
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool _state)
    {
        roomWindow.OnReadyUpdate(player, _state);
    }
}
