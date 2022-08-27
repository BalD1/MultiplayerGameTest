using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (!instance) Debug.LogError("GameManager Instance was not found.");

            return instance;
        }
    }

    public enum E_GameStates
    {
        MainMenu,
        InGame,
        Pause,
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

                case E_GameStates.Pause:
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

    public PlayerCharacter currentPlayerOwner;

    public Camera mainCamera;

    public void OnUIManagerCreated()
    {

        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
            GameState = E_GameStates.InGame;
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

    public void HandlePause()
    {
        if (GameState == E_GameStates.InGame)
            GameState = E_GameStates.Pause;
        else
            GameState = E_GameStates.InGame;
    }

    private void Awake()
    {
        if (instance == null) instance = this;

    }
}
