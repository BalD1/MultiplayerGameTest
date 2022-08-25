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

    [SerializeField] private ReadyButton readyButton;
    public ReadyButton ReadyButton { get => readyButton; }

    [SerializeField] private List<Image> playerPart;
    [SerializeField] private List<PlayerCharacter.PlayerColorableParts> playerPartName;

    public Player itemRef;

    public bool IsSet { get; private set; }

    public void SetPlayerName(Player _player)
    {
        readyButton.SetToggle(_player);

        itemRef = _player;
        playerName.text = _player.NickName;
        IsSet = true;
    }

    public void SetPlayerColor(Color _color, PlayerCharacter.PlayerColorableParts part)
    {
        for (int i = 0; i < playerPartName.Count; i++)
        {
            if (playerPartName[i].Equals(part))
                playerPart[i].color = _color;
        }
    }

    public void ResetField()
    {
        itemRef = null;
        playerName.text = "Waiting For Player...";
        IsSet = false;
    }

}
