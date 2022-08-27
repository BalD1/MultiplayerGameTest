using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseScripts : MonoBehaviour
{
    public void BackButton()
    {
        GameManager.Instance.GameState = GameManager.E_GameStates.InGame;
    }

    public void MainMenuButton()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }
}
