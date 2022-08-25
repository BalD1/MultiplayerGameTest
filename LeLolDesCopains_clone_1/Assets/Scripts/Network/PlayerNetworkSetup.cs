using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerNetworkSetup : MonoBehaviour, IPunObservable
{
    [SerializeField] private Behaviour[] otherComponentsToDisable;
    [SerializeField] private Behaviour[] selfComponentsToDisable;
    [SerializeField] private Renderers playerRenderers;
    [SerializeField] private PhotonView view;

    [SerializeField] private TextMeshProUGUI hudName;

    [System.Serializable]
    public struct Renderers
    {
        public MeshRenderer eyeL_Renderer;
        public MeshRenderer eyeR_Renderer;
        public MeshRenderer head_Renderer;
        public MeshRenderer body_Renderer;
    }

    private void Start()
    {
        PlayerManager.Instance.playersByNetworks.Add(
            new PlayerManager.PlayerByNetwork
            {
                playerObject = this.gameObject,
                playerNetwork = this.view.Owner
            });

        if (!view.IsMine)
            InitiateOtherPlayer();
        else
            InitiateLocalPlayer();
    }

    private void InitiateLocalPlayer()
    {
        foreach (Behaviour item in selfComponentsToDisable)
            item.enabled = false;

        // Hide self components
        playerRenderers.eyeL_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        playerRenderers.eyeR_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        playerRenderers.head_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        playerRenderers.body_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    private void InitiateOtherPlayer()
    {
        hudName.text = view.Owner.NickName;

        SetPlayerColors();

        foreach (Behaviour item in otherComponentsToDisable)
            item.enabled = false;
    }

    private void SetPlayerColors()
    {
        Player _player = view.Owner;
        Color c = Color.white;

        foreach (var item in _player.CustomProperties)
        {
            ColorUtility.TryParseHtmlString((string)item.Value, out c);
            switch (item.Key)
            {
                case "EyeL":
                    playerRenderers.eyeL_Renderer.material.color = c;
                    break;

                case "EyeR":
                    playerRenderers.eyeR_Renderer.material.color = c;
                    break;

                case "Head":
                    playerRenderers.head_Renderer.material.color = c;
                    break;

                case "Body":
                    playerRenderers.body_Renderer.material.color = c;
                    break;

                default:
                    Debug.LogError(item.Key + " was not found in switch statement.");
                    break;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)   // if is owner
        {

        }
        else if (stream.IsReading)  // if is client
        {

        }
    }
}
