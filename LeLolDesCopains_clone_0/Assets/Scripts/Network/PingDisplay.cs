using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PingDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textContainer;

    private void Update()
    {
        textContainer.text = (PhotonNetwork.GetPing().ToString() + " ms");
    }
}
