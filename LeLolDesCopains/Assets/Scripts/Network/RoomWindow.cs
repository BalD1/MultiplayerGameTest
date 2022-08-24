using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class RoomWindow : MonoBehaviour
{
    [SerializeField] private PlayerItem[] playerItems;
    [SerializeField] private Button startButton;

    public void OnReadyUpdate(Player _player, bool state)
    {
        if (_player.Equals(PhotonNetwork.LocalPlayer)) return;

        for (int i = 0; i < playerItems.Length; i++)
        {
            if (playerItems[i].itemRef.Equals(_player))
                playerItems[i].ReadyButton.SetToggleState(state);
        }

        CheckIfEveryoneIsReady();
    }

    public void CheckIfEveryoneIsReady()
    {
        startButton.interactable = false;

        for (int i = 0; i < playerItems.Length; i++)
        {
            if (playerItems[i].ReadyButton.IsReady == false) return;
        }

        startButton.interactable = true;
    }
}
