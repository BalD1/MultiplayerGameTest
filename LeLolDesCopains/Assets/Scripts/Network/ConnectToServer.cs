using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject connectButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private TextMeshProUGUI connectButtonText;

    [SerializeField] private string connectText = "Connecting";
    [SerializeField] private float textChangeTime = .5f;

    private int textIdx;

    public void OnClickConnect()
    {
        string playerName = GameManager.Instance.PickedPlayerName.selectedPlayerName;

        PhotonNetwork.NickName = playerName;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();

        connectButtonText.text = connectText;

        if (connectButtonText != null) InvokeRepeating(nameof(AnimateConnectingText), 0, textChangeTime);
    }

    private void AnimateConnectingText()
    {
        textIdx++;
        connectButtonText.text += ".";
        if (textIdx > 3)
        {
            connectButtonText.text = connectText;
            textIdx = 0;
        }
    }

    public override void OnConnectedToMaster()
    {
        CancelInvoke(nameof(AnimateConnectingText));
        ChangeDisplay();
    }

    private void ChangeDisplay()
    {
        connectButton.SetActive(false);
        playButton.SetActive(true);
        lobbyPanel.SetActive(true);

        playButton.GetComponent<Button>().onClick.Invoke();
    }
}
