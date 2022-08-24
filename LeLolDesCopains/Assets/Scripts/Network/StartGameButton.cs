using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            this.gameObject.SetActive(false);
    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene("MainScene");
    }
}
