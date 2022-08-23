using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                Debug.LogError("GameManager Instance was not found, force creation");
                Create();
            }

            return instance;
        }
    }

    public enum E_GameStates
    {
        MainMenu,
        InGame,
    }

    private E_GameStates gameState;
    public E_GameStates GameState
    {
        get => gameState;
        set
        {
            switch (value)
            {
                case E_GameStates.MainMenu:
                    break;

                case E_GameStates.InGame:
                    break;
            }

            UIManager.Instance.WindowsManager(value);

            gameState = value;
        }
    }

    [SerializeField] private float gravity = -9.81f;
    public float Gravity { get => gravity; }

    [SerializeField] private ColorPlayer pickedPlayerColors;
    public ColorPlayer PickedPlayerColors { get => pickedPlayerColors; }

    [SerializeField] private PlayerName pickedPlayerName;
    public PlayerName PickedPlayerName { get => pickedPlayerName; }

    [SerializeField] private LobbyManager lobbyManager;
    public LobbyManager LobbyManagerRef { get => lobbyManager; }

    private static GameManager Create()
    {
        GameObject gameManager = new GameObject();
        gameManager.name = "GameManager";

        gameManager.AddComponent<GameManager>();
        instance = gameManager.GetComponent<GameManager>();

        return gameManager.GetComponent<GameManager>();
    }

    public void StartGame(bool isHost)
    {
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            bool success = false;

            if (isHost) success = NetworkManager.Singleton.StartHost();
            else success = NetworkManager.Singleton.StartClient();

            if (success)
                GameManager.Instance.GameState = GameManager.E_GameStates.InGame;
        }
    }

    private void Awake()
    {
        instance ??= this;
    }
}
