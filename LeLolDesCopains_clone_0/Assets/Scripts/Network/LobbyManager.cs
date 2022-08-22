using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System;


/*
 * 
 * 
 *      TODO : Ajouter nb de joueurs dans la room + cacher si pleine
 * 
 * 
 * 
 */

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInput;
    [SerializeField] private TMP_InputField roomPasswordInitInput;
    [SerializeField] private TMP_InputField roomPasswordTryEnterInput;

    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject roomPanel;
    [SerializeField] private GameObject passwordPanel;

    [SerializeField] private Transform contentObject;

    [SerializeField] private RoomItem PF_roomItem;
    private List<RoomItem> roomItemsList = new List<RoomItem>();

    private RoomItem currentRoomPasswordAttempt;

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

        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        RoomOptions option = new RoomOptions { MaxPlayers = 2 };

        table.Add("password", roomPasswordInitInput.text);

        option.CustomRoomProperties = table;
        option.CustomRoomPropertiesForLobby = new string[] { "password" };

        PhotonNetwork.CreateRoom(roomNameInput.text, option);
    }

    public void TryJoinRoom(RoomItem room)
    {
        if (room.HasPassword)
        {
            passwordPanel.SetActive(true);
            currentRoomPasswordAttempt = room;
        }
        else
            PhotonNetwork.JoinRoom(room.RoomName);
    }

    public void EnterRoomPassword()
    {
        string passwordAttempt = roomPasswordTryEnterInput.text;
        if (passwordAttempt.Equals(currentRoomPasswordAttempt.Password))
        {
            PhotonNetwork.JoinRoom(currentRoomPasswordAttempt.RoomName);
            currentRoomPasswordAttempt = null;
        }
        else
        {
            roomPasswordTryEnterInput.interactable = false;
            roomPasswordTryEnterInput.text = "Wrong Password.";
            roomPasswordTryEnterInput.textComponent.color = Color.red;
            Invoke(nameof(WrongPasswordInputReset), 1f);
        }
    }

    private void WrongPasswordInputReset()
    {
        roomPasswordTryEnterInput.textComponent.color = Color.black;
        roomPasswordTryEnterInput.text = "";
        roomPasswordTryEnterInput.interactable = true;
    }

    public void AbordPassword()
    {
        currentRoomPasswordAttempt = null;
    }

    public void OnClickLeaveRoom()
    {

        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);

        roomName.text = PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            PhotonNetwork.CurrentRoom.IsVisible = false;

        UpdatePlayers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayers();
        PhotonNetwork.CurrentRoom.IsVisible = true;
    }

    private void UpdatePlayers()
    {
        if (PhotonNetwork.CurrentRoom == null) return;

        player1.ResetField();
        player2.ResetField();

        foreach (KeyValuePair<int, Player> p in PhotonNetwork.CurrentRoom.Players)
        {
            CheckAndSetPlayerValues(p.Value);
        }
    }

    private void CheckAndSetPlayerValues(Player _player)
    {
        if (_player == null) return;

        PlayerItem _playerItem = null;

        if (!player1.IsSet) _playerItem = player1;
        else if (!player2.IsSet) _playerItem = player2;
        else return;

        string nickName = "";
        nickName = _player.NickName;

        if (nickName == null || nickName.Equals("")) nickName = "Unnamed";
        _playerItem.SetPlayerName(_player);

    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= update_TIMER)
        {
            UpdateRoomList(roomList);
            update_TIMER = Time.time + update_COOLDOWN;
        }
    }

    public void UpdateRoomList(List<RoomInfo> RoomInfoList)
    {
        foreach (RoomItem item in roomItemsList) { Destroy(item.gameObject); }
        roomItemsList.Clear();

        foreach (RoomInfo room in RoomInfoList)
        {
            RoomItem newRoom = Instantiate(PF_roomItem, contentObject);

            string password = "";
            password = (string)room.CustomProperties["password"];

            if (password.Equals(""))
                newRoom.SetRoom(room.Name, room.PlayerCount);
            else
                newRoom.SetRoom(room.Name, room.PlayerCount, password);

            roomItemsList.Add(newRoom);
        }
    }

    public void Refresh()
    {
        PhotonNetwork.JoinLobby();
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


}
