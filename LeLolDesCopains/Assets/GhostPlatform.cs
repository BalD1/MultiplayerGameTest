using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GhostPlatform : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] private bool updateMaterials = false;
    [SerializeField] private bool isGhost = true;
    [SerializeField] private BoxCollider selfCollider;

    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color ghostColor;

    private void OnEnable()
    {
        if (!updateMaterials) return;
        if (isGhost) SetGhost();
        else SetActive();
    }

    private void SetActive()
    {
        selfCollider.enabled = true;
        this.renderer.material.color = activeColor;
    }

    private void SetGhost()
    {
        selfCollider.enabled = false;
        this.renderer.material.color = ghostColor;
    }

#endif
}
