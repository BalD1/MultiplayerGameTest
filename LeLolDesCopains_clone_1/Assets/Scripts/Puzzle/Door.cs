using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Iinteractable, IColorable
{
    [SerializeField] private Transform target;

    [SerializeField] private MeshRenderer colorableMesh;

    [SerializeField] private GameObject mesh;

    [SerializeField] private short neededInteractors = 1;
    private short currentInteractors = 0;

    private List<GameObject> interactors = new List<GameObject>();

    [SerializeField] private float lerpSpeed = .5f;
    private float lerpCurrentValue = 0;

    [SerializeField] private float distanceBeforeStop = 1f;

    [SerializeField] private Outline outline;
    private float baseOutlineValue;
    private bool canBeOutlined = true;

    private Vector3 basePos;

    [SerializeField] private bool isLocked = false;
    private bool isOpen = false;

    private bool animationEnded = true;

    private PlayerCharacter playerClient;

    private void Start()
    {
        playerClient = GameManager.Instance.currentPlayerOwner;

        baseOutlineValue = outline.OutlineWidth;

        SetOutlineActive(false);
        basePos = mesh.transform.position;
    }

    public GameObject Interact(GameObject sender)
    {
        if (isLocked && sender.CompareTag("Player")) return null;

        foreach (GameObject i in interactors)
        {
            if (sender.Equals(i))
            {
                interactors.Remove(i);
                currentInteractors--;

                if (isOpen)
                {
                    lerpCurrentValue = 0;
                    animationEnded = false;
                }

                isOpen = false;
                canBeOutlined = true;

                return null;
            }
        }


        interactors.Add(sender);
        currentInteractors++;

        if (currentInteractors >= neededInteractors)
        {
            isOpen = true;
            lerpCurrentValue = 0;
            animationEnded = false;

            canBeOutlined = false;
            SetOutlineActive(false);
        }

        return null;
    }

    public void SetOutlineActive(bool active)
    {
        if (active && canBeOutlined && !isLocked) outline.OutlineWidth = baseOutlineValue;
        else if (!active) outline.OutlineWidth = 0;
    }

    private void Update()
    {
        if (isOpen && !animationEnded)
        {
            Animate(target.position);
        }
        else if (!isOpen && !animationEnded)
        {
            Animate(basePos);
        }
    }

    private void Animate(Vector3 _target)
    {
        lerpCurrentValue += (Time.time * lerpSpeed * Time.deltaTime);
        mesh.transform.position = Vector3.Lerp(mesh.transform.position, _target, lerpCurrentValue);
        if (Vector3.Distance(mesh.transform.position, _target) <= distanceBeforeStop)
            animationEnded = true;
    }

    public void ColorObject(Color _color)
    {
        colorableMesh.material.color = _color;
    }

    private void OnMouseOver()
    {
        if (Vector3.Distance(playerClient.transform.position, this.transform.position) > playerClient.InteractDistance)
            SetOutlineActive(false);
        else
            SetOutlineActive(true);
    }

    private void OnMouseExit()
    {
        SetOutlineActive(true);
    }
}
