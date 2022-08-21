using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInput;

    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject roomPanel;

    [SerializeField] private Transform contentObject;

    [SerializeField] private RoomItem PF_roomItem;
    private List<RoomItem> roomItemsList = new List<RoomItem>();

    [SerializeField] private TextMeshProUGUI roomName;

    [SerializeField] private PlayerItem player1;
    [SerializeField] private PlayerItem player2;

    [SerializeField] private const float update_COOLDOWN = 1.5f;
    private float update_TIMER;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        if (roomNameInput.text.Equals("")) roomNameInput.text = "Unnamed Room";

        PhotonNetwork.CreateRoom(roomNameInput.text, new RoomOptions() { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);

        roomName.text = PhotonNetwork.CurrentRoom.Name;

        UpdatePlayers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayers();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= update_TIMER)
        {
            UpdateRoomList(roomList);
            update_TIMER = Time.time + update_COOLDOWN;

        }
    }

    private void UpdateRoomList(List<RoomInfo> RoomInfoList)
    {
        foreach (RoomItem item in roomItemsList) { Destroy(item.gameObject); }
        roomItemsList.Clear();

        foreach (RoomInfo room in RoomInfoList)
        {
            RoomItem newRoom = Instantiate(PF_roomItem, contentObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    private void UpdatePlayers()
    {
        if (PhotonNetwork.CurrentRoom == null) return;

        Player p1, p2;

        // TODO : bug update quand qq quite le salon

        PhotonNetwork.CurrentRoom.Players.TryGetValue(1, out p1);
        PhotonNetwork.CurrentRoom.Players.TryGetValue(2, out p2);

        SetPlayerValues(ref p1, player1);
        SetPlayerValues(ref p2, player2);
    }

    private void SetPlayerValues(ref Player _player, PlayerItem _playerItem)
    {
        string nickName = "";

        if (_player != null)
        {
            nickName = _player.NickName;

            if (nickName == null || nickName.Equals("")) _player.NickName = "Player 1";
            _playerItem.SetPlayerName(nickName);
        }
        else _playerItem.SetPlayerName("Waiting For Player...");
    }
}
