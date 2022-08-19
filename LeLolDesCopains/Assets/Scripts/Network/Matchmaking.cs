using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Threading.Tasks;
using TMPro;

public class Matchmaking : MonoBehaviour
{
    [SerializeField] private TMP_InputField jointIput;

    [SerializeField] private UnityTransport _transport;

    private const int roomSize = 2;
    private string roomName;

    private async void Awake()
    {
        await Authentificate();
    }

    private static async Task Authentificate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRoom()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(roomSize);
        string lol = "none";
        lol = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        Debug.Log(lol);

        _transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        NetworkManager.Singleton.StartHost();
        GameManager.Instance.GameState = GameManager.E_GameStates.InGame;
    }

    public async void JoinGame()
    {
        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(jointIput.text);

        _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        NetworkManager.Singleton.StartClient();
        GameManager.Instance.GameState = GameManager.E_GameStates.InGame;
    }
}
