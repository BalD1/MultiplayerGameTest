using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private LobbyManager manager;

    [SerializeField] private TextMeshProUGUI roomName;

    public void SetRoomName(string _name)
    {
        roomName.text = _name;
    }

    public void OnClickItem()
    {
        manager ??= GameManager.Instance.LobbyManagerRef;
        manager.JoinRoom(roomName.text);
    }
}
