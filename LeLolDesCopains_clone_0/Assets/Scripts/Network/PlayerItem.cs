using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerItem : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerName;

    public Player itemRef;

    public bool IsSet { get; private set; }

    public void SetPlayerName(Player _player)
    {
        itemRef = _player;
        playerName.text = _player.NickName;
        IsSet = true;
    }

    public void ResetField()
    {
        itemRef = null;
        playerName.text = "Waiting For Player...";
        IsSet = false;
    }

}
