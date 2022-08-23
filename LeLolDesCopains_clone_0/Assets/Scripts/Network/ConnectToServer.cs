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
    [SerializeField] private Button settingsButton;
    [SerializeField] private TextMeshProUGUI connectButtonText;

    [SerializeField] private string connectText = "Connecting";
    [SerializeField] private float textChangeTime = .5f;

    private int textIdx;

    public void OnClickConnect()
    {
        UpdateProprieties();

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
        settingsButton.interactable = true;

        playButton.GetComponent<Button>().onClick.Invoke();
    }

    public void UpdateProprieties()
    {
        string playerName = GameManager.Instance.PickedPlayerName.selectedPlayerName;
        ExitGames.Client.Photon.Hashtable proprieties = new ExitGames.Client.Photon.Hashtable();

        List<Color> colorsList = new List<Color>();
        ColorPlayer colorPlayer = GameManager.Instance.PickedPlayerColors;

        foreach (Image img in colorPlayer.PlayerParts)
        {
            colorsList.Add(img.color);
        }

        proprieties.Add("EyeL", "#" + ColorUtility.ToHtmlStringRGBA(colorsList[0]));
        proprieties.Add("EyeR", "#" + ColorUtility.ToHtmlStringRGBA(colorsList[1]));
        proprieties.Add("Head", "#" + ColorUtility.ToHtmlStringRGBA(colorsList[2]));
        proprieties.Add("Body", "#" + ColorUtility.ToHtmlStringRGBA(colorsList[3]));

        PhotonNetwork.NickName = playerName;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SetPlayerCustomProperties(proprieties);

        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }
}
