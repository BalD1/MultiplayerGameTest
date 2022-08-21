using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerItem : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI playerName;

    public void SetPlayerName(string _nickName)
    {
        playerName.text = _nickName;
    }

}
