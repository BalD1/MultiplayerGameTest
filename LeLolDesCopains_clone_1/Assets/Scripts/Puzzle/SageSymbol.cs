using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SageSymbol : MonoBehaviour, Iinteractable
{
    [SerializeField] private GameObject relatedSocle;
    [SerializeField] private SagePuzzle puzzle;

    [SerializeField] private Outline outline;
    [SerializeField] private Color canInteractColor;
    [SerializeField] private Color canNotInteractColor;
    private float baseOutlineValue;

    private void Start()
    {
        baseOutlineValue = outline.OutlineWidth;
        outline.OutlineColor = canInteractColor;

        SetOutlineActive(false);
    }

    public GameObject Interact(GameObject sender)
    {
        puzzle.Interact(relatedSocle);

        return this.gameObject;
    }

    public void SetOutlineActive(bool active)
    {
        if (active) outline.OutlineWidth = baseOutlineValue;
        else outline.OutlineWidth = 0;
    }
}
