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
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
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
}
