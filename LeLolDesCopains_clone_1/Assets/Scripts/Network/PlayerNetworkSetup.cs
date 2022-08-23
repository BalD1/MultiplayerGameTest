using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkSetup : MonoBehaviour
{
    [SerializeField] private Behaviour[] componentsToDisable;
    [SerializeField] private Renderers playerRenderers;

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
        // Disable the components in array if this is not the local player, or hide self
        // if (!this.isLocalPlayer) foreach (var component in componentsToDisable) { component.enabled = false; }
        // else InitiateLocalPlayer();
    }

    private void InitiateLocalPlayer()
    {
        Camera.main.gameObject.SetActive(false);

        // Hide self components
        playerRenderers.eyeL_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        playerRenderers.eyeR_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        playerRenderers.head_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        playerRenderers.body_Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }

    private void OnDisable()
    {
        if (Camera.main)
            Camera.main.gameObject.SetActive(true);
    }
}
