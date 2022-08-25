using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworkSetup : MonoBehaviour
{
    [SerializeField] private Behaviour[] otherComponentsToDisable;
    [SerializeField] private Behaviour[] selfComponentsToDisable;
    [SerializeField] private Renderers playerRenderers;
    [SerializeField] private PhotonView view;

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
        foreach (Behaviour item in otherComponentsToDisable)
            item.enabled = false;
    }
}
