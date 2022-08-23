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
 *      TODO :  Add search room
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

    [SerializeField] private MessagePanel messagePanel;

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

    /// <summary> <para>
    /// <b>Called through button.</b>
    /// </para> <para>
    /// Creates a room with Name, and if have, password.
    /// </para> <para>
    /// Given in <paramref name="roomNameInput"></paramref> and <paramref name="roomPasswordTryEnterInput"></paramref>
    /// </para> </summary>
    public void OnClickCreate()
    {
        messagePanel.SetText(messagePanel.WaitText);

        if (roomNameInput.text.Equals("")) roomNameInput.text = "Unnamed Room";


        // Sets the max players to 2
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        RoomOptions option = new RoomOptions { MaxPlayers = 2 };

        table.Add("password", roomPasswordInitInput.text);

        option.CustomRoomProperties = table;
        option.CustomRoomPropertiesForLobby = new string[] { "password" };

        PhotonNetwork.CreateRoom(roomNameInput.text, option);

        roomNameInput.text = "";
        roomPasswordTryEnterInput.text = "";

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        messagePanel.SetText(message, Color.red);

        Invoke(nameof(WaitForPanel), 3f);
    }

    private void WaitForPanel()
    {
        messagePanel.ResetObject();
    }

    /// <summary> <para>
    /// <b>Called through button.</b>
    /// </para>
    /// If the room has a password, ask to input it, else just join.
    /// </summary>
    /// <param name="room">room to join</param>
    public void TryJoinRoom(RoomItem room)
    {
        if (room.HasPassword)
        {
            passwordPanel.SetActive(true);
            currentRoomPasswordAttempt = room;
        }
        else
        {
            messagePanel.SetText(messagePanel.WaitText);

            PhotonNetwork.JoinRoom(room.RoomName);
        }
    }

    /// <summary> <para>
    /// <b>Called through button.</b>
    /// </para> <para>
    /// Enter the password of the current room, if it has any. Attempt is collected in <paramref name="roomPasswordTryEnterInput"></paramref>
    /// </para> <para>
    /// If the password is correct, enter. Else, display error message for 1s
    /// </para> </summary>
    public void EnterRoomPassword()
    {
        string passwordAttempt = roomPasswordTryEnterInput.text;
        if (passwordAttempt.Equals(currentRoomPasswordAttempt.Password))
        {
            messagePanel.SetText(messagePanel.WaitText);

            roomPasswordTryEnterInput.text = "";
            PhotonNetwork.JoinRoom(currentRoomPasswordAttempt.RoomName);
            passwordPanel.SetActive(false);
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

    /// <summary> <para>
    /// Removes the error message on a password enter 
    /// </para> <para>
    /// See : <see cref="EnterRoomPassword"/>
    /// </para> </summary>
    private void WrongPasswordInputReset()
    {
        roomPasswordTryEnterInput.textComponent.color = Color.black;
        roomPasswordTryEnterInput.text = "";
        roomPasswordTryEnterInput.interactable = true;
    }

    /// <summary> <para>
    /// <b>Called through button.</b>
    /// </para> <para>
    /// Cancels the input of a password from a locked room.
    /// </para>
    /// </summary>
    public void AbordPassword()
    {
        roomPasswordTryEnterInput.text = "";
        currentRoomPasswordAttempt = null;
    }

    /// <summary> <para>
    /// <b>Called through button.</b>
    /// </para> <para>
    /// Simply leaves the current room.
    /// </para> </summary>
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /// <summary> <para>
    /// <b>Called through PhotonNetwork.</b>
    /// </para> <para>
    /// Switches lobby window to room window. If joining the room fulls it, hide it from other players.
    ///  </para> See <see cref="UpdatePlayers"/></summary>
    public override void OnJoinedRoom()
    {
        messagePanel.ResetObject();

        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);

        roomName.text = PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            PhotonNetwork.CurrentRoom.IsVisible = false;

        UpdatePlayers();
    }

    /// <summary> <para>
    /// <b>Called through PhotonNetwork.</b>
    /// </para> <para>
    /// Switches room window to lobby window.
    /// </para> </summary>
    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }


    /// <summary> <para>
    /// <b>Called through PhotonNetwork.</b>
    /// </para> <para>
    /// Called when a external player enters the current room.
    /// </para> See <see cref="UpdatePlayers"/></summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayers();
    }

    /// <summary> <para>
    /// <b>Called through PhotonNetwork.</b>
    /// </para> <para>
    /// Called when a external player exits the current room. Updates the panels displaying the room's players.
    ///  </para> See <see cref="UpdatePlayers"/></summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayers();
        PhotonNetwork.CurrentRoom.IsVisible = true;
    }

    /// <summary> <para>
    /// Called when a external player exits the current room. Updates the panels displaying the room's players.
    /// </para> See <see cref="CheckAndSetPlayerValues(Player)"/></summary>
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

    /// <summary> <para>
    /// Sets the panels containing the player's infos if the panel hasn't been set already.
    /// </para> <paramref name="_player"/> : The given <see cref="Photon.Realtime.Player"/> infos 
    /// </summary>
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

    /// <summary> <para>
    /// <b>Called through PhotonNetwork.</b>
    /// </para> <para>
    /// Called when a room has been created or destroyed, with <b><see cref="update_TIMER"/></b> as seconds cooldown.
    /// </para><paramref name="roomList"/> : the list of the current rooms. <para>
    /// Calls : <seealso cref="UpdateRoomList(List{RoomInfo})"/>
    /// </para> </summary>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= update_TIMER)
        {
            UpdateRoomList(roomList);
            update_TIMER = Time.time + update_COOLDOWN;
        }
    }

    /// <summary> <para>
    /// Updates the current rooms list, and displays it in the lobby panel.
    /// </para><paramref name="RoomInfoList"/> : the list of the current rooms.
    /// </summary>
    public void UpdateRoomList(List<RoomInfo> RoomInfoList)
    {
        // Clear the current roomItemsList
        foreach (RoomItem item in roomItemsList) { Destroy(item.gameObject); }
        roomItemsList.Clear();

        // Repopulate it
        foreach (RoomInfo room in RoomInfoList)
        {
            RoomItem newRoom = Instantiate(PF_roomItem, contentObject);

            string password = "";
            password = (string)room.CustomProperties["password"];

            // Do we have a password ?
            if (password.Equals(""))
                newRoom.SetRoom(room.Name, room.PlayerCount);
            else
                newRoom.SetRoom(room.Name, room.PlayerCount, password);

            roomItemsList.Add(newRoom);
        }
    }

    /// <summary> <para>
    /// <b>Called through button.</b>
    /// </para>
    /// Rejoins the lobby, in order to refresh the current room's list.
    /// </summary>
    public void Refresh()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }


}
