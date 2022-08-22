using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomItem : MonoBehaviour
{
    [SerializeField] private LobbyManager manager;

    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI roomPlayerCount;

    [SerializeField] private Image lockImage;

    public bool HasPassword { get; private set; }
    public string Password { get; private set; }

    public string RoomName { get; private set; }

    private int playerCount;
    public int PlayerCount
    {
        get => playerCount;
        set
        {
            playerCount = value;
            if (playerCount <= 0) Destroy(this.gameObject);
        }
    }

    public void SetRoom(string _name, int _playerCount)
    {
        RoomName = _name;
        roomName.text = _name;
        playerCount = _playerCount;
        roomPlayerCount.text = playerCount + " / 2";

        lockImage.enabled = false;
    }
    public void SetRoom(string _name, int _playerCount, string _password)
    {
        RoomName = _name;
        roomName.text = _name;
        playerCount = _playerCount;
        roomPlayerCount.text = playerCount + " / 2";

        HasPassword = true;
        Password = _password;

        lockImage.enabled = HasPassword;
    }

    public void OnClickItem()
    {
        manager ??= GameManager.Instance.LobbyManagerRef;
        manager.TryJoinRoom(this);
        playerCount++;
    }
}
