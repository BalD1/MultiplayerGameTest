using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (!instance)
                Debug.LogError("UIManager instance was not found.");

            return instance;
        }
    }

    [SerializeField] private GameObject mainMenu;

    [SerializeField] private GameObject pauseMenu;

    private static UIManager Create()
    {
        GameObject uiManager = new GameObject();
        uiManager.name = "UIManager";

        uiManager.AddComponent<UIManager>();
        instance = uiManager.GetComponent<UIManager>();

        return instance;
    }

    private void Awake()
    {
        if (instance == null) instance = this;

        WindowsManager(GameManager.Instance.GameState);
    }

    public void OnClickedButton(string name)
    {
        switch (name)
        {
            case "HOST":
                GameManager.Instance.StartGame(true);
                break;

            case "JOIN":
                GameManager.Instance.StartGame(false);
                break;

            case "QUIT":
                Application.Quit();
                break;

            default:
                Debug.LogError("Button " + name + " not found in switch statement.");
                break;
        }
    }

    public void WindowsManager(GameManager.E_GameStates value)
    {
        switch (value)
        {
            case GameManager.E_GameStates.MainMenu:
                mainMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                break;

            case GameManager.E_GameStates.InGame:
                pauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                break;

            case GameManager.E_GameStates.Pause:
                pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }
}
